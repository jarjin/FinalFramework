package com.helper;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import com.define.AppConst;

public class MySqlHelper {
	private static Connection conn = null;
	
	private static void initMysql() {
		try {
			Class.forName("com.mysql.jdbc.Driver");
			conn = DriverManager.getConnection(
	        		AppConst.MYSQL_URL,
	        		AppConst.MYSQL_USER,
	        		AppConst.MYSQL_PASS);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		} catch (SQLException e) {
			e.printStackTrace();
		} 
	}

	public static int executeUpdate(String sql) {
		int flag = 0;
		try {
			handleTimeout();
			Statement st = conn.createStatement();
			flag = st.executeUpdate(sql);
			st.close();
		} catch (SQLException e) {
			e.printStackTrace();
		}
		return flag;
	}
	
	public static ResultSet executeQuery(String sql) {
		Statement st = null;
		try {
			handleTimeout();
			st = conn.createStatement();
			return st.executeQuery(sql);
		} catch (SQLException e) {
			e.printStackTrace();
		}
		return null;
	}
	
	public static Connection getConnection() {
		handleTimeout(); 
		return conn;
	}
	
	private static void handleTimeout() {
		try {
			if(conn==null || !conn.isValid(0)) {
				initMysql();
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}
	
	public static void closeResultSet(ResultSet rs) {
		Statement st = null;
		try {
			if(rs != null && !rs.isClosed()) {
				 st = rs.getStatement();
				 rs.close();
				 st.close();
			} 
		} catch (SQLException e) {
			e.printStackTrace();
		} finally {
			st = null;
			rs = null;
		}
	} 
	
	public static Object get(String dbname, String uid, String key) {
		String sql = "select " + key + " from " + dbname + " where userId='" + uid + "'";
		ResultSet rs = executeQuery(sql);
		Object result = null;
		try {
			rs.next();
			result = rs.getObject(key);
		} catch (SQLException e) { 
			System.out.println(e.getMessage());
		}
		if (result != null) {
			String test = result.toString().trim();
			if (test.equals("")) return null;
		}
		return result;
	}
	
	public static void set(String tbname, String uid, String key, String value) {
		String strKey = tbname + "_" + uid + "_" + key;
		//Memcache.delete(strKey);	//
		executeUpdate("update " + tbname + " set " + key + "=" + value + " where userId='" + uid + "'");
	}
}
