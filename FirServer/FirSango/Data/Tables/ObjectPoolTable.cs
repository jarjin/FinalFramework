using System;
using System.Collections.Generic;

[Serializable]
public class ObjectPoolTable
{
    public string name { get; set; }

    private Dictionary<int, ObjectPoolTableItem> dics = null;
    private List<ObjectPoolTableItem> items = new List<ObjectPoolTableItem>();

    public List<ObjectPoolTableItem> Items
    {
        get {
            return items;
        }
    }

    public void Initialize()
    {
        dics = new Dictionary<int, ObjectPoolTableItem>();
        foreach (var item in items)
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

[Serializable]
public class ObjectPoolTableItem
{
    public int id { get; set; }
    public string name { get; set; }
    public int min { get; set; }
    public int max { get; set; }

}