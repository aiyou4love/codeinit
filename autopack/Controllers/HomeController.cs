using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace autopack.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AutoPack(int nType)
        {
            string path_ = "~/Config/Beta/directory.json";
            if ((nType > 2) && (nType < 4))
            {
                path_ = "~/Config/Pack/directory.json";
            }
            StringDirectory stringDirectory_ = StringDirectory.instance();
            stringDirectory_.runInit(Server, path_);
            Script script_ = Script.instance();
            script_.runInit();
            ShellCommandMgr shellCommandMgr_ = ShellCommandMgr.instance();
            shellCommandMgr_.runInit(Server);
            CommandMgr commandMgr_ = CommandMgr.instance();
            commandMgr_.runInit(Server);

            return View();
        }
        public void runNext()
        {
            CommandMgr commandMgr_ = CommandMgr.instance();
            commandMgr_.runNext();
        }

        public void runPrev()
        {
        }

        public string runOutput()
        {
            CommandMgr commandMgr_ = CommandMgr.instance();
            if (commandMgr_.mQueue.Count > 0)
            {
                return commandMgr_.mQueue.Dequeue();
            }
            return "";
        }
    }
}
