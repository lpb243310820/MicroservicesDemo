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
                if (serviceName.Equals("PayService")) _serviceUrl = $"http://{localDebugAddress}:{8001}";
                if (serviceName.Equals("CustomerService")) _serviceUrl = $"http://{localDebugAddress}:{8002}";
                if (serviceName.Equals("DataService")) _serviceUrl = $"http://{localDebugAddress}:{8003}";
                if (serviceName.Equals("DoctorService")) _serviceUrl = $"http://{localDebugAddress}:{8004}";

                if (serviceName.Equals("IdentityServer")) _serviceUrl = $"http://{localDebugAddress}:{8006}";
                if (serviceName.Equals("ResourceService")) _serviceUrl = $"http://{localDebugAddress}:{8007}";
                if (serviceName.Equals("UserService")) _serviceUrl = $"http://{localDebugAddress}:{8008}";
                if (serviceName.Equals("WeixinService")) _serviceUrl = $"http://{localDebugAddress}:{8009}";
                if (serviceName.Equals("SchedulerService")) _serviceUrl = $"http://{localDebugAddress}:{8010}";
                if (serviceName.Equals("FeedbackService")) _serviceUrl = $"http://{localDebugAddress}:{8011}";
                if (serviceName.Equals("DiagnoseService")) _serviceUrl = $"http://{localDebugAddress}:{8012}";
                if (serviceName.Equals("IoTService")) _serviceUrl = $"http://{localDebugAddress}:{8013}";
                if (serviceName.Equals("LessonService")) _serviceUrl = $"http://{localDebugAddress}:{8014}";
                if (serviceName.Equals("ShopService")) _serviceUrl = $"http://{localDebugAddress}:{8015}";
                if (serviceName.Equals("MessageService")) _serviceUrl = $"http://{localDebugAddress}:{8016}";
                if (serviceName.Equals("HuanxinService")) _serviceUrl = $"http://{localDebugAddress}:{8017}";
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

    public class ServiceDiscoveryOptions
    {
        /// <summary>
        /// 8001
        /// </summary>
        public string PayServiceName { get; set; }
        /// <summary>
        /// 8002
        /// </summary>
        public string CustomerServiceName { get; set; }
        /// <summary>
        /// 8003
        /// </summary>
        public string DataServiceName { get; set; }
        /// <summary>
        /// 8004
        /// </summary>
        public string DoctorServiceName { get; set; }
        /// <summary>
        /// 8006
        /// </summary>
        public string IdentityServerName { get; set; }
        /// <summary>
        /// 8007
        /// </summary>
        public string ResourceServiceName { get; set; }
        /// <summary>
        /// 8008
        /// </summary>
        public string UserServiceName { get; set; }
        /// <summary>
        /// 8009
        /// </summary>
        public string WeixinServiceName { get; set; }
        /// <summary>
        /// 8010
        /// </summary>
        public string SchedulerServiceName { get; set; }
        /// <summary>
        /// 8011
        /// </summary>
        public string FeedbackServiceName { get; set; }
        /// <summary>
        /// 8012
        /// </summary>
        public string DiagnoseServerName { get; set; }
        /// <summary>
        /// 8013
        /// </summary>
        public string IoTServerName { get; set; }
        /// <summary>
        /// 8014
        /// </summary>
        public string LessonServiceName { get; set; }
        /// <summary>
        /// 8015
        /// </summary>
        public string ShopServiceName { get; set; }
        /// <summary>
        /// 8016
        /// </summary>
        public string MessageServerName { get; set; }
        /// <summary>
        /// 8017
        /// </summary>
        public string HuanxinServerName { get; set; }

        public string LocalDebugAddress { get; set; }

        public ConsulDisvoveryOptions ConsulDnsEndpoint { get; set; }
    }
}
