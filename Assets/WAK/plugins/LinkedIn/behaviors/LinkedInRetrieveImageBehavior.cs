using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInRetrieveImageBehavior : ApiBehavior<LinkedInRetrieveImageBehavior>
	{
		public string AbsoluteUri = null;

		public Texture2D Image = null;

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.GetImage(
				AbsoluteUri,
				new HttpCallbacks<operations.GetImage> {
					done = new Action<operations.GetImage, HttpResponse>
						((operation, response) => 
						 { 
							Status = ApiBehaviorStatus.SUCCESS;
						}),
					fail = new Action<operations.GetImage, HttpResponse>
						((operation, response) => 
						 { 
							Status = ApiBehaviorStatus.FAILURE;
						}),
					always = new Action<operations.GetImage, HttpResponse>
						((operation, response) => 
						 { 
							Image = operation.ImageTexture;

							OnCompletion(operation, response, LinkedInApiMonitor.Instance);
						})
				}
			);
		}
	}
}


