package com.helper;

import java.io.File;
import java.util.Iterator;

import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.io.SAXReader;

import com.define.AppConst;
import com.gamelogic.entity.CityEntity;

public class XmlHelper {
	public static void init() {
		loadCity();
	}
	
	public static void loadCity() {
		String filename = AppConst.CONFIG_PATH + AppConst.CITY_NAME;
		File file = new File(filename);
		SAXReader reader = new SAXReader(); 
		Document doc = null;
		try {
			doc = reader.read(file);
		} catch (DocumentException e1) {
			e1.printStackTrace();
		}
		
		Element root = doc.getRootElement();
		@SuppressWarnings("all")
		Iterator uIterator = root.elementIterator();

		while(uIterator.hasNext()){
			Element e = (Element)uIterator.next();
			if("city".equals(e.getName())) {
				CityEntity city = new CityEntity();
				String cid = e.attributeValue("id");
				city.id = cid;
				city.name = e.attributeValue("name");
				//Const.citys.put(cid, city);
			}
		}
	}
}
