using System.ComponentModel.DataAnnotations;

namespace AnagramSolver.WebApp.Models.Analysis;

public class FrequencyAnalysisRequestModel
{
    [Required(ErrorMessage = "Text is required")]
    [MinLength(1, ErrorMessage = "Text must not be empty")]
    [MaxLength(100000, ErrorMessage = "Text cannot exceed 100,000 characters")]
    public required string Text { get; set; }
}
