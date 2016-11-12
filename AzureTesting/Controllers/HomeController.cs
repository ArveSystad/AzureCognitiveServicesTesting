using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using AzureTesting.Models;
using Microsoft.ProjectOxford.Vision;
using Newtonsoft.Json;

namespace AzureTesting.Controllers
{
    public class HomeController : Controller
    {
        private static VisionServiceClient Client => new VisionServiceClient(ConfigurationManager.AppSettings["VisionServiceApiKey"]);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateThumbnail()
        {
            return View();
        }

        public ActionResult DescribeImage()
        {
            return View();
        }

        public ActionResult AnalyzeImage()
        {
            return View();
        }

        public async Task<ActionResult> GetDescription(PostImageModel request)
        {
            var image = Request.Files[0];

            var description = await Client.DescribeAsync(image.InputStream, 5);
            var descriptionString = JsonConvert.SerializeObject(description, Formatting.Indented);

            var model = new ImageInfo
            {
                DescriptionResult = descriptionString
            };

            return PartialView("_ImageDescription", model);
        }
        
        public async Task<ActionResult> Analyze(PostImageModel request)
        {
            var visualFeatures = new List<VisualFeature>
                                 {
                                     VisualFeature.Adult,
                                     VisualFeature.Categories,
                                     VisualFeature.Color,
                                     VisualFeature.Description,
                                     VisualFeature.Faces,
                                     VisualFeature.ImageType,
                                     VisualFeature.Tags,
                                 };
            var analysisResult = await Client.AnalyzeImageAsync(Request.Files[0].InputStream, visualFeatures);

            var analysisString = JsonConvert.SerializeObject(analysisResult, Formatting.Indented);
            var model = new ImageInfo
            {
                DescriptionResult = analysisString
            };
            return PartialView("_ImageDescription", model);
        }

        public async Task<ActionResult> GetThumbnail(PostImageModel request)
        {
            var thumbnailAsync = await Client.GetThumbnailAsync(Request.Files[0].InputStream, request.Width, request.Height);
            var imageBase64 = Convert.ToBase64String(thumbnailAsync);
            var imageDataModel = new ImageDataModel { ThumbnailSource = $"data:image/jpeg;base64,{imageBase64}" };

            return PartialView("_PostImage", imageDataModel);
        }
    }
}