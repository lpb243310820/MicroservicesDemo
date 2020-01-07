using System.ComponentModel.DataAnnotations;

namespace Lpb.WebPortal.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}