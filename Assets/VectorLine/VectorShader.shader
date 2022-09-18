Shader "Unlit/NewUnlitShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                float4 base = UnityObjectToClipPos(v.vertex);

                o.vertex = base;

                if (v.uv.x == 0 && v.uv.y == 0 && v.uv.z == 0)
                    return o;

                /*float Precision errors adjuster
                  Distance dependent paremeter, depends on the camera distance, fov, near far... ...
                  Very complicated stuff, we'll approximate it using an experimental method*/
                float4 mul = 0.5;

                float4 extended = UnityObjectToClipPos(v.vertex + mul * v.uv);

                float2 direction = float2(
                    extended.x / extended.w - base.x / base.w,
                    extended.y / extended.w - base.y / base.w);

                float2 directionNormalized = normalize(direction);

                float2 directionCrossed = float2(-directionNormalized.y, directionNormalized.x);

                float2 directionScaled = 8 * directionCrossed / float2(_ScreenParams.x, _ScreenParams.y);

                o.vertex += float4(directionScaled.x * base.w, directionScaled.y * base.w, 0, 0);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(1.0f,0.0f,0.0f,0.0f);
                return col;
            }
            ENDCG
        }
    }
}
