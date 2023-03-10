using System.Collections.Generic;

namespace FirCommon.Data
{
	public class ObjectPoolTable
	{
		public string name;		
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

	public class ObjectPoolTableItem
	{
    	public int id;
    	public string name;
    	public int min;
    	public int max;
	}
}