using AspNetCoreHero.ToastNotification.Abstractions;
using HumanResource.Contexts;
using HumanResource.Models.HRMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HumanResource.Controllers
{
    public class StoreSettingController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly hrmsContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreSettingController(hrmsContext context, INotyfService notyf, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _notyf = notyf;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> CategoryList()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View(StrCatCodesList());
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }
                
        public async Task<IActionResult> CategoryAddOrEdit(int? id)
        {
            try
            {
                    if (id == 0 || id == null)
                {
                    StrCategoryCode code = new StrCategoryCode();
             

                    return View(code);
                }

                var strCatCode = await _context.StrCategoryCodes.FindAsync(id);
                if (strCatCode == null)
                {
                    return View();
                }
                return View(strCatCode);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryAddOrEdit(int id, [Bind("CategoryName")] StrCategoryCode strCatCode)
        {
            try
            {
                if (ModelState.IsValid)
            {
                //Insert
                if (id == 0 || id == null)
                {

                    if (_context.StrCategoryCodes.Count() == 0)
                    {
                        strCatCode.CategoryCode = 1;
                    }
                    else
                    {
                        strCatCode.CategoryCode = _context.StrCategoryCodes.Max(m => m.CategoryCode) + 1;

                    }
                    strCatCode.CategoryDeactivated= 0;
                    _context.Add(strCatCode);
                    await _context.SaveChangesAsync();
                }
                //Update
                else
                {
                    try
                    {
                        var StrCode = await _context.StrCategoryCodes.FindAsync(id);
                        if (StrCode == null)
                        {
                            return View(strCatCode);
                        }

                        StrCode.CategoryName = strCatCode.CategoryName;
                        _context.Update(StrCode);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StrCatCodeExists(strCatCode.CategoryCode))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "CategoryList", StrCatCodesList()) });
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "CategoryAddOrEdit", strCatCode) });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryDeleteConfirmed(int id)
        {

            try
            {
                if (id == 0 )
            {
                return Problem("Entity set 'hrmsContext.StrCatCodes'  is null.");
            }

            var strCatCode = await _context.StrCategoryCodes.FindAsync(id);
            if (strCatCode != null)
            {
                strCatCode.CategoryDeactivated = 1;
                _context.Update(strCatCode);
            }

            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "CategoryList", StrCatCodesList()) });
        }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
    }
}
      

        public async Task<IActionResult> ItemList()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View(strItemCodesList());
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }
       
        public async Task<IActionResult> ItemAddOrEdit(int? id)
        {
            try
            {

                if (id == 0 || id == null)
            {
                ViewBag.ListofCategory = GetCategorylist();
                return View(new StrItemCode());
            }

            var StrCatDet = await _context.StrItemCodes.FindAsync(id);
            if (StrCatDet == null)
            {
                return View(new StrItemCode());
            }

            return View(StrCatDet);
        }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
    }
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemAddOrEdit(int id, [Bind("ItemCode,ItemName,ItemDesc,ItemBrand,ItemModel,ItemCatCode,CategoryCode,TotalQuaAva,MaxQau,MinQau,NoToReq")] StrItemCode strItemCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Insert
                    if (id == 0)
                    {
                        if (_context.StrItemCodes.Count() == 0)
                        {
                            strItemCode.ItemCode = 1;
                        }
                        else
                        {
                            strItemCode.ItemCode = _context.StrItemCodes.Max(m => m.ItemCode) + 1;

                        }

                    try
                    {
                        _context.Add(strItemCode);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StrCatDetExists(strItemCode.ItemCode))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                   
                    }
                    //Update
                    else
                    {
                        try
                        {
                            _context.Update(strItemCode);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!StrCatDetExists(strItemCode.ItemCode))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "ItemList", strItemCodesList()) });
                }

                ViewBag.ListofCategory = GetCategorylist();
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "ItemAddOrEdit", strItemCode) });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }
          
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemDeleteConfirmed(int? id)
        {
            try
            {
                    if (id == 0 || id == null)
                {
                    return Problem("Entity set 'hrmsContext.StrItemCodes'  is null.");
                }

                var strItem = await _context.StrItemCodes.FindAsync(id);
                if (strItem != null)
                {
                    strItem.ItemDetDeactivated = 1;
                    _context.Update(strItem);
                }

                await _context.SaveChangesAsync();
                return Json(new { html = Helper.RenderRazorViewToString(this, "ItemList", _context.StrItemCodes.Where(m => m.ItemDetDeactivated == 0).ToList()) });

            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
       }

        private bool StrCatCodeExists(int id)
        {
            return (_context.StrCategoryCodes?.Any(e => e.CategoryCode == id)).GetValueOrDefault();
        }
        private bool StrCatDetExists(int id)
        {
            return (_context.StrItemCodes?.Any(e => e.ItemCode == id)).GetValueOrDefault();
        }

        private List<StrCategoryCode> StrCatCodesList()
        {
            return (_context.StrCategoryCodes.Where(m => m.CategoryDeactivated == 0 || m.CategoryDeactivated == null).ToList());
        }


        private List<StrItemCode> strItemCodesList()
        {

            var strItems = _context.StrItemCodes.Where(m => m.ItemDetDeactivated == 0 || m.ItemDetDeactivated == null).ToList();
            return strItems;
        }

        private List<SelectListItem> GetCategorylist()
        {
            try
            {
                // ------- Getting Data from Database Using EntityFrameworkCore -------
                List<SelectListItem> categorylist = _context.StrCategoryCodes
                                  .Where(c => c.CategoryDeactivated ==0)
                                  .OrderByDescending(c => c.CategoryCode)
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.CategoryCode.ToString(),
                                      Text = c.CategoryName.ToString()
                                  }).ToList();


                categorylist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "--- select Items ---"
                });

           
                return categorylist;
            }
            catch (Exception)
            {

                return null;
            }

        }


    }
}
