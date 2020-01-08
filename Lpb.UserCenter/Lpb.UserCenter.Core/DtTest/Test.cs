using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace Lpb.UserCenter
{
    public class Test : Entity<long>, IHasCreationTime
    {
        public DateTime dateTime { get; set; }

        public string Name { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
