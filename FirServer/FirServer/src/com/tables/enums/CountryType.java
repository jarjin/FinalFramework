package com.tables.enums;

public enum CountryType 
{
    NONE(0),
    WEI(1),
    SHU(2),
    WU(3);

	private Integer value;
	CountryType(Integer key) {
		this.value = key;
	}
	
	public int getValue() {
        return value;
    }
}