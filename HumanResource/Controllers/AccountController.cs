using HumanResource.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using HumanResource.Models;
using HumanResource.Models.HRMS;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace HumanResource.Controllers
{
    public class AccountController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly hrmsContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(hrmsContext context, INotyfService notyf, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _notyf = notyf;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login()
        {
           

            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userLogin.UserName == null || string.IsNullOrEmpty(userLogin.UserName) || userLogin.Password==0)
                    {
                        _notyf.Error("Email And Password are Required", 10);
                        //ModelState.AddModelError(nameof(userLogin.UserName), "Email And Password are Required");
                        return RedirectToAction("Login");
                    }

                  
                   
                    var User = _context.VempDtls.Where(m => m.EmailId.ToLower().Trim() == userLogin.UserName.ToLower().Trim()).Select(x => new VempDtl {NationalityIdNo = x.NationalityIdNo,EmpNo=x.EmpNo,  EmpNameAra = x.EmpNameAra, EmailId=x.EmailId }).FirstOrDefault();//.ToList();

                    if (User != null)
                    {
                        if (Convert.ToInt32(User.NationalityIdNo) == Convert.ToInt32(userLogin.Password))
                        {

                            //Create the identity for the user  
                            var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Email, User.EmailId),
                                            new Claim(ClaimTypes.Name, User.EmpNameAra),
                                            new Claim("EmpNo", User.EmpNo.ToString())
                                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                            

                            var principal = new ClaimsPrincipal(identity);
                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                            return RedirectToAction("UserRequestList", "Store");


                        }
                        else
                        {
                            _notyf.Error("Invaild User Password", 10);
                            return RedirectToAction("Login");

                        }
                   }
                    else
                    {
                        _notyf.Error("Invaild UserName OR Password",10);
                        return RedirectToAction("Login");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    _notyf.Error("IUnable to save changes. Try again, and if the problem persists see your system administrator", 10);

                }
              


            }
            return View();
        }

   
        public IActionResult UserDashboard()
        {
            return View();
        }

        
        
        public async Task<IActionResult> Logout()
        {
            // Clear authentication cookies
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
