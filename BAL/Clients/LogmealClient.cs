using BAL.Clients.Base;
using BAL.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BAL.Clients
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class LogmealClient : APIClient
    {

        public LogmealClient(string Logmeal_bearer)
        {
            httpClient.BaseAddress = new Uri("https://api.logmeal.es/v2/recognition/dish/v0.8?skip_types=%5B1%2C3%5D&language=eng");
            httpClient.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Bearer", Logmeal_bearer);
        }


        public async Task<PhotoResponse> SendRequest(Image img)
        {

            var requestContent = new MultipartFormDataContent();

            byte[] bytes = (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));

            var imageContent = new ByteArrayContent(bytes);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");


            requestContent.Add(imageContent, "image", "image.jpeg");

            var result = await httpClient.PostAsync("https://api.logmeal.es/v2/recognition/dish/v0.8?skip_types=%5B1%2C3%5D&language=eng", requestContent);
            string resultContent = await result.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PhotoResponse>(resultContent);
        }


        public async Task<string> FileUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "NotOk";
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var img = Image.FromStream(memoryStream))
                {

                    PhotoResponse resend = new PhotoResponse();
                    resend = await SendRequest(img);
                    try
                    {
                        var answer = resend.recognition_results[0].name;
                        return answer;

                    }
                    catch
                    {
                        return "productNotFound";
                    }
                }
            }
        }
    }

}
