namespace Netherpad.Models.NetherpadWorkflows.Steps.DeleteDocument
{
	using Workflows;

	public class DeleteDocumentStepResult : WorkflowStepResult
	{
		public DeleteDocumentStepResult(bool success, string workDescription, string errorMessage)
			: base(success, workDescription, errorMessage)
		{
		}
	}
}