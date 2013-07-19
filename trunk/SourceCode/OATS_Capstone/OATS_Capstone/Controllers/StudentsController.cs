using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OATS_Capstone.Models;
using TugberkUg.MVC.Helpers;
namespace OATS_Capstone.Controllers
{
    public class StudentsController : Controller
    {
        //
        // GET: /Students/
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewStudent(int id)
        {
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(i=>i.UserID == id);
            return View(user);
        }
        public JsonResult AssignTestToStudent(int userId, int testId)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString+=(model)=>{
                var result = string.Empty;
                try
                {
                    result=this.RenderPartialViewToString("P_Assign_Test_To_Student", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.AssignTestToStudent(userId, testId);
            return Json(new { common.success, common.message,common.generatedHtml });

        }
        public JsonResult UnassignTest(int userId, int testId)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Assign_Test_To_Student", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.UnassignTest(userId, testId);
            return Json(new { common.success, common.message ,common.generatedHtml});
        }
    }
}
