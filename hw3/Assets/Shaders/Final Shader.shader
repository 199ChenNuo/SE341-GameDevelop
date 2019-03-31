Shader "Unlit/Final Shader"
{
	Properties{
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex("Main Tex", 2D) = "white" {}
		_Specular("Specular", Color) = (1, 1, 1, 1)
		_Shininess("Shininess", Range(8.0, 256)) = 20
		_MainColor("Main Color", Color) = (1,1,1,1)
	}
		SubShader{
			Pass {
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma shader_feature USE_COLOR
				#pragma shader_feature USE_NORMAL
				#pragma shader_feature USE_TEXTURE
				#pragma shader_feature USE_BLINN

				#include "Lighting.cginc"
			#include "UnityCG.cginc"

				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Specular;
				float _Shininess;
				float4 _MainColor;

				struct a2v {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float3 worldNormal : TEXCOORD0;
					float3 worldPos : TEXCOORD1;
					float2 uv : TEXCOORD2;
				};

				v2f vert(a2v v) {
					v2f i;
#if USE_COLOR
					
					//// old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
					// 位置： 局部坐标系 -> 屏幕空间中的位置
					i.pos = UnityObjectToClipPos(v.vertex);
					return i;
#endif
#if USE_NORMAL
					// v2f i;
					//// old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
					// 位置： 局部坐标系 -> 屏幕空间中的位置
					i.pos = UnityObjectToClipPos(v.vertex);
					// 法线方向：物体空间 -> 世界坐标系
					i.worldNormal = UnityObjectToWorldNormal(v.normal);
					return i;
#endif
#if USE_TEXTURE
					i.pos = UnityObjectToClipPos(v.vertex);
					i.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					return i;
#endif
#if USE_BLINN
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);

					o.worldNormal = UnityObjectToWorldNormal(v.normal);

					o.worldPos = UnityObjectToClipPos(v.vertex).xyz;

					o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					return o;
#endif
					i.pos = UnityObjectToClipPos(v.vertex);
					return i;
				}

				fixed4 frag(v2f i) : SV_Target {

#if USE_COLOR
					return _MainColor;
#endif
#if USE_NORMAL
					return float4(i.worldNormal, 1);
#endif
#if USE_TEXTURE
					return tex2D(_MainTex, i.uv);
#endif
#if USE_BLINN
					fixed3 worldNormal = normalize(i.worldNormal);
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

					// Use the texture to sample the diffuse color
					fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;

					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

					fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

					fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
					fixed3 halfDir = normalize(worldLightDir + viewDir);
					fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(worldNormal, halfDir)), _Shininess);

					return fixed4(ambient + diffuse + specular, 1.0);
#endif
					return _MainColor;
				}

				ENDCG
			}
		}
			// FallBack "Specular"
			CustomEditor "CustomShaderGUI"
}