Shader "Unlit/3.2 Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		// 纯色的shader使用的颜色
		// 语法： 变量名("unity中显示的名称", 变量的类型) = 变量的default value
		_MainColor("Main Color", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM

			// 指定vertex和fragment的代码函数名
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			// Color在Shader中用float4表示
			// 需要在Pass{}中再次声明
			float4 _MainColor;
			void MyVertexProgram() {}
			void MyFragmentProgram() {}

			sampler2D _MainTex;
			float4 _MainTex_ST;

			// vertex shader的输入
			struct VertexData {
				// 顶点的位置信息
				float4 position : POSITION;
				// 传入的物体的法向normal
				float3 normal : NORMAL;
			};

			// fragment shader的输入
			struct FragmentData {
				float4 position : SV_POSITION;
				// 传入的物体的法向normal
				float3 normal : TEXCOORD0;
			};

			FragmentData MyVertexProgram(VertexData v) {
				FragmentData i;
				//// old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
				// 位置： 局部坐标系 -> 屏幕空间中的位置
				i.position = UnityObjectToClipPos(v.position);
				// 法线方向：物体空间 -> 世界坐标系
				i.normal = UnityObjectToWorldNormal(v.normal);
				return i;
			}

			float4 MyFragmentProgram(FragmentData i) : SV_TARGET{
				// 直接返回_MainColor颜色
				// return _MainColor;
				// 直接返回法向方向
				return float4(i.normal, 1);
			}
		
		ENDCG
	}
	}
}

