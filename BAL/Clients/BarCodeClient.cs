using BAL.Clients.Base;
using BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BAL.Clients
{
    public class BarCodeClient : APIClient
    {
        private static string apiID;

        private static readonly string codeEndpoint = @"search/item?upc=";
        private static readonly string consumeEndpoint = @"natural/nutrients";

        public BarCodeClient(string BarcodeAPI_id, string BarcodeAPI_key)
        {
            httpClient = new HttpClient();
            Url = @"https://trackapi.nutritionix.com/v2/";
            apiID = BarcodeAPI_id;
            Token = BarcodeAPI_key;

            httpClient.DefaultRequestHeaders.Add("x-app-id", apiID);
            httpClient.DefaultRequestHeaders.Add("x-app-key", Token);
        }


        public async Task<Meal> GetBarcodeInfo(string barcode)
        {
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(Url + codeEndpoint + barcode);


            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var foodlist = JsonSerializer.Deserialize<NutritionixResponse>(body);

                return foodlist.Meals[0];

            }

        }


        public async Task<Meal> Get100gInfo(string name)
        {
            string servings = $"100g of {name}";
            return await GetNaturalInfo(servings);

        }


        public async Task<Meal> GetNaturalInfo(string queryparameters)
        {

            Query query = new Query(queryparameters);
            var jsonQuery = JsonSerializer.Serialize(query);
            var stringContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");


            var response = await httpClient.PostAsync(Url + consumeEndpoint, stringContent);

            string result = await response.Content.ReadAsStringAsync();

            var responseitemslist = JsonSerializer.Deserialize<NutritionixResponse>(result);

            return responseitemslist.Meals[0];
        }

    }

}
