namespace Workflows
{
	using System;

	public class WorkflowExecutionExeception : Exception
	{
		public WorkflowExecutionExeception(string message) : base(message)
		{
			
		}
	}
}