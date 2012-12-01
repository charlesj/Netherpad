namespace Netherpad.Models.NetherpadWorkflows.Steps.UpdateDocument
{
	using System;

	using Workflows;

	public class UpdateDocumentStep : IWorkflowStep
	{
		private readonly NetherpadDataManager db;

		private readonly Document document;

		private readonly string content;

		public UpdateDocumentStep(NetherpadDataManager db, Document document, string content)
		{
			this.db = db;
			this.document = document;
			this.content = content;
		}

		public UpdateDocumentStepResult Run()
		{
			var newVersion = new Document();
			newVersion.Content = content;
			newVersion.CreatedOn = DateTime.Now;
			newVersion.Identifier = document.Identifier;
			newVersion.Name = document.Name;
			newVersion.ParentId = document.DocumentId;  // this is where the magic happens!
			newVersion.Version = document.Version + 1;
			db.Context.Documents.Add(newVersion);
			return new UpdateDocumentStepResult(true, this.PostWorkDescription, string.Empty, newVersion);
		}


		public string PreWorkDescription
		{
			get
			{
				return string.Format("A document will be updated");
			}
		}

		public string PostWorkDescription
		{
			get
			{
				return string.Format("A document was updated");
			}
		}
	}

	public class UpdateDocumentStepResult : WorkflowStepResult
	{
		public Document Document { get; set; }

		public UpdateDocumentStepResult(bool success, string workDescription, string errorMessage, Document document)
			: base(success, workDescription, errorMessage)
		{
			Document = document;
		}
	}
}