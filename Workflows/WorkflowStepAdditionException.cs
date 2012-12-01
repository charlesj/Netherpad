namespace Workflows
{
	using System;

	public class WorkflowStepAdditionException : Exception
	{
		public WorkflowStepAdditionException(string message)
			: base(message)
		{
			
		}
	}
}