using DAL.IDal;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Export]
    public class MyNoteController : BaseController
    {
        /// <summary>
        /// MyNoteDal
        /// </summary>
        [Import]
        public INoteDal MyNoteDal { get; set; }

        /// <summary>
        /// MyNote页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MyNotePage()
        {
            ViewBag.noteList = MyNoteDal.Entities.ToList();

            return View();
        }
    }
}