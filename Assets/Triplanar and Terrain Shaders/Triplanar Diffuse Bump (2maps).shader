// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|diff-7432-OUT,spec-1530-OUT,gloss-1299-OUT,normal-3202-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:4046,x:32274,y:32986,varname:node_4046,prsc:2,chbt:0|M-2578-OUT,R-7302-RGB,G-844-RGB,B-9841-RGB;n:type:ShaderForge.SFN_Tex2d,id:7302,x:31957,y:32387,varname:node_7302,prsc:2,ntxv:0,isnm:False|UVIN-7833-UVOUT,TEX-7060-TEX;n:type:ShaderForge.SFN_Tex2d,id:844,x:31957,y:32612,ptovrint:False,ptlb:Diffuse Top/Down (Y)(Spec A),ptin:_DiffuseTopDownYSpecA,varname:node_844,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7729-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:9841,x:31957,y:32781,varname:node_9841,prsc:2,ntxv:0,isnm:False|UVIN-8964-UVOUT,TEX-7060-TEX;n:type:ShaderForge.SFN_NormalVector,id:8915,x:31756,y:33065,prsc:2,pt:False;n:type:ShaderForge.SFN_FragmentPosition,id:8535,x:30099,y:32926,varname:node_8535,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:7561,x:30939,y:32477,varname:node_7561,prsc:2,cc1:1,cc2:2,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:560,x:30946,y:33162,varname:node_560,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:174,x:30904,y:32784,varname:node_174,prsc:2,cc1:2,cc2:0,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_Rotator,id:7833,x:31429,y:32342,varname:node_7833,prsc:2|UVIN-7561-OUT,ANG-2863-OUT;n:type:ShaderForge.SFN_Slider,id:6303,x:31117,y:32519,ptovrint:False,ptlb:Rotation (X),ptin:_RotationX,varname:_RotationX,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.785,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:2863,x:31429,y:32482,varname:node_2863,prsc:2,a:0,b:6|IN-6303-OUT;n:type:ShaderForge.SFN_Slider,id:3430,x:31075,y:33321,ptovrint:False,ptlb:Rotation (Z),ptin:_RotationZ,varname:_RotationY,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:3991,x:31428,y:33312,varname:node_3991,prsc:2,a:0,b:6|IN-3430-OUT;n:type:ShaderForge.SFN_Rotator,id:8964,x:31428,y:33171,varname:node_8964,prsc:2|UVIN-560-OUT,ANG-3991-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:2171,x:32166,y:33658,varname:node_2171,prsc:2,chbt:0|M-2578-OUT,R-7330-RGB,G-3223-RGB,B-676-RGB;n:type:ShaderForge.SFN_Tex2d,id:7330,x:31797,y:33502,varname:_NormalMapX,prsc:2,ntxv:3,isnm:True|UVIN-7833-UVOUT,TEX-7355-TEX;n:type:ShaderForge.SFN_Tex2d,id:3223,x:31797,y:33683,ptovrint:False,ptlb:_Normal Top/Down (Y),ptin:__NormalTopDownY,varname:_NormalMapGround,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-7729-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:676,x:31797,y:33857,varname:_NormalMapy,prsc:2,ntxv:3,isnm:True|UVIN-8964-UVOUT,TEX-7355-TEX;n:type:ShaderForge.SFN_Vector3,id:845,x:32126,y:33573,varname:node_845,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:1146,x:32342,y:33635,varname:node_1146,prsc:2|A-845-OUT,B-2171-OUT,T-5092-OUT;n:type:ShaderForge.SFN_Slider,id:205,x:31959,y:34008,ptovrint:False,ptlb:Normal Intensity,ptin:_NormalIntensity,varname:node_205,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Normalize,id:3202,x:32520,y:33635,varname:node_3202,prsc:2|IN-1146-OUT;n:type:ShaderForge.SFN_Multiply,id:7246,x:32461,y:32478,varname:node_7246,prsc:2|A-7302-A,B-844-B,C-9841-A;n:type:ShaderForge.SFN_Slider,id:6827,x:32338,y:32281,ptovrint:False,ptlb:Specular Intensity,ptin:_SpecularIntensity,varname:node_6827,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:1530,x:32875,y:32311,varname:node_1530,prsc:2|A-4923-RGB,B-5580-OUT,C-7246-OUT;n:type:ShaderForge.SFN_Color,id:4923,x:32628,y:32035,ptovrint:False,ptlb:_Specular Color,ptin:__SpecularColor,varname:node_4923,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ConstantLerp,id:5580,x:32647,y:32325,varname:node_5580,prsc:2,a:0,b:5|IN-6827-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:1299,x:32715,y:31867,varname:node_1299,prsc:2,a:0.4,b:1|IN-641-OUT;n:type:ShaderForge.SFN_Slider,id:641,x:32406,y:31823,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:_SpecularIntensity_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:1;n:type:ShaderForge.SFN_Multiply,id:7432,x:32435,y:32868,varname:node_7432,prsc:2|A-5581-RGB,B-4046-OUT;n:type:ShaderForge.SFN_Color,id:5581,x:32274,y:32776,ptovrint:False,ptlb:_Diffuse Color,ptin:__DiffuseColor,varname:node_5581,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:6336,x:31135,y:32869,ptovrint:False,ptlb:Rotation (Y),ptin:_RotationY,varname:_RotationW,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:6849,x:31488,y:32860,varname:node_6849,prsc:2,a:0,b:6|IN-6336-OUT;n:type:ShaderForge.SFN_Rotator,id:7729,x:31488,y:32719,varname:node_7729,prsc:2|UVIN-174-OUT,ANG-6849-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:7060,x:31701,y:32567,ptovrint:False,ptlb:Diffuse Sides (XZ),ptin:_DiffuseSidesXZ,varname:node_7060,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:7355,x:31488,y:33673,ptovrint:False,ptlb:Normal Sides (XZ),ptin:_NormalSidesXZ,varname:node_7355,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:6855,x:31797,y:33320,ptovrint:False,ptlb:Power value,ptin:_Powervalue,varname:node_5172,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:3945,x:32135,y:33298,varname:node_3945,prsc:2,a:0.1,b:2|IN-6855-OUT;n:type:ShaderForge.SFN_Power,id:2578,x:32319,y:33177,varname:node_2578,prsc:2|VAL-2949-OUT,EXP-3945-OUT;n:type:ShaderForge.SFN_Abs,id:2949,x:32111,y:33152,varname:node_2949,prsc:2|IN-8915-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:5092,x:32261,y:33824,varname:node_5092,prsc:2,a:0,b:2|IN-205-OUT;proporder:5581-6855-844-6336-7060-6303-3430-4923-6827-641-3223-7355-205;pass:END;sub:END;*/

Shader "Ciconia Studio/Effects/Triplanar/Diffuse Bump(2maps)" {
    Properties {
        __DiffuseColor ("_Diffuse Color", Color) = (1,1,1,1)
        _Powervalue ("Power value", Range(0, 1)) = 1
        _DiffuseTopDownYSpecA ("Diffuse Top/Down (Y)(Spec A)", 2D) = "white" {}
        _RotationY ("Rotation (Y)", Range(0, 1)) = 1
        _DiffuseSidesXZ ("Diffuse Sides (XZ)", 2D) = "white" {}
        _RotationX ("Rotation (X)", Range(0, 1)) = 0.785
        _RotationZ ("Rotation (Z)", Range(0, 1)) = 0
        __SpecularColor ("_Specular Color", Color) = (1,1,1,1)
        _SpecularIntensity ("Specular Intensity", Range(0, 1)) = 0.5
        _Glossiness ("Glossiness", Range(0, 1)) = 0.25
        __NormalTopDownY ("_Normal Top/Down (Y)", 2D) = "bump" {}
        _NormalSidesXZ ("Normal Sides (XZ)", 2D) = "bump" {}
        _NormalIntensity ("Normal Intensity", Range(0, 1)) = 0.5
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
            uniform sampler2D _DiffuseTopDownYSpecA; uniform float4 _DiffuseTopDownYSpecA_ST;
            uniform float _RotationX;
            uniform float _RotationZ;
            uniform sampler2D __NormalTopDownY; uniform float4 __NormalTopDownY_ST;
            uniform float _NormalIntensity;
            uniform float _SpecularIntensity;
            uniform float4 __SpecularColor;
            uniform float _Glossiness;
            uniform float4 __DiffuseColor;
            uniform float _RotationY;
            uniform sampler2D _DiffuseSidesXZ; uniform float4 _DiffuseSidesXZ_ST;
            uniform sampler2D _NormalSidesXZ; uniform float4 _NormalSidesXZ_ST;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 node_2578 = pow(abs(i.normalDir),lerp(0.1,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float2 node_7833 = (mul(i.posWorld.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float3 _NormalMapX = UnpackNormal(tex2D(_NormalSidesXZ,TRANSFORM_TEX(node_7833, _NormalSidesXZ)));
                float node_7729_ang = lerp(0,6,_RotationY);
                float node_7729_spd = 1.0;
                float node_7729_cos = cos(node_7729_spd*node_7729_ang);
                float node_7729_sin = sin(node_7729_spd*node_7729_ang);
                float2 node_7729_piv = float2(0.5,0.5);
                float2 node_7729 = (mul(i.posWorld.rgb.br-node_7729_piv,float2x2( node_7729_cos, -node_7729_sin, node_7729_sin, node_7729_cos))+node_7729_piv);
                float3 __NormalTopDownY_var = UnpackNormal(tex2D(__NormalTopDownY,TRANSFORM_TEX(node_7729, __NormalTopDownY)));
                float node_8964_ang = lerp(0,6,_RotationZ);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(i.posWorld.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float3 _NormalMapy = UnpackNormal(tex2D(_NormalSidesXZ,TRANSFORM_TEX(node_8964, _NormalSidesXZ)));
                float3 normalLocal = normalize(lerp(float3(0,0,1),(node_2578.r*_NormalMapX.rgb + node_2578.g*__NormalTopDownY_var.rgb + node_2578.b*_NormalMapy.rgb),lerp(0,2,_NormalIntensity)));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = lerp(0.4,1,_Glossiness);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 node_7302 = tex2D(_DiffuseSidesXZ,TRANSFORM_TEX(node_7833, _DiffuseSidesXZ));
                float4 _DiffuseTopDownYSpecA_var = tex2D(_DiffuseTopDownYSpecA,TRANSFORM_TEX(node_7729, _DiffuseTopDownYSpecA));
                float4 node_9841 = tex2D(_DiffuseSidesXZ,TRANSFORM_TEX(node_8964, _DiffuseSidesXZ));
                float3 specularColor = (__SpecularColor.rgb*lerp(0,5,_SpecularIntensity)*(node_7302.a*_DiffuseTopDownYSpecA_var.b*node_9841.a));
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = (__DiffuseColor.rgb*(node_2578.r*node_7302.rgb + node_2578.g*_DiffuseTopDownYSpecA_var.rgb + node_2578.b*node_9841.rgb));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
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
            uniform sampler2D _DiffuseTopDownYSpecA; uniform float4 _DiffuseTopDownYSpecA_ST;
            uniform float _RotationX;
            uniform float _RotationZ;
            uniform sampler2D __NormalTopDownY; uniform float4 __NormalTopDownY_ST;
            uniform float _NormalIntensity;
            uniform float _SpecularIntensity;
            uniform float4 __SpecularColor;
            uniform float _Glossiness;
            uniform float4 __DiffuseColor;
            uniform float _RotationY;
            uniform sampler2D _DiffuseSidesXZ; uniform float4 _DiffuseSidesXZ_ST;
            uniform sampler2D _NormalSidesXZ; uniform float4 _NormalSidesXZ_ST;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 node_2578 = pow(abs(i.normalDir),lerp(0.1,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float2 node_7833 = (mul(i.posWorld.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float3 _NormalMapX = UnpackNormal(tex2D(_NormalSidesXZ,TRANSFORM_TEX(node_7833, _NormalSidesXZ)));
                float node_7729_ang = lerp(0,6,_RotationY);
                float node_7729_spd = 1.0;
                float node_7729_cos = cos(node_7729_spd*node_7729_ang);
                float node_7729_sin = sin(node_7729_spd*node_7729_ang);
                float2 node_7729_piv = float2(0.5,0.5);
                float2 node_7729 = (mul(i.posWorld.rgb.br-node_7729_piv,float2x2( node_7729_cos, -node_7729_sin, node_7729_sin, node_7729_cos))+node_7729_piv);
                float3 __NormalTopDownY_var = UnpackNormal(tex2D(__NormalTopDownY,TRANSFORM_TEX(node_7729, __NormalTopDownY)));
                float node_8964_ang = lerp(0,6,_RotationZ);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(i.posWorld.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float3 _NormalMapy = UnpackNormal(tex2D(_NormalSidesXZ,TRANSFORM_TEX(node_8964, _NormalSidesXZ)));
                float3 normalLocal = normalize(lerp(float3(0,0,1),(node_2578.r*_NormalMapX.rgb + node_2578.g*__NormalTopDownY_var.rgb + node_2578.b*_NormalMapy.rgb),lerp(0,2,_NormalIntensity)));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = lerp(0.4,1,_Glossiness);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 node_7302 = tex2D(_DiffuseSidesXZ,TRANSFORM_TEX(node_7833, _DiffuseSidesXZ));
                float4 _DiffuseTopDownYSpecA_var = tex2D(_DiffuseTopDownYSpecA,TRANSFORM_TEX(node_7729, _DiffuseTopDownYSpecA));
                float4 node_9841 = tex2D(_DiffuseSidesXZ,TRANSFORM_TEX(node_8964, _DiffuseSidesXZ));
                float3 specularColor = (__SpecularColor.rgb*lerp(0,5,_SpecularIntensity)*(node_7302.a*_DiffuseTopDownYSpecA_var.b*node_9841.a));
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = (__DiffuseColor.rgb*(node_2578.r*node_7302.rgb + node_2578.g*_DiffuseTopDownYSpecA_var.rgb + node_2578.b*node_9841.rgb));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
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
