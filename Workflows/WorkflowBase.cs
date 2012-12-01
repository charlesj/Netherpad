namespace Workflows
{
	using System;
	using System.Collections.Generic;
	using System.Linq;


	public abstract class WorkflowBase
	{
		private readonly IWorkflowDataContext context;

		private readonly List<WorkflowStepWrapper> steps;
		// string is the paramater name, type is the type of the parameter and int is the step number, object is the value
		private readonly List<Tuple<string, Type, int>> availableParams; 
		private readonly List<Tuple<string, Type, object>> paramsWithValues;

		protected WorkflowBase(IWorkflowDataContext context)
		{
			this.context = context;
			this.steps = new List<WorkflowStepWrapper>();
			
			this.availableParams = new List<Tuple<string, Type, int>>();
			this.paramsWithValues = new List<Tuple<string, Type, object>>();

		}

		/// <summary>
		/// Adds a step to the workflow.  
		/// </summary>
		/// <param name="step">The step to add.  Must contain a method called "run" which returns a type derived from WorkflowStepResult.</param>
		/// <param name="stepNumber">The step number</param>
		protected void Add(IWorkflowStep step, int stepNumber)
		{
			var type = step.GetType();
			// get the method called 'run'
			var runMethod = type.GetMethod("Run");
			// get the return type of the method
			var returnType = runMethod.ReturnType;
			// insure that the type it returns derives from workflowsStepResult
			if (returnType.BaseType != typeof(WorkflowStepResult))
			{
				throw new WorkflowStepAdditionException("Attempted to add a workflow step that did not return a type derived from WorkflowStepBase");
			}

			var args = runMethod.GetParameters();
			// if there are args (there might not be any)
			// see if there are any available types that have the same name (ignoring case) and same type.  There should only ever be exactly 1
			if (args.Any() && args.Any(arg => availableParams.Count(at => String.Compare(arg.Name, at.Item1, StringComparison.OrdinalIgnoreCase) == 0 && arg.ParameterType == at.Item2 && at.Item3 <= stepNumber) != 1))
			{
				throw new WorkflowStepAdditionException("Attempted to add a workflow step that would be unable to execute.");
			}

			// setup the available args
			// get the returnType properties, in order to filter them out.
			var unusableTypeList = this.GetWorkflowStepResultPropertyBlacklist();
			var returnTypeProps = returnType.GetProperties().ToList().Where(p => !unusableTypeList.Contains(p.Name)).ToList();

			foreach (var returnTypeProp in returnTypeProps)
			{
				// must add one to the available step, because otherwise, it would be passing something to itself, which wouldn't be correct;
				// the value will be null to start with.
				availableParams.Add(Tuple.Create(returnTypeProp.Name, returnTypeProp.PropertyType, stepNumber));
			}

			this.steps.Add(new WorkflowStepWrapper { ExecutionMethod = runMethod, ExecutionParameters = args, StepNumber = stepNumber, WorkflowStep = step});

		}

		public WorkflowResult Execute()
		{
			var results = new WorkflowResult();
			this.context.StartTransaction();
			foreach (var step in this.steps)
			{
				// run the step.
				var resultObj = step.ExecutionMethod.Invoke(step.WorkflowStep, step.BuildArgs(paramsWithValues));
				if (resultObj is WorkflowStepResult)
				{
					// extract chainable values
					this.ExtractAvailableParametersFromResult(resultObj, step.StepNumber);
					// add to result set
					var stepResult = resultObj as WorkflowStepResult;
					// add the step to the results
					results.Results.Add(stepResult);
					// if the step was not sucessful, break from the running
					// and rollback the transaction
					if (!stepResult.Success)
					{
						this.context.Rollback();
						break;
					}
				}
			}

			// if they were all successful, commit the transaction.
			if (results.TotalSuccess)
			{
				this.context.Commit();
			}

			return results;
		}

		public List<string> GetPreWorkDescription()
		{
			return this.steps.Select(step => step.WorkflowStep.PreWorkDescription).ToList();
		}

		private void ExtractAvailableParametersFromResult(object resultObj, int step)
		{
			var type = resultObj.GetType();

			var unusableTypeList = this.GetWorkflowStepResultPropertyBlacklist();
			var props = type.GetProperties().ToList().Where(p => !unusableTypeList.Contains(p.Name)).ToList();

			foreach (var prop in props)
			{
				// get the availableParameter tuple
				var tuple = this.availableParams.SingleOrDefault(t => (String.Compare(t.Item1, prop.Name, StringComparison.OrdinalIgnoreCase) == 0) && t.Item2 == prop.PropertyType && t.Item3 == step);
				if (tuple == null)
				{
					string message = string.Format("Could not locate tuple while attempting to add the value {0} (from {1})", prop.Name, type.Name);
					throw new WorkflowExecutionExeception(message);
				}

				try
				{
					var value = prop.GetValue(resultObj);
					this.paramsWithValues.Add(Tuple.Create(tuple.Item1, tuple.Item2, value));
				}
				catch (Exception)
				{
					string message = string.Format("While attempting to add {0} to the available values, it could not be located on type {1}", prop.Name, type.Name);
					throw new WorkflowExecutionExeception(message);
				}

			}
		}

		private List<string> GetWorkflowStepResultPropertyBlacklist()
		{
			var type = typeof(WorkflowStepResult);
			return type.GetProperties().Select(p => p.Name).ToList();
		}
	}
}