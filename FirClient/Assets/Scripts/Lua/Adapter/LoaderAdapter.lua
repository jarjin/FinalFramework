local BaseAdapter = require 'Adapter.BaseAdapter'
local LoaderAdapter = class("LoaderAdapter", BaseAdapter)

function LoaderAdapter:OnEnterLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

function LoaderAdapter:OnLeaveLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

return LoaderAdapter