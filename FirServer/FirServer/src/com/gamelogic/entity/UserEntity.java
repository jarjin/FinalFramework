package com.gamelogic.entity;

import com.gamelogic.define.classes.ModelNames;
import com.gamelogic.model.UserModel;

public class UserEntity extends BaseEntity {
	public String userid = "";
	public String name = "";
	public long money = 0;

	private UserModel userModel;

	public UserEntity(String userid) {
		this.userid = userid;
	}

	@Override
    public void Initialize() {
		if (userid == null && userid == "") return;
		userModel = (UserModel)modelMgr().GetModel(ModelNames.User);

		this.name = userModel.GetName(userid);
		this.money = userModel.GetMoney(userid);
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((userid == null) ? 0 : userid.hashCode());
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
			UserEntity other = (UserEntity) obj;
		if (userid == null) {
			if (other.userid != null)
				return false;
		} else if (!userid.equals(other.userid))
			return false;
		return true;
	}

	@Override
	public String toString() {
		return "User [money=" + money + ", name=" + name + ", userid=" + userid + "]";
	}

	@Override
    public void OnDispose() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnDispose'");
    }
}
