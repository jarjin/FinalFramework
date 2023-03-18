package com.tables.Tables;

import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class ObjectPoolTable
{
	public String name;		
	public List<ObjectPoolTableItem> items = new ArrayList<ObjectPoolTableItem>();
	
	private Map<Integer, ObjectPoolTableItem> dics = null;


	public void Initialize()
	{
		dics = new HashMap<Integer, ObjectPoolTableItem>();
		for (ObjectPoolTableItem item : items)
		{
			dics.put(item.id, item);
		}
	}

	public List<ObjectPoolTableItem> GetItems()
	{
		return items;
	}

	public void AddItem(ObjectPoolTableItem item)
	{
		items.add(item);
	}

	public ObjectPoolTableItem GetItemByKey(Integer key)
	{
        ObjectPoolTableItem item = null;
        if (dics.containsKey(key))
        {
            item = dics.get(key);
        }
        return item;
	}
}