local CItemBox = class("CItemBox")

local ItemType = {
    Head = 0,
    Item = 1,
    Skill = 2,
}
local tableMgr = nil

function CItemBox:initialize(gameObject)
    self:Dispose()
    self.gameObject = gameObject
end

function CItemBox:Awake()
    local transform = self.gameObject.transform
    self.imgIcon = transform:Find("Icon"):GetComponent('Image')
    self.imgQuality = transform:Find("Quality"):GetComponent('Image')
    self.qualityDefault = self.imgQuality.sprite

    self.imgState = transform:Find("State"):GetComponent('Image')
    self.btnClick = transform:Find("BtnClick"):GetComponent('Button')
end

function CItemBox:SetItemState(state)
    if isnil(self.imgState) then return end
    if state == ItemBoxState.Normal then
        self.imgState:Disable()
    elseif state == ItemBoxState.Highlighted then
        self.imgState:Enable()
        self.imgState.sprite = stateHighlighted
    elseif state == ItemBoxState.Disabled then
        self.imgState:Enable()
        self.imgState.sprite = stateDisabled
    end
end

function CItemBox:SetItem(itemid)
    if tableMgr == nil then
        tableMgr = MgrCenter:GetManager(ManagerNames.Table)
    end
    if tableMgr == nil then return end
    self.gameObject.name = tostring(itemid)
    local itemData = tableMgr.itemTable:GetItemByKey(itemid)
    if itemData ~= nil then
        self:LoadIcon(itemData)
    end
end

function CItemBox:LoadIcon(itemData)
    local resType = itemData.typeid
    if not isnil(self.imgQuality) then
        local atlas = LuaUtil.GetAtlas("Quality")
        if atlas ~= nil then
            if itemData.quality == 0 then
                self.imgQuality:Disable()
            else
                local qualityItem = tableMgr.qualityTable:GetItemByKey(itemData.quality)
                if qualityItem ~= nil then
                    self.imgQuality:Enable()
                    self.imgQuality.sprite = atlas:GetSprite(qualityItem.icon)
                end
            end
        end
    end
    if not isnil(self.imgIcon) then
        local atlasName = self:GetItemAtlasName(resType)
        if atlasName ~= nil then
            local atlas = LuaUtil.GetAtlas(atlasName)
            if atlas ~= nil then
                self.imgIcon:Enable()
                self.imgIcon.sprite = atlas:GetSprite(itemData.icon)
            end
        end
    end
end

function CItemBox:GetItemAtlasName(resType)
    if resType == ItemType.Head then
        return "HeadIcon"
    elseif resType == ItemType.Item then
        return "ItemIcon"
    elseif resType == ItemType.Skill then
        return "SkillIcon"
    end
    return nil
end

function CItemBox:ResetItem()
    self.count.text = ''
    self.imgIcon.sprite = nil
    self.imgQuality.sprite = nil
    self.imgState.sprite = nil
end
function CItemBox:Dispose()
    self.imgIcon = nil
    self.imgQuality = nil
    self.imgState = nil
    self.btnClick = nil
    self.gameObject = nil
    self.qualityDefault = nil
end

function CItemBox:OnDestroy()
    self:Dispose()
end

return CItemBox