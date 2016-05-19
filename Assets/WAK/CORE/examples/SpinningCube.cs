using UnityEngine;

namespace hg.ApiWebKit.apis.example
{
	
	public class SpinningCube: MonoBehaviour
	{
		public float speed = 10f;
		
		
		void Update ()
		{
			transform.Rotate(Vector3.up, speed * Time.deltaTime);
		}
	}
}
