using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace URL_Crawler.Models
{
    public class UrlFormViewModel
    {
        // Disabling model binder nullable warning for view model
        // Also could use #nullable disable on view model class if containing multiple properties.
        [Url]
        [Required]
        [DisplayName("URL")]
        public string Url { get; set; } = null!;
    }
}