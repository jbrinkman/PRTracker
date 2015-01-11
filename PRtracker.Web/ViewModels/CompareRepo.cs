namespace PRTracker.Web.ViewModels
{
    public class CompareRepo
    {
        public string label { get; set; }
        public string @ref { get; set; }
        public string sha { get; set; }
        public User user { get; set; }
        public Repo repo { get; set; }
    }
}