Shader "Unlit/MyShader.shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		// 纯色的shader使用的颜色
		// 语法： 变量名("unity中显示的名称", 变量的类型) = 变量的default value
		_MainColor("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

			// vertex shader的输入
			struct VertexData {
				float4 position : POSITION;
			};

			// fragment shader的输入
			struct FragmentData {
				float4 position : SV_POSITION;
			};

			FragmentData MyVertexProgram(VertexData v) {
				FragmentData i;
				//// old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
				// 局部坐标系 -> 屏幕空间中的位置
				i.position = UnityObjectToClipPos(v.position);
				return i;
			}

			// 直接返回_MainColor颜色
			float4 MyFragmentProgram(FragmentData i) : SV_TARGET{
				return _MainColor;
			}

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
