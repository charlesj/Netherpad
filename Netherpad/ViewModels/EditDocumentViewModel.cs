using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.ViewModels
{
	using Netherpad.Models;

	public class EditDocumentViewModel
	{
		public Document Document { get; set; }

		public List<Document> Versions { get; set; }

	}
}