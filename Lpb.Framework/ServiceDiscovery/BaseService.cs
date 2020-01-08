using Abp.UI;
using Abp.Web.Models;
using Castle.Core.Logging;
using Lpb.Extend;
using DnsClient;
using Microsoft.Extensions.Hosting;
using PollyHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UseConsul;

namespace ServiceDiscovery
{
    public class BaseService
    {
        public string _serviceUrl { get; set; }
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;

        public BaseService(ILogger logger, IHttpClient httpClient, IDnsQuery dnsQuery, IHostingEnvironment env, string serviceName, string localDebugAddress)
        {
            _logger = logger;
            _httpClient = httpClient;
            _serviceUrl = null;

            var result = dnsQuery.ResolveService("service.consul", serviceName).ToList();
            if (result.Count > 0)
            {
                var address = result.First().AddressList.FirstOrDefault();
                var port = result.First().Port;
                if (address == null || string.IsNullOrWhiteSpace(address.ToString()) || !address.ToString().Contains("."))
                {
                    address = result.Last().AddressList.FirstOrDefault();
                    port = result.Last().Port;
                }

                _serviceUrl = $"http://{address}:{port}";
                if (env.IsDevelopment())
                {
                    _serviceUrl = $"http://{localDebugAddress}:{port}";
                }
            }

            if (string.IsNullOrWhiteSpace(_serviceUrl))
            {
                if (serviceName.Equals("Service1")) _serviceUrl = $"http://{localDebugAddress}:{5001}";
                if (serviceName.Equals("Service2")) _serviceUrl = $"http://{localDebugAddress}:{5002}";

                if (serviceName.Equals("IdentityServer")) _serviceUrl = $"http://{localDebugAddress}:{5003}";

                _logger.Debug($"localDebugAddress：{serviceName}_{localDebugAddress}");
            }

            _logger.Debug($"serviceName：{serviceName}_{_serviceUrl}");
        }

        public async Task<R> HttpGetServices<R>(string url, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.GetAsync(url, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse<R>>(responseContent);
                        if (result.Success)
                        {
                            return result.Result;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpGetServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpGetServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpGetServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpGetServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task<R> HttpPostServices<R, T>(string url, T t, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.PostAsync(url, t, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse<R>>(responseContent);
                        if (result.Success)
                        {
                            return result.Result;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpPostServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task<R> HttpPostServices<R>(string url, Dictionary<string, string> form, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.PostAsync(url, form, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse<R>>(responseContent);
                        if (result.Success)
                        {
                            return result.Result;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpPostServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task HttpPostServices<T>(string url, T t, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.PostAsync(url, t, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse>(responseContent);
                        if (result.Success)
                        {
                            return;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpPostServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task HttpPostServices(string url, Dictionary<string, string> form, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.PostAsync(url, form, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse>(responseContent);
                        if (result.Success)
                        {
                            return;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpPostServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpPostServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpPostServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task HttpDeleteServices(string url, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.DeleteAsync(url, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent)/* && JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse>(responseContent);
                        if (result.Success)
                        {
                            return;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpDeleteServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpDeleteServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpDeleteServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpDeleteServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task HttpDeleteServices<T>(string url, T t, string token = null)
        {
            GetOriginFromUri(url);
            string errorMsg = string.Empty;
            try
            {
                var response = await _httpClient.DeleteTAsync(url, t, token);
                //if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        var result = JsonHelper.ToObject<AjaxResponse>(responseContent);
                        if (result.Success)
                        {
                            return;
                        }
                        errorMsg = result.Error.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"HttpDeleteServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"HttpDeleteServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"HttpDeleteServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            _logger.Error($"HttpDeleteServices{url}调用其他服务接口错误：" + errorMsg);
            throw new UserFriendlyException(errorMsg);
        }

        public async Task<R> TokenPostServices<R>(string url, Dictionary<string, string> form, string token = null)
        {
            GetOriginFromUri(url);
            R model = default(R);
            try
            {
                var response = await _httpClient.PostAsync(url, form);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent) /*&& JsonHelper.IsJson(responseContent)*/)
                    {
                        model = JsonHelper.ToObject<R>(responseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"TokenPostServices{url}调用其他服务接口错误：{ ex.Message }");
                _logger.Error($"TokenPostServices{url}调用其他服务接口错误：{ ex.StackTrace }");
                throw new UserFriendlyException($"TokenPostServices{url}调用其他服务接口错误：{ ex.Message }");
            }

            return model;
        }

        private string GetOriginFromUri(string uri)
        {
            _logger.Error($"url：{uri}");
            string origin = null;
            try
            {
                var url = new Uri(uri);
                origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            }
            catch (Exception ex)
            {
                _logger.Error($"GetOriginFromUri:{uri}错误：{ ex.StackTrace }");
                origin = uri.Substring(0, uri.IndexOf("/", 10));
            }

            if (!origin.Contains("http"))
            {
                throw new Exception(uri + "格式错误");
            }

            return origin;
        }


    }

}
