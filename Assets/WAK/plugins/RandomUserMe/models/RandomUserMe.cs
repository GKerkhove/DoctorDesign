using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.randomuserme.models
{
	public class Results 
	{
		public Result[] results;
	}
		
	public class Result
	{
		public User user;
		public string seed;
		public string version;
	}

	public class User
	{
		public string gender;
		public Name name;
		public Location location;
		public string email;
		public string username;
		public string password;
		public string salt;
		public string md5;
		public string sha1;
		public string sha256;
		public int registered;
		public int dob;
		public string phone;
		public string cell;
		public string SSN;
		public Picture picture;
	}
	
	public class Picture
	{
		public string large;
		public string medium;
		public string thumbnail;
	}

	public class Name
	{
		public string title;
		public string first;
		public string last;
	}

	public class Location
	{
		public string street;
		public string city;
		public string state;
		public int zip;
	}
}


