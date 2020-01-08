using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lpb.UserCenter.EntityMapper
{
    public class TestCfg : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {

            builder.ToTable("Tests");


            builder.Property(a => a.Name).IsUnicode();


        }
    }
}


