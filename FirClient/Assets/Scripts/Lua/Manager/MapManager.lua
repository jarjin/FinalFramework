local MapManager = class("MapManager")

function MapManager:Initialize()
	self.mapObject = nil
	self.sceneMapData = nil
	logWarn('MapManager:InitializeOK...')
end

function MapManager:GetCurrMapData()
	return self.sceneMapdata
end

----------------------------SceneMap----------------------------
function MapManager:CreateMap(createOK)
    if isnil(self.mapObject) then
		local path = "Prefabs/Maps/MapObject"
		local resMgr = MgrCenter:GetManager(ManagerNames.Resource)
        resMgr:LoadAssetAsync(path, { "MapObject" }, typeof(GameObject), function(objs)
            if objs[0] == nil then
                return
            end
			local parent = find('/MainGame/BattleScene')
            self.mapObject = newObject(objs[0])
            self.mapObject.name = 'MapObject'
            self.mapObject.transform:SetParent(parent.transform)
            self.mapObject.transform.localPosition = Vector3.zero
            self.mapObject.transform.localScale = Vector3.one

			if createOK ~= nil then 
				createOK()
			end
        end)
    else
		if createOK ~= nil then 
			createOK()
		end
    end
end

function MapManager:LoadSceneMap(mapid, createOK)
	log('mapid:'..mapid)
	local configMgr = MgrCenter:GetManager(ManagerNames.Config)
	self.sceneMapdata = configMgr:GetMapData(mapid)
	if self.sceneMapdata ~= nil then
		local atlasName = self.sceneMapdata.atlas
		local atlasPath = "Maps/"..atlasName
		log('atlasName:'..atlasName)
		log('atlasPath:'..atlasPath)
		self:LoadMapAtlas(MapType.Scene, atlasName, atlasPath, createOK)
	end
end

function MapManager:LoadBattleMap(dungeonMapData, createOK)
	if dungeonMapData ~= nil then
		local atlasName = dungeonMapData.atlas
		local atlasPath = "Dungeons/"..atlasName
		self:LoadMapAtlas(MapType.Battle, atlasName, atlasPath, createOK)
	end
end

function MapManager:LoadMapAtlas(mapType, mapAtlasName, atlasPath, createOK)
	if self.mapObject ~= nil then
		local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
		local mapAtlas = componentMgr:GetComponent(mapAtlasName)
		if mapAtlas == nil then
			local resMgr = MgrCenter:GetManager(ManagerNames.Resource)
			resMgr:LoadAssetAsync(atlasPath, nil, typeof(Sprite), function(objs) 
				if objs.Length == 0 then
					return
				end
				componentMgr:AddComponent(ComponentNames.Atlas, mapAtlasName, objs)

				local mapAtlas = componentMgr:GetComponent(ComponentNames.Atlas, mapAtlasName)
				self:DoInitMap(mapType, mapAtlas, createOK)
			end)
		else
			self:DoInitMap(mapType, mapAtlas, createOK)
		end
	end
end

function MapManager:DoInitMap(mapType, atlas, createOK)
	local parent = self.mapObject.transform

	local bg_vista = atlas:GetSprite("bg_vista")
	self:SetChildSprite(parent, "bg_vista", bg_vista)

	local bg_main = atlas:GetSprite("bg_main")
	self:SetChildSprite(parent, "bg_main", bg_main)

	local fg_left = atlas:GetSprite("fg_left")
	self:SetChildSprite(parent, "fg_left", fg_left)

	local fg_center = atlas:GetSprite("fg_center")
	self:SetChildSprite(parent, "fg_center", fg_center)

	local fg_right = atlas:GetSprite("fg_right")
	self:SetChildSprite(parent, "fg_right", fg_right)
	
	self:UpdateNow(mapType) 
	self:AddVistaFollowCamera()

    if createOK ~= nil then
        createOK(self)
    end
end

function MapManager:AddVistaFollowCamera()
	local objFollow = find('/MainGame'):GetComponent('CObjectFollow')
	if not isnil(objFollow) then
		local objTarget = self.mapObject.transform:Find("bg_vista")
		local type = FirClient.Component.FollowType.ObjectFollowCamera
		objFollow:AddItem('ObjectFollowCamera', type, 0.2, nil, objTarget)
	end
end

function MapManager:SetChildSprite(trans, nodeName, sprite)
	local node = trans:Find(nodeName)
	if not isnil(node) then
		local render = node:GetComponent('SpriteRenderer')
		if not isnil(render) then
			render.sprite = sprite
		end
	end
end

function MapManager:UpdateNow(mapType)
    local bg_vista = self:GetMapNodeRender("bg_vista")
    local bg_main = self:GetMapNodeRender("bg_main")
    local fg_left = self:GetMapNodeRender("fg_left")
    local fg_center = self:GetMapNodeRender("fg_center")
    local fg_right = self:GetMapNodeRender("fg_right")

    local scale = bg_main.transform.localScale
    local cameraHeight = Camera.main.orthographicSize * 2
    local cameraWidth = cameraHeight * Camera.main.aspect
    --scale *= cameraWidth / bgRender.bounds.size.x
    scale = scale * (cameraHeight / 2 / bg_main.bounds.size.y)

    bg_vista.transform.localScale = scale
    bg_main.transform.localScale = scale
    fg_left.transform.localScale = scale
    fg_center.transform.localScale = scale
    fg_right.transform.localScale = scale

	if mapType == MapType.Scene then
		self:UpdateSceneMap(cameraHeight, bg_vista, bg_main, fg_left, fg_center, fg_right)
	elseif mapType == MapType.Battle then
		self:UpdateBattleMap(cameraWidth, bg_vista, bg_main, fg_left, fg_center, fg_right)
	end
end

function MapManager:UpdateSceneMap(cameraHeight, bg_vista, bg_main, fg_left, fg_center, fg_right)
    local y = cameraHeight / 2 / 2

    bg_vista.transform.position = Vector3.New(0, y, 1)
    bg_main.transform.position = Vector3.New(0, y)

    local fgleft_x = bg_main.bounds.size.x / 2 - fg_left.bounds.size.x / 2
    local fgleft_y = fg_left.bounds.size.y / 2
    fg_left.transform.position = Vector2.New(-fgleft_x, fgleft_y)

    local fgcenter_y = fg_center.bounds.size.y / 2
    fg_center.transform.position = Vector2.New(0, fgcenter_y)

    local fgright_x = bg_main.bounds.size.x / 2 - fg_right.bounds.size.x / 2
    local fgright_y = fg_right.bounds.size.y / 2
    fg_right.transform.position = Vector2.New(fgright_x, fgright_y)
end

function MapManager:UpdateBattleMap(cameraWidth, bg_vista, bg_main, fg_left, fg_center, fg_right)
    bg_vista.transform.position = Vector3.New(0, 0, 1)
    bg_main.transform.position = Vector3.New(0, 0, 0)

    local x = cameraWidth / 2
    fg_left.transform.position = Vector2.New(-x, 0)
    fg_center.transform.position = Vector2.zero
    fg_right.transform.position = Vector2.New(x, 0)
end

function MapManager:GetMapNodeRender(nodeName)
	if isnil(self.mapObject) then 
		return nil 
	end
	local node = self.mapObject.transform:Find(nodeName)
	if isnil(node) then
		return nil
	end
	return node:GetComponent('SpriteRenderer')
end

return MapManager