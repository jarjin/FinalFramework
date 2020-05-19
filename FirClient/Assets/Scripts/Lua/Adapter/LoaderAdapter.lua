local LoaderAdapter = class("LoaderAdapter")

function LoaderAdapter:OnEnterLevel(execOK)
	if execOK ~= nil then
		execOK:DynamicInvoke()
	end
end

function LoaderAdapter:OnLeaveLevel(execOK)
	if execOK ~= nil then
		execOK:DynamicInvoke()
	end
end

return LoaderAdapter