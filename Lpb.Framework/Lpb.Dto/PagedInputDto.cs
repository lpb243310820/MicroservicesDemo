using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Lpb.Dto
{
    public class PagedInputDto : IPagedResultRequest
    {
        [Range(1, int.MaxValue)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }


        public PagedInputDto()
        {
            MaxResultCount = 10;
        }
    }
}