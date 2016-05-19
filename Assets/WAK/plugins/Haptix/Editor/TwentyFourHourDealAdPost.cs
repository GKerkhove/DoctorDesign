using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.haptix.models;

namespace hg.ApiWebKit.apis.haptix.editor
{
	[HaptixEditorWindowTitle("Post Ad")]
	public class TwentyFourHourDealAdPost : HaptixEditorWindow
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			
			FSM.Goto(__tiny__Main());
		}
		
		private AdvertisementPostModel _adPost = new AdvertisementPostModel();
		
		IEnumerator __tiny__Main()
		{
			if(!_adPost.SubmissionHasError)
				_adPost = new AdvertisementPostModel();
			else 
				_adPost.ResetSubmission();
		
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					
					EditorGUILayout.HelpBox("Your advertisement will be shown along Unity's 24 Hour Deal.  It will be rotated with other advertisements.  Once uploaded, you will receive a confirmation email with further instructions.  Haptix Games Inc staff will be the final approver of your submitted advertisement.",MessageType.None);
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.BeginVertical();
					
					if(_adPost.IsImageValid)
						EditorGUILayout.HelpBox("Valid image selected.",MessageType.Info);
					else
						EditorGUILayout.HelpBox("Select an image that is 80px X 200px and Read/Write is enabled.",MessageType.Error);
					
					_adPost.AdvertisementObject = EditorGUILayout.ObjectField("Image:", _adPost.AdvertisementObject, typeof(Texture2D), true);
					
					EditorGUILayout.Space();
					
					if(_adPost.IsUrlValid)
						EditorGUILayout.HelpBox("Valid URL.",MessageType.Info);
					else
						EditorGUILayout.HelpBox("Enter a valid URL you want to take your audience to.",MessageType.Error);
					
					_adPost.UrlLink = EditorGUILayout.TextField("URL:",_adPost.UrlLink.Trim());
					
					EditorGUILayout.Space();
					
					if(_adPost.IsEmailValid)
						EditorGUILayout.HelpBox("Valid email address.",MessageType.Info);
					else
						EditorGUILayout.HelpBox("Enter a valid email address from which you can manage your advertisement.",MessageType.Error);
					
					_adPost.EmailAddress = EditorGUILayout.TextField("Email:",_adPost.EmailAddress.Trim());
					
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					
					if(!_adPost.IsPostValid) GUI.enabled = false;
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Submit"))
						{
							FSM.Goto(__tiny__Submit());
						}
					}
					
					GUI.enabled = true;
					
					EditorGUILayout.EndVertical();
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
			
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
		
		IEnumerator __tiny__Submit()
		{
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					
					EditorGUILayout.HelpBox("Please wait...",MessageType.Info);
					
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
				new hg.ApiWebKit.apis.haptix.operations.SubmitHostedAd { AdContact = _adPost.EmailAddress, AdLink = _adPost.UrlLink, AdImage = _adPost.AdvertisementImage }
					.Send(
						new Action<hg.ApiWebKit.apis.haptix.operations.SubmitHostedAd, hg.ApiWebKit.core.http.HttpResponse> 
						((operation_postAd, response_postAd) => 
						{ 
							_adPost.SubmissionId = operation_postAd.AdId;
							FSM.Goto(__tiny__SubmitSuccess());
						}),
						new Action<hg.ApiWebKit.apis.haptix.operations.SubmitHostedAd, hg.ApiWebKit.core.http.HttpResponse> 
						((operation_postAd, response_postAd) => 
					 	{ 
							_adPost.SubmissionError = "Submission failed!\n\nHTTP Code: " + response_postAd.StatusCode.ToString() + 
								"\nInternal Error Code: " + operation_postAd.ErrorCode +
								"\nInternal Error Description: " + operation_postAd.ErrorDescription;
							FSM.Goto(__tiny__SubmitFailure());
						}),
						null
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
		
		IEnumerator __tiny__SubmitSuccess()
		{
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					
					EditorGUILayout.HelpBox("Your ad has been submitted.  Please check your email.",MessageType.Info);
					
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("OK"))
						{
							FSM.Goto(__tiny__Main());
						}
					}
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
				
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
		
		IEnumerator __tiny__SubmitFailure()
		{
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					
					EditorGUILayout.HelpBox(_adPost.SubmissionError,MessageType.Error);
					
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Retry"))
						{
							FSM.Goto(__tiny__Main());
						}
					}
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
				
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