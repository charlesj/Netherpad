using System;
using System.Linq;

namespace Netherpad.Models.NetherpadWorkflows.Steps.GenerateUniqueIdentifier
{
	using System.Text;

	using Workflows;

	public class GenerateUniqueIdentifierStep : IWorkflowStep
	{
		private readonly NetherpadDataManager db;

		private string identifier;

		public GenerateUniqueIdentifierStep(NetherpadDataManager db)
		{
			this.db = db;
		}

		public GenerateUniqueIdentifierStepResult Run()
		{
			var identifierLength = 7;
			var attemptCount = 0;
			var identifierCandidate = this.GenerateRandomString(identifierLength);

			while(db.Context.Documents.Count(doc => doc.Identifier == identifierCandidate) > 0)
			{
				if (attemptCount > 10)
				{
					identifierLength++;
					attemptCount = 0;
				}
				
				identifierCandidate = identifierLength > 10 ? Guid.NewGuid().ToString() : this.GenerateRandomString(identifierLength);
				
				attemptCount++;
			}

			this.identifier =  identifierCandidate;
			return new GenerateUniqueIdentifierStepResult(true, this.PostWorkDescription, string.Empty, this.identifier);
		}

		public string PreWorkDescription { get
		{
			return string.Format("A unique identifer will be created");
		} }

		public string PostWorkDescription
		{
			get
			{
				return string.Format("A unique identifer of {0} was created", this.identifier);
			}
		}

		private string GenerateRandomString(int length)
		{
			var eligibleValues = "abcdefghijklmnopqrstuvwxyz1234567890";
			var rnd = new Random(DateTime.Now.Millisecond);
			StringBuilder sb = new StringBuilder();
			
			for (int i = 0; i < length; i++)
			{
				sb.Append(eligibleValues[rnd.Next(eligibleValues.Length - 1)]);
			}
			return sb.ToString();
		}
	}
}