/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50740
Source Host           : localhost:3306
Source Database       : firdatabase

Target Server Type    : MYSQL
Target Server Version : 50740
File Encoding         : 65001

Date: 2023-03-18 12:08:35
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for testdemo
-- ----------------------------
DROP TABLE IF EXISTS `testdemo`;
CREATE TABLE `testdemo` (
  `userid` int(11) NOT NULL,
  `name` varchar(20) DEFAULT NULL,
  `money` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of testdemo
-- ----------------------------
INSERT INTO `testdemo` VALUES ('1000', '张三', '999999');
