using UnityEngine;
using System.Collections;

// post-processing 需要绑定到相机上
// 同时在编辑模式下也可以执行该脚本
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
// 基本的post-processing effects
public class PostEffectsBase : MonoBehaviour
{
    protected void CheckResources()
    {
        bool isSupported = CheckSupport();
        if (!isSupported) NotSupported();
    }

    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false)
        {
            Debug.LogWarning("This platform does not support image effects or render textures.");
            return false;
        }
        return true;
    }

    protected void NotSupported()
    {
        enabled = false;
    }

    protected void Start()
    {
        CheckResources();
    }

    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null) return null;

        // 检查shader和material
        if (shader.isSupported && material && material.shader == shader)
            return material;

        if (!shader.isSupported) return null;

        material = new Material(shader);
        // 不把这个material存下来
        // material.hideFlags = HideFlags.DontSave;
        if (material)
            return material;
        return null;
    }
}