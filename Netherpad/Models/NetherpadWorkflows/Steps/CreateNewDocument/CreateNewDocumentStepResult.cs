namespace Netherpad.Models.NetherpadWorkflows.Steps.CreateNewDocument
{
	using Workflows;

	public class CreateNewDocumentStepResult : WorkflowStepResult
	{
		public CreateNewDocumentStepResult(bool success, string workDescription, string errorMessage, Document document)
			: base(success, workDescription, errorMessage)
		{
			this.Document = document;
		}

		public Document Document { get; set; }
	}
}