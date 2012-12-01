namespace Workflows
{
	public abstract class WorkflowStepResult
	{
		protected WorkflowStepResult(bool success, string workDescription, string errorMessage)
		{
			Success = success;
			PostWorkDescription = workDescription;
			ErrorMessage = errorMessage;
		}

		public bool Success { get; private set; }
		public string PostWorkDescription { get; private set; }
		public string ErrorMessage { get; private set; }
	}
}