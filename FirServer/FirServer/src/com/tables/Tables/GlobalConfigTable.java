package com.tables.Tables;

import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class GlobalConfigTable
{
	public String name;		
	private List<GlobalConfigTableItem> items = new ArrayList<GlobalConfigTableItem>();
	
	private Map<String, GlobalConfigTableItem> dics = null;


	public void Initialize()
	{
		dics = new HashMap<String, GlobalConfigTableItem>();
		for (GlobalConfigTableItem item : items)
		{
			dics.put(item.id, item);
		}
	}

	public List<GlobalConfigTableItem> GetItems()
	{
		return items;
	}

	public void AddItem(GlobalConfigTableItem item)
	{
		items.add(item);
	}

	public GlobalConfigTableItem GetItemByKey(String key)
	{
        GlobalConfigTableItem item = null;
        if (dics.containsKey(key))
        {
            item = dics.get(key);
        }
        return item;
	}
}