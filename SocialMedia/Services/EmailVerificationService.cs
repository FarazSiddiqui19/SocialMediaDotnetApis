using Microsoft.AspNetCore.WebUtilities;
using SocialMedia.DTO.Users;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class EmailVerificationService: IEmailVerificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmailVerificationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> GetDataAsync(string email)
        {
            HttpClient client = _httpClientFactory.CreateClient("EmailVerify");

            Dictionary<string,string> queryParams = new Dictionary<string, string>
            {
                ["email"] = email
               
            };

         
            string uri = QueryHelpers.AddQueryString("validate", queryParams);

            HttpResponseMessage response = await client.GetAsync(uri);


            EmailApiDTO? content = null;

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadFromJsonAsync<EmailApiDTO>();

                if (content == null) 
                {
                    return false;
                }

                if (content.Score > 90)
                { 
                    return true;
                }
            }
            return false;
        }
    }
}
