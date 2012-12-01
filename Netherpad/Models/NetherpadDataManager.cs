using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Models
{
	using System.Reflection;
	using System.Transactions;

	using Workflows;

	public class NetherpadDataManager : IWorkflowDataContext
	{
		private readonly NetherpadEntities context;

		private TransactionScope transaction;

		public NetherpadDataManager(NetherpadEntities context)
		{
			this.context = context;
			this.DocumentLocator = new DocumentLocator(context);
		}

		public NetherpadEntities Context
		{
			get
			{
				return this.context;
			}
		}

		public DocumentLocator DocumentLocator { get; private set; }


		public void StartTransaction()
		{
			if (transaction != null)
			{
				throw new Exception("You should not start a transaction within a transaction");
			}
			this.transaction = new TransactionScope();
		}

		public void Rollback()
		{
			if (this.transaction == null)
			{
				throw new Exception("Attempted to rollback a non-existant transaction");
			}
			this.transaction.Dispose();
		}

		public void Commit()
		{
			if (this.transaction == null)
			{
				throw new Exception("Attempted to commit a non-existant transaction");
			}
			this.context.SaveChanges();
			this.transaction.Complete();
			this.transaction.Dispose();
		}
	}
}