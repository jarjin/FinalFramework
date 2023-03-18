using System.Collections.Generic;

namespace FirCommon.Data
{
	public class NpcTable
	{
		public string name;		
		public List<NpcTableItem> items = new List<NpcTableItem>();
		
		private Dictionary<int, NpcTableItem> dics = null;

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

	public class NpcTableItem
	{
    	public int id;
    	public string name;
    	public bool isMainCharacter;
    	public int sex;
    	public CountryType country;
    	public FVector3 scale;
    	public int itemid;
	}
}