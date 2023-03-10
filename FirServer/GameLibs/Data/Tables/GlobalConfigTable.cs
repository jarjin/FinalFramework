using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirCommon.Data
{
	public class GlobalConfigTable
	{
		public string name;		
		private List<GlobalConfigTableItem> items = new List<GlobalConfigTableItem>();
		
		private Dictionary<string, GlobalConfigTableItem> dics = null;

		public List<GlobalConfigTableItem> Items
		{
			get {
				return items;
			}
		}

		public void Initialize()
		{
			dics = new Dictionary<string, GlobalConfigTableItem>();
			foreach (GlobalConfigTableItem item in items)
			{
				dics.Add(item.id, item);
			}
		}

		public List<GlobalConfigTableItem> GetItems()
		{
			return items;
		}

		public void AddItem(GlobalConfigTableItem item)
		{
			items.Add(item);
		}

		public GlobalConfigTableItem GetItemByKey(string key)
		{
			GlobalConfigTableItem item = null;
			if (dics.ContainsKey(key))
			{
				dics.TryGetValue(key, out item);
			}
			return item;
		}
	}

	public class GlobalConfigTableItem
	{
    	public string id;
    	public string value;
	}
}