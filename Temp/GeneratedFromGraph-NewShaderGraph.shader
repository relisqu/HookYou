Shader "New Shader Graph"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _WindIntensity("WindIntensity", Range(0, 5)) = 0.88
        Vector1_579b3daaec3a44e6a0573e2228bb0d99("WindPower", Range(1, 20)) = 3
        Vector1_d00ae83b125047dbae9f32cf0caeaa41("WindDirection", Range(0, 1)) = 0.94
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_SCREENPOSITION
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITELIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            output.interp2.xyzw =  input.screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            output.screenPosition = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _WindIntensity;
        float Vector1_579b3daaec3a44e6a0573e2228bb0d99;
        float Vector1_d00ae83b125047dbae9f32cf0caeaa41;
        CBUFFER_END

        // Object and Global properties
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        SAMPLER(SamplerState_Linear_Repeat);

            // Graph Functions
            
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }


        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }

        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        { 
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Preview_float(float In, out float Out)
        {
            Out = In;
        }

        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Split_51004472986b4dbea479f5e791fa26f6_R_1 = IN.ObjectSpacePosition[0];
            float _Split_51004472986b4dbea479f5e791fa26f6_G_2 = IN.ObjectSpacePosition[1];
            float _Split_51004472986b4dbea479f5e791fa26f6_B_3 = IN.ObjectSpacePosition[2];
            float _Split_51004472986b4dbea479f5e791fa26f6_A_4 = 0;
            float3 _Add_bf856e09b8a24257b71514c6f20af139_Out_2;
            Unity_Add_float3(IN.ObjectSpacePosition, (IN.TimeParameters.x.xxx), _Add_bf856e09b8a24257b71514c6f20af139_Out_2);
            float _GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2;
            Unity_GradientNoise_float((_Add_bf856e09b8a24257b71514c6f20af139_Out_2.xy), 1, _GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2);
            float _Property_94626c5b85684beea221fb2d679cd5d4_Out_0 = Vector1_d00ae83b125047dbae9f32cf0caeaa41;
            float _Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2;
            Unity_Subtract_float(_GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2, _Property_94626c5b85684beea221fb2d679cd5d4_Out_0, _Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2);
            float _Property_183b09d4dac740699dc42464ab7afb6f_Out_0 = _WindIntensity;
            float _Multiply_e8b7a0f54178487da0ea535473128d69_Out_2;
            Unity_Multiply_float(_Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2, _Property_183b09d4dac740699dc42464ab7afb6f_Out_0, _Multiply_e8b7a0f54178487da0ea535473128d69_Out_2);
            float4 _UV_70ab25ddbc204c029b847c3656faa244_Out_0 = IN.uv0;
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_R_1 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[0];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_G_2 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[1];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_B_3 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[2];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_A_4 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[3];
            float _Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1;
            Unity_Preview_float(_Split_d9fe4765658749cfafc1c24080a9a7a4_G_2, _Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1);
            float _Property_c109c6ea19974f558c5d46fd7cc21b4c_Out_0 = Vector1_579b3daaec3a44e6a0573e2228bb0d99;
            float _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2;
            Unity_Power_float(_Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1, _Property_c109c6ea19974f558c5d46fd7cc21b4c_Out_0, _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2);
            float _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2;
            Unity_Multiply_float(_Multiply_e8b7a0f54178487da0ea535473128d69_Out_2, _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2, _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2);
            float _Add_475bd07f40da493886567780b04a3fce_Out_2;
            Unity_Add_float(_Split_51004472986b4dbea479f5e791fa26f6_R_1, _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2, _Add_475bd07f40da493886567780b04a3fce_Out_2);
            float4 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4;
            float3 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGB_5;
            float2 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RG_6;
            Unity_Combine_float(_Add_475bd07f40da493886567780b04a3fce_Out_2, _Split_51004472986b4dbea479f5e791fa26f6_G_2, _Split_51004472986b4dbea479f5e791fa26f6_B_3, _Split_51004472986b4dbea479f5e791fa26f6_A_4, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGB_5, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RG_6);
            description.Position = (_Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4.xyz);
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float4 SpriteMask;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0.tex, _Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_R_4 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.r;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_G_5 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.g;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_B_6 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.b;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_A_7 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_A_7;
            surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS;
            output.ObjectSpacePosition =         input.positionOS;
            output.uv0 =                         input.uv0;
            output.TimeParameters =              _TimeParameters.xyz;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





            output.uv0 =                         input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Sprite Normal"
            Tags
            {
                "LightMode" = "NormalsRendering"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITENORMAL
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 TangentSpaceNormal;
            float4 uv0;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _WindIntensity;
        float Vector1_579b3daaec3a44e6a0573e2228bb0d99;
        float Vector1_d00ae83b125047dbae9f32cf0caeaa41;
        CBUFFER_END

        // Object and Global properties
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        SAMPLER(SamplerState_Linear_Repeat);

            // Graph Functions
            
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }


        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }

        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        { 
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Preview_float(float In, out float Out)
        {
            Out = In;
        }

        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Split_51004472986b4dbea479f5e791fa26f6_R_1 = IN.ObjectSpacePosition[0];
            float _Split_51004472986b4dbea479f5e791fa26f6_G_2 = IN.ObjectSpacePosition[1];
            float _Split_51004472986b4dbea479f5e791fa26f6_B_3 = IN.ObjectSpacePosition[2];
            float _Split_51004472986b4dbea479f5e791fa26f6_A_4 = 0;
            float3 _Add_bf856e09b8a24257b71514c6f20af139_Out_2;
            Unity_Add_float3(IN.ObjectSpacePosition, (IN.TimeParameters.x.xxx), _Add_bf856e09b8a24257b71514c6f20af139_Out_2);
            float _GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2;
            Unity_GradientNoise_float((_Add_bf856e09b8a24257b71514c6f20af139_Out_2.xy), 1, _GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2);
            float _Property_94626c5b85684beea221fb2d679cd5d4_Out_0 = Vector1_d00ae83b125047dbae9f32cf0caeaa41;
            float _Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2;
            Unity_Subtract_float(_GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2, _Property_94626c5b85684beea221fb2d679cd5d4_Out_0, _Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2);
            float _Property_183b09d4dac740699dc42464ab7afb6f_Out_0 = _WindIntensity;
            float _Multiply_e8b7a0f54178487da0ea535473128d69_Out_2;
            Unity_Multiply_float(_Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2, _Property_183b09d4dac740699dc42464ab7afb6f_Out_0, _Multiply_e8b7a0f54178487da0ea535473128d69_Out_2);
            float4 _UV_70ab25ddbc204c029b847c3656faa244_Out_0 = IN.uv0;
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_R_1 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[0];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_G_2 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[1];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_B_3 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[2];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_A_4 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[3];
            float _Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1;
            Unity_Preview_float(_Split_d9fe4765658749cfafc1c24080a9a7a4_G_2, _Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1);
            float _Property_c109c6ea19974f558c5d46fd7cc21b4c_Out_0 = Vector1_579b3daaec3a44e6a0573e2228bb0d99;
            float _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2;
            Unity_Power_float(_Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1, _Property_c109c6ea19974f558c5d46fd7cc21b4c_Out_0, _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2);
            float _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2;
            Unity_Multiply_float(_Multiply_e8b7a0f54178487da0ea535473128d69_Out_2, _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2, _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2);
            float _Add_475bd07f40da493886567780b04a3fce_Out_2;
            Unity_Add_float(_Split_51004472986b4dbea479f5e791fa26f6_R_1, _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2, _Add_475bd07f40da493886567780b04a3fce_Out_2);
            float4 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4;
            float3 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGB_5;
            float2 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RG_6;
            Unity_Combine_float(_Add_475bd07f40da493886567780b04a3fce_Out_2, _Split_51004472986b4dbea479f5e791fa26f6_G_2, _Split_51004472986b4dbea479f5e791fa26f6_B_3, _Split_51004472986b4dbea479f5e791fa26f6_A_4, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGB_5, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RG_6);
            description.Position = (_Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4.xyz);
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0.tex, _Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_R_4 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.r;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_G_5 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.g;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_B_6 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.b;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_A_7 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_A_7;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS;
            output.ObjectSpacePosition =         input.positionOS;
            output.uv0 =                         input.uv0;
            output.TimeParameters =              _TimeParameters.xyz;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.uv0 =                         input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Sprite Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 TangentSpaceNormal;
            float4 uv0;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _WindIntensity;
        float Vector1_579b3daaec3a44e6a0573e2228bb0d99;
        float Vector1_d00ae83b125047dbae9f32cf0caeaa41;
        CBUFFER_END

        // Object and Global properties
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        SAMPLER(SamplerState_Linear_Repeat);

            // Graph Functions
            
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }


        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }

        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        { 
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Preview_float(float In, out float Out)
        {
            Out = In;
        }

        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Split_51004472986b4dbea479f5e791fa26f6_R_1 = IN.ObjectSpacePosition[0];
            float _Split_51004472986b4dbea479f5e791fa26f6_G_2 = IN.ObjectSpacePosition[1];
            float _Split_51004472986b4dbea479f5e791fa26f6_B_3 = IN.ObjectSpacePosition[2];
            float _Split_51004472986b4dbea479f5e791fa26f6_A_4 = 0;
            float3 _Add_bf856e09b8a24257b71514c6f20af139_Out_2;
            Unity_Add_float3(IN.ObjectSpacePosition, (IN.TimeParameters.x.xxx), _Add_bf856e09b8a24257b71514c6f20af139_Out_2);
            float _GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2;
            Unity_GradientNoise_float((_Add_bf856e09b8a24257b71514c6f20af139_Out_2.xy), 1, _GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2);
            float _Property_94626c5b85684beea221fb2d679cd5d4_Out_0 = Vector1_d00ae83b125047dbae9f32cf0caeaa41;
            float _Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2;
            Unity_Subtract_float(_GradientNoise_bed315e072fd423fae71e095f50230d0_Out_2, _Property_94626c5b85684beea221fb2d679cd5d4_Out_0, _Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2);
            float _Property_183b09d4dac740699dc42464ab7afb6f_Out_0 = _WindIntensity;
            float _Multiply_e8b7a0f54178487da0ea535473128d69_Out_2;
            Unity_Multiply_float(_Subtract_abd4637789ed474a8002f51c5fe641c1_Out_2, _Property_183b09d4dac740699dc42464ab7afb6f_Out_0, _Multiply_e8b7a0f54178487da0ea535473128d69_Out_2);
            float4 _UV_70ab25ddbc204c029b847c3656faa244_Out_0 = IN.uv0;
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_R_1 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[0];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_G_2 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[1];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_B_3 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[2];
            float _Split_d9fe4765658749cfafc1c24080a9a7a4_A_4 = _UV_70ab25ddbc204c029b847c3656faa244_Out_0[3];
            float _Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1;
            Unity_Preview_float(_Split_d9fe4765658749cfafc1c24080a9a7a4_G_2, _Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1);
            float _Property_c109c6ea19974f558c5d46fd7cc21b4c_Out_0 = Vector1_579b3daaec3a44e6a0573e2228bb0d99;
            float _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2;
            Unity_Power_float(_Preview_fe8bc09fa81b4a0bbc1cc014cc7a13af_Out_1, _Property_c109c6ea19974f558c5d46fd7cc21b4c_Out_0, _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2);
            float _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2;
            Unity_Multiply_float(_Multiply_e8b7a0f54178487da0ea535473128d69_Out_2, _Power_d40387fd4f0046e9be47e9fc47e67d16_Out_2, _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2);
            float _Add_475bd07f40da493886567780b04a3fce_Out_2;
            Unity_Add_float(_Split_51004472986b4dbea479f5e791fa26f6_R_1, _Multiply_2b98d3ae542e424caf755ffd49aae40f_Out_2, _Add_475bd07f40da493886567780b04a3fce_Out_2);
            float4 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4;
            float3 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGB_5;
            float2 _Combine_405a7cbaec16458fae3b82f162a1ec1f_RG_6;
            Unity_Combine_float(_Add_475bd07f40da493886567780b04a3fce_Out_2, _Split_51004472986b4dbea479f5e791fa26f6_G_2, _Split_51004472986b4dbea479f5e791fa26f6_B_3, _Split_51004472986b4dbea479f5e791fa26f6_A_4, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RGB_5, _Combine_405a7cbaec16458fae3b82f162a1ec1f_RG_6);
            description.Position = (_Combine_405a7cbaec16458fae3b82f162a1ec1f_RGBA_4.xyz);
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0.tex, _Property_a8d4c8889b2444fcafb8a142ffc68fa7_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_R_4 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.r;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_G_5 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.g;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_B_6 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.b;
            float _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_A_7 = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_a0fe17daac174fe2b71c1e38f55e8496_A_7;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS;
            output.ObjectSpacePosition =         input.positionOS;
            output.uv0 =                         input.uv0;
            output.TimeParameters =              _TimeParameters.xyz;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.uv0 =                         input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"

            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}