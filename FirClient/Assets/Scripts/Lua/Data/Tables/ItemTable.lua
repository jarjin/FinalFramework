local ItemTable = class("ItemTable")

function ItemTable:Initialize()
    self.Items = {
        [70000] = {id = 70000, name = '战士男', quality = 1, typeid = 0, icon = '1'},
        [70001] = {id = 70001, name = '战士女', quality = 2, typeid = 0, icon = '2'},
        [70002] = {id = 70002, name = '法师男', quality = 3, typeid = 0, icon = '3'},
        [70003] = {id = 70003, name = '法师女', quality = 4, typeid = 0, icon = '4'},
        [70004] = {id = 70004, name = '射手男', quality = 5, typeid = 0, icon = '5'},
        [70005] = {id = 70005, name = '射手女', quality = 6, typeid = 0, icon = '6'},
        [70006] = {id = 70006, name = '技能图标', quality = 0, typeid = 2, icon = '1001'},
        [70007] = {id = 70007, name = '道具图标', quality = 6, typeid = 1, icon = '20001'},
        [70008] = {id = 70008, name = '商铺女', quality = 1, typeid = 0, icon = '7'},
        [70009] = {id = 70009, name = '天泠', quality = 2, typeid = 0, icon = '8'},
        [70010] = {id = 70010, name = '花影', quality = 2, typeid = 0, icon = '9'},
        [70011] = {id = 70011, name = '秦洛', quality = 2, typeid = 0, icon = '10'},
        [70012] = {id = 70012, name = '引导妹子', quality = 1, typeid = 0, icon = '12'},
        [70013] = {id = 70013, name = '武器女', quality = 1, typeid = 0, icon = '13'},
        [70014] = {id = 70014, name = '长生妹', quality = 1, typeid = 0, icon = '14'},
        [70015] = {id = 70015, name = '道具商', quality = 1, typeid = 0, icon = '15'},
        [70016] = {id = 70016, name = '吕布', quality = 6, typeid = 0, icon = '16'},
        [70017] = {id = 70017, name = '长生', quality = 1, typeid = 0, icon = '17'},
        [70018] = {id = 70018, name = '貂蝉', quality = 5, typeid = 0, icon = '18'},
        [70019] = {id = 70019, name = '左慈', quality = 2, typeid = 0, icon = '19'},
        [70020] = {id = 70020, name = '周瑜', quality = 5, typeid = 0, icon = '20'},
        [70021] = {id = 70021, name = '周泰', quality = 4, typeid = 0, icon = '21'},
        [70022] = {id = 70022, name = '于吉', quality = 2, typeid = 0, icon = '22'},
        [70023] = {id = 70023, name = '小乔', quality = 3, typeid = 0, icon = '23'},
        [70024] = {id = 70024, name = '太史慈', quality = 4, typeid = 0, icon = '24'},
        [70025] = {id = 70025, name = '孙尚香', quality = 3, typeid = 0, icon = '25'},
        [70026] = {id = 70026, name = '孙策', quality = 5, typeid = 0, icon = '26'},
        [70027] = {id = 70027, name = '陆逊', quality = 5, typeid = 0, icon = '27'},
        [70028] = {id = 70028, name = '凌统', quality = 3, typeid = 0, icon = '28'},
        [70029] = {id = 70029, name = '黄盖', quality = 4, typeid = 0, icon = '29'},
        [70030] = {id = 70030, name = '甘宁', quality = 4, typeid = 0, icon = '30'},
        [70031] = {id = 70031, name = '大乔', quality = 3, typeid = 0, icon = '31'},
        [70032] = {id = 70032, name = '程普', quality = 4, typeid = 0, icon = '32'},
        [70033] = {id = 70033, name = '甄姬', quality = 3, typeid = 0, icon = '33'},
        [70034] = {id = 70034, name = '张辽', quality = 4, typeid = 0, icon = '34'},
        [70035] = {id = 70035, name = '张郃', quality = 4, typeid = 0, icon = '35'},
        [70036] = {id = 70036, name = '荀彧', quality = 4, typeid = 0, icon = '36'},
        [70037] = {id = 70037, name = '许褚', quality = 4, typeid = 0, icon = '37'},
        [70038] = {id = 70038, name = '夏侯渊', quality = 4, typeid = 0, icon = '38'},
        [70039] = {id = 70039, name = '夏侯惇', quality = 4, typeid = 0, icon = '39'},
        [70040] = {id = 70040, name = '司马懿', quality = 5, typeid = 0, icon = '40'},
        [70041] = {id = 70041, name = '郭嘉', quality = 6, typeid = 0, icon = '41'},
        [70042] = {id = 70042, name = '典韦', quality = 5, typeid = 0, icon = '42'},
        [70043] = {id = 70043, name = '曹仁', quality = 4, typeid = 0, icon = '43'},
        [70044] = {id = 70044, name = '蔡文姬', quality = 4, typeid = 0, icon = '44'},
        [70045] = {id = 70045, name = '祝融夫人', quality = 3, typeid = 0, icon = '45'},
        [70046] = {id = 70046, name = '诸葛亮', quality = 6, typeid = 0, icon = '46'},
        [70047] = {id = 70047, name = '周仓', quality = 4, typeid = 0, icon = '47'},
        [70048] = {id = 70048, name = '赵云', quality = 6, typeid = 0, icon = '48'},
        [70049] = {id = 70049, name = '张飞', quality = 6, typeid = 0, icon = '49'},
        [70050] = {id = 70050, name = '星彩', quality = 3, typeid = 0, icon = '50'},
        [70051] = {id = 70051, name = '魏延', quality = 5, typeid = 0, icon = '51'},
        [70052] = {id = 70052, name = '庞统', quality = 4, typeid = 0, icon = '52'},
        [70053] = {id = 70053, name = '糜竺', quality = 3, typeid = 0, icon = '53'},
        [70054] = {id = 70054, name = '孟获', quality = 4, typeid = 0, icon = '54'},
        [70055] = {id = 70055, name = '马谡', quality = 4, typeid = 0, icon = '55'},
        [70056] = {id = 70056, name = '马岱', quality = 4, typeid = 0, icon = '56'},
        [70057] = {id = 70057, name = '马超', quality = 6, typeid = 0, icon = '57'},
        [70058] = {id = 70058, name = '刘备', quality = 6, typeid = 0, icon = '58'},
        [70059] = {id = 70059, name = '李严', quality = 3, typeid = 0, icon = '59'},
        [70060] = {id = 70060, name = '姜维', quality = 4, typeid = 0, icon = '60'},
        [70061] = {id = 70061, name = '黄忠', quality = 5, typeid = 0, icon = '61'},
        [70062] = {id = 70062, name = '黄月英', quality = 3, typeid = 0, icon = '62'},
        [70063] = {id = 70063, name = '关羽', quality = 6, typeid = 0, icon = '63'},
        [70064] = {id = 70064, name = '关兴', quality = 4, typeid = 0, icon = '64'},
        [70065] = {id = 70065, name = '关平', quality = 4, typeid = 0, icon = '65'},
        [70066] = {id = 70066, name = '法正', quality = 4, typeid = 0, icon = '66'},
        [70067] = {id = 70067, name = '刘禅', quality = 4, typeid = 0, icon = '67'}
	}
end

function ItemTable:GetItems()
    return self.Items
end

function ItemTable:GetItemByKey(key)
    return self.Items[key]
end

return ItemTable