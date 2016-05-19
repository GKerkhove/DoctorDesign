using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithMyConnectionsWait : MonoBehaviour
	{
		public behaviors.LinkedInMyConnectionsBehavior ConnectionsBehavior;

		public bool FilterKnownCompanies = false;

		public List<models.Person> _modifiedCollectionOfPeople = new List<models.Person>();

		[SerializeField]
		private models.Connections _connections;

		IEnumerator Start()
		{
			Configuration.Log("MyConnections Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			yield return StartCoroutine(ConnectionsBehavior.ExecuteAndWait());
			_connections = ConnectionsBehavior.Connections;
			applyFilters();

			Configuration.Log("MyConnections call is done with status : " + ConnectionsBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("MyConnections " + _connections, LogSeverity.VERBOSE); 
			
			yield break;
		}

		private void applyFilters()
		{
			if(FilterKnownCompanies)
			{
				foreach(models.Person person in _connections.values)
				{
					if(person.id != "private" && person.positions._total > 0 && person.positions.values[0].company.id > 0)
						_modifiedCollectionOfPeople.Add (person);
				}
			}
		}
	}
}
