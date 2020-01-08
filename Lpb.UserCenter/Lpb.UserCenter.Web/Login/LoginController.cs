using Abp.Application.Services.Dto;
using Abp.Json;
using Abp.Runtime.Caching;
using Lpb.Dto.UserCenter.Token;
using Lpb.Extend;
using Lpb.UserCenter.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Lpb.RedisKey;
using ServiceDiscovery.IdentityServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lpb.UserCenter.Web.Login
{
    /// <summary>
    /// 获取或者刷新token，绑定手机号码（无需登录）
    /// </summary>
    [Route("[controller]/[action]")]
    public class LoginController : UserCenterControllerBase
    {
        private readonly ITokenService _tokenService;

        private readonly IConfiguration _appConfiguration;
        private readonly ICacheManager _cacheManager;


        public LoginController(IConfiguration appConfiguration, ICacheManager cacheManager, ITokenService tokenService)
        {
            _appConfiguration = appConfiguration;
            _cacheManager = cacheManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="input">RequestTokenDto</param>
        /// <returns>ResponseTokenModel</returns>
        [HttpPost]
        public async Task<ResponseTokenModel> RequestToken([FromBody]RequestTokenDto input)
        {
            ResponseTokenModel model = new ResponseTokenModel
            {
                Success = false,
                SetPassword = false
            };
            if (input == null)
            {
                model.Msg = L("UserCenter_ParamError");
                return model;
            }
            if (string.IsNullOrWhiteSpace(input.ClientId))
            {
                model.Msg = L("UserCenter_ParamError");
                return model;
            }

            var form = new Dictionary<string, string>();
            form["client_id"] = input.ClientId;

            //避免将refresh_token传递到客户端，使用用户Id+缓存的形式替代
            string userId = null;
            //用户登陆
            if (input.ClientId == "app_customer_client")
            {
                form["client_secret"] = _appConfiguration["Customer:ClientSecrets"];
                form["grant_type"] = _appConfiguration["Customer:GrantType"];
                form["userInfo"] = DesEncrypt.Encrypt("userInfo");
                form["userId"] = "1";
            }
            //医生登陆
            if (input.ClientId == "app_doctor_client")
            {
                form["client_secret"] = _appConfiguration["Doctor:ClientSecrets"];
                form["grant_type"] = _appConfiguration["Doctor:GrantType"];
                form["userInfo"] = DesEncrypt.Encrypt("userInfo");
                form["userId"] = "1";
            }

            var tokenModel = await _tokenService.RequestToken(form, input.ClientId);
            if (tokenModel != null)
            {
                model.Success = true;
                model.AccessToken = tokenModel.access_token;
                model.ExpiresIn = tokenModel.expires_in;
                model.TokenType = tokenModel.token_type;
                model.UserId = userId;

                List<string> deviceUsers = _cacheManager.GetCache(CacheKeyService.DeviceUser).Get(input.ClientId + userId, () => new List<string>());
                if (!deviceUsers.Contains(input.DeviceUUID))
                {
                    deviceUsers.Add(input.DeviceUUID);
                    _cacheManager.GetCache(CacheKeyService.DeviceUser).Set(input.ClientId + userId, deviceUsers);
                }

                //当前用户的token黑名单
                List<string> tokenBlacklist = _cacheManager.GetCache(CacheKeyService.BlacklistToken).Get(input.ClientId + userId, () => new List<string>());
                for (int i = 0; i < deviceUsers.Count; i++)
                {
                    var deviceUser = deviceUsers[i];
                    if (deviceUser.Equals(input.DeviceUUID))
                    {
                        List<string> curDeviceToken = _cacheManager.GetCache(CacheKeyService.DeviceToken).Get(input.ClientId + userId + deviceUser, () => new List<string>());
                        curDeviceToken.ForEach(p => tokenBlacklist.Remove(p));
                        continue;
                    }

                    //将其他所有设备的token放入黑名单
                    List<string> deviceToken = _cacheManager.GetCache(CacheKeyService.DeviceToken).Get(input.ClientId + userId + deviceUser, () => new List<string>());
                    if (deviceToken.Count > 0)
                    {
                        tokenBlacklist.AddRange(deviceToken);
                    }
                }
                _cacheManager.GetCache(CacheKeyService.BlacklistToken).Set(input.ClientId + userId, tokenBlacklist);

                //当前用户的token
                List<string> proToken = _cacheManager.GetCache(CacheKeyService.DeviceToken).Get(input.ClientId + userId + input.DeviceUUID, () => new List<string>());
                proToken.Add(tokenModel.access_token);
                _cacheManager.GetCache(CacheKeyService.DeviceToken).Set(input.ClientId + userId + input.DeviceUUID, proToken);

                _cacheManager.GetCache(CacheKeyService.RefreshToken).Set(input.ClientId + userId, tokenModel.refresh_token);
            }

            return model;
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="input">RefreshTokenDto</param>
        /// <returns>ResponseTokenModel</returns>
        [HttpPost]
        public async Task<ResponseTokenModel> RefreshToken([FromBody]RefreshTokenDto input)
        {
            ResponseTokenModel model = new ResponseTokenModel
            {
                Success = false,
                SetPassword = true
            };
            if (input == null)
            {
                model.Msg = L("UserCenter_ParamError");
                return model;
            }
            if (string.IsNullOrWhiteSpace(input.ClientId))
            {
                model.Msg = L("UserCenter_ParamError");
                return model;
            }

            var form = new Dictionary<string, string>();
            form["client_id"] = input.ClientId;
            form["refresh_token"] = _cacheManager.GetCache(CacheKeyService.RefreshToken).Get(input.ClientId + input.UserId, () => string.Empty);

            //用户登陆
            if (input.ClientId == "app_customer_client")
            {
                form["client_secret"] = _appConfiguration["Customer:ClientSecrets"];
                form["grant_type"] = "refresh_token";
            }
            //医生登陆
            if (input.ClientId == "app_doctor_client")
            {
                form["client_secret"] = _appConfiguration["Doctor:ClientSecrets"];
                form["grant_type"] = "refresh_token";
            }

            var tokenModel = await _tokenService.RefreshToken(form, input.ClientId);
            if (tokenModel != null)
            {
                model.Success = true;
                model.AccessToken = tokenModel.access_token;
                model.ExpiresIn = tokenModel.expires_in;
                model.TokenType = tokenModel.token_type;
                model.UserId = input.UserId;

                List<string> proToken = _cacheManager.GetCache(CacheKeyService.DeviceToken).Get(input.ClientId + input.UserId + input.DeviceUUID, () => new List<string>());
                proToken.Add(tokenModel.access_token);
                _cacheManager.GetCache(CacheKeyService.DeviceToken).Set(input.ClientId + input.UserId + input.DeviceUUID, proToken);
                _cacheManager.GetCache(CacheKeyService.RefreshToken).Set(input.ClientId + input.UserId, tokenModel.refresh_token);
            }

            return model;
        }

    }
}