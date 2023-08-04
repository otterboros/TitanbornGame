/// <summary>
/// This is a test shader to see what I can do with URP unity shaders.
/// </summary>
Shader "Unlit/testShader_color"
{
    Properties
    { // input data
        _Color("Color", Color) = (1,1,1,1)
        _isPowered("isPowered", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            float _isPowered;

            struct appdata // per-vertex mesh data
            {
                float4 vertex : POSITION; // vertex position
                float3 normals: NORMAL; // normals
                float2 uv : TEXCOORD0; // uv coordinates
            };

            struct v2f // data that gets passed from vertex to fragment shader (interpolators)
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : TEXCOORD0; // normals
                //float2 uv : TEXCOORD0; // used for passing data, dont have to be uvs
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // local space to clip space
                o.normal = UnityObjectToWorldNormal(v.normals); // convert normals from object to world space
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // basic test - return color
                //return float4(1,0,0,1); // red

                // return color property
                //return _Color;

                // isPowered with static colors
                //if(_isPowered == 0) {return float4(1,0,0,1);}
                //else {return float4(0,0,1,1);}

                return float4(i.normal,1);
            }
            ENDCG
        }
    }
}
