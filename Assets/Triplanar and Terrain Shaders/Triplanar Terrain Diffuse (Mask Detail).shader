// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|diff-376-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:4046,x:32490,y:32714,varname:node_4046,prsc:2,chbt:0|M-3880-OUT,R-7302-RGB,G-9157-OUT,B-9841-RGB;n:type:ShaderForge.SFN_Tex2d,id:7302,x:31938,y:32482,varname:node_7302,prsc:2,ntxv:0,isnm:False|UVIN-7833-UVOUT,TEX-7668-TEX;n:type:ShaderForge.SFN_Tex2d,id:844,x:31938,y:32707,ptovrint:False,ptlb:Diffuse map,ptin:_Diffusemap,varname:node_844,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-174-OUT;n:type:ShaderForge.SFN_Tex2d,id:9841,x:31938,y:32876,varname:node_9841,prsc:2,ntxv:0,isnm:False|UVIN-8964-UVOUT,TEX-7668-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:7668,x:31385,y:32637,ptovrint:False,ptlb:Diffuse Height,ptin:_DiffuseHeight,varname:node_7668,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:230,x:31284,y:33308,ptovrint:False,ptlb:Mask(Grayscale),ptin:_MaskGrayscale,varname:node_230,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f4c46ad68f5ebb64eaf83898ea395fd1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5899,x:31925,y:33066,ptovrint:False,ptlb:_Diffuse Map detail,ptin:__DiffuseMapdetail,varname:node_5899,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:1447,x:31503,y:33780,ptovrint:False,ptlb:Sharpen,ptin:_Sharpen,varname:node_1447,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Lerp,id:3558,x:32175,y:33291,varname:node_3558,prsc:2|A-8249-OUT,B-9659-OUT,T-1447-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8535,x:30399,y:32676,varname:node_8535,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:7561,x:31096,y:31949,varname:node_7561,prsc:2,cc1:1,cc2:2,cc3:-1,cc4:-1|IN-1172-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:560,x:31226,y:32949,varname:node_560,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-1172-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:174,x:31188,y:32667,varname:node_174,prsc:2,cc1:0,cc2:2,cc3:-1,cc4:-1|IN-1172-XYZ;n:type:ShaderForge.SFN_ObjectPosition,id:3944,x:30399,y:32800,varname:node_3944,prsc:2;n:type:ShaderForge.SFN_Subtract,id:1590,x:30697,y:32725,varname:node_1590,prsc:2|A-8535-XYZ,B-3944-XYZ;n:type:ShaderForge.SFN_Transform,id:1172,x:30915,y:32572,varname:node_1172,prsc:2,tffrom:0,tfto:1|IN-1590-OUT;n:type:ShaderForge.SFN_Rotator,id:7833,x:31667,y:32257,varname:node_7833,prsc:2|UVIN-7561-OUT,ANG-2863-OUT;n:type:ShaderForge.SFN_Slider,id:6303,x:31081,y:32398,ptovrint:False,ptlb:Rotation (X),ptin:_RotationX,varname:node_6303,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:2863,x:31426,y:32371,varname:node_2863,prsc:2,a:0,b:6|IN-6303-OUT;n:type:ShaderForge.SFN_Slider,id:3430,x:30965,y:33151,ptovrint:False,ptlb:Rotation (Y),ptin:_RotationY,varname:_RotationY,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:3991,x:31310,y:33124,varname:node_3991,prsc:2,a:0,b:6|IN-3430-OUT;n:type:ShaderForge.SFN_Rotator,id:8964,x:31551,y:33010,varname:node_8964,prsc:2|UVIN-560-OUT,ANG-3991-OUT;n:type:ShaderForge.SFN_Lerp,id:9157,x:32283,y:32908,varname:node_9157,prsc:2|A-844-RGB,B-5899-RGB,T-3558-OUT;n:type:ShaderForge.SFN_Blend,id:9659,x:31853,y:33364,varname:node_9659,prsc:2,blmd:12,clmp:True|SRC-8249-OUT,DST-3979-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:3979,x:31595,y:33546,varname:node_3979,prsc:2,a:0,b:2|IN-1447-OUT;n:type:ShaderForge.SFN_Multiply,id:376,x:32652,y:32490,varname:node_376,prsc:2|A-2599-RGB,B-4046-OUT;n:type:ShaderForge.SFN_Color,id:2599,x:32490,y:32344,ptovrint:False,ptlb:_Diffuse Color,ptin:__DiffuseColor,varname:node_2599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_SwitchProperty,id:8249,x:31550,y:33382,ptovrint:False,ptlb:Invert mask,ptin:_Invertmask,varname:node_8249,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-230-RGB,B-8556-OUT;n:type:ShaderForge.SFN_OneMinus,id:8556,x:31398,y:33485,varname:node_8556,prsc:2|IN-230-RGB;n:type:ShaderForge.SFN_NormalVector,id:682,x:31824,y:31836,prsc:2,pt:False;n:type:ShaderForge.SFN_Power,id:3880,x:32287,y:31920,varname:node_3880,prsc:2|VAL-2105-OUT,EXP-451-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:451,x:32084,y:32033,varname:node_451,prsc:2,a:0.1,b:2|IN-1668-OUT;n:type:ShaderForge.SFN_Abs,id:2105,x:32030,y:31881,varname:node_2105,prsc:2|IN-682-OUT;n:type:ShaderForge.SFN_Slider,id:1668,x:31723,y:32067,ptovrint:False,ptlb:Power value,ptin:_Powervalue,varname:node_5172,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.1,cur:1,max:1;proporder:2599-844-7668-6303-3430-5899-230-1447-8249-1668;pass:END;sub:END;*/

Shader "Ciconia Studio/Effects/Triplanar/Terrain (For Unity)/Diffuse (Mask Detail)" {
    Properties {
        __DiffuseColor ("_Diffuse Color", Color) = (1,1,1,1)
        _Diffusemap ("Diffuse map", 2D) = "white" {}
        _DiffuseHeight ("Diffuse Height", 2D) = "white" {}
        _RotationX ("Rotation (X)", Range(0, 1)) = 0.8
        _RotationY ("Rotation (Y)", Range(0, 1)) = 0
        __DiffuseMapdetail ("_Diffuse Map detail", 2D) = "white" {}
        _MaskGrayscale ("Mask(Grayscale)", 2D) = "white" {}
        _Sharpen ("Sharpen", Range(0, 1)) = 1
        [MaterialToggle] _Invertmask ("Invert mask", Float ) = 0.6980392
        _Powervalue ("Power value", Range(0.1, 1)) = 1
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
            uniform sampler2D _Diffusemap; uniform float4 _Diffusemap_ST;
            uniform sampler2D _DiffuseHeight; uniform float4 _DiffuseHeight_ST;
            uniform sampler2D _MaskGrayscale; uniform float4 _MaskGrayscale_ST;
            uniform sampler2D __DiffuseMapdetail; uniform float4 __DiffuseMapdetail_ST;
            uniform float _Sharpen;
            uniform float _RotationX;
            uniform float _RotationY;
            uniform float4 __DiffuseColor;
            uniform fixed _Invertmask;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
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
                float3 node_3880 = pow(abs(i.normalDir),lerp(0.1,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float3 node_1172 = mul( _World2Object, float4((i.posWorld.rgb-objPos.rgb),0) ).xyz;
                float2 node_7833 = (mul(node_1172.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float4 node_7302 = tex2D(_DiffuseHeight,TRANSFORM_TEX(node_7833, _DiffuseHeight));
                float2 node_174 = node_1172.rgb.rb;
                float4 _Diffusemap_var = tex2D(_Diffusemap,TRANSFORM_TEX(node_174, _Diffusemap));
                float4 __DiffuseMapdetail_var = tex2D(__DiffuseMapdetail,TRANSFORM_TEX(i.uv0, __DiffuseMapdetail));
                float4 _MaskGrayscale_var = tex2D(_MaskGrayscale,TRANSFORM_TEX(i.uv0, _MaskGrayscale));
                float3 _Invertmask_var = lerp( _MaskGrayscale_var.rgb, (1.0 - _MaskGrayscale_var.rgb), _Invertmask );
                float node_8964_ang = lerp(0,6,_RotationY);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(node_1172.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float4 node_9841 = tex2D(_DiffuseHeight,TRANSFORM_TEX(node_8964, _DiffuseHeight));
                float3 diffuseColor = (__DiffuseColor.rgb*(node_3880.r*node_7302.rgb + node_3880.g*lerp(_Diffusemap_var.rgb,__DiffuseMapdetail_var.rgb,lerp(_Invertmask_var,saturate((_Invertmask_var > 0.5 ?  (1.0-(1.0-2.0*(_Invertmask_var-0.5))*(1.0-lerp(0,2,_Sharpen))) : (2.0*_Invertmask_var*lerp(0,2,_Sharpen))) ),_Sharpen)) + node_3880.b*node_9841.rgb));
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
            uniform sampler2D _Diffusemap; uniform float4 _Diffusemap_ST;
            uniform sampler2D _DiffuseHeight; uniform float4 _DiffuseHeight_ST;
            uniform sampler2D _MaskGrayscale; uniform float4 _MaskGrayscale_ST;
            uniform sampler2D __DiffuseMapdetail; uniform float4 __DiffuseMapdetail_ST;
            uniform float _Sharpen;
            uniform float _RotationX;
            uniform float _RotationY;
            uniform float4 __DiffuseColor;
            uniform fixed _Invertmask;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
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
                float3 node_3880 = pow(abs(i.normalDir),lerp(0.1,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float3 node_1172 = mul( _World2Object, float4((i.posWorld.rgb-objPos.rgb),0) ).xyz;
                float2 node_7833 = (mul(node_1172.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float4 node_7302 = tex2D(_DiffuseHeight,TRANSFORM_TEX(node_7833, _DiffuseHeight));
                float2 node_174 = node_1172.rgb.rb;
                float4 _Diffusemap_var = tex2D(_Diffusemap,TRANSFORM_TEX(node_174, _Diffusemap));
                float4 __DiffuseMapdetail_var = tex2D(__DiffuseMapdetail,TRANSFORM_TEX(i.uv0, __DiffuseMapdetail));
                float4 _MaskGrayscale_var = tex2D(_MaskGrayscale,TRANSFORM_TEX(i.uv0, _MaskGrayscale));
                float3 _Invertmask_var = lerp( _MaskGrayscale_var.rgb, (1.0 - _MaskGrayscale_var.rgb), _Invertmask );
                float node_8964_ang = lerp(0,6,_RotationY);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(node_1172.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float4 node_9841 = tex2D(_DiffuseHeight,TRANSFORM_TEX(node_8964, _DiffuseHeight));
                float3 diffuseColor = (__DiffuseColor.rgb*(node_3880.r*node_7302.rgb + node_3880.g*lerp(_Diffusemap_var.rgb,__DiffuseMapdetail_var.rgb,lerp(_Invertmask_var,saturate((_Invertmask_var > 0.5 ?  (1.0-(1.0-2.0*(_Invertmask_var-0.5))*(1.0-lerp(0,2,_Sharpen))) : (2.0*_Invertmask_var*lerp(0,2,_Sharpen))) ),_Sharpen)) + node_3880.b*node_9841.rgb));
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
