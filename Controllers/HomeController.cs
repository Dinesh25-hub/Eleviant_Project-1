using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using User_Application.Data;
using User_Application.Models;

namespace User_Application.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly DataContext datacontext;


        public HomeController(DataContext datacontext)
        {
            this.datacontext = datacontext;
        }

        public IActionResult Log_in()
        {


            return View();
        }
        public IActionResult Home()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Log_in(Authentication auth)
        {
            var data = new Authentication()
            {
                Id = Guid.NewGuid(),
                Name = auth.Name,
                Username = auth.Username,
                Email = auth.Email,
                Password = auth.Password,
            };
            await datacontext.authentication.AddAsync(data);
            await datacontext.SaveChangesAsync();
            return View();
        }

        [HttpPost]
        public IActionResult Update(Authentication auth)
        {
            var temp = datacontext.authentication.FirstOrDefault(x => x.Id == auth.Id);
            var data = new Authentication()
            {
                Id = temp.Id,
                Name = temp.Name,
                Username = temp.Username,
                Email = temp.Email,
                Password = temp.Password,
            };
            datacontext.authentication.Update(data);
            datacontext.SaveChanges();
            return View("Log_In");
        }
     
        public ActionResult Sign_Up()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Home(Authentication auth)
        {
            var data =await datacontext.authentication.FirstOrDefaultAsync(x => x.Username == auth.Username);
            if (data != null && data.Password == auth.Password) {
                return View();
            }

            return View("Log_In");
        }
       

        public ActionResult Reset_Password()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Reset_Password(Authentication auth)
        {
            var data = await datacontext.authentication.FirstOrDefaultAsync(x => x.Email == auth.Email);
            if (data != null) {
                return  await Task.Run(() => View("Change_Password", data));
            }
            return View("Reset_Password");
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        
    }
}