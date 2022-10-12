Shader "Unlit/VectorLineMaterial"
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
            #pragma target 4.5

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            int identifier;
            float3 mousePosition;
            int selectedIdentifier;
            uniform RWStructuredBuffer<uint> _collisionBuffer : register(u1);

            v2f vert (appdata v)
            {
                v2f o;
                float4 base = UnityObjectToClipPos(v.vertex);

                o.vertex = base;

                if (v.uv.x == 0 && v.uv.y == 0 && v.uv.z == 0)
                    return o;

                float4 extended = UnityObjectToClipPos(v.vertex + v.uv);

                float2 direction = float2(
                    extended.x / extended.w - base.x / base.w,
                    extended.y / extended.w - base.y / base.w);

                float2 directionNormalized = normalize(direction);

                float2 directionCrossed = float2(-directionNormalized.y, directionNormalized.x);

                float2 directionScaled = 8 * directionCrossed / float2(_ScreenParams.x, _ScreenParams.y);

                o.vertex += float4(directionScaled.x * base.w, directionScaled.y * base.w, 0, 0);


                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //float4 mousePos = ComputeScreenPos(i.vertex);
                //mousePos.xy /= mousePos.w;
                
                if (abs(mousePosition.x - i.vertex.x) < 1.0 && abs(mousePosition.y - i.vertex.y) < 1.0)
                {
                    int a;
                    InterlockedExchange(_collisionBuffer[0], identifier/*(int)(i.vertex.y)*/, a);
                }
                
                if (selectedIdentifier != identifier)
                {
                    return fixed4(1.0f, 0.0f, 0.0f, 1.0f);
                }
                else
                {
                    return fixed4(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
            ENDCG
        }
    }
}
