using System.Web;

namespace AzureTesting.Models
{
    public class PostImageModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public HttpPostedFile Image { get; set; }
    }
}