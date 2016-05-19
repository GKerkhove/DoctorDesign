using hg.LitJson;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hg.ApiWebKit.apis.zebra.models
{
	public class ZebraConfigurations
	{
		Dictionary<string, ZebraConfigurationCategory> _categories = new Dictionary<string, ZebraConfigurationCategory>(StringComparer.OrdinalIgnoreCase);

		Dictionary<string, ZebraConfigurationPoint> _configurations = new Dictionary<string, ZebraConfigurationPoint>(StringComparer.OrdinalIgnoreCase);


		public int Load()
		{
			int counter = 0;

			try
			{
				string allconfig = (Resources.Load("allconfig") as TextAsset).text;
				JsonData _allConfigJsonData = JsonMapper.ToObject(allconfig);

				/*_allConfigJsonData["allconfig"] = 
					from pair in _allConfigJsonData["allconfig"]
					orderby pair.Value ascending
					select pair;*/

				foreach(DictionaryEntry de in _allConfigJsonData["allconfig"])
				{
					ZebraConfigurationCategory category = addCategory(de.Key.ToString());
					addConfiguration(category, de.Key.ToString(), de.Value as JsonData);
					counter++;
				}

				Debug.Log ("ZebraConfigurations.Load processed " + counter + " configurations.");

				return counter;
			}
			catch(Exception ex)
			{
				Debug.LogError("ZebraConfigurations.Load failed : " + ex.Message);
				return -1;
			}
		}

		private ZebraConfigurationCategory addCategory(string configurationPointName)
		{
			string[] path = configurationPointName.Split(new[]{'.'},StringSplitOptions.RemoveEmptyEntries);

			ZebraConfigurationCategory lastParent = null;

			string fullName = "";

			for(int i = 0; i < path.Length - 1; i++)
			{
				fullName += path[i] + '.';

				var existingCategory = _categories.Where(x => x.Value.Name == path[i]).Select(x => x.Value).FirstOrDefault();

				if(existingCategory==null)
				{
					string id = Guid.NewGuid().ToString();
					_categories.Add(id, lastParent = new ZebraConfigurationCategory(id, fullName, path[i], lastParent));
				}
				else
				{
					lastParent = existingCategory;
				}
			}

			return lastParent;
		}

		private void addConfiguration(ZebraConfigurationCategory category, string name, JsonData jsonConfiguration)
		{
			_configurations.Add (name, new ZebraConfigurationPoint(category, name, jsonConfiguration));
		}

		public Dictionary<string, ZebraConfigurationPoint> AllWriteablePoints()
		{
			return _configurations
				.Where(x => x.Value.CanWrite)
				.Select(x => x)
				.ToDictionary(x => x.Key, x => x.Value);
		}

		public Dictionary<string, ZebraConfigurationPoint> AllReadablePoints()
		{
			return _configurations
				.Where(x => !x.Value.CanWrite)
				.Select(x => x)
				.ToDictionary(x => x.Key, x => x.Value);
		}

		public Dictionary<string, ZebraConfigurationPoint> ConfigurablePoints(string categoryId)
		{
			return _configurations
				.Where(x => x.Value.Category.Id == categoryId)
				.Select(x => x)
				.ToDictionary(x => x.Key, x => x.Value);
		}

		public Dictionary<string, ZebraConfigurationCategory> TopLevelCategories()
		{ 
			return _categories
				.Where(x => x.Value.IsTopLevel)
				.Select (x => x)
				.ToDictionary(x => x.Key, x => x.Value);
		}

		public Dictionary<string, ZebraConfigurationCategory> ChildCategories(string categoryId)
		{ 
			return _categories
				.Where(x => x.Value.Parent != null && x.Value.Parent.Id == categoryId)
				.Select (x => x)
				.ToDictionary(x => x.Key, x => x.Value);
		}

		public ZebraConfigurationCategory ParentCategory(string categoryId)
		{
			ZebraConfigurationCategory parent = _categories
				.Where(x => x.Value.Id == categoryId)
				.Select(x => x.Value.Parent)
				.FirstOrDefault();

			return parent;
		}

		public ZebraConfigurationCategory Category(string categoryId)
		{
			ZebraConfigurationCategory cat = _categories
				.Where(x => x.Value.Id == categoryId)
				.Select(x => x.Value)
				.FirstOrDefault();
			
			return cat;
		}
	}
}


