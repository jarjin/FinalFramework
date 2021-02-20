local GlobalConfigTable = class("GlobalConfigTable")

function GlobalConfigTable:Initialize()
    self.Items = {
        ['CommonWhite'] = {id = 'CommonWhite', value = '250_250_250_255'},
        ['CommonGreen'] = {id = 'CommonGreen', value = '0_246_0_255'},
        ['CommonRed'] = {id = 'CommonRed', value = '250_17_17_255'}
	}
end

function GlobalConfigTable:GetItems()
    return self.Items
end

function GlobalConfigTable:GetItemByKey(key)
    return self.Items[key]
end

return GlobalConfigTable