using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.jailbase.models
{
	public class Inmates
	{
		public int status;
		public int next_page;
		public Record[] records;
		public int current_page;
		public int	total_records;
		public string msg;
	}

	public class Record
	{
		public string name;
		public string[] charges;
		public string id;
		public string book_date_formatted;
		public string[][] details;
		public string mugshot;
		public string book_date;
		public string more_info_url;
		public string county_state;
		public string source;
		public string source_id;
	}
	
	public class Sources
	{
		public int status;
		public string msg;
		public SourceRecord[] records;
	}
	
	public class SourceRecord
	{
		public string source_id;
		public string name;
		public string state;
		public string state_full;
		public bool has_mugshots;
	}
}