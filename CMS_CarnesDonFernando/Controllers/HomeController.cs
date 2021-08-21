using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CMS_CarnesDonFernando.Models;
using CMS_CarnesDonFernando.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CMS_CarnesDonFernando.Controllers
{
    public class HomeController : Controller
    {

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public ActionResult Login()
        {
            HttpContext.Session.SetString("JWToken", "");
            return View();
        }

        [HttpPost]
        public int IniciarSeccion(UserModel user)
        {
            TokenProviderHelper _token = new TokenProviderHelper();

            if ( user.UserMail != null && user.UserPassword != null)
            {
                var userToken = _token.LoginUser(user.UserMail.Trim(), user.UserPassword.Trim());

                if (userToken != null)
                {
                    //Salvar token en objecto de seccion
                    HttpContext.Session.SetString("JWToken", userToken);
                    return 1;
                }

                return 0;
                
            }
            return 0;
        }


        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public ActionResult Dashboard()
        {
            return View();
        }

        public void CerrarSession()
        {
            HttpContext.Session.SetString("JWToken", "");
            Response.Redirect("/Home/Login");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
