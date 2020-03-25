using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Lpb.Extend;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lpb.Identity.Web.Authentication
{
    public class PortalAuthCodeValidator : IExtensionGrantValidator
    {
        private readonly ILogger<CustomerAuthCodeValidator> _logger;

        public PortalAuthCodeValidator(ILogger<CustomerAuthCodeValidator> logger)
        {
            _logger = logger;
        }

        public string GrantType => "portal_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var userInfo = context.Request.Raw["userInfo"];
            var userId = context.Request.Raw["userId"];
            var claimsIdentity = context.Request.Raw["claimsIdentity"];

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
                    new Claim(JwtClaimTypes.Role, "portal"),
                    //new Claim(JwtClaimTypes.p)
                };

                var ClaimsIdentity = CreateJwtClaims(JsonHelper.ToObject<ClaimsIdentity>(claimsIdentity));
                ClaimsIdentity.AddRange(claims);

                context.Result = new GrantValidationResult(userId, GrantType, ClaimsIdentity);
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

        private List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }
    }
}
