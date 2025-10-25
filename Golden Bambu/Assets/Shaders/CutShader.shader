Shader "Unlit/CutShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _Plane ("Plane (xyz,d)", Vector) = (0,1,0,0)
        _SplitDistance ("Split Distance", Float) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Cull Off
            HLSLPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2g
            {
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float side : TEXCOORD0;
            };

            float4 _Plane;
            float4 _Color;
            float _SplitDistance;

            // Vertex shader
            v2g vert(appdata v)
            {
                v2g o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                return o;
            }

            // Função auxiliar de emissão
            void EmitTri(inout TriangleStream<g2f> triStream, float3 A, float3 B, float3 C, float3 normal, float side, float3 offset)
            {
                g2f o;
                float3 verts[3] = { A + offset, B + offset, C + offset };
                for (int i = 0; i < 3; i++)
                {
                    o.worldNormal = normalize(normal);
                    o.pos = UnityWorldToClipPos(verts[i]);
                    o.side = side;
                    triStream.Append(o);
                }
                triStream.RestartStrip();
            }

            [maxvertexcount(12)]
            void geom(triangle v2g input[3], inout TriangleStream<g2f> triStream)
            {
                float3 n = normalize(_Plane.xyz);
                float dist[3];
                for (int i = 0; i < 3; i++)
                    dist[i] = dot(n, input[i].worldPos) + _Plane.w;

                int posCount = (dist[0] > 0) + (dist[1] > 0) + (dist[2] > 0);

                float3 offPos = n * (_SplitDistance * 0.5);
                float3 offNeg = -offPos;

                float3 N = normalize(input[0].normal + input[1].normal + input[2].normal);

                // Todos de um lado
                if (posCount == 0 || posCount == 3)
                {
                    float side = (posCount == 3) ? 1 : -1;
                    float3 offset = (side > 0) ? offPos : offNeg;
                    EmitTri(triStream, input[0].worldPos, input[1].worldPos, input[2].worldPos, N, side, offset);
                    return;
                }

                int inside[3];
                for (int i = 0; i < 3; i++)
                    inside[i] = (dist[i] > 0) ? 1 : 0;

                // 2 positivos, 1 negativo
                if (posCount == 2)
                {
                    int negIndex = (inside[0] == 0) ? 0 : ((inside[1] == 0) ? 1 : 2);
                    int i1 = (negIndex + 1) % 3;
                    int i2 = (negIndex + 2) % 3;

                    float t1 = dist[i1] / (dist[i1] - dist[negIndex]);
                    float t2 = dist[i2] / (dist[i2] - dist[negIndex]);

                    float3 P1 = lerp(input[i1].worldPos, input[negIndex].worldPos, t1);
                    float3 P2 = lerp(input[i2].worldPos, input[negIndex].worldPos, t2);

                    // lado positivo
                    EmitTri(triStream, input[i1].worldPos, input[i2].worldPos, P2, N, 1, offPos);
                    EmitTri(triStream, input[i1].worldPos, P2, P1, N, 1, offPos);

                    // lado negativo
                    EmitTri(triStream, P1, P2, input[negIndex].worldPos, N, -1, offNeg);
                }

                // 1 positivo, 2 negativos
                else if (posCount == 1)
                {
                    int posIndex = (inside[0] == 1) ? 0 : ((inside[1] == 1) ? 1 : 2);
                    int i1 = (posIndex + 1) % 3;
                    int i2 = (posIndex + 2) % 3;

                    float t1 = dist[posIndex] / (dist[posIndex] - dist[i1]);
                    float t2 = dist[posIndex] / (dist[posIndex] - dist[i2]);

                    float3 P1 = lerp(input[posIndex].worldPos, input[i1].worldPos, t1);
                    float3 P2 = lerp(input[posIndex].worldPos, input[i2].worldPos, t2);

                    // lado positivo
                    EmitTri(triStream, input[posIndex].worldPos, P1, P2, N, 1, offPos);

                    // lado negativo
                    EmitTri(triStream, P1, input[i1].worldPos, input[i2].worldPos, N, -1, offNeg);
                    EmitTri(triStream, P1, input[i2].worldPos, P2, N, -1, offNeg);
                }
            }

            fixed4 frag(g2f i) : SV_Target
            {
                float3 n = normalize(i.worldNormal);
                float3 l = normalize(float3(0.4, 0.6, 0.8));
                float diff = saturate(dot(n, l));

                return float4(_Color.rgb * (0.3 + 0.7 * diff), 1);
            }

            ENDHLSL
        }
    }
}