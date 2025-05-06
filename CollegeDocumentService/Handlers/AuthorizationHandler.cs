using CollegeDocumentService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CollegeDocumentService.Handlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly IStorageService _storageService;
        private readonly ITokenService _tokenService;

        public AuthorizationHandler(
            IStorageService storageService, 
            ITokenService tokenService)
        {
            _storageService = storageService;
            _tokenService = tokenService;
        }

        private bool IsRefreshing;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _storageService.Get("AccessToken");
            if (accessToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
