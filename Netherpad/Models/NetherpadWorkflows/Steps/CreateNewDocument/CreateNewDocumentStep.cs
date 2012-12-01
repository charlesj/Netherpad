using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Models.NetherpadWorkflows.Steps.CreateNewDocument
{
	using Workflows;

	public class CreateNewDocumentStep : IWorkflowStep
	{
		private readonly NetherpadDataManager db;

		private readonly Document document;

		public CreateNewDocumentStep(NetherpadDataManager db, Document document)
		{
			this.db = db;
			this.document = document;
		}

		public CreateNewDocumentStepResult Run(string uniqueIdentifier)
		{
			this.document.Identifier = uniqueIdentifier;
			this.db.Context.Documents.Add(this.document);
			return new CreateNewDocumentStepResult(true, this.PostWorkDescription, string.Empty, this.document);
		}

		public string PreWorkDescription { get
		{
			return string.Format("A document will be created");
		}}

		public string PostWorkDescription
		{
			get
			{
				return string.Format("A document was created");
			}
		}
	}
}