package com.entity;

public class User {
	public String uid = "";
	public String name = "";
	public String factionId = "";
	public String factionName = "";
	public byte roleId = 0;
	public byte sex = 0;
	public long exp = 0;
	public short level = 0;
	public long money = 0;
	public int credits = 0;
	public String cid = "";
	public String title = "";
	public String position = "";
	public int teamId = 0;
	public String bid = "";
	public boolean isCompare = false;
	public boolean isPractice = false;
	public byte vip = 0;
	public int clothesId = 0;
	public int characterId = 0;

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((uid == null) ? 0 : uid.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		User other = (User) obj;
		if (uid == null) {
			if (other.uid != null)
				return false;
		} else if (!uid.equals(other.uid))
			return false;
		return true;
	}

	@Override
	public String toString() {
		return "User [cid=" + cid + ", credits="
				+ credits + ", exp=" + exp + ", level=" + level + ", money="
				+ money + ", name=" + name + ", position=" + position
				+ ", roleId=" + roleId + ", sex=" + sex + ", teamId=" + teamId + ", uid=" + uid + "]";
	}

}
