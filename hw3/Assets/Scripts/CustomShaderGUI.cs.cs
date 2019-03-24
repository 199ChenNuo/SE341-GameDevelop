using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
using System;

enum SpecularChoice
{
    True, False
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

        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        GUIContent mainTexLabel = new GUIContent(mainTex.displayName);
        editor.TextureProperty(mainTex, mainTexLabel.text);

        SpecularChoice specularChoice = SpecularChoice.False;
        if (target.IsKeywordEnabled("USE_SPECULAR"))
            specularChoice = SpecularChoice.True;

        EditorGUI.BeginChangeCheck();
        specularChoice = (SpecularChoice)EditorGUILayout.EnumPopup(
            new GUIContent("Use Specular?"), specularChoice
        );

        if (EditorGUI.EndChangeCheck())
        {
            if (specularChoice == SpecularChoice.True)
                target.EnableKeyword("USE_SPECULAR");
            else
                target.DisableKeyword("USE_SPECULAR");
        }

        if (specularChoice == SpecularChoice.True)
        {
            MaterialProperty shininess = FindProperty("_Shininess", properties);
            GUIContent shininessLabel = new GUIContent(shininess.displayName);
            editor.FloatProperty(shininess, "Specular Factor");
        }
    }
}