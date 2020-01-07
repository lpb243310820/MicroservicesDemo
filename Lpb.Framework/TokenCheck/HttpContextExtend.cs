using Abp.Json;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TokenCheck
{
    public static class HttpContextExtend
    {
        public static T JwtDecoder<T>(this HttpContext context)
        {
            T t = default;

            try
            {
                var authHeader = context.Request.Headers["Authorization"];
                if (authHeader.Count == 1)
                {
                    if (!string.IsNullOrEmpty(authHeader.ToString()) && authHeader.ToString().StartsWith("Bearer "))
                    {
                        string token = authHeader.ToString().Substring(7);
                        if (!string.IsNullOrEmpty(token))
                        {
                            //jwt 解密token
                            IJsonSerializer serializer = new JsonNetSerializer();
                            IDateTimeProvider provider = new UtcDateTimeProvider();
                            IJwtValidator validator = new JwtValidator(serializer, provider);
                            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                            var requestService = context.RequestServices;
                            string resultstr = decoder.Decode(token);//token为之前生成的字符串
                            Console.WriteLine(resultstr);
                            t = JsonConvert.DeserializeObject<T>(resultstr);//反序列化 将jwt中的信息解压出来
                            Console.WriteLine(t.ToJsonString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                t = default;
            }

            return t;
        }

        public static HttpContextToken JwtDecoder(this HttpContext context)
        {
            HttpContextToken httpContextToken = null;

            try
            {
                var authHeader = context.Request.Headers["Authorization"];
                if (authHeader.Count == 1)
                {
                    if (!string.IsNullOrEmpty(authHeader.ToString()) && authHeader.ToString().StartsWith("Bearer "))
                    {
                        string token = authHeader.ToString().Substring(7);
                        if (!string.IsNullOrEmpty(token))
                        {
                            //jwt 解密token
                            IJsonSerializer serializer = new JsonNetSerializer();
                            IDateTimeProvider provider = new UtcDateTimeProvider();
                            IJwtValidator validator = new JwtValidator(serializer, provider);
                            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                            var requestService = context.RequestServices;
                            string resultstr = decoder.Decode(token);//token为之前生成的字符串
                            Console.WriteLine(resultstr);
                            httpContextToken = JsonConvert.DeserializeObject<HttpContextToken>(resultstr);//反序列化 将jwt中的信息解压出来
                            Console.WriteLine(httpContextToken.ToJsonString());

                            httpContextToken.token = token;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return httpContextToken;
        }

    }

    public class HttpContextToken
    {
        public string token { get; set; }

        public long nbf { get; set; }
        public long exp { get; set; }
        public string iss { get; set; }
        public List<string> aud { get; set; }
        public string client_id { get; set; }
        public string sub { get; set; }
        public long auth_time { get; set; }
        public string idp { get; set; }
        public string userInfo { get; set; }
        public string role { get; set; }
        public List<string> scope { get; set; }
        public List<string> amr { get; set; }
    }
}
