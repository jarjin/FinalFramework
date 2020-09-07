local this = LuaUtil
local batchTask = nil

--显示进度文字--
function LuaUtil.ShowLoaderProgressText(text)
    local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	local loaderCtrl = ctrlMgr:GetCtrl(UiNames.Loader)
	if loaderCtrl ~= nil then
		loaderCtrl:ShowProgressText(text)
	end
end

--更新Loader进度条--
function LuaUtil.UpdateLoaderProgress(curr, total)
    local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	local loaderCtrl = ctrlMgr:GetCtrl(UiNames.Loader)
	if loaderCtrl ~= nil then
		loaderCtrl:ShowProgressValue(curr, total)
	end
end

function LuaUtil.InitBatchObject()
    if batchTask == nil then
        local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
        if componentMgr ~= nil then
            batchTask = componentMgr:GetComponent(ComponentNames.BatchTask, "default")
        end
    end
end

function LuaUtil.InitBatchTask(cbText, cbProgress, execOK)
    this.InitBatchObject()
    if batchTask ~= nil then
        batchTask:InitBatchTask(cbText, cbProgress, execOK)
    end
end

function LuaUtil.AddBatchTask(name, desc, execOK)
    if batchTask ~= nil then
        batchTask:AddBatchTask(name, desc, execOK)
    end
end

function LuaUtil.StartBatchTask()
    if batchTask ~= nil then
        batchTask:StartBatchTask()
    end
end

function LuaUtil.GetComponent(gameObj, typeName)
    if not isnil(gameObj) then
        local instanceId = gameObj:GetInstanceID()
        local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
        return componentMgr:GetComponent(typeName, instanceId)
    end
    return nil
end

function LuaUtil.GetAtlas(name)
    local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
    return componentMgr:GetComponent(ComponentNames.Atlas, name)
end