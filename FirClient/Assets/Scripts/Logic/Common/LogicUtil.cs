using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic
{
    public static class LogicUtil
    {
        /// 计算一个点是否在一个多边形范围内
        /// 如果过该点的线段与多边形的交点不为零且距该点左右方向交点数量都为奇数时  该点再多边形范围内
        public static bool PolygonIsContainPoint(Vector3 point, List<Vector3> vertexs)
        {
            //判断测试点和横坐标方向与多边形的边的交叉点
            int leftNum = 0;  //左方向上的交叉点数
            int rightNum = 0;  //右方向上的交叉点数
            int index = 1;
            for (int i = 0; i < vertexs.Count; i++)
            {
                if (i == vertexs.Count - 1) { index = -i; }
                //找到相交的线段 
                if (point.z >= vertexs[i].z && point.z < vertexs[i + index].z || point.z < vertexs[i].z && point.z >= vertexs[i + index].z)
                {
                    Vector3 vecNor = (vertexs[i + index] - vertexs[i]);
                    //处理直线方程为常数的情况
                    if (vecNor.x == 0.0f)
                    {
                        if (vertexs[i].x < point.x)
                        {
                            leftNum++;
                        }
                        else if (vertexs[i].x == point.x)
                        {
                        }
                        else
                        {
                            rightNum++;
                        }
                    }
                    else
                    {
                        vecNor = vecNor.normalized;
                        float k = vecNor.z / vecNor.x;
                        //print(vertexs[i + index]);
                        float b = vertexs[i].z - k * vertexs[i].x;
                        if ((point.z - b) / k < point.x)
                        {
                            leftNum++;
                        }
                        else if ((point.z - b) / k == point.x)
                        {
                        }
                        else
                        {
                            rightNum++;
                        }
                    }
                }
            }
            if (leftNum % 2 != 0 || rightNum % 2 != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断当前位置是否在不规则形状里面
        /// </summary>
        /// <param name="nvert">不规则形状的定点数</param>
        /// <param name="vertx">当前x坐标</param>
        /// <param name="verty">当前y坐标</param>
        /// <param name="testx">不规则形状x坐标集合</param>
        /// <param name="testy">不规则形状y坐标集合</param>
        /// <returns></returns>
        public static bool Pnpoly(int nvert, List<double> vertx, List<double> verty, double testx, double testy)
        {
            int i, j, c = 0;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((verty[i] > testy) != (verty[j] > testy)) && (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                {
                    c = 1 + c; ;
                }
            }
            if (c % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static uint Random(uint min, uint max)
        {
            return (uint)UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
