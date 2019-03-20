Shader "Unlit/4.1 Shader"
{
	Properties{
		// 物体的整体色调
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		// 物体的纹理（初始值全白）
		_MainTex("Main Tex", 2D) = "white" {}
		_Specular("Specular", Color) = (1, 1, 1, 1)
		_Gloss("Gloss", Range(8.0, 256)) = 20
	}
		SubShader{
			Pass {
				// 定义本Pass{}在Unity光照流水线中的角色
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM

				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram

				#include "Lighting.cginc"

				fixed4 _Color;
				sampler2D _MainTex;
				// ST: Scale + translation: 缩放(xy)和平移(zw)
				float4 _MainTex_ST;
				fixed4 _Specular;
				float _Gloss;

				struct a2v {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					// 模型的第一组纹理坐标
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float3 worldNormal : TEXCOORD0;
					float3 worldPos : TEXCOORD1;
					// 使用a2v中的texcoord来进行纹理采样
					float2 uv : TEXCOORD2;
				};

				v2f MyVertexProgram(a2v v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos = UnityObjectToClipPos(v.vertex).xyz;
					// 变换后的纹理坐标
					o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					return o;
				}

				fixed4 MyFragmentProgram(v2f i) : SV_Target {
					// 世界空间下的法线方向和光照方向
					fixed3 worldNormal = normalize(i.worldNormal);
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
					// 纹理采样
					fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
					// 漫反射 环境光照
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
					fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
					return fixed4(ambient + diffuse, 1.0);
				}

				ENDCG
			}
		}
			// 设置FallBack
			FallBack "Specular"
}
