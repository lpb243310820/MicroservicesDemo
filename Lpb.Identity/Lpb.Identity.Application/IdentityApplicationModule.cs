using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using IdentityServer4.Models;
using System;

namespace Lpb.Identity
{
    [DependsOn(
        typeof(IdentityCoreModule),
        typeof(AbpAutoMapperModule))]
    public class IdentityApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IdentityApplicationModule).GetAssembly());

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                config =>
                {
                    //PersistedGrant -> PersistedGrantEntity
                    config.CreateMap<PersistedGrant, PersistedGrantEntity>()
                        .ForMember(d => d.Id, c => c.MapFrom(s => s.Key))
                        .ForMember(d => d.CreationTime, c => c.MapFrom(s => s.CreationTime.ToUniversalTime().ToLocalTime()))
                        .ForMember(d => d.Expiration, c => c.MapFrom(s => s.Expiration.HasValue ? (DateTime?)(s.Expiration.Value.ToUniversalTime().ToLocalTime()) : null));

                    //PersistedGrantEntity -> PersistedGrant
                    config.CreateMap<PersistedGrantEntity, PersistedGrant>()
                        .ForMember(d => d.Key, c => c.MapFrom(s => s.Id))
                        .ForMember(d => d.CreationTime, c => c.MapFrom(s => s.CreationTime.ToLocalTime().ToUniversalTime()))
                        .ForMember(d => d.Expiration, c => c.MapFrom(s => s.Expiration.HasValue ? (DateTime?)(s.Expiration.Value.ToLocalTime().ToUniversalTime()) : null));
                }
            );
        }
    }
}