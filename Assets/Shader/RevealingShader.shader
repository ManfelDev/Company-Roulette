Shader "Custom/RevealingShader_URP_XR"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _LightDirection("Light Direction", Vector) = (0,0,1,0)
        _LightPosition("Light Position", Vector) = (0,0,0,0)
        _LightAngle("Light Angle", Range(0,180)) = 45
        _StrengthScalar("Strength", Float) = 50
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID // For XR Instancing
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO // For XR Instancing
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float4 _Color;
            float _Glossiness;
            float _Metallic;

            float3 _LightDirection;
            float3 _LightPosition;
            float _LightAngle;
            float _StrengthScalar;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);  // Setup for XR instancing
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                // Convert object space to world space
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                
                // Convert world position to homogeneous clip space for XR
                OUT.positionCS = TransformWorldToHClip(OUT.worldPos);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN); // Ensure proper stereo eye indexing

                // Get the light direction
                float3 lightDir = normalize(_LightPosition - IN.worldPos);
                float scale = dot(lightDir, normalize(_LightDirection));
                float strength = scale - cos(_LightAngle * (3.14159 / 360.0));
                strength = saturate(strength * _StrengthScalar);

                // Sample the main texture
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half3 albedo = texColor.rgb * _Color.rgb;

                // Emission effect based on strength
                half3 emission = albedo * texColor.a * strength;

                // Apply PBR-like shading with metallic and smoothness
                half3 finalColor = albedo + emission;

                return half4(finalColor, strength * texColor.a);
            }
            ENDHLSL
        }
    }
}
