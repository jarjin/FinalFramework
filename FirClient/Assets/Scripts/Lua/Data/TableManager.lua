local TableBase = {}
function TableBase.New(o)
    o = o or {}
    setmetatable(o, {
		__index = TableBase.Search
	})
    return o
end

function TableBase.Search(_, k)
    if TableBase.tableMgr == nil then
        TableBase.tableMgr = ManagementCenter.GetExtManager("TableManager")
    end
	return TableBase.tableMgr[k]
end

local TableManager = TableBase.New()

function TableManager.Initialize()
    TableManager.globalConfigTable = require 'Data.Tables.GlobalConfigTable'
    TableManager.globalConfigTable:Initialize()
    TableManager.itemTable = require 'Data.Tables.ItemTable'
    TableManager.itemTable:Initialize()
    TableManager.qualityTable = require 'Data.Tables.QualityTable'
    TableManager.qualityTable:Initialize()
---[APPEND_VAR]---
end

function TableManager.print(...)
	print(...)
end

return TableManager