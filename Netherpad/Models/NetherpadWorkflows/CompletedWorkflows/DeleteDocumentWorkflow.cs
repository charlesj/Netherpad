namespace Netherpad.Models.NetherpadWorkflows.CompletedWorkflows
{
	using Netherpad.Models.NetherpadWorkflows.Steps.DeleteDocument;

	using Workflows;

	public class DeleteDocumentWorkflow : WorkflowBase
	{
		public DeleteDocumentWorkflow(NetherpadDataManager context, Document document)
			: base(context)
		{
			this.Add(new DeleteDocumentStep(context, document),1);
		}
	}
}