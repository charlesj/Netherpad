using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Models
{
	public class DocumentLocator : LocatorBase
	{
		public DocumentLocator(NetherpadEntities context)
			: base(context)
		{
		}

		public Document Find(string identifier)
		{
			return
				this.Context.Documents.Where(doc => doc.Identifier == identifier)
				    .OrderByDescending(doc => doc.Version)
				    .FirstOrDefault();
		}

		public Document Find(int documentId)
		{
			return this.Context.Documents.SingleOrDefault(doc => doc.DocumentId == documentId);
		}

		public List<Document> FindPreviousVersions(Document document)
		{
			var rtn = new List<Document>();
			if (document.ParentId != 0)
			{
				var parent = this.Find(document.ParentId);
				rtn.Add(parent);
				if (document.ParentId != 0)
				{
					rtn = rtn.Concat(this.FindPreviousVersions(parent)).ToList();
				}
			}

			return rtn;
		}


	}
}