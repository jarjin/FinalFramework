local LoaderAdapter = class("LoaderAdapter")

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