using HumanResource.Contexts;
using HumanResource.Models.HRMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HumanResource.Menu
{
    public class NavMenu : ViewComponent
    {
        private readonly hrmsContext _context;

        public NavMenu(hrmsContext context)
        {
            _context = context;
        }

      

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if(identity.Claims.Count() != 0)
            {
                int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);
                if (EmoNo != 0 || EmoNo != null)
                {
                    var result =(from n in _context.NavigationMenus
                     join r in _context.RoleMenuPermissions
                     on n.PageId equals r.PageId
                     where r.EmpNo == EmoNo
                     select new NavigationMenu
                     {
                         PageId = n.PageId,
                         ParentMenuId = n.ParentMenuId,
                         PageName = n.PageName,
                         ActionName = n.ActionName,
                         ControllerName = n.ControllerName
                     }).AsEnumerable();

                    return View(result);
                }
            }
            return View();
        }
    }




}
