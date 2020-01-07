using Abp.Application.Services.Dto;

namespace Lpb.WebPortal.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

