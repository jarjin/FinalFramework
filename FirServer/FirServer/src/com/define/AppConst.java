package com.define;

import com.define.enums.DBType;

public class AppConst {
	///数据库类型
	public final static DBType DbType = DBType.None;
    /** mysql url*/
	public static String MYSQL_URL = "";
	/** mysql user*/
	public static String MYSQL_USER = "";
	/** mysql pass*/
	public static String MYSQL_PASS = "";
    
    public static String CONFIG_PATH = "";
	public final static String CITY_NAME = "city.xml";

	public final static String ExtCmdName = "FirServer";
	public final static String ProtoNameKey = "ProtoName";
	public final static String ByteArrayKey = "ByteArray";
	public final static String MsgTypeKey = "MsgType";
}
