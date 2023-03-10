using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace FirCommon.Data
{
	[ProtoContract]
	public class ObjectPoolTable
	{
		[ProtoMember(1)]
		public string name;		
		[ProtoMember(2)]
		private List<ObjectPoolTableItem> items = new List<ObjectPoolTableItem>();
		
		private Dictionary<int, ObjectPoolTableItem> dics = null;

		public List<ObjectPoolTableItem> Items
		{
			get {
				return items;
			}
		}

		public void Initialize()
		{
			dics = new Dictionary<int, ObjectPoolTableItem>();
			foreach (ObjectPoolTableItem item in items)
			{
				dics.Add(item.id, item);
			}
		}

		public List<ObjectPoolTableItem> GetItems()
		{
			return items;
		}

		public void AddItem(ObjectPoolTableItem item)
		{
			items.Add(item);
		}

		public ObjectPoolTableItem GetItemByKey(int key)
		{
			ObjectPoolTableItem item = null;
			if (dics.ContainsKey(key))
			{
				dics.TryGetValue(key, out item);
			}
			return item;
		}
	}

	[ProtoContract]
	public class ObjectPoolTableItem
	{
    	[ProtoMember(1)]
    	public int id;
    	[ProtoMember(2)]
    	public string name;
    	[ProtoMember(3)]
    	public int min;
    	[ProtoMember(4)]
    	public int max;
	}
}