// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|diff-7432-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:4046,x:32407,y:32880,varname:node_4046,prsc:2,chbt:0|M-438-OUT,R-7302-RGB,G-844-RGB,B-9841-RGB,BTM-4221-OUT;n:type:ShaderForge.SFN_Tex2d,id:7302,x:31957,y:32387,varname:node_7302,prsc:2,ntxv:0,isnm:False|UVIN-7833-UVOUT,TEX-7060-TEX;n:type:ShaderForge.SFN_Tex2d,id:844,x:31957,y:32612,varname:node_844,prsc:2,ntxv:0,isnm:False|UVIN-7729-UVOUT,TEX-7060-TEX;n:type:ShaderForge.SFN_Tex2d,id:9841,x:31957,y:32781,varname:node_9841,prsc:2,ntxv:0,isnm:False|UVIN-8964-UVOUT,TEX-7060-TEX;n:type:ShaderForge.SFN_NormalVector,id:8915,x:31694,y:33254,prsc:2,pt:False;n:type:ShaderForge.SFN_FragmentPosition,id:8535,x:30099,y:32926,varname:node_8535,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:7561,x:30939,y:32477,varname:node_7561,prsc:2,cc1:1,cc2:2,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:560,x:30946,y:33162,varname:node_560,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:174,x:30944,y:32789,varname:node_174,prsc:2,cc1:0,cc2:2,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_Rotator,id:7833,x:31429,y:32342,varname:node_7833,prsc:2|UVIN-7561-OUT,ANG-2863-OUT;n:type:ShaderForge.SFN_Slider,id:6303,x:31117,y:32519,ptovrint:False,ptlb:Rotation (X),ptin:_RotationX,varname:_RotationX,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.785,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:2863,x:31429,y:32482,varname:node_2863,prsc:2,a:0,b:6|IN-6303-OUT;n:type:ShaderForge.SFN_Slider,id:3430,x:31075,y:33321,ptovrint:False,ptlb:Rotation (Z),ptin:_RotationZ,varname:_RotationY,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:3991,x:31428,y:33312,varname:node_3991,prsc:2,a:0,b:6|IN-3430-OUT;n:type:ShaderForge.SFN_Rotator,id:8964,x:31428,y:33171,varname:node_8964,prsc:2|UVIN-560-OUT,ANG-3991-OUT;n:type:ShaderForge.SFN_Multiply,id:7432,x:32486,y:32695,varname:node_7432,prsc:2|A-5581-RGB,B-4046-OUT;n:type:ShaderForge.SFN_Color,id:5581,x:32284,y:32620,ptovrint:False,ptlb:_Diffuse Color,ptin:__DiffuseColor,varname:node_5581,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:6336,x:31135,y:32869,ptovrint:False,ptlb:Rotation (Y),ptin:_RotationY,varname:_RotationW,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:6849,x:31488,y:32860,varname:node_6849,prsc:2,a:0,b:6|IN-6336-OUT;n:type:ShaderForge.SFN_Rotator,id:7729,x:31488,y:32719,varname:node_7729,prsc:2|UVIN-174-OUT,ANG-6849-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:7060,x:31693,y:32626,ptovrint:False,ptlb:Diffuse map,ptin:_Diffusemap,varname:node_7060,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Power,id:438,x:32131,y:33379,varname:node_438,prsc:2|VAL-3868-OUT,EXP-7776-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:7776,x:31902,y:33449,varname:node_7776,prsc:2,a:0.2,b:2|IN-5296-OUT;n:type:ShaderForge.SFN_Abs,id:3868,x:31937,y:33281,varname:node_3868,prsc:2|IN-8915-OUT;n:type:ShaderForge.SFN_Slider,id:5296,x:31489,y:33522,ptovrint:False,ptlb:Power value,ptin:_Powervalue,varname:node_5172,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Vector3,id:4221,x:32258,y:33179,varname:node_4221,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Transform,id:6791,x:30583,y:33031,varname:node_6791,prsc:2,tffrom:0,tfto:1|IN-1229-OUT;n:type:ShaderForge.SFN_ObjectPosition,id:8029,x:30167,y:33167,varname:node_8029,prsc:2;n:type:ShaderForge.SFN_Subtract,id:1229,x:30444,y:33156,varname:node_1229,prsc:2|A-8535-XYZ,B-8029-XYZ;proporder:5581-5296-7060-6303-6336-3430;pass:END;sub:END;*/

Shader "Ciconia Studio/Effects/Triplanar/Diffuse (1map)" {
    Properties {
        __DiffuseColor ("_Diffuse Color", Color) = (1,1,1,1)
        _Powervalue ("Power value", Range(0, 1)) = 1
        _Diffusemap ("Diffuse map", 2D) = "white" {}
        _RotationX ("Rotation (X)", Range(0, 1)) = 0.785
        _RotationY ("Rotation (Y)", Range(0, 1)) = 0
        _RotationZ ("Rotation (Z)", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _RotationX;
            uniform float _RotationZ;
            uniform float4 __DiffuseColor;
            uniform float _RotationY;
            uniform sampler2D _Diffusemap; uniform float4 _Diffusemap_ST;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 node_438 = pow(abs(i.normalDir),lerp(0.2,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float2 node_7833 = (mul(i.posWorld.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float4 node_7302 = tex2D(_Diffusemap,TRANSFORM_TEX(node_7833, _Diffusemap));
                float node_7729_ang = lerp(0,6,_RotationY);
                float node_7729_spd = 1.0;
                float node_7729_cos = cos(node_7729_spd*node_7729_ang);
                float node_7729_sin = sin(node_7729_spd*node_7729_ang);
                float2 node_7729_piv = float2(0.5,0.5);
                float2 node_7729 = (mul(i.posWorld.rgb.rb-node_7729_piv,float2x2( node_7729_cos, -node_7729_sin, node_7729_sin, node_7729_cos))+node_7729_piv);
                float4 node_844 = tex2D(_Diffusemap,TRANSFORM_TEX(node_7729, _Diffusemap));
                float node_8964_ang = lerp(0,6,_RotationZ);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(i.posWorld.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float4 node_9841 = tex2D(_Diffusemap,TRANSFORM_TEX(node_8964, _Diffusemap));
                float3 diffuseColor = (__DiffuseColor.rgb*(node_438.r*node_7302.rgb + node_438.g*node_844.rgb + node_438.b*node_9841.rgb));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _RotationX;
            uniform float _RotationZ;
            uniform float4 __DiffuseColor;
            uniform float _RotationY;
            uniform sampler2D _Diffusemap; uniform float4 _Diffusemap_ST;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 node_438 = pow(abs(i.normalDir),lerp(0.2,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float2 node_7833 = (mul(i.posWorld.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float4 node_7302 = tex2D(_Diffusemap,TRANSFORM_TEX(node_7833, _Diffusemap));
                float node_7729_ang = lerp(0,6,_RotationY);
                float node_7729_spd = 1.0;
                float node_7729_cos = cos(node_7729_spd*node_7729_ang);
                float node_7729_sin = sin(node_7729_spd*node_7729_ang);
                float2 node_7729_piv = float2(0.5,0.5);
                float2 node_7729 = (mul(i.posWorld.rgb.rb-node_7729_piv,float2x2( node_7729_cos, -node_7729_sin, node_7729_sin, node_7729_cos))+node_7729_piv);
                float4 node_844 = tex2D(_Diffusemap,TRANSFORM_TEX(node_7729, _Diffusemap));
                float node_8964_ang = lerp(0,6,_RotationZ);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(i.posWorld.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float4 node_9841 = tex2D(_Diffusemap,TRANSFORM_TEX(node_8964, _Diffusemap));
                float3 diffuseColor = (__DiffuseColor.rgb*(node_438.r*node_7302.rgb + node_438.g*node_844.rgb + node_438.b*node_9841.rgb));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
