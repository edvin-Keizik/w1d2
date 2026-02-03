namespace AnagramSolver.WebApp.ViewModels
{
    public class AnagramViewModel
    {
        public string InputWord { get; set; } = string.Empty;
        public List<string> Result { get; set; } = new List<string>();
    }
}
