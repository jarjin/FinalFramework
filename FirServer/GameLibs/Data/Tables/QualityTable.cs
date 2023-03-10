using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace FirCommon.Data
{
	[ProtoContract]
	public class QualityTable
	{
		[ProtoMember(1)]
		public string name;		
		[ProtoMember(2)]
		private List<QualityTableItem> items = new List<QualityTableItem>();
		
		private Dictionary<int, QualityTableItem> dics = null;

		public List<QualityTableItem> Items
		{
			get {
				return items;
			}
		}

		public void Initialize()
		{
			dics = new Dictionary<int, QualityTableItem>();
			foreach (QualityTableItem item in items)
			{
				dics.Add(item.id, item);
			}
		}

		public List<QualityTableItem> GetItems()
		{
			return items;
		}

		public void AddItem(QualityTableItem item)
		{
			items.Add(item);
		}

		public QualityTableItem GetItemByKey(int key)
		{
			QualityTableItem item = null;
			if (dics.ContainsKey(key))
			{
				dics.TryGetValue(key, out item);
			}
			return item;
		}
	}

	[ProtoContract]
	public class QualityTableItem
	{
    	[ProtoMember(1)]
    	public int id;
    	[ProtoMember(2)]
    	public string name;
    	[ProtoMember(3)]
    	public string icon;
	}
}