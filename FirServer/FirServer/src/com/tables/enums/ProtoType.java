package com.tables.enums;

public enum ProtoType 
{
    CSProtoMsg(0),
    LuaProtoMsg(1);

	private Integer value;
	ProtoType(Integer key) {
		this.value = key;
	}
	
	public int getValue() {
        return value;
    }
}