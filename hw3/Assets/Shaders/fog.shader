Shader "Unlit/fog"
{
    Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_FogDensity ("Fog Density", Float) = 1.0
	_FogColor ("Fog Color", Color) = (1, 1, 1, 1)
	_FogStart ("Fog Start", Float) = 0.0
	_FogEnd ("Fog End", Float) = 1.0
}
SubShader {
	...

	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half2 uv_depth : TEXCOORD1;				// 深度纹理
		float4 interpolatedRay : TEXCOORD2;		// 插值后的像素向量
	};
	
	v2f vert(appdata_img v) {
		...
		
		// 靠近哪个角选哪个
		int index = 0;
		if (v.texcoord.x < 0.5 && v.texcoord.y < 0.5) {
			index = 0;
		} else if (v.texcoord.x > 0.5 && v.texcoord.y < 0.5) {
			index = 1;
		} else if (v.texcoord.x > 0.5 && v.texcoord.y > 0.5) {
			index = 2;
		} else {
			index = 3;
		}

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			index = 3 - index;
		#endif
		
		o.interpolatedRay = _FrustumCornersRay[index];
		return o;
	}
	
	fixed4 frag(v2f i) : SV_Target {
		// 采样然后得到视角空间的线性深度值
		float linearDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth));
		float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.interpolatedRay.xyz;	// 世界空间相机相加
					
		float fogDensity = (_FogEnd - worldPos.y) / (_FogEnd - _FogStart); 
		fogDensity = saturate(fogDensity * _FogDensity);					// 限制在0~1
		
		fixed4 finalColor = tex2D(_MainTex, i.uv);
		finalColor.rgb = lerp(finalColor.rgb, _FogColor.rgb, fogDensity);
		
		return finalColor;
	}
	
	ENDCG
	
	Pass {
		ZTest Always Cull Off ZWrite Off
		     	
		CGPROGRAM  
		
		#pragma vertex vert  
		#pragma fragment frag  
		  
		ENDCG  
	}
} 

			FallBack "off"
}
