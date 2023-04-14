local TableManager = class("TableManager")

function TableManager:__index(k)
    if TableManager.tableMgr == nil then
        TableManager.tableMgr = MgrCenter:GetExtManager("TableManager")
    end
	return TableManager.tableMgr[k]
end

function TableManager:Initialize()
    TableManager.itemTable = require 'Data.Tables.ItemTable'
    TableManager.itemTable:Initialize()
    TableManager.qualityTable = require 'Data.Tables.QualityTable'
    TableManager.qualityTable:Initialize()
---[APPEND_VAR]---
end

return TableManager