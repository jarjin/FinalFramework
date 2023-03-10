using System;
using System.Collections.Generic;
using protobuf-net;
using UnityEngine;

namespace FirCommon.Data
{
	[ProtoContract]
	public class NpcTable
	{
		[ProtoMember(1)]
		public string name;
		[ProtoMember(2)]
		private Dictionary<int, NpcTableItem> dics = null;
		[ProtoMember(3)]
		private List<NpcTableItem> items = new List<NpcTableItem>();

		public List<NpcTableItem> Items
		{
			get {
				return items;
			}
		}

		public void Initialize()
		{
			dics = new Dictionary<int, NpcTableItem>();
			foreach (NpcTableItem item in items)
			{
				dics.Add(item.id, item);
			}
		}

		public List<NpcTableItem> GetItems()
		{
			return items;
		}

		public void AddItem(NpcTableItem item)
		{
			items.Add(item);
		}

		public NpcTableItem GetItemByKey(int key)
		{
			NpcTableItem item = null;
			if (dics.ContainsKey(key))
			{
				dics.TryGetValue(key, out item);
			}
			return item;
		}
	}

	[ProtoContract]
	public class NpcTableItem
	{
    	[ProtoMember(1)]
    	public int id;
    	[ProtoMember(2)]
    	public string name;
    	[ProtoMember(3)]
    	public bool isMainCharacter;
    	[ProtoMember(4)]
    	public int sex;
    	[ProtoMember(5)]
    	public CountryType country;
    	[ProtoMember(6)]
    	public Vector3 scale;
    	[ProtoMember(7)]
    	public int itemid;
	}
}