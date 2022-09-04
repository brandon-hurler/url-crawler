using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace URL_Crawler.Models
{
    public class UrlFormModel
    {
        // Disabling model binder nullable warning for view model
        // Also could use #nullable disable on view model class if containing multiple properties.
        [Url]
        [Required(ErrorMessage = "Enter a URL.")]
        [DisplayName("URL")]
        public string Url { get; set; } = null!;

        [Required(ErrorMessage = "Enter a number.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid, positive, non-zero integer.")]
        public int? TopWordCount { get; set; } = 10;
    }
}