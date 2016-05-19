using UnityEngine;

// from: http://wiki.unity3d.com/index.php/Singleton

namespace hg.ApiWebKit.apis
{
	/// <summary>
	/// Be aware this will not prevent a non singleton constructor
	///   such as `T myT = new T();`
	/// To prevent that, add `protected T () {}` to your singleton class.
	/// 
	/// As a note, this is made as MonoBehaviour because we need Coroutines.
	/// </summary>
	public abstract class Singleton<T> : SingletonBehavior where T : SingletonBehavior
	{
		private static T _instance;
		
		private static object _lock = new object();
		
		public static T Instance
		{
			get
			{
				if (!Application.isEditor &&  applicationIsQuitting) {
					Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +
					                 "' already destroyed on application quit." +
					                 " Won't create again - returning null.");
					return null;
				}
				
				lock(_lock)
				{
					if (_instance == null)
					{
						_instance = (T) FindObjectOfType(typeof(T));
						
						if ( FindObjectsOfType(typeof(T)).Length > 1 )
						{
							Debug.LogError("[Singleton] Something went really wrong " +
							               " - there should never be more than 1 singleton!" +
							               " Reopenning the scene might fix it.");
							return _instance;
						}
						
						if (_instance == null)
						{
							//hg remove
							//GameObject singleton = new GameObject();

							//hg add
							GameObject singleton = Configuration.Bootstrap();

							_instance = singleton.AddComponent<T>();

							//hg add
							_instance.InstanceCreated(singleton);

							//hg remove
							/*singleton.name = "(singleton) "+ typeof(T).ToString();
							DontDestroyOnLoad(singleton);
							Debug.Log("[Singleton] An instance of " + typeof(T) + 
							          " is needed in the scene, so '" + singleton +
							          "' was created with DontDestroyOnLoad.");*/
						} else {
							Debug.Log("[Singleton] Using instance already created: " +
							          _instance.gameObject.name);
						}
					}

					//_instance.InstanceAccessed();
					return _instance;
				}
			}
		}
		
		private static bool applicationIsQuitting = false;
		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		public void OnDestroy () {
			applicationIsQuitting = true;
		}
	}
}

