local middleclass = require "3rd.middleclass"

function _G.formatLog(...)
	local msgs = {...}
	local strs = ''
	for i = 1, #msgs do
		strs = strs..tostring(msgs[i])..' '
	end
	return strs
end

--输出日志--
function _G.log(...)
    UnityEngine.Debug.Log(formatLog(...))
end

--错误日志--
function _G.logError(...) 
	UnityEngine.Debug.LogError(formatLog(...))
end

--警告日志--
function _G.logWarn(...) 
	UnityEngine.Debug.LogWarning(formatLog(...))
end

--查找对象--
function _G.find(objName)
	return GameObject.Find(objName)
end

function _G.findByTag(objName)
	return GameObject.FindByTag(objName)
end

--创建一个对象--
function _G.newObject(prefab)
	return GameObject.Instantiate(prefab)
end

--销毁一个对象--
function _G.destroy(obj)
	GameObject.Destroy(obj)
end

--创建一个新类--
function _G.class(name, super) 
	return middleclass(name, super)
end

--字符串分割--
function string:split(sep) 
	local sep, fields = sep or "\t",{} 
	local pattern = string.format("([^%s]+)", sep) 
	self:gsub(pattern, function(c) fields[#fields+1] = c end) 
	return fields 
end

function string.startsWith(str, substr)
	if str == nil or substr == nil then
		return nil, nil
	end
	return string.find(str, substr) == 1
end

function string.endWith(str, substr)
	if str == nil or substr == nil then
		return nil, nil
	end
	local str_tmp = string.reverse(str)
	local substr_tmp = string.reverse(substr)
	return string.find(str_tmp, substr_tmp) == 1
end

--添加LuaAnimator组件--
--addLuaAnimator(self.gameObject, self, self.OnClick)0--
function _G.addLuaAnimator(gameObj, selfCtrl, endCall)
	if isnil(gameObj) or selfCtrl == nil or endCall == nil then
		logError("addLuaAnimator faild!!~~~")
		return nil
	end
	local classType = FirClient.Component.CLuaAnimator
    local luaAnim = gameObj:AddComponent(typeof(classType))
    luaAnim:Initialize(selfCtrl, endCall)
    return luaAnim
end

--启动一个计时器--
function _G.startTimer(expires, interval, selfTable, func, param, runNow)
	if expires == nil or interval == nil or func == nil then
		return nil
	end
	if runNow == nil then
		runNow = false
	end
	local timerMgr = MgrCenter:GetManager(ManagerNames.Timer)
	return timerMgr:AddLuaTimer(expires, interval, selfTable, func, param, runNow)
end

--停止一个计时器--
function _G.stopTimer(timer)
	if timer ~= nil then
		timerMgr:RemoveTimer(timer)
	end
end

--获得全局配置--
function _G.getGlobalItemByKey(key)
	local tableMgr = MgrCenter:GetManager(ManagerNames.Table)
	return tableMgr.globalConfigTable:GetItemByKey(key)
end

function _G.randomseed()
	math.randomseed(tonumber(tostring(os.time()):reverse():sub(1,6)))
end

function _G.range(min, max)
	return math.random(min, max)
end

function _G.vocationToRoleid(vocation)
	if vocation == 'warrior' then
		return 0
	elseif vocation == 'master' then
		return 1
	elseif vocation == 'ghost' then
		return 2
	end
end

function _G.roleidToVocation(roleid)
	if roleid == 0 then
		return 'warrior'
	elseif roleid == 1 then
		return 'master'
	elseif roleid == 2 then
		return 'ghost'
	end
end

--打印table--
function table.print(t)
	if t == nil then return end
	log("========= content of table ===========")
    for key, value in pairs(t) do
        log('('..tostring(key)..')=('..tostring(value)..')\n')
    end
end

--查找Key--
function table.foreachKey(t, k)
	if t == nil then return nil end
	for key, value in pairs(t) do
		if k == key then
			return value
		end
	end
	return nil
end

--删除表中KEY--
function table.removeKey(t, k)
    if t == nil then
		return
	end
	local v = t[k]
	t[k] = nil
    return v
end

function _G.isnil(uobj)
    return uobj == nil or uobj:Equals(nil)
end

function _G.execAction(action)
	LuaHelper.CallAction(action)
end

function _G.tointeger(number)
    return math.floor(tonumber(number) or error("Could not cast '" .. tostring(number) .."' to number.'"))
end

function table.copy(src)
    if src == nil then
        print("!!! try copy nil")
        return nil
    end
    local dst = {}
	for k,v in pairs(src) do
        dst[k] = v
    end
    return dst
end

function table.deepcopy(orig)
	if orig == nil then return nil end
	local function deep_copy(orig)
		local copy
		if type(orig) == "table" then
		  	copy = {}
		  	for orig_key, orig_value in next, orig, nil do
				copy[deep_copy(orig_key)] = deep_copy(orig_value)
		  	end
		  	setmetatable(copy, deep_copy(getmetatable(orig)))
		else
		  copy = orig
		end
		return copy
	end
	return deep_copy(orig)
end

function _G.handler(method, obj)
	return function(...)
		return method(obj, ...)
	end
end

function _G.AddEvent(eventName, func)
	local eventMgr = MgrCenter:GetManager(ManagerNames.Event)
	if eventMgr then
		eventMgr:AddEvent(eventName, func)
	end
end

function _G.FireEvent(eventName, ...)
	local eventMgr = MgrCenter:GetManager(ManagerNames.Event)
	if eventMgr then
		eventMgr:FireEvent(eventName, ...)
	end
end

function _G.RemoveEvent(eventName)
	local eventMgr = MgrCenter:GetManager(ManagerNames.Event)
	if eventMgr then
		eventMgr:RemoveEvent(eventName)
	end
end