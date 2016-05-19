using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInMyConnectionsBehavior : ApiBehavior<LinkedInMyConnectionsBehavior>
	{
		[NullifyOnQueryAttribute]
		public models.Connections Connections;

		public string Start = null;
		public string Count = null;
		public string Modified = null;
		public string ModifiedSince = null;

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.GetMyConnections(
				new HttpCallbacks<operations.GetConnections> {
				done = new Action<operations.GetConnections, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.SUCCESS;
					}),
				fail = new Action<operations.GetConnections, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.FAILURE;
					}),
				always = new Action<operations.GetConnections, HttpResponse>
					((operation, response) => 
					 { 
						Connections = operation.connections;
						
						OnCompletion(operation, response, LinkedInApiMonitor.Instance);
					})
				},
				Start,
				Count,
				Modified,
				ModifiedSince
			);
		}
	}
}
