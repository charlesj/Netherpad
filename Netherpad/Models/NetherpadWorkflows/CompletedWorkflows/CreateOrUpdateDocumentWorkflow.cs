using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Models.NetherpadWorkflows.CompletedWorkflows
{
	using Netherpad.Models.NetherpadWorkflows.Steps.CreateNewDocument;
	using Netherpad.Models.NetherpadWorkflows.Steps.GenerateUniqueIdentifier;
	using Netherpad.Models.NetherpadWorkflows.Steps.UpdateDocument;

	using Workflows;

	public class CreateOrUpdateDocumentWorkflow : WorkflowBase
	{
		public CreateOrUpdateDocumentWorkflow(NetherpadDataManager context, Document document, string content)
			: base(context)
		{
			if (string.IsNullOrEmpty(document.Identifier))
			{
				this.Add(new GenerateUniqueIdentifierStep(context), 1);
				this.Add(new CreateNewDocumentStep(context, document), 2);
			}
			else
			{
				this.Add(new UpdateDocumentStep(context, document, content), 1);
			}
		}
	}
}