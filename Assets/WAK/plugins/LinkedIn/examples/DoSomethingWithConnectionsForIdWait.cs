using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithConnectionsForIdWait : MonoBehaviour
	{
		public behaviors.LinkedInConnectionsForIdBehavior ConnectionsBehavior;

		[SerializeField]
		private models.Connections _connections;

		[SerializeField]
		private string _profileId;

		IEnumerator Start()
		{
			Configuration.Log("ConnectionsForId Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			ConnectionsBehavior.ProfileId = _profileId;
			yield return StartCoroutine(ConnectionsBehavior.ExecuteAndWait());
			_connections = ConnectionsBehavior.Connections;

			Configuration.Log("ConnectionsForId call is done with status : " + ConnectionsBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("ConnectionsForId " + _connections, LogSeverity.VERBOSE);

			yield break;
		}
	}
}
