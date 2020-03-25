using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lpb.Identity.Web.Authentication
{
    public class DoctorAuthCodeValidator : IExtensionGrantValidator
    {
        private readonly ILogger<DoctorAuthCodeValidator> _logger;

        public DoctorAuthCodeValidator(ILogger<DoctorAuthCodeValidator> logger)
        {
            _logger = logger;
        }

        public string GrantType => "doctor_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var userInfo = context.Request.Raw["userInfo"];
            var userId = context.Request.Raw["userId"];

            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            //用户不存在
            if (string.IsNullOrWhiteSpace(userInfo) || string.IsNullOrWhiteSpace(userId))
            {
                errorValidationResult.ErrorDescription = "用户不存在";
                context.Result = errorValidationResult;
                return;
            }
            try
            {
                var claims = new Claim[]
                {
                    new Claim("userInfo",userInfo),
                    //new Claim("phone",userInfo.PhoneNumber??string.Empty),
                    //new Claim("cardId",userInfo.CardId??string.Empty),
                    //new Claim(JwtClaimTypes.Subject, userInfo.Id.ToString()),
                    new Claim(JwtClaimTypes.Role, "doctor"),
                };

                context.Result = new GrantValidationResult(userId, GrantType, claims);
            }
            catch (Exception e)
            {
                _logger.LogError("ValidateAsync错误：" + e.StackTrace);
                errorValidationResult.ErrorDescription = e.Message;
                context.Result = errorValidationResult;
            }

            await Task.CompletedTask;
            return;
        }
    }
}
