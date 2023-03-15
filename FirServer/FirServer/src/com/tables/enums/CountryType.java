package com.tables.enums;

public enum CountryType 
{
    NONE(0),
    WEI(1),
    SHU(2),
    WU(3);

	private Integer v;
	CountryType(Integer key) {
		this.v = key;
	}
}