using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflows
{
	public interface IWorkflowDataContext
	{
		void StartTransaction();

		void Rollback();

		void Commit();
	}
}
