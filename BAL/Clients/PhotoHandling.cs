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
    public class PhotoHandling
    {
        private static readonly string bearer = "8b188dce6682963601305fb729d764f4373f616a";

        public object JsonConvert { get; private set; }

        public async Task<PhotoResponse> SendRequest(string path)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.logmeal.es/v2/recognition/dish/v0.8?skip_types=%5B1%2C3%5D&language=eng");
                var requestContent = new MultipartFormDataContent();
                //    here you can specify boundary if you need---^
                //Read Image File into Image object.
                Image img = Image.FromFile(path);

                //ImageConverter Class convert Image object to Byte array.
                byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

                var imageContent = new ByteArrayContent(bytes);
                imageContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("image/jpeg");

                client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Bearer", bearer);


                requestContent.Add(imageContent, "image", "image.jpeg");
                var result = await client.PostAsync("https://api.logmeal.es/v2/recognition/dish/v0.8?skip_types=%5B1%2C3%5D&language=eng", requestContent);
                string resultContent = await result.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<PhotoResponse>(resultContent);
            };
        }

        public async Task<PhotoResponse> SendRequest(Image img)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.logmeal.es/v2/recognition/dish/v0.8?skip_types=%5B1%2C3%5D&language=eng");
                var requestContent = new MultipartFormDataContent();


                //ImageConverter Class convert Image object to Byte array.
                byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

                var imageContent = new ByteArrayContent(bytes);
                imageContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("image/jpeg");

                client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Bearer", "8b188dce6682963601305fb729d764f4373f616a");


                requestContent.Add(imageContent, "image", "image.jpeg");

                var result = await client.PostAsync("https://api.logmeal.es/v2/recognition/dish/v0.8?skip_types=%5B1%2C3%5D&language=eng", requestContent);
                string resultContent = await result.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<PhotoResponse>(resultContent);
            };
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
