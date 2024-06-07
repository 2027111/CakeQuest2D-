Shader "Custom/ColorSwapShaderURP"
{
    Properties
    {
        _MainTex("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _ColorCount("Number of Colors", Range(1, 10)) = 1
        _Tolerance("Color Match Tolerance", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            int _ColorCount;
            float4 _ColorsToReplace[10];
            float4 _ReplaceColors[10];
            float _Tolerance;

            Varyings Vert(Attributes input)
            {
                Varyings output;
                float4 positionWS = TransformObjectToWorld(input.positionOS);
                output.positionHCS = TransformWorldToHClip(positionWS);
                output.uv = input.uv;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetCameraPositionWS() - positionWS.xyz;
                return output;
            }

            // Function to convert RGB to HSV
            float3 RGBtoHSV(float3 rgb)
            {
                float3 hsv;
                float4 K = float4(0.0, -1.0/3.0, 2.0/3.0, -1.0);
                float4 p = (rgb.g < rgb.b) ? float4(rgb.bg, K.wz) : float4(rgb.gb, K.xy);
                float4 q = (rgb.r < p.x) ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                hsv.x = abs(q.z + (q.w - q.y) / (6.0 * d + e));
                hsv.y = d / (q.x + e);
                hsv.z = q.x;
                return hsv;
            }

            float colorDistance(float3 hsv1, float3 hsv2)
            {
                float hueDistance = min(abs(hsv1.x - hsv2.x), 1.0 - abs(hsv1.x - hsv2.x));
                return hueDistance + abs(hsv1.y - hsv2.y); // Ignore value (brightness)
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 baseColor = tex2D(_MainTex, input.uv) * _BaseColor;
                float3 hsvCol = RGBtoHSV(baseColor.rgb);

                for (int i = 0; i < _ColorCount; i++)
                {
                    float3 hsvColorToReplace = RGBtoHSV(_ColorsToReplace[i].rgb);
                    if (colorDistance(hsvCol, hsvColorToReplace) < _Tolerance)
                    {
                        float3 colorDifference = baseColor.rgb - _ColorsToReplace[i].rgb;
                        baseColor.rgb = _ReplaceColors[i].rgb + colorDifference;
                        break;
                    }
                }

                // Apply lighting
                half3 lighting = Lighting(input.normalWS, input.viewDirWS, half3(0.0, 0.0, 0.0), baseColor.rgb);
                return half4(lighting, baseColor.a);
            }
            ENDHLSL
        }
    }
}
