using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace FirCommon.Data
{
	[ProtoContract]
	public class ItemTable
	{
		[ProtoMember(1)]
		public string name;		
		[ProtoMember(2)]
		private List<ItemTableItem> items = new List<ItemTableItem>();
		
		private Dictionary<int, ItemTableItem> dics = null;

		public List<ItemTableItem> Items
		{
			get {
				return items;
			}
		}

		public void Initialize()
		{
			dics = new Dictionary<int, ItemTableItem>();
			foreach (ItemTableItem item in items)
			{
				dics.Add(item.id, item);
			}
		}

		public List<ItemTableItem> GetItems()
		{
			return items;
		}

		public void AddItem(ItemTableItem item)
		{
			items.Add(item);
		}

		public ItemTableItem GetItemByKey(int key)
		{
			ItemTableItem item = null;
			if (dics.ContainsKey(key))
			{
				dics.TryGetValue(key, out item);
			}
			return item;
		}
	}

	[ProtoContract]
	public class ItemTableItem
	{
    	[ProtoMember(1)]
    	public int id;
    	[ProtoMember(2)]
    	public string name;
    	[ProtoMember(3)]
    	public int quality;
    	[ProtoMember(4)]
    	public int typeid;
    	[ProtoMember(5)]
    	public string icon;
	}
}