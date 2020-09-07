
SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `game_gamer`
-- ----------------------------
DROP TABLE IF EXISTS `game_gamer`;
CREATE TABLE `game_gamer` (
  `GamerId` binary(16) NOT NULL COMMENT '设置主键玩家guid',
  `Username` varchar(128) NOT NULL COMMENT '玩家账户名',
  `GamerDisplay` blob(0) COMMENT '玩家数据快照开服加载',
  `GamerData` longblob(0) COMMENT '玩家存档数据', 
  `CreatedTime` timestamp(0) COMMENT '创建时间', 
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
