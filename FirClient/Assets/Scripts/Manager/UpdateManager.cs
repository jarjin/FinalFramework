using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FirClient.Component;
using FirClient.Define;
using FirClient.UI;
using FirClient.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace FirClient.Manager
{
    public class UpdateManager : BaseManager
    {
        public override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator OnResUpdate(Action updateOK)
        {
            if (!AppConst.UpdateMode)
            {
                updateOK.Invoke();
                yield break;
            }
            var verfile = Util.DataPath + "version.txt";
            var localVerInfo = Util.GetVersionInfo(verfile);
            if (localVerInfo == null)
            {
                OnUpdateFailed("localVerInfo was null!!!");
                yield break;
            }
            long localPatchVer = long.Parse(localVerInfo.patchVersion); //本地版本号

            ///获取版本列表文件内容
            string versionUrl = AppConst.ResUrl + "version.txt?v=" + Util.RandomTime();
            Debug.LogWarning("LoadVersionUrl---->>>" + versionUrl);

            var versionText = new DownloadText();
            yield return StartCoroutine(GetDownloadText(versionUrl, versionText));
            var remoteVerInfo = Util.InitVersionInfo(versionText.text);
            if (remoteVerInfo == null)
            {
                OnUpdateFailed("remoteVerInfo was null!!!");
                yield break;
            }

            ///比较版本，是否需要整包更新
            var localMainVer = int.Parse(localVerInfo.mainVersion);
            var remoteMainVer = int.Parse(remoteVerInfo.mainVersion);
            var localPrimaryVer = int.Parse(localVerInfo.primaryVersion);
            var remotePrimaryVer = int.Parse(remoteVerInfo.primaryVersion);
            if (localMainVer < remoteMainVer || localPrimaryVer < remotePrimaryVer)
            {
                Debug.LogError("版本太老，需要整包资源更新，才可以继续游戏。。");
                yield break;
            }

            //获取补丁列表文件内容
            var patchsFile = "patchs_" + remoteVerInfo.mainVersion + "_" + remoteVerInfo.primaryVersion + ".txt";
            string patchsUrl = AppConst.PatchUrl + patchsFile + "?v=" + Util.RandomTime();
            Debug.LogWarning("LoadPatchsUrl---->>>" + patchsUrl);

            var patchsText = new DownloadText();
            yield return StartCoroutine(GetDownloadText(patchsUrl, patchsText));

            var lines = patchsText.text.Split('\n');

            var zipFiles = new List<string>();
            var loadingText = "正在更新资源，请稍等...";
            Util.UpdateLoadingProgress(loadingText, 0, lines.Length);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                var strs = line.Split('|');
                var file = Path.GetFileNameWithoutExtension(strs[0]);

                var remoteStrs = file.Split('_');
                var remoteVer = long.Parse(remoteStrs[1].Trim());   //远程版本号

                if (remoteVer <= localPatchVer) continue;

                zipFiles.Add(file + ".zip");
                var outfile = Util.DataPath + file + ".zip";

                yield return StartCoroutine(DownloadFile(strs[0], outfile));
                Util.UpdateLoadingProgress(loadingText, i + 1, lines.Length);
            }
            var localVerFile = Util.DataPath + "version.txt";
            var remoteVerStr = zipFiles.Count > 0 ? versionText.text : string.Empty;
            ProcZipFiles(zipFiles, () => OnUpdateOK(localVerFile, remoteVerStr, updateOK));
        }

        /// <summary>
        /// 更新完成
        /// </summary>
        /// <param name="verfile"></param>
        /// <param name="remoteVerStr"></param>
        /// <param name="updateOK"></param>
        void OnUpdateOK(string verfile, string remoteVerStr, Action updateOK)
        {
            if (string.IsNullOrEmpty(remoteVerStr))
            {
                Debug.LogWarning("没有发现可更新资源，更新完成！");
            }
            else
            {
                File.WriteAllText(verfile, remoteVerStr);
                Debug.LogWarning("更新完成！更新版本号：" + remoteVerStr);
            }
            if (updateOK != null)
            {
                updateOK();
            }
        }

        /// <summary>
        /// 处理压缩文件
        /// </summary>
        /// <param name="zipFiles"></param>
        /// <param name="updateOK"></param>
        void ProcZipFiles(List<string> zipFiles, Action updateOK)
        {
            if (zipFiles.Count == 0)
            {
                if (updateOK != null)
                {
                    updateOK();
                }
            }
            var zip = CZip.Create();
            foreach (var file in zipFiles)
            {
                var zipfile = Util.DataPath + file;
                if (File.Exists(zipfile))
                {
                    var str = file.Replace(".zip", "");
                    var strs = str.Split('_');
                    var fileCount = uint.Parse(strs[2]);
                    zip.AddUnzip(zipfile, Util.DataPath, fileCount);
                }
            }
            /// <summary>
            /// 开始解压文件
            /// </summary>
            var loadingText = "正在解压资源，请稍等...(此过程不消耗流量)";
            zip.DoUnzip(delegate (float curr, float max)
            {
                Util.UpdateLoadingProgress(loadingText, curr, max);
                if (curr >= max)
                {
                    foreach (var file in zipFiles)
                    {
                        var zipfile = Util.DataPath + file;
                        if (File.Exists(zipfile))
                        {
                            File.Delete(zipfile);
                        }
                    }
                    if (updateOK != null)
                    {
                        updateOK();
                    }
                }
            });
        }

        /// <summary>
        /// 线程下载
        /// </summary>
        IEnumerator DownloadFile(string url, string file)
        {
            Debug.Log("DownloadFile:>" + file);
            using (var www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError)
                {
                    OnUpdateFailed(string.Empty);
                    yield break;
                }
                while (!www.isDone)
                {
                    var v = Math.Floor(www.downloadProgress * 100) + "%";
                    Debug.Log(url + " " + v);
                    yield return null;
                }
                if (www.isDone)
                {
                    File.WriteAllBytes(file, www.downloadHandler.data);
                    Debug.Log(url + " " + 100 + "%");
                }
            }
        }

        IEnumerator GetDownloadText(string url, DownloadText retText)
        {
            var www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                OnUpdateFailed(www.error);
                yield break;
            }
            retText.text = www.downloadHandler.text;
        }

        void OnUpdateFailed(string file)
        {
            Debug.LogError("更新失败!>" + file);
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
    }
}

