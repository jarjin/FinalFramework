using System.IO;
using UnityEditor;
using UnityEngine;

public class ModelPostImporter : BaseEditor
{
    public static void PreProcModel(string assetPath, ref ModelImporter importer)
    {
        if (assetPath.Contains("@"))
        {
            importer.materialImportMode = ModelImporterMaterialImportMode.None;
        }
    }

    public static void PostProcModel(string assetPath, GameObject model)
    {
        if (!assetPath.Contains("@"))
        {
            var renderers = model.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterial.name != model.name)
                {
                    Debug.LogError("材质名和模型名不匹配！:>" + model);
                    //FileUtil.DeleteFileOrDirectory(Application.dataPath+assetPath.Replace("Assets",""));
                    AssetDatabase.Refresh();
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 提取动画MESH
    /// </summary>
    /// <param name="respath"></param>
    /// <param name="saveDir"></param>
    static void ExtractAnimMesh(string respath, string saveDir = null)
    {
        respath = GetUnityAssetPath(respath);
        if (!string.IsNullOrEmpty(respath) && respath.ToLower().EndsWith(".fbx"))
        {
            GameObject target = AssetDatabase.LoadAssetAtPath<GameObject>(respath);
            if (target != null)
            {
                string dir = saveDir == null ? Path.GetDirectoryName(respath) : GetUnityAssetPath(saveDir);
                string filename = Path.GetFileNameWithoutExtension(respath);
                GameObject instance = GameObject.Instantiate<GameObject>(target);
                MeshFilter[] meshes = instance.GetComponentsInChildren<MeshFilter>(true);
                for (int i = 0; i < meshes.Length; i++)
                {
                    Mesh oldmesh = meshes[i].sharedMesh;
                    Mesh newmesh = new Mesh();
                    newmesh.vertices = oldmesh.vertices;
                    newmesh.normals = oldmesh.normals;
                    newmesh.triangles = oldmesh.triangles;
                    newmesh.tangents = oldmesh.tangents;
                    newmesh.colors = oldmesh.colors;
                    newmesh.uv = oldmesh.uv;
                    newmesh.uv2 = oldmesh.uv2;
                    newmesh.uv3 = oldmesh.uv3;
                    newmesh.uv4 = oldmesh.uv4;
                    newmesh.uv5 = oldmesh.uv5;
                    newmesh.uv6 = oldmesh.uv6;
                    newmesh.uv7 = oldmesh.uv7;
                    newmesh.uv8 = oldmesh.uv8;

                    MeshUtility.Optimize(newmesh);
                    MeshUtility.SetMeshCompression(newmesh, ModelImporterMeshCompression.High);

                    meshes[i].sharedMesh = newmesh;
                    AssetDatabase.CreateAsset(newmesh, dir + "/" + filename + "_mesh_" + i + ".asset");
                }
                PrefabUtility.SaveAsPrefabAsset(instance, dir + "/" + filename + ".prefab");
                GameObject.DestroyImmediate(instance);
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 提取动画剪辑
    /// </summary>
    /// <param name="respath"></param>
    static void ExtractAnimClip(string respath)
    {
        var objs = AssetDatabase.LoadAllAssetsAtPath(respath);
        foreach(var obj in objs)
        {
            if (obj.GetType() == typeof(AnimationClip))
            {
                var srcClip = obj as AnimationClip;
                if (srcClip != null)
                {
                    srcClip.legacy = true;

                    AnimationClip newClip = new AnimationClip();
                    EditorUtility.CopySerialized(srcClip, newClip);

                    newClip.frameRate = srcClip.frameRate;
                    if (newClip.wrapMode != WrapMode.Loop)
                    {
                        newClip.wrapMode = WrapMode.Once;
                    }
                    OptmizeAnimationFloat(ref srcClip, ref newClip);
                    OptmizeAnimationScaleCurve(ref srcClip, ref newClip);

                    string outpath = Path.GetDirectoryName(respath) + "/" + srcClip.name + ".anim";
                    AssetDatabase.CreateAsset(newClip, outpath);
                }
                else
                {
                    Debug.LogError("没有发现动画剪辑:" + respath);
                }
            }
        }
        AssetDatabase.SaveAssets();//保存修改
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 优化浮点数精度压缩到f3
    /// </summary>
    static void OptmizeAnimationFloat(ref AnimationClip srcClip, ref AnimationClip newClip)
    {
        var curves = AnimationUtility.GetAllCurves(srcClip);
        for (int i = 0; i < curves.Length; ++i)
        {
            AnimationClipCurveData curveDate = curves[i];
            if (curveDate.curve == null || curveDate.curve.keys == null)
            {
                //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                continue;
            }
            var keyFrames = curveDate.curve.keys;
            for (int j = 0; j < keyFrames.Length; j++)
            {
                var key = keyFrames[j];
                key.value = float.Parse(key.value.ToString("f3"));
                key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                keyFrames[j] = key;
            }
            curveDate.curve.keys = keyFrames;
            newClip.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
        }
    }

    /// <summary>
    /// 优化scale曲线
    /// </summary>
    static void OptmizeAnimationScaleCurve(ref AnimationClip srcClip, ref AnimationClip newClip)
    {
        var setting = AnimationUtility.GetAnimationClipSettings(srcClip);
        AnimationUtility.SetAnimationClipSettings(newClip, setting);
        var curveBindings = AnimationUtility.GetCurveBindings(srcClip);
        for (int i = 0; i < curveBindings.Length; i++)
        {
            string name = curveBindings[i].propertyName.ToLower();
            if (name.Contains("scale"))
            {
                var curve = AnimationUtility.GetEditorCurve(srcClip, curveBindings[i]);
                AnimationUtility.SetEditorCurve(newClip, curveBindings[i], curve);
            }
        }
    }
}
