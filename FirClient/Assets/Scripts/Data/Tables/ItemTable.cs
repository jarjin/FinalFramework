using System;
using System.Collections.Generic;

[Serializable]
public class ItemTable
{
    public string name;

    private Dictionary<int, ItemTableItem> dics = null;
    private List<ItemTableItem> items = new List<ItemTableItem>();

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

[Serializable]
public class ItemTableItem
{
    public int id;
    public string name;
    public int quality;
    public int typeid;
    public string icon;

}