using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Lpb.RedisKey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TokenCheck
{
    public class TokenCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;

        public TokenCheckMiddleware(RequestDelegate next, ICacheManager cacheManager, ILogger logger)
        {
            _next = next;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //var authHeader = context.Request.Headers["Authorization"];
                //if (authHeader.Count == 1)
                //{
                //    if (!string.IsNullOrEmpty(authHeader.ToString()) && authHeader.ToString().StartsWith("Bearer "))
                //    {
                //        //token黑名单
                //        List<string> tokenBlacklist = _cacheManager.GetCache(CacheKeyService.BlacklistToken).Get("Blacklist", () => new List<string>());
                //        string token = authHeader.ToString().Substring(7);
                //        if (tokenBlacklist.Count > 0 && !string.IsNullOrWhiteSpace(token))
                //        {
                //            //包含在黑名单中
                //            if (tokenBlacklist.Contains(token))
                //            {
                //                context.Response.StatusCode = 456;
                //                await HandleExceptionAsync(context, 456, "您已经在其他设备登陆，请重新登陆");
                //                return;
                //            }
                //        }
                //    }
                //}

                var model = context.JwtDecoder();
                if (model != null)
                {
                    //token黑名单
                    List<string> tokenBlacklist = _cacheManager.GetCache(CacheKeyService.BlacklistToken).Get(model.client_id + model.sub, () => new List<string>());
                    if (tokenBlacklist.Count > 0 && !string.IsNullOrWhiteSpace(model.token))
                    {
                        //包含在黑名单中
                        if (tokenBlacklist.Contains(model.token))
                        {
                            //_cacheManager.GetCache(CacheKeyService.BlacklistToken).Remove(model.client_id + model.sub);
                            _logger.Info("在其他设备登陆：" + model.client_id + model.sub);
                            context.Response.StatusCode = 456;
                            await HandleExceptionAsync(context, 456, "您已经在其他设备登陆，请重新登陆");
                            return;
                        }
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "未授权";
                }
                else if (statusCode == 404)
                {
                    msg = "未找到服务";
                }
                else if (statusCode == 502)
                {
                    msg = "请求错误";
                }
                else if (statusCode != 200)
                {
                    msg = "未知错误";
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    //await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var data = new { code = statusCode.ToString(), is_success = false, msg = msg };
            var result = JsonConvert.SerializeObject(new { data = data });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }

        private async Task<string> ReadBodyAsync(HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                await EnableRewindAsync(request).ConfigureAwait(false);
                //var encoding = GetRequestEncoding(request);
                return await this.ReadStreamAsync(request.Body, Encoding.UTF8).ConfigureAwait(false);
            }
            return null;
        }

        //private Encoding GetRequestEncoding(HttpRequest request)
        //{
        //    var requestContentType = request.ContentType;
        //    var requestMediaType = requestContentType == null ? default(MediaType) : new MediaType(requestContentType);
        //    var requestEncoding = requestMediaType.Encoding;
        //    if (requestEncoding == null)
        //    {
        //        requestEncoding = Encoding.UTF8;
        //    }
        //    return requestEncoding;
        //}

        private async Task EnableRewindAsync(HttpRequest request)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }
        }

        private async Task<string> ReadStreamAsync(Stream stream, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(stream, encoding, true, 1024, true))//这里注意Body部分不能随StreamReader一起释放
            {
                var str = await sr.ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);//内容读取完成后需要将当前位置初始化，否则后面的InputFormatter会无法读取
                return str;
            }
        }

    }
}
