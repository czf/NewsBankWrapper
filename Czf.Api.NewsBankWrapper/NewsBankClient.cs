using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Czf.Domain.NewsBankWrapper.Extensions;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Czf.Domain.NewsBankWrapper.Enum;
using Czf.Domain.NewsBankWrapper.Interfaces;
using Czf.Domain.NewsBankWrapper.Objects;
using System.Linq;
using System.Security.Authentication;
using System.Runtime.CompilerServices;
using System.Web;
using System.Collections.Specialized;

[assembly: InternalsVisibleTo("Czf.Test.Api.NewsBankWrapper")]
namespace Czf.Api.NewsBankWrapper
{
    public class NewsBankClient
    {
        private const string SEARCH_PATH = "apps/news/results";
        private const string LOGIN_FORM_PARAMETER = "user";
        private const string PASSWORD_FORM_PARAMETER = "pass";
        private IEZProxySignInUriProvider _eZProxySignInUri;
        private IEZProxySignInCredentialsProvider _eZProxyCredentialsProvider;
        private IProductBaseUriProvider _productBaseUriProvider;
        private HttpClient _httpClient;
        private ICanLog _log;
        private bool _hasSignedIn;


        private HttpClientHandler _httpClientHandler;

        public NewsBankClient(IEZProxySignInUriProvider signInUrlProvider, 
            IEZProxySignInCredentialsProvider credentialsProvider,
            IProductBaseUriProvider baseUriProvider,
            ICanLog log)
        {

            _eZProxySignInUri = signInUrlProvider;
            _eZProxyCredentialsProvider = credentialsProvider;
            _productBaseUriProvider = baseUriProvider;
            _httpClientHandler = new HttpClientHandler() { CookieContainer = new CookieContainer()};
            _httpClient = new HttpClient(_httpClientHandler);
            _log = log;
        }


        /// <summary>
        /// Execute a search request
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns>the result object of the request</returns>
        public async Task<SearchResult> Search(SearchRequest searchRequest)
        {
            if (!_hasSignedIn) { await SignIn(); }
            if (!_hasSignedIn) { throw new AuthenticationException("SignIn did not succeed."); }

            SearchResult result = null;
            Uri searchRequestUri = GenerateSearchRequestURI(searchRequest);
            int tries = 3;
            bool success = false;
            do
            {
                tries--;

                using (HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(searchRequestUri))
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK
                        &&
                       httpResponseMessage.RequestMessage.RequestUri.GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped) == searchRequestUri.GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped))
                    {
                        result = new SearchResult(httpResponseMessage);
                        success = true;
                    }
                    else if (httpResponseMessage.RequestMessage.RequestUri.Query.Contains(searchRequestUri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped))) //redirected to signin
                    {
                        _log.Info("search was redirected to signin, will attempt to signin.");
                        await SignIn();
                        if (!_hasSignedIn) { throw new AuthenticationException("SignIn did not succeed."); }
                    }
                }
            } while (tries > 0 && !success);

            return result;
        }

        internal async Task<bool> SignIn()
        {
            try
            {
                Dictionary<string, string> login = new Dictionary<string, string>()
                {
                    { LOGIN_FORM_PARAMETER, _eZProxyCredentialsProvider.GetAccount()},
                    { PASSWORD_FORM_PARAMETER, _eZProxyCredentialsProvider.GetPassword()}
                };

               
                using (FormUrlEncodedContent content = new FormUrlEncodedContent(login))
                using (HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(_eZProxySignInUri.GetSignInUri(), content))
                {

                    if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        _log.Error($"Status code did not return 200: {httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase}");
                    }
                    else
                    {
                        _log.Info("Sign in successful");
                        _hasSignedIn = true;
                        
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return _hasSignedIn;
        }

        private Uri GenerateSearchRequestURI(SearchRequest searchRequest)
        {
            /// apps / news / browse - pub ? p = WORLDNEWS & t = pubname % 3ASTWES % 21Seattle % 2BTimes % 252C % 2BThe % 253A % 2BWeb % 2BEdition % 2BArticles % 2B % 2528WA % 2529 & action = browse & f = advanced
            StringBuilder path = new StringBuilder();
            path.Append($"{_productBaseUriProvider.GetProductBaseUri()}{SEARCH_PATH}");


            path.Append($"?p={Uri.EscapeDataString(searchRequest.Product.GetDescription())}");
            if (searchRequest.Publications?.Count > 0)
            {
                path.Append($"&t=pubname:{ Uri.EscapeDataString(searchRequest.Publications.Select(x => x.GetDescription()).Aggregate((a, b) => a + b))}");//  is any thing supposed to be between entries?
            }
            path.Append($"&sort={Uri.EscapeDataString(searchRequest.SortOrder.GetDescription())}");
            path.Append($"&maxresults=20");
            path.Append("&f=advanced");
            #region parameters
            if (searchRequest.SearchParameter0 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter0, 0)}");
            }
            if (searchRequest.SearchParameter1 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter1, 1)}");
            }
            if (searchRequest.SearchParameter2 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter2, 2)}");
            }
            if (searchRequest.SearchParameter3 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter3, 3)}");
            }
            if (searchRequest.SearchParameter4 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter4, 4)}");
            }
            if (searchRequest.SearchParameter5 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter5, 5)}");
            }
            if (searchRequest.SearchParameter6 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter6, 6)}");
            }
            if (searchRequest.SearchParameter7 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter7, 7)}");
            }
            if (searchRequest.SearchParameter8 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter8, 8)}");
            }
            if (searchRequest.SearchParameter9 != null)
            {
                path.Append($"&{FormatParameter(searchRequest.SearchParameter9, 9)}");
            }
            #endregion parameters
            return new Uri(path.ToString());
        }

        private string FormatParameter(SearchParameter searchParameter, int index)
        {
            return $"val-base-{index}={Uri.EscapeDataString(searchParameter.Value)}&fld-base-{index}={Uri.EscapeDataString(searchParameter.Field.GetDescription())}&bln-base-{index}={Uri.EscapeDataString(searchParameter.ParameterCompoundOperator.GetDescription())}";
        }
    }
}
