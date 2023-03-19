package com.helper;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import com.define.AppConst;

public class MySQLHelper {
	private Connection conn = null;
	
	public void initMySQL() {
		try {
			Class.forName("com.mysql.jdbc.Driver");
			conn = DriverManager.getConnection(
	        		AppConst.MYSQL_URL + AppConst.MYSQL_DBNAME,
	        		AppConst.MYSQL_USER, AppConst.MYSQL_PASS);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		} catch (SQLException e) {
			e.printStackTrace();
		} 
	}

	public int executeUpdate(String sql) {
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
	
	public ResultSet executeQuery(String sql) {
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
	
	public Connection getConnection() {
		handleTimeout(); 
		return conn;
	}
	
	private void handleTimeout() {
		try {
			if(conn == null || !conn.isValid(0)) {
				initMySQL();
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}
	
	public void closeResultSet(ResultSet rs) {
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
	
	public Object get(String dbname, String uid, String key) {
		String sql = "select " + key + " from " + dbname + " where userid='" + uid + "'";
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
	
	public void set(String tbname, String uid, String key, Object value) {
		executeUpdate("update " + tbname + " set " + key + "=" + value + " where userid='" + uid + "'");
	}

	public void closeMySQL() {
		try {
			conn.close();
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}
}
