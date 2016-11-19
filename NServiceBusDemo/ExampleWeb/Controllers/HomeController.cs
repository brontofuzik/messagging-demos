using System.Web.Mvc;
using UserService.Messages.Commands;
namespace ExampleWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Json(new { text = "Hello world." });
        }

        public ActionResult CreateUser(string name, string email)
        {
            var cmd = new CreateNewUserCmd
            {
                Name = name,
                EmailAddress = email
            };

            ServiceBus.Bus.Send(cmd);

            return Json(new { sent = cmd });
        }

        protected override JsonResult Json(object data,
          string contentType,
          System.Text.Encoding contentEncoding,
          JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}