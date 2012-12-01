using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Models.NetherpadWorkflows.Steps.DeleteDocument
{
	using Workflows;

	public class DeleteDocumentStep : IWorkflowStep
	{
		private readonly NetherpadDataManager db;

		private readonly Document document;

		public DeleteDocumentStep(NetherpadDataManager db, Document document)
		{
			this.db = db;
			this.document = document;
		}

		public DeleteDocumentStepResult Run()
		{
			this.DoDeletion(this.document);
			return new DeleteDocumentStepResult(true, this.PostWorkDescription, string.Empty);
		}

		public string PreWorkDescription
		{
			get
			{
				return string.Format("A document will be deleted");
			}
		}

		public string PostWorkDescription
		{
			get
			{
				return string.Format("A document was deleted");
			}
		}

		private void DoDeletion(Document doc)
		{
			doc.DeletedOn = DateTime.Now;
			if (doc.ParentId != 0)
			{
				this.DoDeletion(this.db.DocumentLocator.Find(doc.ParentId));
			}
		}
	}
}