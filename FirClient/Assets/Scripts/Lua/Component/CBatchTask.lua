local CBatchTask = class("CBatchTask")

local refIndex = 0
local mTaskList = nil
local progressText = nil
local progressCallback = nil
local loadOk = nil
local taskState = BatchTaskState.Stoped

function CBatchTask:initialize(name)
    logWarn("New BatchTask..."..name)
end

function CBatchTask:InitBatchTask(cbText, cbProgress, execOK)
    if taskState == BatchTaskState.Stoped then
        loadOk = execOK
        progressText = cbText
        progressCallback = cbProgress
        mTaskList = {}
    end
end

function CBatchTask:AddBatchTask(name, desc, execOK)
    if taskState == BatchTaskState.Stoped then
        local task = {}
        task.name = name
        task.desc = desc
        task.execOK = execOK
        table.insert(mTaskList, task)
    end
end

function CBatchTask:StartBatchTask()
    if taskState == BatchTaskState.Stoped then
        refIndex = 0
        taskState = BatchTaskState.Running
        self:NextBatchTask()
    end
end

function CBatchTask:NextBatchTask()
    if #mTaskList == 0 then
        self:StopBatchTask()
        return
    end
    local task = table.remove(mTaskList, 1)
    if task ~= nil then
        log("New Task---->>>"..task.name.." desc:>"..task.desc);
        if progressText ~= nil then
            progressText(self, task.desc)
        end
        if task.execOK ~= nil then
            task.execOK(self, self.OnBatchTaskOK)
            task.execOK = nil
        end
    end
end

function CBatchTask:StopBatchTask()
    if loadOk ~= nil then
        loadOk(self)
    end
    loadOk = nil
    mTaskList = nil
    progressText = nil
    progressCallback = nil
    taskState = BatchTaskState.Stoped;
end

function CBatchTask:OnBatchTaskOK()
    refIndex = refIndex + 1
    if progressCallback ~= nil then
        progressCallback(self, refIndex, #mTaskList)
    end
    self:NextBatchTask()
end

return CBatchTask