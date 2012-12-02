using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netherpad.Hubs
{
	using Microsoft.AspNet.SignalR.Hubs;

	[HubName("documentShare")]
	public class DocumentShare : Hub
	{
		public void PushTransform(string transform)
		{
			if (transform.Contains("join_session"))
			{
				Clients.All.addTransform(transform.Replace("join_session", "assign_uid'"));
				Clients.All.addTransform(transform.Replace("join_session", "sync_end'"));
			}
			else
			{
				Clients.All.addTransform(transform);	
			}
			
		}

	}
}