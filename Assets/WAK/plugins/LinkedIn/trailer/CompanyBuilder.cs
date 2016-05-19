using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.trailer
{
	public class CompanyBuilder : MonoBehaviour
	{
		public Transform TopLevelApplicationObject;
		public GameObject pfProfile;
		private GameObject _pfProfileInstance;
		public GameObject pfCompany;
		public List<GameObject> _pfCompanyInstances;

		private behaviors.LinkedInMyProfileBehavior _profileBehavior;
		private behaviors.LinkedInMyConnectionsBehavior _connectionsBehavior;
		private behaviors.LinkedInCompanyByIdBehavior _companyBehavior;

		private models.Person _myProfile;
		private IList<models.Person> _myConnectionsWithIdentifiedCompanies;
		public List<float> _companyIds;

		IEnumerator Start()
		{

			_companyBehavior = this.gameObject.AddComponent<behaviors.LinkedInCompanyByIdBehavior>();
			_profileBehavior = this.gameObject.AddComponent<behaviors.LinkedInMyProfileBehavior>();

			yield return StartCoroutine(_profileBehavior.ExecuteAndWait());
			if(_profileBehavior.Status == ApiBehaviorStatus.SUCCESS)
			{
				_myProfile = _profileBehavior.Profiles.values[0];

				_pfProfileInstance = (GameObject) Instantiate(pfProfile,Vector3.zero,Quaternion.identity);
				_pfProfileInstance.name = "MyProfile";
				_pfProfileInstance.transform.SetParent(TopLevelApplicationObject);
				_pfProfileInstance.GetComponent<ImageObject>().FetchImage(_myProfile.pictureUrl);

				_connectionsBehavior = this.gameObject.AddComponent<behaviors.LinkedInMyConnectionsBehavior>();
				yield return StartCoroutine(_connectionsBehavior.ExecuteAndWait());
				if(_connectionsBehavior.Status == ApiBehaviorStatus.SUCCESS)
				{
					_myConnectionsWithIdentifiedCompanies = new List<models.Person>();
					_companyIds = new List<float>();

					foreach(models.Person person in _connectionsBehavior.Connections.values)
					{
						if(person.id != "private" && person.positions._total > 0 && person.positions.values[0].company.id > 0)
						{
							_myConnectionsWithIdentifiedCompanies.Add (person);

							if(!_companyIds.Contains(person.positions.values[0].company.id))
								_companyIds.Add (person.positions.values[0].company.id);
						}
					}

					_pfCompanyInstances = new List<GameObject>();
					Vector3 position = Vector3.zero;
					int column =0;

					position.x = -14;
					position.y = -7;

					int count = 0;

					foreach(float companyId in _companyIds)
					{
						_companyBehavior.CompanyId = companyId.ToString();
						yield return StartCoroutine(_companyBehavior.ExecuteAndWait());
						count++;
						/*
						if(column>=5)
						{
							position.y -= 7;
							position.x = -14;
							column = 0;
						}

						position.x += (column * 6);
						*/
						// code from http://answers.unity3d.com/questions/714835/best-way-to-spawn-prefabs-in-a-circle.html

						Vector3 center = transform.position;
	
						Vector3 pos = RandomCircle(center, 1, 40*count);

						Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
						

						GameObject companyInstance = (GameObject) Instantiate(pfCompany,pos+(new Vector3(0,0,3*count)), rot);
						companyInstance.name = "company-" + companyId.ToString();
						companyInstance.transform.SetParent(TopLevelApplicationObject);

						if(_companyBehavior.Status == ApiBehaviorStatus.SUCCESS)
							companyInstance.GetComponent<ImageObject>().FetchImage(_companyBehavior.Company.logoUrl);

						_pfCompanyInstances.Add(companyInstance);

						column ++;
					}
				}
			}

			yield break;
		}


		Vector3 RandomCircle ( Vector3 center , float radius, float ang )
		{
			//float ang = startAngle+15;//UnityEngine.Random.value * 360;
			Vector3 pos;
			pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
			pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
			pos.z = center.z;
			return pos;
		}

		float cameraZ;

		void Update()
		{
			cameraZ = Camera.main.transform.position.z + 0.02f;
			Camera.main.transform.position = new Vector3(0,0, cameraZ);
		}

	}
}
