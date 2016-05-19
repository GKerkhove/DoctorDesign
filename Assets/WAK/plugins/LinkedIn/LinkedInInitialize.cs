using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.linkedin
{
	public class LinkedInInitialize : ApiWebKitInitialize 
	{
		public bool LogVerbose = false;
		public bool LogInformation = true;
		public bool LogWarning = true;
		public bool LogError = true;

		public string BaseUri = "https://api.linkedin.com/v1";
		public string AccessToken = "";

		public string ProfileFieldSelectors = 		":(id,first-name,last-name,maiden-name,formatted-name,phonetic-first-name,phonetic-last-name,formatted-phonetic-name,headline,location:(name,country:(code)),industry,distance,current-share,num-connections,num-connections-capped,summary,specialties,positions:(company:(id,name,type,size,industry,ticker)),picture-url,site-standard-profile-request,api-standard-profile-request,public-profile-url,email-address,connections,phone-numbers)";
		public string ConnectionsFieldSelectors = 	":(id,first-name,last-name,maiden-name,formatted-name,phonetic-first-name,phonetic-last-name,formatted-phonetic-name,headline,location:(name,country:(code)),industry,distance,current-share,num-connections,num-connections-capped,summary,specialties,positions:(company:(id,name,type,size,industry,ticker)),picture-url,site-standard-profile-request,api-standard-profile-request,public-profile-url,email-address,connections,phone-numbers)";
		public string CompanyFieldSelectors = 		":(id,name,universal-name,email-domains,company-type,ticker,website-url,status,logo-url,square-logo-url,blog-rss-url,twitter-id,employee-count-range,industry,specialties,num-followers)";
		public string JobFieldSelectors = 			":(jobs:(id,customer-job-code,active,posting-date,expiration-date,posting-timestamp,expiration-timestamp,company:(id,name),position:(title,location,job-functions,industries,job-type,experience-level),skills-and-experience,description-snippet,description,salary,job-poster:(id,first-name,last-name,headline),referral-bonus,site-job-url,location-description))";

		public override void Start()
		{
			Configuration.SetSetting("log-VERBOSE", LogVerbose);
			Configuration.SetSetting("log-INFO", LogInformation);
			Configuration.SetSetting("log-WARNING", LogWarning);
			Configuration.SetSetting("log-ERROR", LogError);

			Configuration.SetBaseUri("linkedin",BaseUri);
			Configuration.SetSetting("linkedin.accesstoken", AccessToken);
			Configuration.SetSetting("linkedin.dataformat", "json");

			Configuration.SetSetting("linkedin.profile.fieldSelectors", ProfileFieldSelectors); 
			Configuration.SetSetting("linkedin.connections.fieldSelectors", ConnectionsFieldSelectors); 
			Configuration.SetSetting("linkedin.company.fieldSelectors", CompanyFieldSelectors); 
			Configuration.SetSetting("linkedin.jobs.fieldSelectors", JobFieldSelectors); 

			base.Start();
		}
	}
}
