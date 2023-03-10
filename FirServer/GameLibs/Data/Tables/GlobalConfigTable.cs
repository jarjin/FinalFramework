using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace FirCommon.Data
{
	[ProtoContract]
	public class GlobalConfigTable
	{
		[ProtoMember(1)]
		public string name;		
		[ProtoMember(2)]
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

	[ProtoContract]
	public class GlobalConfigTableItem
	{
    	[ProtoMember(1)]
    	public string id;
    	[ProtoMember(2)]
    	public string value;
	}
}