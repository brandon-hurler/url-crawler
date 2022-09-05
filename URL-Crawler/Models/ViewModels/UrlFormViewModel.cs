using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace URL_Crawler.Models.ViewModels
{
    public class UrlFormViewModel
    {
        // Disabling model binder nullable warning for view model
        // Also could use #nullable disable on view model class if containing multiple properties.
        [Url]
        [Required(ErrorMessage = "Enter a URL.")]
        [DisplayName("URL")]
        public string Url { get; set; } = null!;

        // Making nullable int for Required Data Annotation to display correct error message.
        [Required(ErrorMessage = "Enter a number.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a positive, non-zero integer.")]
        public int? TopWordCount { get; set; } = 10;
    }
}