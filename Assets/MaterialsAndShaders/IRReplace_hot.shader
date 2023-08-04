/// <summary>
/// This shader changes the color of every object. 
/// The key shader for this game.
/// </summary>
Shader "Unlit/IRReplace_hot"
{
    Properties
    {
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"XRay" = "ColoredOutline"
		}

		ZWrite Off
		ZTest Always
		Blend One One

		Pass
		{
			// something about this stencil buffer line breaks the shader
			//Stencil
			//{
			//	Ref 1
			//	Comp Always
			//	Pass Replace
			//}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				// the unity_ObjectToWorld line is about transforming the vertices to world space
				return o;
			}

			float4 _EdgeColor;

			fixed4 frag(v2f i) : SV_Target
			{
				float NdotV = 1 - dot(i.normal, i.viewDir) * 0.8;
				return _EdgeColor * NdotV;
			}

			ENDCG
        }
    }
}
