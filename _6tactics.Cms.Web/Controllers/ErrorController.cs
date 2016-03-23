using _6tactics.Cms.Core.ViewModels.Error;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View(new ErrorViewModel
            {
                Message = "Error Occurred"
            });
        }

        public ActionResult NotFound()
        {
            return View(new ErrorViewModel
            {
                StatusCode = "404",
                Message = "NOT FOUND"
            });
        }

        public ActionResult MethodNotAllowed()
        {
            return View(new ErrorViewModel
            {
                StatusCode = "405",
                Message = "METHOD NOT ALLOWED"
            });
        }
    }
}
