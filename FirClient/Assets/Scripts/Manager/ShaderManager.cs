using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Manager
{
    class ShaderCollectionInfo
    {
        public string shaderName;
        public ShaderVariantCollection varCols;

        public ShaderCollectionInfo(string name, ShaderVariantCollection cols)
        {
            shaderName = name;
            varCols = cols;
        }
    }
    public class ShaderManager : BaseManager
    {
        private Dictionary<string, Shader> mShaders = new Dictionary<string, Shader>();

        [NoToLua]
        public override void Initialize()
        {
        }

        public void LoadShaders(Action initOK = null)
        {
            var list = new List<ShaderCollectionInfo>();
            resMgr.LoadAssetAsync<Shader>("Shaders", null, delegate(UnityEngine.Object[] objs)
            {
                foreach (var item in objs)
                {
                    var shader = item as Shader;
                    var shaderVarList = new ShaderVariantCollection();
                    var shaderVariant = new ShaderVariantCollection.ShaderVariant();
                    shaderVariant.shader = shader;
                    shaderVarList.Add(shaderVariant);
                    list.Add(new ShaderCollectionInfo(shader.name, shaderVarList));

                    this.AddShader(shader.name, shader);
                }
                Utility.Util.StartCoroutine(InitShaderInternal(list, initOK));
            });
        }

        IEnumerator InitShaderInternal(List<ShaderCollectionInfo> shaderList, Action initOK) 
        {
            foreach (var shaders in shaderList)
            {
                if (!shaders.varCols.isWarmedUp)
                {
                    shaders.varCols.WarmUp();
                    Debug.Log("Shader WramUp:>" + shaders.shaderName);
                    yield return null;
                }
            }
            if (initOK != null)
            {
                initOK();
            }
            Debug.Log("Init Shader OK!!!");
        }

        /// <summary>
        /// 添加SHADER
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shader"></param>
        public void AddShader(string name, Shader shader)
        {
            if (!mShaders.ContainsKey(name))
            {
                mShaders.Add(name, shader);
            }
        }

        /// <summary>
        /// 获取SHADER
        /// </summary>
        /// <param name="shaderName"></param>
        /// <returns></returns>
        public Shader GetShader(string shaderName)
        {
            return mShaders.TryGetValue(shaderName, out var shader) ? shader : Shader.Find(shaderName);
        }

        [NoToLua]
        public override void OnUpdate(float deltaTime)
        {
        }

        [NoToLua]
        public override void OnDispose()
        {
        }
    }
}
