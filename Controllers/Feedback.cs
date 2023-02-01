using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using User_Application.Models.Query;

namespace User_Application.Controllers
{
    public class Feedback : Controller
    {
        public IActionResult Query()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Query(FeedbackViewModel fd)
        {
            string path = @"C:\Users\RAMYA\Desktop\Feeedback.txt";

            using (FileStream f = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(fd.feedBack);
                f.Write(info);

            }
            return RedirectToAction("Query");
        }

    }
}
