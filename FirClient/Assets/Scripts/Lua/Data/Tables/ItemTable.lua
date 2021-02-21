local ItemTable = class("ItemTable")

function ItemTable:Initialize()
    self.Items = {
        [1] = {id = 1, name = '战士男', quality = 1, typeid = 0, icon = '1'},
        [2] = {id = 2, name = '战士女', quality = 2, typeid = 0, icon = '2'},
        [3] = {id = 3, name = '法师男', quality = 3, typeid = 0, icon = '3'},
        [4] = {id = 4, name = '法师女', quality = 4, typeid = 0, icon = '4'},
        [5] = {id = 5, name = '射手男', quality = 5, typeid = 0, icon = '5'},
        [6] = {id = 6, name = '射手女', quality = 6, typeid = 0, icon = '6'},
        [1000] = {id = 1000, name = '技能图标', quality = 0, typeid = 2, icon = '1001'},
        [2000] = {id = 2000, name = '道具图标', quality = 6, typeid = 1, icon = '20001'},
        [7] = {id = 7, name = '商铺女', quality = 1, typeid = 0, icon = '7'},
        [8] = {id = 8, name = '天泠', quality = 2, typeid = 0, icon = '8'},
        [9] = {id = 9, name = '花影', quality = 2, typeid = 0, icon = '9'},
        [10] = {id = 10, name = '秦洛', quality = 2, typeid = 0, icon = '10'},
        [11] = {id = 11, name = '引导妹子', quality = 1, typeid = 0, icon = '12'},
        [12] = {id = 12, name = '武器女', quality = 1, typeid = 0, icon = '13'},
        [13] = {id = 13, name = '长生妹', quality = 1, typeid = 0, icon = '14'},
        [14] = {id = 14, name = '道具商', quality = 1, typeid = 0, icon = '15'}
	}
end

function ItemTable:GetItems()
    return self.Items
end

function ItemTable:GetItemByKey(key)
    return self.Items[key]
end

return ItemTable