namespace Netherpad.Models.NetherpadWorkflows.Steps.GenerateUniqueIdentifier
{
	using Workflows;

	public class GenerateUniqueIdentifierStepResult : WorkflowStepResult
	{
		public string UniqueIdentifier { get; set; }

		public GenerateUniqueIdentifierStepResult(bool success, string workDescription, string errorMessage, string uniqueIdentifier)
			: base(success, workDescription, errorMessage)
		{
			this.UniqueIdentifier = uniqueIdentifier;
		}
	}
}