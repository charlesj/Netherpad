using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Netherpad.Controllers
{
	using Netherpad.Models;
	using Netherpad.Models.NetherpadWorkflows.CompletedWorkflows;
	using Netherpad.ViewModels;

	public class DocumentsController : Controller
    {
		private readonly NetherpadDataManager dataManager;

		public DocumentsController()
		{
			this.dataManager = new NetherpadDataManager(new NetherpadEntities());
		}

		public ActionResult Index()
        {
            return View();
        }

		public ActionResult Create(string name)
		{
			var document = new Document() { Name = name, Content = "", CreatedOn = DateTime.Now};
			var workflow = new CreateOrUpdateDocumentWorkflow(this.dataManager, document, string.Empty);
			var result = workflow.Execute();
			if (result.TotalSuccess)
			{
				return RedirectToAction("Edit", new { identifier = document.Identifier });
			}
			else
			{
				throw new Exception("hey there was a problem");
			}
		}

	    public ActionResult Edit(string identifier)
	    {
		    var document = this.dataManager.DocumentLocator.Find(identifier);
		    return View(new EditDocumentViewModel{Document = document, Versions = dataManager.DocumentLocator.FindPreviousVersions(document)});
	    }

		public ActionResult Save(string identifier, string content)
		{
			var document = this.dataManager.DocumentLocator.Find(identifier);
			var workflow = new CreateOrUpdateDocumentWorkflow(this.dataManager, document, content);
			var result = workflow.Execute();
			if (result.TotalSuccess)
			{
				return RedirectToAction("Edit", new { identifier = document.Identifier });
			}
			else
			{
				throw new Exception("hey there was a problem");
			}
		}
    }
}
