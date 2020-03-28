using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lpb.Identityserver.Data
{
    //public class PersistedGrantMysqlDbContext : PersistedGrantDbContext<PersistedGrantDbContext>
    //{
    //    public PersistedGrantMysqlDbContext(DbContextOptions<PersistedGrantMysqlDbContext> options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
    //    {

    //    }
    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);
    //        modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", e => e.Property<string>("Data").HasMaxLength(20000));//这里原来是5W 超长了
    //    }
    //}

    public class PersistedGrantSqlDbContext : PersistedGrantDbContext<PersistedGrantDbContext>
    {
        public PersistedGrantSqlDbContext(DbContextOptions<PersistedGrantSqlDbContext> options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", e => e.Property<string>("Data").HasMaxLength(20000));//这里原来是5W 超长了
        }
    }
}
