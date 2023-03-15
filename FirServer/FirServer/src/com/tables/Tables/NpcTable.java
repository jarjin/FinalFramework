package com.tables.Tables;

import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class NpcTable
{
	public String name;		
	private List<NpcTableItem> items = new ArrayList<NpcTableItem>();
	
	private Map<Integer, NpcTableItem> dics = null;


	public void Initialize()
	{
		dics = new HashMap<Integer, NpcTableItem>();
		for (NpcTableItem item : items)
		{
			dics.put(item.id, item);
		}
	}

	public List<NpcTableItem> GetItems()
	{
		return items;
	}

	public void AddItem(NpcTableItem item)
	{
		items.add(item);
	}

	public NpcTableItem GetItemByKey(Integer key)
	{
        NpcTableItem item = null;
        if (dics.containsKey(key))
        {
            item = dics.get(key);
        }
        return item;
	}
}