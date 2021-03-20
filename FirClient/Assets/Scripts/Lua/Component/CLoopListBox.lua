local CLoopListBox = class("CLoopListBox")

function CLoopListBox:initialize(gameObject)
    self:Dispose()
    self.transform = gameObject.transform
end

function CLoopListBox:Awake()
end

function CLoopListBox:InitListView(target, totalCount, onUpdateItem)
    self.target = target
    self.onUpdateItem = onUpdateItem
    self.itemTotalCount = totalCount
    self.mLoopListView = self.transform:Find("Scroll View"):GetComponent('LoopListView2')
    if self.mLoopListView ~= nil then
        totalCount = tointeger(totalCount)
        self.mLoopListView:InitListView(self, totalCount, self.OnGetItemByIndex);
    end
end

function CLoopListBox:NewListViewItem(prefabName)
    if self.mLoopListView ~= nil then
        return self.mLoopListView:NewListViewItem(prefabName)
    end
end

function CLoopListBox:SetListItemCount(itemCount, resetPos)
    if self.mLoopListView ~= nil then
        self.itemTotalCount = itemCount
        self.mLoopListView:SetListItemCount(itemCount, resetPos)
    end
end

function CLoopListBox:MoveToItemIndex(index)
    if self.mLoopListView ~= nil then
        self.mLoopListView:MovePanelToItemIndex(index, 0)
    end
end

function CLoopListBox:GetLoopListView()
    return self.mLoopListView
end

function CLoopListBox:OnGetItemByIndex(index)
    if index < 0 or index >= self.itemTotalCount then
        return nil
    end
    if self.target and self.onUpdateItem ~= nil then
        return self.onUpdateItem(self.target, index)
    end
    return nil
end

function CLoopListBox:ResetListView(target, func)
    if self.mLoopListView ~= nil then
        self.mLoopListView:ResetListView(target, func)
    end
end
function CLoopListBox:Dispose()
    self.target = nil
    self.onUpdateItem = nil
    self.transform = nil
    self.itemTotalCount = 0

    if self.mLoopListView ~= nil then
        self.mLoopListView:DestroyListView(self, self.OnGetItemByIndex)
    end
    self.mLoopListView = nil
end

return CLoopListBox