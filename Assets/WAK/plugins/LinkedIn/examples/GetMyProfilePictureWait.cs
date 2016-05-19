using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class GetMyProfilePictureWait : MonoBehaviour
	{
		public behaviors.LinkedInMyProfileBehavior ProfileBehavior;
		public behaviors.LinkedInRetrieveImageBehavior ImageBehavior;

		[SerializeField]
		private models.PersonList _profiles;

		[SerializeField]
		private Texture2D _image;

		[SerializeField]
		private Renderer _assignImageTo;

		IEnumerator Start()
		{
			Configuration.Log("MyPicture Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			Configuration.Log("Retrieving Profile", LogSeverity.VERBOSE);

			yield return StartCoroutine(ProfileBehavior.ExecuteAndWait());
			_profiles = ProfileBehavior.Profiles;

			if(ProfileBehavior.Status == ApiBehaviorStatus.SUCCESS)
			{
				Configuration.Log("Retrieving Image", LogSeverity.VERBOSE);

				ImageBehavior.AbsoluteUri = _profiles.values[0].pictureUrl;
				yield return StartCoroutine(ImageBehavior.ExecuteAndWait());

				if(ImageBehavior.Status == ApiBehaviorStatus.SUCCESS)
				{
					_image = ImageBehavior.Image;

					if(_assignImageTo)
						_assignImageTo.material.mainTexture = _image;

					Configuration.Log("Is that you?", LogSeverity.VERBOSE);
				}
				else
					Configuration.Log("Can't retrieve image ! ", LogSeverity.VERBOSE);
			}

			yield break;
		}
	}
}
