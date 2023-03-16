package com.tables.Tables;

import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class QualityTable
{
	public String name;		
	public List<QualityTableItem> items = new ArrayList<QualityTableItem>();
	
	private Map<Integer, QualityTableItem> dics = null;


	public void Initialize()
	{
		dics = new HashMap<Integer, QualityTableItem>();
		for (QualityTableItem item : items)
		{
			dics.put(item.id, item);
		}
	}

	public List<QualityTableItem> GetItems()
	{
		return items;
	}

	public void AddItem(QualityTableItem item)
	{
		items.add(item);
	}

	public QualityTableItem GetItemByKey(Integer key)
	{
        QualityTableItem item = null;
        if (dics.containsKey(key))
        {
            item = dics.get(key);
        }
        return item;
	}
}