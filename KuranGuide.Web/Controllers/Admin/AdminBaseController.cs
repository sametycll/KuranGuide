using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminBaseController : Controller
    {
        
    }
}
