using System.Text.Json.Serialization;

namespace AcademyProject.Entities
{
    public class FacebookUser
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string picture_url { get; set; }
    }
}
