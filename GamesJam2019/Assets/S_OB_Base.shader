Shader "OakboundShading/ToonBase" 
{
	//Custom Oakbound shader used for development. Characters will use the outline function, other object will not.
	Properties
	{
		_MainTex("Base Colour", 2D) = "white" {}
		_Occlusion("Ambient Occlusion", 2D) = "white"{}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Emissive("Emissive", 2D) = "black" {}
		_RampTex("Ramp", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0

		_OutlineColour("Outline Colour", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(0.0, 0.05)) = 0.005

		_TintColour("TintColour", Color) = (1,1,1,1)
		
		_HueShift("Hue (0-360)", Float) = 0
	}

	SubShader
	{
			Tags
			{
				"Queue" = "AlphaTest" 
				"RenderType" = "TransparentCutout" 
				"IgnoreProjector" = "True" 
			}
			Cull Off

			LOD 200
			
			Pass  //Outline Pass
			{
				Tags
				{ 
					"RenderType" = "Opaque" 
				}
				

				Cull Front //Culling front faces

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f
				{
					float4 pos : SV_POSITION;
				};

				float _OutlineWidth, _OutlineVisibility;
				float4 _OutlineColour;

				float4 vert(appdata_base v) : SV_POSITION //vertex function
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
					normal.x *= UNITY_MATRIX_P[0][0];
					normal.y *= UNITY_MATRIX_P[1][1];
					o.pos.xy += normal.xy * _OutlineWidth;
					return o.pos;
				}

				half4 frag(v2f i) : COLOR //fragment function
				{

					return _OutlineColour;
				}

					ENDCG
		}

			CGPROGRAM
			#pragma surface surf Toon
			#pragma target 3.0

			float _HueShift;

			//Hueshift Function
			float3 shift_Col(float3 RGB, float3 shift)
			{
				float3 RESULT = float3(RGB);
				float VSU = shift.z*shift.y*cos(shift.x *3.14159265 / 180);
				float VSW = shift.z*shift.y*sin(shift.x*3.14159265 / 180);

				RESULT.x = (.299*shift.z + .701*VSU + .168*VSW)*RGB.x + (.587*shift.z - .587*VSU + .330*VSW)*RGB.y + (.114*shift.z - .114*VSU - .497*VSW)*RGB.z;

				RESULT.y = (.299*shift.z - .299*VSU - .328*VSW)*RGB.x + (.587*shift.z + .413*VSU + .035*VSW)*RGB.y + (.114*shift.z - .114*VSU + .292*VSW)*RGB.z;

				RESULT.z = (.299*shift.z - .3*VSU + 1.25*VSW)*RGB.x + (.587*shift.z - .588*VSU - 1.05*VSW)*RGB.y + (.114*shift.z + .886*VSU - .203*VSW)*RGB.z;

				return (RESULT);
			}

			struct Input //Used for passing parameters
			{
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float2 uv_Emissive;
				float3 viewDir;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _Occlusion;
			sampler2D _Emissive;

			float4 _RimColor;
			float _RimPower;
			float4 _TintColour;

			void surf(Input IN, inout SurfaceOutput o) // Surface Function
			{
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

				float3 hsv;
				hsv.x = _HueShift;

				//Creating locals to pass the parameters through
				fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 Occ = tex2D(_Occlusion, IN.uv_MainTex);
				fixed4 Emis = tex2D(_Emissive, IN.uv_Emissive);
				fixed3 Norm = tex2D(_BumpMap, IN.uv_BumpMap);

				//Squaring the occlusion maps to add emphasis on the baked shadows.
				fixed4 OccSquared = Occ * Occ;

				fixed4 b;

				o.Emission = Emis.rgb + _RimColor.rgb * pow(rim, _RimPower);
				o.Normal = UnpackNormal(tex2D(_BumpMap, Norm));

				b.rgb = tex.rgb * OccSquared.rgb * _TintColour.rgb;

				b.rgb = shift_Col(b.rgb, hsv);

				//Baking shadow information onto the albedo
				o.Albedo = tex.rgb * OccSquared.rgb * _TintColour.rgb;
				//o.Albedo = b.rgb;


			}

			
			sampler2D _RampTex; //Used for shadow colour

			//Toon lighting
			fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				half NdotL = dot(s.Normal, lightDir);
				NdotL = tex2D(_RampTex, fixed2(NdotL, 0.5));

				fixed4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten * 2;
				c.a = s.Alpha;

				return c;
			}

			ENDCG
		}
		FallBack "Diffuse"
}