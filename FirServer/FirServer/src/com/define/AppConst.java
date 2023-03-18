package com.define;

import com.define.enums.DBType;

public class AppConst {
	///数据库类型
	public final static DBType DbType = DBType.None;
    /** mysql url*/
	public final static String MYSQL_URL = "jdbc:mysql://localhost:3306/test";
	/** mysql user*/
	public final static String MYSQL_USER = "root";
	/** mysql pass*/
	public final static String MYSQL_PASS = "111111";
    
    public final static String CONFIG_PATH = "";
	public final static String CITY_NAME = "city.xml";

	public final static String MsgTypeKey = "MsgType";
	public final static String ExtCmdName = "FirServer";
	public final static String ProtoNameKey = "ProtoName";
	public final static String ByteArrayKey = "ByteArray";
}
