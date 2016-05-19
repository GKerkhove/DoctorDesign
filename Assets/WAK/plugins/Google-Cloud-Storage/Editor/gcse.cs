using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.models;
using hg.ApiWebKit.apis.google.cloudStorage.models;
using hg.ApiWebKit.apis.google.cloudStorage.operations;
using hg.ApiWebKit.apis.haptix.editor;


namespace hg.ApiWebKit.apis.google.cloudStorage.editor
{
	[HaptixEditorWindowTitle("Cloud Storage")]
	public class gcse : HaptixEditorWindow
	{
		protected override void OnEnable()
		{
			base.OnEnable();
				
			Configuration.SetBaseUri("google.cloud-storage","https://www.googleapis.com/storage/v1");
			Configuration.SetSetting("google.projectId","perfect-tape-656");
				
			FSM.Goto(__tiny__ListBuckets());
		}

		
		IEnumerator __tiny__ListBuckets()
		{
			BucketResource[] buckets = null;
			Dictionary<string,ObjectResource[]> bucketObjects = new Dictionary<string, ObjectResource[]>();
			
			IGoogleOperationInterceptor interceptor = Configuration.Bootstrap().GetComponent<GoogleOperationInterceptor>() as IGoogleOperationInterceptor;
			
			bool[] expandFoldouts = null;
			int i = 0;

			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					GUILayout.BeginVertical();

					if(interceptor != null)
					{
						if(interceptor.PendingOAuthOperation)
						{
							EditorGUILayout.HelpBox("Waiting for a valid Google access token...",MessageType.Warning);
						}
						else if(interceptor.IsExecutingOperations)
						{
							EditorGUILayout.HelpBox("Busy...",MessageType.None);
						}
						else
						{
							EditorGUILayout.HelpBox("Project: " + Configuration.GetSetting<string>("google.projectId"),MessageType.None);
						}
					}
					
					if(buckets!=null)
					{
						if(FSM.CanTransition)
						{
							if(GUILayout.Button("Refresh"))
							{
								FSM.Goto(__tiny__ListBuckets());
							}
							GUILayout.Space(10);
						}

						if(interceptor != null && interceptor.PendingOAuthOperation)
						{
							EditorGUILayout.HelpBox("Waiting for a valid Google access token...",MessageType.Warning);
						}

						if(buckets == null || buckets.Length == 0)
						{
							GUILayout.Label("No buckets found.");
						}
						else
						{
							i = 0;

							foreach(BucketResource bucket in buckets)
							{
								bool expandStateWas = expandFoldouts[i];

								expandFoldouts[i] =  EditorGUILayout.Foldout(expandFoldouts[i], bucket.name);

								// foldout is expanded
								if(expandFoldouts[i])
								{
									if(bucketObjects.ContainsKey(bucket.id))
									{
										if(bucketObjects[bucket.name] == null)
										{
											EditorGUILayout.LabelField("Failed to retrieve objects.  Collapse and expand to refresh.");
										}
										else
										{
											if(bucketObjects[bucket.name].Length == 0)
											{
												EditorGUILayout.LabelField("No objects found.");
											}
											else
											{
												Color og = GUI.color;
												GUI.color = Color.yellow;

												EditorGUILayout.BeginHorizontal();

												EditorGUILayout.LabelField("Name", GUILayout.Width(200));
												EditorGUILayout.LabelField("Size");
												EditorGUILayout.LabelField("Content Type");
												
												EditorGUILayout.EndHorizontal();

												GUI.color = og;

												foreach(ObjectResource o in bucketObjects[bucket.name])
												{
													EditorGUILayout.BeginHorizontal();
													
													if(GUILayout.Button(o.name,GUILayout.Width(200)))
													{
														Application.OpenURL(o.mediaLink);
													}

													EditorGUILayout.LabelField("[" +o.size + " bytes]");
													EditorGUILayout.LabelField(o.contentType);

													EditorGUILayout.EndHorizontal();
												}
											}
										}
									}
									else
									{
										// foldout was just expanded
										if(!expandStateWas && expandFoldouts[i])
										{
											//TODO closure issue with 'bucket.name'
											new operations.ListBucketObjects { bucketName = bucket.name }.ToGoogle(
												new System.Action<operations.ListBucketObjects, HttpResponse> ((operation,response) => {
													if(bucketObjects.ContainsKey(operation.InParameters[0]))
													{
														bucketObjects[operation.InParameters[0]]=((operation.Objects.items==null) ? new ObjectResource[0] : operation.Objects.items);
													}
													else
													{
														bucketObjects.Add(operation.InParameters[0], (operation.Objects.items==null) ? new ObjectResource[0] : operation.Objects.items);
													}
												}),
												new System.Action<operations.ListBucketObjects, HttpResponse> ((operation,response) => {
												if(bucketObjects.ContainsKey(operation.InParameters[0]))
												    {
														bucketObjects[operation.InParameters[0]] = null;
													}
													else
													{
														bucketObjects.Add(operation.InParameters[0],null);
													}
												}),
												new System.Action<operations.ListBucketObjects, HttpResponse> ((operation,response) => {
													
												}),
												bucket.name
											);
										}

										EditorGUILayout.LabelField("Wait...");
									}
								}
								// foldout is collapsed
								else
								{
									// foldout was just now collapsed
									if(expandStateWas && !expandFoldouts[i])
									{
										bucketObjects.Remove(bucket.name);
									}
								}

								GUILayout.Space(15);

								i++;
							}
						}
					}
					
					
					GUILayout.EndVertical();
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
				new ListBuckets().ToGoogle(
					new System.Action<ListBuckets, HttpResponse> ((operation,response) => {
						expandFoldouts = new bool[operation.Buckets.items.Length];
						buckets = operation.Buckets.items;
					}),
					new System.Action<ListBuckets, HttpResponse> ((operation,response) => {
						buckets = null;
					}),
					new System.Action<ListBuckets, HttpResponse> ((operation,response) => {
						
					})
				);
			}
			
			UPDATE_STATE:
			while(!FSM.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			
			}
			
			FSM.CurrentStateCompleted();
			yield return null;
		}
	}
}