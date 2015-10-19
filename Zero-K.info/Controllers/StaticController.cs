using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;

namespace ZeroKWeb.Controllers
{
    public class StaticController : Controller
    {
        //
        // GET: /Static/
        public ActionResult Index(string name = "LobbyStart")
        {
            if (name == "UnitGuide") return View("Index", (object)"http://manual.zero-k.info");

            // We can't host a linked item in Scripts so we have to do this.
            if (name == "zkwl.bundle.js")
            {
                var result = new JavaScriptResult();
                var streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ZeroKWeb.zkwl.bundle.js"));
                result.Script = streamReader.ReadToEnd();
                return result;
            }

		    return View("Index", (object)string.Format("~/{0}.inc", name));
        }

    }
}
