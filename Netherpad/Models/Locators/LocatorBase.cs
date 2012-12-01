using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Models
{
	using System.Data.Entity;

	public abstract class LocatorBase
	{
		protected readonly NetherpadEntities Context;

		protected LocatorBase(NetherpadEntities context)
		{
			this.Context = context;
		}

	}
}