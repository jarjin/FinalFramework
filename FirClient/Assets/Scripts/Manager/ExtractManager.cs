using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FirClient.Component;
using FirClient.Define;
using FirClient.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace FirClient.Manager
{
    public class ExtractManager : BaseManager
    {
        public override void Initialize()
        {
        }

        public bool IsResNeedExtract()
        {
            bool isExists = Directory.Exists(Util.DataPath) &&
                            Directory.Exists(Util.DataPath + "datas/") && 
                            Directory.Exists(Util.DataPath + "scripts/") && 
                            File.Exists(Util.DataPath + "files.txt");
            return isExists == false;
        }

        /// <summary>
        /// 释放文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IEnumerator ExtractFile(string fileName) 
        {
            string dataPath = Util.DataPath;  //数据目录
            string streamPath = Util.AppContentPath(); //游戏包资源目录

            string infile = streamPath + fileName;
            string outfile = dataPath + fileName;
            Debug.Log("正在解包文件:>" + infile);
            if (File.Exists(outfile)) File.Delete(outfile);

            if (Application.platform == RuntimePlatform.Android) 
            {
                var www = UnityWebRequest.Get(infile);
                yield return www.SendWebRequest();

                if (www.isDone) 
                {
                    File.WriteAllBytes(outfile, www.downloadHandler.data);
                }
            } 
            else 
            {
                File.Copy(infile, outfile, true);
            }
        }

        public IEnumerator OnResExtract(Action extractOK) 
        {
            string dataPath = Util.DataPath;  //数据目录
            string streamPath = Util.AppContentPath(); //游戏包资源目录

            if (Directory.Exists(dataPath)) 
            {
                Directory.Delete(dataPath, true);
            }
            Directory.CreateDirectory(dataPath);

            yield return StartCoroutine(ExtractFile("files.txt"));
            yield return StartCoroutine(ExtractFile("version.txt"));

            //释放所有文件到数据目录
            var zipFiles = new List<string>();
            string[] files = File.ReadAllLines(dataPath + "files.txt");
            foreach (var file in files) 
            {
                if (string.IsNullOrEmpty(file) || !file.Contains("|")) 
                {
                    continue;
                }
                string[] fs = file.Split('|');
                var location = (ResPlaceType)int.Parse(fs[2]);
                if (location == ResPlaceType.StreamAsset) 
                {
                    continue;
                }
                foreach(string prefix in AppConst.DataPrefixs)
                {
                    if (fs[0].StartsWith(prefix))
                    {
                        zipFiles.Add(fs[0]);
                    }
                }
                var outfile = dataPath + fs[0];
                string dir = Path.GetDirectoryName(outfile);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                yield return StartCoroutine(ExtractFile(fs[0]));
            }

            ///解压缩数据文件
            if (zipFiles.Count > 0) 
            {
                ProcZipFiles(zipFiles, extractOK);
            }
            else
            {
                if (extractOK != null) {
                    extractOK();
                }
            }
        }

        /// <summary>
        /// 处理压缩文件
        /// </summary>
        /// <param name="zipFiles"></param>
        /// <param name="extractOK"></param>
        void ProcZipFiles(List<string> zipFiles, Action extractOK) 
        {
            var zip = CZip.Create();
            foreach(var file in zipFiles) 
            {
                var zipfile = Util.DataPath + file;
                if (File.Exists(zipfile)) 
                {
                    var str = file.Replace(".zip", "");
                    var strs = str.Split('_');
                    var outdir = Util.DataPath + strs[0];
                    var fileCount = uint.Parse(strs[1]);

                    zip.AddUnzip(zipfile, outdir, fileCount);
                }
            }
            /// 开始解压文件
            var loadingText = "正在解压资源，请稍等...(此过程不消耗流量)";
            zip.DoUnzip(delegate(float curr, float max) 
            {
                Util.UpdateLoadingProgress(loadingText, curr, max);
                if (curr >= max) 
                {
                    foreach(var file in zipFiles) 
                    {
                        var zipfile = Util.DataPath + file;
                        if (File.Exists(zipfile)) 
                        {
                            File.Delete(zipfile);
                        }
                    }
                    if (extractOK != null) {
                        extractOK();
                    }
                }
            });
        }
        
        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnDispose()
        {
        }
    }
}