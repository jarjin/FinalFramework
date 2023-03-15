package com.tables.Tables;

import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class ItemTable
{
	public String name;		
	private List<ItemTableItem> items = new ArrayList<ItemTableItem>();
	
	private Map<Integer, ItemTableItem> dics = null;


	public void Initialize()
	{
		dics = new HashMap<Integer, ItemTableItem>();
		for (ItemTableItem item : items)
		{
			dics.put(item.id, item);
		}
	}

	public List<ItemTableItem> GetItems()
	{
		return items;
	}

	public void AddItem(ItemTableItem item)
	{
		items.add(item);
	}

	public ItemTableItem GetItemByKey(Integer key)
	{
        ItemTableItem item = null;
        if (dics.containsKey(key))
        {
            item = dics.get(key);
        }
        return item;
	}
}