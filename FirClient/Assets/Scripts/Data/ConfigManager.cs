using FirClient.Data;
using FirClient.Extensions;
using LuaInterface;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ConfigManager : BaseObject
{
    private static ConfigManager instance;
    private Dictionary<uint, RoleData> roleDatas = new Dictionary<uint, RoleData>();
    private Dictionary<string, BulletData> bulletDatas = new Dictionary<string, BulletData>();
    private Dictionary<string, EffectData> effectDatas = new Dictionary<string, EffectData>();
    private Dictionary<uint, MapData> mapDatas = new Dictionary<uint, MapData>();
    private Dictionary<uint, TeamData> teamDatas = new Dictionary<uint, TeamData>();
    private Dictionary<uint, ChapterData> chapterDatas = new Dictionary<uint, ChapterData>();
    private Dictionary<uint, Dictionary<string, SkillData>> skillDatas = new Dictionary<uint, Dictionary<string, SkillData>>();
    private Dictionary<EmbattleType, Dictionary<uint, Vector3>> embattleOffsetPos = new Dictionary<EmbattleType, Dictionary<uint, Vector3>>();
    private Dictionary<uint, StoryData> storyDatas = new Dictionary<uint, StoryData>();

    public static ConfigManager Create()
    {
        if (instance == null)
        {
            instance = new ConfigManager();
        }
        return instance;
    }

    [NoToLua]
    public override void Initialize()
    {
        LoadMapData();
        LoadSkillData();
        LoadNpcData();
        LoadBulletData();
        LoadEffectData();
        LoadTeamData();
        LoadDungeonData();
        LoadEmbattlePosData();
        LoadStoryData();
    }

    private void LoadStoryData()
    {
        string dataPath = "datas/Storys.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var dataNode = asset.Children[i] as SecurityElement;
                var storyData = new StoryData();
                storyData.id = dataNode.Attribute("id").ToUint();
                storyData.name = dataNode.Attribute("name");
                storyData.pageDatas = new Dictionary<uint, PageData>();

                if (dataNode.Children != null)
                {
                    for (int j = 0; j < dataNode.Children.Count; j++)
                    {
                        var pageNode = dataNode.Children[j] as SecurityElement;
                        var pageData = new PageData();
                        pageData.id = pageNode.Attribute("id").ToUint();
                        pageData.title = pageNode.Attribute("title");
                        pageData.dialogDatas = new Dictionary<uint, DialogData>();

                        if (pageNode.Children != null)
                        {
                            for (int x = 0; x < pageNode.Children.Count; x++)
                            {
                                var dialogNode = pageNode.Children[x] as SecurityElement;

                                var dialogData = new DialogData();
                                dialogData.id = dialogNode.Attribute("id").ToUint();
                                dialogData.roleid = dialogNode.Attribute("role").ToUint();
                                dialogData.posid = dialogNode.Attribute("pos").ToUint();
                                dialogData.txtContent = dialogNode.Attribute("text");
                                pageData.dialogDatas.Add(dialogData.id, dialogData);
                            }
                        }
                        storyData.pageDatas.Add(pageData.id, pageData);
                    }
                }
                storyDatas.Add(storyData.id, storyData);
            }
        }
    }

    public DialogData GetDialogDataByKey(uint storyid, uint pageid, uint dlgid)
    {
        if (storyDatas.ContainsKey(storyid))
        {
            var pageData = storyDatas[storyid].pageDatas;
            if (pageData != null && pageData.ContainsKey(pageid))
            {
                var dialogDatas = pageData[pageid].dialogDatas;
                if (dialogDatas != null && dialogDatas.ContainsKey(dlgid))
                {
                    return dialogDatas[dlgid];
                }
            }
        }
        return null;
    }

    private void LoadEmbattlePosData()
    {
        string dataPath = "datas/EmbattlePos.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var node = asset.Children[i] as SecurityElement;
                var embattleType = (EmbattleType)node.Attribute("typeid").ToUint();

                Dictionary<uint, Vector3> dirPos = null;
                embattleOffsetPos.TryGetValue(embattleType, out dirPos);
                if (dirPos == null)
                {
                    dirPos = new Dictionary<uint, Vector3>();
                    embattleOffsetPos.Add(embattleType, dirPos);
                }
                for (int j = 0; j < node.Children.Count; j++)
                {
                    var itemNode = node.Children[j] as SecurityElement;
                    var id = itemNode.Attribute("id").ToUint();
                    var offsetPos = itemNode.Attribute("offsetPos").ToVec3(',');

                    dirPos.Add(id, offsetPos.Value);
                }
            }
        }
    }

    public Dictionary<uint, Vector3> GetEmbattlePosData(EmbattleType embattleType)
    {
        return embattleOffsetPos[embattleType];
    }

    private void LoadSkillData()
    {
        string dataPath = "datas/Skills.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var skillNode = asset.Children[i] as SecurityElement;
                var id = skillNode.Attribute("id").ToUint();
                var items = new Dictionary<string, SkillData>();
                for (int j = 0; j < skillNode.Children.Count; j++)
                {
                    var itemNode = skillNode.Children[j] as SecurityElement;
                    var skillData = new SkillData();
                    skillData.name = itemNode.Attribute("name");
                    skillData.frameDatas = new Dictionary<uint, uint>();
                    skillData.skillParams = new SkillParamData();

                    var effectId = itemNode.Attribute("effectId");
                    if (!string.IsNullOrEmpty(effectId))
                    {
                        skillData.skillParams.effectId = effectId.ToUint();
                    }
                    var bulletId = itemNode.Attribute("bulletId");
                    if (!string.IsNullOrEmpty(bulletId))
                    {
                        skillData.skillParams.bulletId = bulletId.ToUint();
                    }
                    var animFrame = itemNode.Attribute("animFrame");
                    if (!string.IsNullOrEmpty(animFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.Anim, animFrame.ToUint());
                    }
                    var bulletFrame = itemNode.Attribute("bulletFrame");
                    if (!string.IsNullOrEmpty(bulletFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.Bullet, bulletFrame.ToUint());
                    }
                    var effectFrame = itemNode.Attribute("effectFrame");
                    if (!string.IsNullOrEmpty(effectFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.Effect, effectFrame.ToUint());
                    }
                    var soundFrame = itemNode.Attribute("soundFrame");
                    if (!string.IsNullOrEmpty(soundFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.Sound, soundFrame.ToUint());
                    }
                    var healthFrame = itemNode.Attribute("healthFrame");
                    if (!string.IsNullOrEmpty(healthFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.Health, healthFrame.ToUint());
                    }
                    var damageNumFrame = itemNode.Attribute("damageNumFrame");
                    if (!string.IsNullOrEmpty(damageNumFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.DamageNum, damageNumFrame.ToUint());
                    }
                    var beAttackFrame = itemNode.Attribute("beAttackFrame");
                    if (!string.IsNullOrEmpty(beAttackFrame))
                    {
                        skillData.frameDatas.Add((uint)SkillFrameType.BeAttack, beAttackFrame.ToUint());
                    }
                    var beAttackEffectId = itemNode.Attribute("beAttackEffectId");
                    if (!string.IsNullOrEmpty(beAttackEffectId))
                    {
                        skillData.skillParams.beAttackEffectId = beAttackEffectId.ToUint();
                    }
                    items.Add(skillData.name, skillData);
                }
                skillDatas.Add(id, items);
            }
        }
    }

    public Dictionary<string, SkillData> GetSkillData(uint roleid)
    {
        Dictionary<string, SkillData> items = null;
        skillDatas.TryGetValue(roleid, out items);
        return items;
    }

    private void LoadDungeonData()
    {
        string dataPath = "datas/Dungeons.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var chapterNode = asset.Children[i] as SecurityElement;
                var chapterData = new ChapterData();
                chapterData.id = chapterNode.Attribute("id").ToUint();
                chapterData.name = chapterNode.Attribute("name");
                chapterData.dungeonDatas = new Dictionary<uint, DungeonData>();

                for (int j = 0; j < chapterNode.Children.Count; j++)
                {
                    var dungeonNode = chapterNode.Children[j] as SecurityElement;
                    var dungeonData = new DungeonData();
                    dungeonData.id = dungeonNode.Attribute("id").ToUint();
                    dungeonData.name = dungeonNode.Attribute("name");
                    dungeonData.eventid = dungeonNode.Attribute("eventid").ToUint();
                    dungeonData.atlas = dungeonNode.Attribute("atlas");
                    dungeonData.star = dungeonNode.Attribute("star").ToUint();
                    dungeonData.drop = dungeonNode.Attribute("drop").ToList<uint>(',');
                    dungeonData.events = LoadSceneEvent(dungeonData.eventid);
                    chapterData.dungeonDatas.Add(dungeonData.id, dungeonData);
                }
                chapterDatas.Add(chapterData.id, chapterData);
            }
        }
    }

    public Dictionary<uint, ChapterData> GetChapterList()
    {
        return chapterDatas;
    }

    public DungeonData GetDungeonData(uint chapterid, uint dungeonid)
    {
        if (chapterDatas.ContainsKey(chapterid))
        {
            var chapterData = chapterDatas[chapterid];
            if (chapterData.dungeonDatas.ContainsKey(dungeonid))
            {
                return chapterData.dungeonDatas[dungeonid];
            }
        }
        return null;
    }

    private void LoadTeamData()
    {
        string dataPath = "datas/Teams.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var teamsNode = asset.Children[i] as SecurityElement;
                var teamData = new TeamData();
                teamData.id = teamsNode.Attribute("id").ToUint();
                teamData.teamNpcs = new List<TeamNpcData>();
                for (int j = 0; j < teamsNode.Children.Count; j++)
                {
                    var itemNode = teamsNode.Children[j] as SecurityElement;
                    var teamNpcData = new TeamNpcData();
                    teamNpcData.roleid = itemNode.Attribute("id").ToUint();
                    teamNpcData.hp = itemNode.Attribute("hp").ToLong();
                    teamNpcData.hpMax = itemNode.Attribute("hpMax").ToLong();
                    teamNpcData.hpInc = itemNode.Attribute("hpInc").ToUint();
                    teamNpcData.mp = itemNode.Attribute("mp").ToLong();
                    teamNpcData.mpMax = itemNode.Attribute("mpMax").ToLong();
                    teamNpcData.mpInc = itemNode.Attribute("mpInc").ToUint();
                    teamNpcData.attack = itemNode.Attribute("attack").ToUint();
                    teamNpcData.defense = itemNode.Attribute("defense").ToUint();
                    teamNpcData.skillConsume = itemNode.Attribute("skillConsume").ToLong();
                    teamNpcData.exp = itemNode.Attribute("exp").ToLong();
                    teamNpcData.money = itemNode.Attribute("money").ToLong();
                    teamNpcData.drops = itemNode.Attribute("drops").ToList<uint>('_');
                    teamData.teamNpcs.Add(teamNpcData);
                }
                teamDatas.Add(teamData.id, teamData);
            }
        }
    }

    public TeamData GetTeamData(uint teamid)
    {
        if (teamDatas.ContainsKey(teamid))
        {
            return teamDatas[teamid];
        }
        return null;
    }

    void LoadMapData()
    {
        string mapDataPath = "datas/Maps.xml";
        var asset = XmlHelper.LoadXml(mapDataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var map = asset.Children[i] as SecurityElement;
                var mapData = new MapData();
                mapData.id = map.Attribute("id").ToUint();
                mapData.type = map.Attribute("type").ToUint();
                mapData.eventid = map.Attribute("eventid").ToUint();
                mapData.events = LoadSceneEvent(mapData.eventid);
                mapData.atlas = map.Attribute("atlas");
                mapData.sound = map.Attribute("sound");
                mapDatas.Add(mapData.id, mapData);
            }
        }
    }

    public MapData GetMapData(uint id)
    {
        if (mapDatas.ContainsKey(id))
        {
            return mapDatas[id];
        }
        return null;
    }

    private List<SceneEvent> LoadSceneEvent(uint eventName)
    {
        string mapDataPath = "datas/Events/" + eventName + ".xml";
        var asset = XmlHelper.LoadXml(mapDataPath);
        if (asset == null) return null;

        var list = new List<SceneEvent>();
        for (int i = 0; i < asset.Children.Count; i++)
        {
            var evNode = asset.Children[i] as SecurityElement;
            var sceneEvent = new SceneEvent();
            sceneEvent.name = evNode.Attribute("name");
            sceneEvent.pos = evNode.Attribute("pos").ToVec2('_');
            sceneEvent.eventObjs = LoadEventObject(evNode.Attribute("eventids"));
            list.Add(sceneEvent);
        }
        return list;
    }

    List<EventData> LoadEventObject(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }
        var list = new List<EventData>();
        var keyValues = data.ToList<string>('_');
        foreach (string ev in keyValues)
        {
            string strType = ev;
            string strValue = string.Empty;
            if (ev.IndexOf(':') > -1)
            {
                var strs = ev.Split(':');
                strType = strs[0];
                strValue = strs[1];
            }
            list.Add(new EventData()
            {
                type = (EventsType)uint.Parse(strType),
                value = strValue,
            });
        }
        return list;
    }

    /// <summary>
    /// 初始化NPC数据
    /// </summary>
    void LoadNpcData()
    {
        string tankDataPath = "datas/Npcs.xml";
        var asset = XmlHelper.LoadXml(tankDataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var item = asset.Children[i] as SecurityElement;
                RoleData data = new RoleData();
                data.id = item.Attribute("id").ToUint();
                data.name = item.Attribute("name");
                data.nick = item.Attribute("nick");

                var scale_str = item.Attribute("scale");
                if (scale_str == null)
                {
                    scale_str = "1,1,1";
                }
                var scale = scale_str.Split(',');
                data.scale = new Vector3(scale[0].ToFloat(), scale[1].ToFloat(), scale[2].ToFloat());

                data.job = (JobType)item.Attribute("job").ToUint();
                var clips_str = item.Attribute("clips");
                if (clips_str != null)
                {
                    data.clips = clips_str.Split('_');
                }
                var skillid = item.Attribute("skill").ToUint();
                data.skills = GetSkillData(skillid);
                roleDatas.Add(data.id, data);
            }
        }
    }

    public RoleData GetRoleData(uint roleid)
    {
        RoleData data = null;
        roleDatas.TryGetValue(roleid, out data);
        return data;
    }

    /// <summary>
    /// 加载子弹
    /// </summary>
    void LoadBulletData()
    {
        string dataPath = "datas/Bullets.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var item = asset.Children[i] as SecurityElement;
                BulletData data = new BulletData();
                data.name = item.Attribute("name");
                data.resource = item.Attribute("resource");
                data.animName = item.Attribute("animName");
                var scale_str = item.Attribute("scale");
                if (scale_str == null)
                {
                    scale_str = "1_1_1";
                }
                var scale = scale_str.Split('_');
                data.scale = new Vector3(scale[0].ToFloat(), scale[1].ToFloat(), scale[2].ToFloat());
                data.sound = item.Attribute("sound");
                bulletDatas.Add(data.name, data);
            }
        }
    }

    public BulletData GetBulletData(string name)
    {
        BulletData data = null;
        bulletDatas.TryGetValue(name, out data);
        return data;
    }

    public Dictionary<string, BulletData> GetBulletList()
    {
        return bulletDatas;
    }

    /// <summary>
    /// 加载特效
    /// </summary>
    void LoadEffectData()
    {
        string dataPath = "datas/Effects.xml";
        var asset = XmlHelper.LoadXml(dataPath);
        if (asset != null)
        {
            for (int i = 0; i < asset.Children.Count; i++)
            {
                var item = asset.Children[i] as SecurityElement;
                EffectData data = new EffectData();
                data.name = item.Attribute("name");
                data.type = (EffectType)item.Attribute("type").ToUint();
                data.resource = item.Attribute("resource");
                data.animName = item.Attribute("animName");
                var scale_str = item.Attribute("scale");
                if (scale_str == null)
                {
                    scale_str = "1_1_1";
                }
                var scale = scale_str.Split('_');
                data.scale = new Vector3(scale[0].ToFloat(), scale[1].ToFloat(), scale[2].ToFloat());
                data.sound = item.Attribute("sound");
                effectDatas.Add(data.name, data);
            }
        }
    }

    public EffectData GetEffectData(string name)
    {
        EffectData data = null;
        effectDatas.TryGetValue(name, out data);
        return data;
    }

    public Dictionary<string, EffectData> GetEffectList()
    {
        return effectDatas;
    }

    [NoToLua]
    public override void OnUpdate(float deltaTime)
    {
    }

    [NoToLua]
    public override void OnDispose()
    {
        throw new System.NotImplementedException();
    }
}