using ColledgeDocument.Shared.Requests;
using CollegeDocumentService.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CollegeDocumentService.Services
{
    public interface ITokenService
    {
        Task<HttpResponseMessage> SignInAsync(TokenRequest request);
    }

    public class TokenService : ITokenService
    {
        private readonly IHttpClientFactory _factory;
        private readonly HttpClientOptions _httpClientOptions;

        public TokenService(IHttpClientFactory factory, IOptions<HttpClientOptions> options)
        {
            _factory = factory;
            _httpClientOptions = options.Value;
        }

        public Task<HttpResponseMessage> SignInAsync(TokenRequest request)
        {
            var httpClient = _factory.CreateClient(_httpClientOptions.ClientName);
            var response = httpClient.PostAsJsonAsync("/api/token", request);
            return response;
        }
    }
}