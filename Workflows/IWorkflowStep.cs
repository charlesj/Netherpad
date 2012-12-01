namespace Workflows
{
	public interface IWorkflowStep
	{
		string PreWorkDescription { get; }

		string PostWorkDescription { get; }
	}
}