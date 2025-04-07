using LiquorStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;


namespace LiquorStore.Controllers{
      public class AdminController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = context.HttpContext.Session.GetString("role");
        if (role != "Admin")
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
        }
        base.OnActionExecuting(context);
    }
}

}