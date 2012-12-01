

namespace Workflows
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class WorkflowStepWrapper
	{
		public IWorkflowStep WorkflowStep { get; set; }

		public int StepNumber { get; set; }

		public MethodInfo ExecutionMethod { get; set; }

		public ParameterInfo[] ExecutionParameters;

		public object[] BuildArgs(List<Tuple<string, Type, object>> availableParams)
		{
			var args = new List<object>();
			foreach (var param in this.ExecutionParameters)
			{
				var tuple = availableParams.SingleOrDefault(t => (String.Compare(t.Item1, param.Name, StringComparison.OrdinalIgnoreCase) == 0) && t.Item2 == param.ParameterType);
				if (tuple == null)
				{
					string message = string.Format("When attempting to retrieve a value for {0} on type {1}, it could not be located", param.Name, WorkflowStep.GetType());
					throw new WorkflowExecutionExeception(message);
				}
				args.Add(tuple.Item3);
			}

			return args.ToArray();
		}
	}
}
