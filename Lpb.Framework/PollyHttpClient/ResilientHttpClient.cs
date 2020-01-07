using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Wrap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PollyHttpClient
{
    /// <summary>
    /// HttpClient wrapper that integrates Retry and Circuit
    /// breaker policies when invoking HTTP services. 
    /// Based on Polly library: https://github.com/App-vNext/Polly
    /// </summary>
    public class ResilientHttpClient : IHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ResilientHttpClient> _logger;
        //根据Url origin去创建policy
        private readonly Func<string, IEnumerable<IAsyncPolicy>> _policyCreator;
        //把policy打包成组合policy wrapper 进行本地缓存
        private ConcurrentDictionary<string, AsyncPolicyWrap> _policyWrappers;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClient(Func<string, IEnumerable<IAsyncPolicy>> policyCreator, ILogger<ResilientHttpClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            _logger = logger;
            _policyCreator = policyCreator;
            _policyWrappers = new ConcurrentDictionary<string, AsyncPolicyWrap>();
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async (p) =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                var response = await _client.SendAsync(requestMessage);

                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails

                //注释下面代码，把错误向上返回
                //if (response.StatusCode == HttpStatusCode.InternalServerError)
                //{
                //    throw new HttpRequestException();
                //}

                return await response.Content.ReadAsStringAsync();
            });
        }

        public Task<HttpResponseMessage> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async (p) =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                var response = await _client.SendAsync(requestMessage);

                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails

                //注释下面代码，把错误向上返回
                //if (response.StatusCode == HttpStatusCode.InternalServerError)
                //{
                //    throw new HttpRequestException();
                //}

                return response;
            });
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, uri, item);
            return DoPostPutAsync(HttpMethod.Post, uri, func, authorizationToken, requestId, authorizationMethod);
        }

        public Task<HttpResponseMessage> PostAsync(string uri, Dictionary<string, string> form, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, uri, form);
            return DoPostPutAsync(HttpMethod.Post, uri, func, authorizationToken, requestId, authorizationMethod);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, uri, item);
            return DoPostPutAsync(HttpMethod.Put, uri, func, authorizationToken, requestId, authorizationMethod);
        }

        private Task<HttpResponseMessage> DoPostPutAsync(HttpMethod method, string uri, Func<HttpRequestMessage> requestMessageFunc, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            // a new StringContent must be created for each retry 
            // as it is disposed after each call
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async (p) =>
            {
                HttpRequestMessage requestMessage = requestMessageFunc();

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    //SetAuthorizationHeader(requestMessage);
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                var response = await _client.SendAsync(requestMessage);

                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails

                //注释下面代码，把错误向上返回
                //if (response.StatusCode == HttpStatusCode.InternalServerError)
                //{
                //    throw new HttpRequestException();
                //}

                return response;
            });
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async (p) =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                return await _client.SendAsync(requestMessage);
            });
        }

        public Task<HttpResponseMessage> DeleteTAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async (p) =>
            {
                var requestMessage = CreateHttpRequestMessage(HttpMethod.Delete, uri, item); //new HttpRequestMessage(HttpMethod.Delete, uri);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                return await _client.SendAsync(requestMessage);
            });
        }

        private async Task<T> HttpInvoker<T>(string origin, Func<Context, Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);

            if (!_policyWrappers.TryGetValue(normalizedOrigin, out AsyncPolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(_policyCreator(normalizedOrigin).ToArray());
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }

            // Executes the action applying all 
            // the policies defined in the wrapper
            return await policyWrap.ExecuteAsync(action, new Context(normalizedOrigin));
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string uri, Dictionary<string, string> form)
        {
            return new HttpRequestMessage(method, uri) { Content = new FormUrlEncodedContent(form) };
        }

        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method, string uri, T item)
        {
            return new HttpRequestMessage(method, uri) { Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json") };
        }

        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }

        private static string GetOriginFromUri(string uri)
        {
            string origin = null;
            try
            {
                var url = new Uri(uri);
                origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                origin = uri.Substring(0, uri.IndexOf("/", 10));
            }

            if (!origin.Contains("http"))
            {
                throw new Exception(uri + "格式错误");
            }

            return origin;

        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            //var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //if (!string.IsNullOrEmpty(authorizationHeader))
            //{
            //    requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            //}
            if (!requestMessage.Headers.Contains("Authorization"))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { "Authorization" });
            }
        }
    }
}