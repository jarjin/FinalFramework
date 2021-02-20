using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirCommon.Data
{
	[Serializable]
	public class NpcTable
	{
		public string name;

		private Dictionary<int, NpcTableItem> dics = null;
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

	[Serializable]
	public class NpcTableItem
	{
    	public int id;
    	public string name;
    	public bool isMainCharacter;
    	public int sex;
    	public CountryType country;
    	public Vector3 scale;
    	public int itemid;
	}
}