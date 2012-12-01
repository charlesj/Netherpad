namespace Workflows
{
	using System.Collections.Generic;
	using System.Linq;

	public class WorkflowResult
	{
		public WorkflowResult()
		{
			this.Results = new List<WorkflowStepResult>();
		}

		

		public List<WorkflowStepResult> Results { get; set; } 

		public bool TotalSuccess
		{
			get
			{
				return this.Results.All(r => r.Success);
			}
		}

		public void Add(WorkflowStepResult result)
		{
			this.Results.Add(result);
		}

		public List<string> DescribeResults()
		{
			return this.Results.Select(result => result.PostWorkDescription).ToList();
		}
	}
}