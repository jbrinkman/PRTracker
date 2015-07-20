namespace PRTracker.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string DnnLogin { get; set; }
        public string Name { get; set; }
        public bool IsEmployee { get; set; }
        public int? DnnUserId { get; set; }

    }
}