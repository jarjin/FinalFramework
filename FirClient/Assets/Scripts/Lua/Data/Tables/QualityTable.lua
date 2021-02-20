local QualityTable = class("QualityTable")

function QualityTable:Initialize()
    self.Items = {
        [1] = {id = 1, name = '灰色品质框', icon = 'gray'},
        [2] = {id = 2, name = '蓝色品质框', icon = 'blue'},
        [3] = {id = 3, name = '绿色品质框', icon = 'green'},
        [4] = {id = 4, name = '紫色品质框', icon = 'purple'},
        [5] = {id = 5, name = '橙色品质框', icon = 'orange'},
        [6] = {id = 6, name = '金色品质框', icon = 'yellow'}
	}
end

function QualityTable:GetItems()
    return self.Items
end

function QualityTable:GetItemByKey(key)
    return self.Items[key]
end

return QualityTable