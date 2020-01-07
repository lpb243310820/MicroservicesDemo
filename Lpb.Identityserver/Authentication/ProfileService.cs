using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Lpb.Identityserver.Authentication
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(p => p.Type == "sub")?.Value;

            if (!long.TryParse(subjectId, out long intUserId))
                throw new ArgumentException("Invalid subject identity");

            context.IssuedClaims = context.Subject.Claims.ToList();

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(p => p.Type == "sub")?.Value;
            context.IsActive = long.TryParse(subjectId, out long intUserId);

            return Task.CompletedTask;
        }
    }
}
