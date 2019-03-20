Shader "Unlit/3.3 Shader"
{
    Properties
    {
		// 纹理属性
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram 
            #pragma fragment MyFragmentProgram
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // 再次声明纹理变量
            sampler2D _MainTex;
                    
            struct VertexData {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct FragmentData {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
			FragmentData MyVertexProgram(VertexData v) {
				FragmentData i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = v.uv;
				return i;
			}

			float4 MyFragmentProgram(FragmentData i) : SV_TARGET{
				return tex2D(_MainTex, i.uv);
			}

            ENDCG
        }
    }
}
