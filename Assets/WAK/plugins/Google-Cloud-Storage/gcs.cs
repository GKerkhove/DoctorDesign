using UnityEngine;
using System.Collections;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.tinyfsm;

namespace hg.ApiWebKit.apis.google.cloudStorage
{
	public class gcs : MonoBehaviour 
	{
		TinyStateMachine _fsm;
	
		void Start () 
		{
			_fsm = new TinyStateMachine(null);
			_fsm.Goto(__tiny__Main());
		}

		models.BucketResources _buckets = null;
		models.BucketResource _currentBucket = null;
		models.ObjectResource[] _itemsInCurrentBucket = null;

		public void Update()
		{
			_fsm.Update();
		}
		
		public void OnGUI()
		{
			_fsm.OnGUI();
		}
	
		
		IEnumerator __tiny__Main()
		{
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					GUILayout.BeginVertical();
					
					if(_fsm.CanTransition)
					{
						if(GUILayout.Button("List Buckets"))
						{
							_fsm.Goto(__tiny__ListBuckets());
						}
					}
					
					GUILayout.EndVertical();
				});
			}
			
		ENTER_STATE:
			{
				////Debug.Log ("____ enter MAIN");
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update MAIN");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				////Debug.Log ("____ exit MAIN");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		IEnumerator __tiny__ListBuckets()
		{
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					GUILayout.BeginVertical();

					if(_buckets==null)
					{
						GUILayout.Label("Wait...");
					}
					else
					{
						if(_fsm.CanTransition)
						{
							if(GUILayout.Button("Refresh"))
							{
								_fsm.Goto(__tiny__ListBuckets());
							}
							GUILayout.Space(35);
						}
					}

					if(_buckets!=null)
					{
						if(_buckets.items == null || _buckets.items.Length == 0)
						{
							GUILayout.Label("No buckets found.");
						}
						else
						{
							foreach(models.BucketResource bucket in _buckets.items)
							{
								GUILayout.BeginHorizontal();

								GUILayout.Label(bucket.name);

								if(_fsm.CanTransition)
								{
									if(GUILayout.Button("View"))
									{
										_currentBucket = bucket;
										_fsm.Goto(__tiny__ListObjects());
									}
								}

								GUILayout.EndHorizontal();
								GUILayout.Space(15);
							}
						}
					}
					
					
					GUILayout.EndVertical();
				});
			}
			
		ENTER_STATE:
			{
				////Debug.Log ("____ enter LIST_BUCKETS");

				_buckets = null;
				_currentBucket = null;

				new operations.ListBuckets().ToGoogle(
					new System.Action<operations.ListBuckets, HttpResponse> ((operation,response) => {
						_buckets = operation.Buckets;
					}),
					new System.Action<operations.ListBuckets, HttpResponse> ((operation,response) => {
						_buckets = null;
					}),
					new System.Action<operations.ListBuckets, HttpResponse> ((operation,response) => {

					})
				);
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update LIST_BUCKETS");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				////Debug.Log ("____ exit LIST_BUCKETS");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		IEnumerator __tiny__ListObjects()
		{
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					GUILayout.BeginVertical();
					
					if(_fsm.CanTransition)
					{
						if(GUILayout.Button("Refresh"))
						{
							_fsm.Goto(__tiny__ListObjects());
						}
						GUILayout.Space(35);
					}
					
					if(_itemsInCurrentBucket!=null)
					{
						foreach(models.ObjectResource o in _itemsInCurrentBucket)
						{
							GUILayout.BeginHorizontal();
							
							GUILayout.Label(o.name);

							if(GUILayout.Button("Download"))
							{

							}

							GUILayout.EndHorizontal();

							GUILayout.BeginVertical();

							GUILayout.Label("Size: " + o.size + " bytes");
							GUILayout.Label("Mime: " + o.contentType);

							GUILayout.EndVertical();

							GUILayout.Space(15);
						}
					}
					
					
					GUILayout.EndVertical();
				});
			}
			
		ENTER_STATE:
			{
				////Debug.Log ("____ enter LIST_OBJECTS");

				_itemsInCurrentBucket = null;

				new operations.ListBucketObjects { bucketName = _currentBucket.name }.ToGoogle(
					new System.Action<operations.ListBucketObjects, HttpResponse> ((operation,response) => {
						_itemsInCurrentBucket = operation.Objects.items;
					}),
					new System.Action<operations.ListBucketObjects, HttpResponse> ((operation,response) => {

					}),
					new System.Action<operations.ListBucketObjects, HttpResponse> ((operation,response) => {
					
					})
				);
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update LIST_OBJECTS");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				////Debug.Log ("____ exit LIST_OBJECTS");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}
	}
}