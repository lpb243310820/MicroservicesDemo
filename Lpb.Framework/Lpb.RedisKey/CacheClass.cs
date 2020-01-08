using Abp.Timing;
using System;

namespace Lpb.RedisKey
{
    public class FormIdSaveInput
    {
        public string FormId { get; set; }

        public int FormCount { get; set; }

        public DateTime FormTime { get; set; }

        public FormIdSaveInput()
        {
            FormCount = 1;
            FormTime = Clock.Now;
        }
    }
}
