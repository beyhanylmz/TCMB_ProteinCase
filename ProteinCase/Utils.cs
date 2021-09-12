using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProteinCase
{
    public static class Utils
    {
        public static async Task<string> SendHttpClientRequest
        (
            IHttpClientFactory httpClientFactory,
            string uri
        )
        {
            string responseBody;
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync(uri);
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something Went Wrong! Error Occured");
                }
                
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}\n{e.InnerException?.Message}");
            }

            return responseBody;
        }
    }
}