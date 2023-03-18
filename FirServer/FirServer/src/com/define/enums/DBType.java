package com.define.enums;

public enum DBType {
    None(0),
    MySQL(1),
    MongoDB(2);

    private Integer value;
	DBType(Integer key) {
		this.value = key;
	}
	
	public int getValue() {
        return value;
    }
}
