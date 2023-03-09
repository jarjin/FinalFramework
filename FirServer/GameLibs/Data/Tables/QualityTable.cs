using System;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace FirCommon.Data
{
	[MessagePackObject(true)]
	public class QualityTable
	{
		public string name;

		private Dictionary<int, QualityTableItem> dics = null;
		private List<QualityTableItem> items = new List<QualityTableItem>();

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

	[MessagePackObject(true)]
	public class QualityTableItem
	{
    	public int id;
    	public string name;
    	public string icon;
	}
}