using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;

enum SpecularChoice
{
    True, False
}

enum ShaderChioce
{
    COLOR_ONLY,
    NORMAL_ONLY,
    TEX_ONLY,
    BLINN_PHONG
}



public class CustomShaderGUI : ShaderGUI
{
    MaterialEditor editor;
    MaterialProperty[] properties;
    Material target;

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        this.editor = editor;
        this.properties = properties;
        this.target = editor.target as Material;

        ShaderChioce shaderChioce = ShaderChioce.BLINN_PHONG;
       
        if (target.IsKeywordEnabled("USE_COLOR"))
            shaderChioce = ShaderChioce.COLOR_ONLY;
        else if (target.IsKeywordEnabled("USE_NORMAL"))
            shaderChioce = ShaderChioce.NORMAL_ONLY;
        else if (target.IsKeywordEnabled("USE_TEXTURE"))
            shaderChioce = ShaderChioce.TEX_ONLY;
        else
            shaderChioce = ShaderChioce.BLINN_PHONG;
  
        EditorGUI.BeginChangeCheck();
        shaderChioce = (ShaderChioce)EditorGUILayout.EnumPopup(
            new GUIContent("Shader Chioce?"), shaderChioce
        );

        if (EditorGUI.EndChangeCheck())
        {
            if (shaderChioce == ShaderChioce.COLOR_ONLY)
            {
                

                onColorOnly();
            }
            if (shaderChioce == ShaderChioce.NORMAL_ONLY)
            {
                target.EnableKeyword("USE_NORMAL");
                target.DisableKeyword("USE_COLOR");
                target.DisableKeyword("USE_TEXTURE");
                target.DisableKeyword("USE_BLINN");
            }
            if (shaderChioce == ShaderChioce.TEX_ONLY)
            {
                target.EnableKeyword("USE_TEXTURE");
                target.DisableKeyword("USE_COLOR");
                target.DisableKeyword("USE_NORMAL");
                target.DisableKeyword("USE_BLINN");

                onTexOnly();
            }
            if (shaderChioce == ShaderChioce.BLINN_PHONG)
            {
                target.DisableKeyword("USE_TEXTURE");
                target.DisableKeyword("USE_COLOR");
                target.DisableKeyword("USE_NORMAL");
                target.EnableKeyword("USE_BLINN");

                onBlinn();
            }
        }
    }

    public void onColorOnly()
    {
        MaterialProperty mainColor = FindProperty("_MainColor", properties);
        GUIContent mainColorLaber = new GUIContent(mainColor.displayName);
        editor.ColorProperty(mainColor, mainColorLaber.text);

        target.EnableKeyword("USE_COLOR");
        target.DisableKeyword("USE_NORMAL");
        target.DisableKeyword("USE_TEXTURE");
        target.DisableKeyword("USE_BLINN");
    }
    public void onTexOnly()
    {
        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        GUIContent mainTexLabel = new GUIContent(mainTex.displayName);
        editor.TextureProperty(mainTex, mainTexLabel.text);
    }

    public void onBlinn()
    {
        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        GUIContent mainTexLabel = new GUIContent(mainTex.displayName);
        editor.TextureProperty(mainTex, mainTexLabel.text);

        SpecularChoice specularChoice = SpecularChoice.False;
        if (target.IsKeywordEnabled("USE_SPECULAR"))
            specularChoice = SpecularChoice.True;
        if (specularChoice == SpecularChoice.True)
        {
            MaterialProperty shininess = FindProperty("_Shininess", properties);
            GUIContent shininessLabel = new GUIContent(shininess.displayName);
            editor.FloatProperty(shininess, "Specular Factor");
        }
    }
}