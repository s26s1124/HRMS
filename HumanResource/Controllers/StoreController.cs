using HumanResource.Contexts;
using HumanResource.Models;
using HumanResource.Models.HRMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Oracle.ManagedDataAccess.Types;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace HumanResource.Controllers
{

    public class StoreController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly hrmsContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreController(hrmsContext context, INotyfService notyf, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _notyf = notyf;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserRequestList()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
               int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);

            return View(await _context.StrReqMstrs.Where(m => m.CancelFlage == 0  && m.EmpNo== EmoNo).OrderByDescending(m => m.ReqDate).ToListAsync());
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }

        
        public async Task<IActionResult> UserReqAddOrEdit(int? id)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);

                var user = _context.VempDtls.Where(m => m.EmpNo == EmoNo).FirstOrDefault();

                //-------Assigning Values to ViewBag------ -
                ViewBag.EmpNo = user.EmpNo;
                ViewBag.EmpNameA = user.EmpNameA;
                ViewBag.ListofCategory = GetCategorylist();
                ViewBag.ListofManagers = GetManagerList(user.EmpNo, user.DgCode, user.DeptCode, user.DesgType);

                if (id == 0 || id == null)
                {

                    StrReqMstr reqMstr = new StrReqMstr();
                    reqMstr.StrRequestDetails.Add(new StrRequestDetails() { ReqDetailNo = 1 });
                    ViewBag.ListofItems = GetItemlist();

                    return View(reqMstr);

                }

                var reqMstrEdit = _context.StrReqMstrs.Include(a => a.StrRequestDetails).Where(x => x.ReqNo == id).FirstOrDefault();
                ViewBag.ListofItems = GetItemlist(reqMstrEdit.StrRequestDetails.FirstOrDefault().CategoryCode);
                if (reqMstrEdit == null)
                {
                    return View();
                }
                return View(reqMstrEdit);

            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserReqAddOrEdit(int id, StrReqMstr strReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    strReq.StrRequestDetails.RemoveAll(a => a.IsDeleted == true);

                    //Insert
                    if (id == 0 || id == null)
                    {

                        if (_context.StrReqMstrs.Count() == 0)
                        {
                            strReq.ReqNo = 1001;
                        }
                        else
                        {
                            strReq.ReqNo = _context.StrReqMstrs.Max(m => m.ReqNo) + 1;
                        }

                        var i = 1;
                        foreach (var m in strReq.StrRequestDetails)
                        {
                            m.ReqNo = strReq.ReqNo;
                            m.ReqDetailNo = i;
                            i++;
                        }

                        strReq.ReqDate = DateTime.Now;
                        strReq.CancelFlage = 0;

                        _context.Add(strReq);
                        await _context.SaveChangesAsync();
                    }
                    //Update
                    else
                    {
                        try
                        {
                            _context.Update(strReq);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!StrRequestCodeExists(strReq.ReqNo))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }
                    _notyf.Success("Your request has been successfully submitted and is being processed" , 10);

                    return RedirectToAction("UserRequestList");
                    //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "UserRequestList", _context.StrReqMstrs.Where(m => m.CancelFlage == 0).ToList()) });
                }
                var errors = ModelState.Values.Where(E => E.Errors.Count > 0)
              .SelectMany(E => E.Errors)
              .Select(E => E.ErrorMessage)
              .ToList();
                var user = _context.VempDtls.Where(m => m.EmpNo == strReq.EmpNo).FirstOrDefault();

                //-------Assigning Values to ViewBag------ -
                ViewBag.EmpNo = user.EmpNo;
                ViewBag.EmpNameA = user.EmpNameA;
                ViewBag.ListofCategory = GetCategorylist();
                ViewBag.ListofManagers = GetManagerList(user.EmpNo, user.DgCode, user.DeptCode, user.DesgType);
                ViewBag.ListofItems = GetItemlist();
                return View(strReq);
                ///   return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UserReqAddOrEdit", strReq) });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }


        public async Task<IActionResult> UserReqRetrieval()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                    int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);
                    ViewBag.EmpNo = EmoNo;
                    ViewBag.Requst = GetStoreReqlist(EmoNo);
                    ViewBag.ListofItems = GetRetrivalItemlist();
                    return View();
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserReqRetrieval(int ReqNo, int ReqDetailNo, int QutRetrieve, string RetrieveReason, int EmpNo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     var details = _context.StrRequestDetails.Find(ReqNo,ReqDetailNo);

                   details.IsItemRetrieve = 1;
                    details.RetrieveReason = RetrieveReason;
                    details.QutRetrieve = QutRetrieve;
                    _context.Update(details);
                    await _context.SaveChangesAsync();
                    
                    _notyf.Success("Your request has been successfully Updated and is being processed", 10);
                    return RedirectToAction("UserRequestList", "Store");

                }
                ViewBag.EmpNo = EmpNo;
                ViewBag.Requst = GetStoreReqlist(EmpNo);
                ViewBag.ListofItems = GetRetrivalItemlist();
                return View();
            }
            catch (InvalidCastException ww)
            {
                return RedirectToAction("Errorpage404", "Account");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }

        }
        public async Task<IActionResult> ManagerApprList()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);
                var ReqList = _context.StrReqMstrs.Where(m => m.StrManagrAppr == EmoNo).OrderByDescending(m => m.ReqDate).ToList();
                return View(ReqList);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }


        public async Task<IActionResult> OrderAprroval(int id, string status, string Page)
        {
            try
            {

                var ReqList = _context.StrReqMstrs.Include(x => x.StrRequestDetails).ThenInclude(x => x.StrCategory).ThenInclude(x => x.StrItemCodes).Where(m => m.ReqNo == id).FirstOrDefault();
                @ViewBag.EmpNameA = _context.VempDtls.FirstOrDefault(x => x.EmpNo == ReqList.EmpNo).EmpNameA;
                @ViewBag.Status = status;
                @ViewBag.Page = Page;

                return View(ReqList);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }

        }

        [HttpPost]
        public ActionResult OrderAprroval(int? id, string foo, string Page)
        {
            try
            {
                if (id != 0 || id != null)
                 {
                var StrReq = _context.StrReqMstrs.FirstOrDefault(x => x.ReqNo == id);

                if (StrReq != null)
                {

                    if (!string.IsNullOrEmpty(Page) && Page.Equals("OrderAprrovalList"))
                    {
                        if (!string.IsNullOrEmpty(foo) && foo == "Approve")
                         {                        
                            StrReq.ManagrApprDate = DateTime.Now.Date;
                            StrReq.ManagrAppr = 1;                                           
                        }
                        else if (!string.IsNullOrEmpty(foo) && foo == "Unapprove")
                        {
                            StrReq.ManagrApprDate = DateTime.Now.Date;
                            StrReq.ManagrAppr = 2;
                        }
                     

                    }
                    else if (!string.IsNullOrEmpty(Page) && Page.Equals("StoreManagerAprrList"))
                    {
                        if (!string.IsNullOrEmpty(foo) && foo == "Approve")
                        {
                            StrReq.StrManagrApprDate = DateTime.Now.Date;
                            StrReq.StrManagrAppr = 1;
                        }
                         else if (!string.IsNullOrEmpty(foo) && foo == "Unapprove")
                         {
                            StrReq.StrManagrApprDate = DateTime.Now.Date;
                            StrReq.StrManagrAppr = 2;
                         }
                    }

                }

                    _notyf.Success("Your request has been successfully update", 10);
                    _context.Update(StrReq);
                _context.SaveChangesAsync();
            }
            return RedirectToAction(Page);
        }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
    }

}


        public async Task<IActionResult> StoreManagerApprList()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var ReqList = _context.StrReqMstrs.Where(m => m.ManagrApprFlag == 1).ToList();
                 return View(ReqList);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }

        public async Task<IActionResult> StorManagerAppr(int? id)
        {
            try
            {
                var ReqList = _context.StrReqMstrs.Include(x => x.StrRequestDetails).ThenInclude(x => x.StrCategory).ThenInclude(x => x.StrItemCodes).Where(m => m.ReqNo == id).FirstOrDefault();
                  @ViewBag.EmpNameA = _context.VempDtls.FirstOrDefault(x => x.EmpNo == ReqList.EmpNo).EmpNameA;
                  return View(ReqList);
              }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
             }
        }

        [HttpPost]
        public async Task<IActionResult> StorManagerAppr(StrReqMstr strReqM)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);

                if (strReqM.StrRequestDetails != null)
                {
                    strReqM.StrManagrAppr = EmoNo;
                    strReqM.StrManagrApprFlag = 1;
                    strReqM.StrManagrApprDate = DateTime.Now;
                    _context.Update(strReqM);


                    foreach (var item in strReqM.StrRequestDetails)
                    {
                        var code = _context.StrItemCodes.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);

                        if (code != null)
                        {
                            code.TotalQuaAva = (int)code.TotalQuaAva - (int)item.ItemQutReceived;
                        }
                        _context.Update(code);

                    }


                    await _context.SaveChangesAsync();

                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrRequestCodeExists(strReqM.ReqNo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _notyf.Success("The request has been successfully Updated", 10);
            return RedirectToAction("ManagerApprList");
        }

        public async Task<IActionResult> StrManagerRetrievalList(int? id)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (identity == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                int EmoNo = Convert.ToInt32(identity.FindFirst("EmpNo").Value);
                var ReqList = _context.StrReqMstrs.Where(m => m.StrRequestDetails.Any(s=> s.IsItemRetrieve==1)).OrderByDescending(m => m.ReqDate).ToList();
                return View(ReqList);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Errorpage404", "Account");
            }
        }
        public JsonResult GetItems(int CategoryID)
        {
            List<StrItemCode> ItemList = new List<StrItemCode>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            ItemList = (from item in _context.StrItemCodes
                        where item.CategoryCode == CategoryID && item.ItemDetDeactivated == 0
                        select item).ToList();


            return Json(new SelectList(ItemList, "ItemCode", "ItemName"));
        }

        private List<SelectListItem> GetManagerList(int? empId, int? dg, int? dept, int? desgnation)
        {
            try
            {

                using (var ctx = new hrmsContext())
                using (var cmd = ctx.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "MANGERLIST";

                    var EMPNOParam = new OracleParameter("inEMPNO", OracleDbType.Int32, empId, ParameterDirection.Input);
                    var DGParam = new OracleParameter("inDG", OracleDbType.Int32, dg, ParameterDirection.Input);
                    var DEPTParam = new OracleParameter("inDEPT", OracleDbType.Int32, dept, ParameterDirection.Input);
                    var DESGNParam = new OracleParameter("inDESGN", OracleDbType.Int32, desgnation, ParameterDirection.Input);
                    var l_idParam = new OracleParameter("Outl_id", OracleDbType.RefCursor, ParameterDirection.Output);

                    cmd.Parameters.AddRange(new[] { EMPNOParam, DGParam, DEPTParam, DESGNParam, l_idParam });
                    cmd.Connection.Open();
                    var result = cmd.ExecuteNonQuery();
                    OracleDataReader dr = ((OracleRefCursor)cmd.Parameters[4].Value).GetDataReader();

                    List<SelectListItem> Mangers = new List<SelectListItem>();
                    while (dr.Read())
                    {
                        Mangers.Add(new SelectListItem() { Text = (string)dr["EmpName"], Value = (string)dr["EmpNo"] });


                    }

                    cmd.Connection.Close();




                    return Mangers;

                }
            }
            catch (Exception x)
            {

                return null;
            }

        }

        private List<SelectListItem> GetStoreReqlist( int EmoNo)
        {
            List<SelectListItem> requst = _context.StrReqMstrs.Include(E => E.StrRequestDetails)
                 .Where(e => e.StrManagrApprDate != null && e.EmpNo== EmoNo &&  e.StrRequestDetails.Any(a=>a.IsItemRetrieve==0 && a.ItemQutReceived !=0))
                 .Select(c => new SelectListItem
                  {
                      Value = c.ReqNo.ToString(),
                      Text = c.ReqNo.ToString()
                  }).ToList();

            requst.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "--- select Order ---"
            });

            return requst;

        }

        private List<SelectListItem> GetCategorylist()
        {
            try
            {

                // ------- Getting Data from Database Using EntityFrameworkCore -------
                List<SelectListItem> categorylist = _context.StrCategoryCodes
                                                  .Where(c => c.CategoryDeactivated == 0 || c.CategoryDeactivated == null)
                                                  .Select(c => new SelectListItem
                                                  {
                                                      Value = c.CategoryCode.ToString(),
                                                      Text = c.CategoryName
                                                  }).ToList();

                categorylist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "--- select Cayegory ---"
                });

                return categorylist;


            }
            catch (Exception)
            {

                return null;
            }

        }

        private List<SelectListItem> GetItemlist(int Code = 1)
        {
            try
            {

                // ------- Getting Data from Database Using EntityFrameworkCore -------
                List<SelectListItem> Items = _context.StrItemCodes
                                  .Where(c => c.CategoryCode == Code && (c.ItemDetDeactivated == 0 || c.ItemDetDeactivated == null))
                                  .OrderBy(c => c.ItemName)
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.ItemCode.ToString(),
                                      Text = c.ItemName
                                  }).ToList();


                Items.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "--- select Items ---"
                });
                return Items;


            }
            catch (Exception)
            {

                return null;
            }

        }

        private List<SelectListItem> GetRetrivalItemlist(int reqNo = 1)
        {
            try
            {

                // ------- Getting Data from Database Using EntityFrameworkCore -------
                List<SelectListItem> Items = _context.StrRequestDetails
                                  .Where(c => c.ReqNo == reqNo && c.ItemQutReceived > 0 && c.IsItemRetrieve==0)
                                  .OrderByDescending(c => c.ReqDetailNo)
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.ReqDetailNo.ToString(),
                                      Text = c.StrItem.ItemName + '(' + c.ItemQuantity.ToString() + ')'
                                  }).ToList();


                Items.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "--- select Items ---"
                });
                return Items;


            }
            catch (Exception)
            {

                return null;
            }

        }
        private bool StrRequestCodeExists(int id)
        {
            return (_context.StrReqMstrs?.Any(e => e.ReqNo == id)).GetValueOrDefault();
        }

        [HttpGet]
        public JsonResult GetItemsByCategory(int CategoryCode)
        {
            List<SelectListItem> categorylist = GetItemlist(CategoryCode);
            return Json(categorylist);
        }

        [HttpGet]
        public JsonResult GetItemsByRequest(int reqNo)
        {
            List<SelectListItem> categorylist = GetRetrivalItemlist(reqNo);
     
            return Json(categorylist);
        }

        [AllowAnonymous]
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyQuantity(int ItemQuantity, int ItemCode)
        {
            var qnt = _context.StrItemCodes.FirstOrDefault(x => x.ItemCode == ItemCode);


            if (ItemQuantity > qnt.TotalQuaAva)
              
            return Json(data: false);
            else
                return Json(data: true);
        }



    }
}

