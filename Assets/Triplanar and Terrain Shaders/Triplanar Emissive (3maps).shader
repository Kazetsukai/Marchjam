// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|emission-7432-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:4046,x:32274,y:32986,varname:node_4046,prsc:2,chbt:0|M-869-OUT,R-7302-RGB,G-844-RGB,B-9841-RGB;n:type:ShaderForge.SFN_Tex2d,id:7302,x:31957,y:32387,ptovrint:False,ptlb:Diffuse Side (X)(Spec A),ptin:_DiffuseSideXSpecA,varname:node_7302,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7833-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:844,x:31957,y:32612,ptovrint:False,ptlb:Diffuse Top/Down (Y)(Spec A),ptin:_DiffuseTopDownYSpecA,varname:node_844,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7729-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:9841,x:31957,y:32781,ptovrint:False,ptlb:Diffuse Side (Z)(Spec A),ptin:_DiffuseSideZSpecA,varname:node_9841,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8964-UVOUT;n:type:ShaderForge.SFN_NormalVector,id:8915,x:31715,y:33251,prsc:2,pt:False;n:type:ShaderForge.SFN_FragmentPosition,id:8535,x:30099,y:32926,varname:node_8535,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:7561,x:30939,y:32477,varname:node_7561,prsc:2,cc1:1,cc2:2,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:560,x:30946,y:33162,varname:node_560,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:174,x:30944,y:32789,varname:node_174,prsc:2,cc1:2,cc2:0,cc3:-1,cc4:-1|IN-8535-XYZ;n:type:ShaderForge.SFN_Rotator,id:7833,x:31429,y:32342,varname:node_7833,prsc:2|UVIN-7561-OUT,ANG-2863-OUT;n:type:ShaderForge.SFN_Slider,id:6303,x:31117,y:32519,ptovrint:False,ptlb:Rotation (X),ptin:_RotationX,varname:_RotationX,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.785,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:2863,x:31429,y:32482,varname:node_2863,prsc:2,a:0,b:6|IN-6303-OUT;n:type:ShaderForge.SFN_Slider,id:3430,x:31075,y:33321,ptovrint:False,ptlb:Rotation (Z),ptin:_RotationZ,varname:_RotationY,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:3991,x:31428,y:33312,varname:node_3991,prsc:2,a:0,b:6|IN-3430-OUT;n:type:ShaderForge.SFN_Rotator,id:8964,x:31428,y:33171,varname:node_8964,prsc:2|UVIN-560-OUT,ANG-3991-OUT;n:type:ShaderForge.SFN_Multiply,id:7432,x:32435,y:32868,varname:node_7432,prsc:2|A-5581-RGB,B-4046-OUT;n:type:ShaderForge.SFN_Color,id:5581,x:32274,y:32776,ptovrint:False,ptlb:_Diffuse Color,ptin:__DiffuseColor,varname:node_5581,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:6336,x:31135,y:32869,ptovrint:False,ptlb:Rotation (Y),ptin:_RotationY,varname:_RotationW,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:6849,x:31488,y:32860,varname:node_6849,prsc:2,a:0,b:6|IN-6336-OUT;n:type:ShaderForge.SFN_Rotator,id:7729,x:31488,y:32719,varname:node_7729,prsc:2|UVIN-174-OUT,ANG-6849-OUT;n:type:ShaderForge.SFN_Power,id:869,x:32117,y:33309,varname:node_869,prsc:2|VAL-4457-OUT,EXP-469-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:469,x:31919,y:33384,varname:node_469,prsc:2,a:0.1,b:2|IN-5614-OUT;n:type:ShaderForge.SFN_Abs,id:4457,x:31919,y:33261,varname:node_4457,prsc:2|IN-8915-OUT;n:type:ShaderForge.SFN_Slider,id:5614,x:31558,y:33418,ptovrint:False,ptlb:Power value,ptin:_Powervalue,varname:node_5172,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ObjectPosition,id:9404,x:30121,y:33142,varname:node_9404,prsc:2;n:type:ShaderForge.SFN_Subtract,id:5703,x:30288,y:32992,varname:node_5703,prsc:2|A-8535-XYZ,B-9404-XYZ;n:type:ShaderForge.SFN_Transform,id:3342,x:30483,y:33049,varname:node_3342,prsc:2,tffrom:0,tfto:1|IN-5703-OUT;proporder:5581-5614-844-6336-7302-6303-9841-3430;pass:END;sub:END;*/

Shader "Ciconia Studio/Effects/Triplanar/Emissive (3maps)" {
    Properties {
        __DiffuseColor ("_Diffuse Color", Color) = (1,1,1,1)
        _Powervalue ("Power value", Range(0, 1)) = 1
        _DiffuseTopDownYSpecA ("Diffuse Top/Down (Y)(Spec A)", 2D) = "white" {}
        _RotationY ("Rotation (Y)", Range(0, 1)) = 0
        _DiffuseSideXSpecA ("Diffuse Side (X)(Spec A)", 2D) = "white" {}
        _RotationX ("Rotation (X)", Range(0, 1)) = 0.785
        _DiffuseSideZSpecA ("Diffuse Side (Z)(Spec A)", 2D) = "white" {}
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform sampler2D _DiffuseSideXSpecA; uniform float4 _DiffuseSideXSpecA_ST;
            uniform sampler2D _DiffuseTopDownYSpecA; uniform float4 _DiffuseTopDownYSpecA_ST;
            uniform sampler2D _DiffuseSideZSpecA; uniform float4 _DiffuseSideZSpecA_ST;
            uniform float _RotationX;
            uniform float _RotationZ;
            uniform float4 __DiffuseColor;
            uniform float _RotationY;
            uniform float _Powervalue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 node_869 = pow(abs(i.normalDir),lerp(0.1,2,_Powervalue));
                float node_7833_ang = lerp(0,6,_RotationX);
                float node_7833_spd = 1.0;
                float node_7833_cos = cos(node_7833_spd*node_7833_ang);
                float node_7833_sin = sin(node_7833_spd*node_7833_ang);
                float2 node_7833_piv = float2(0.5,0.5);
                float2 node_7833 = (mul(i.posWorld.rgb.gb-node_7833_piv,float2x2( node_7833_cos, -node_7833_sin, node_7833_sin, node_7833_cos))+node_7833_piv);
                float4 _DiffuseSideXSpecA_var = tex2D(_DiffuseSideXSpecA,TRANSFORM_TEX(node_7833, _DiffuseSideXSpecA));
                float node_7729_ang = lerp(0,6,_RotationY);
                float node_7729_spd = 1.0;
                float node_7729_cos = cos(node_7729_spd*node_7729_ang);
                float node_7729_sin = sin(node_7729_spd*node_7729_ang);
                float2 node_7729_piv = float2(0.5,0.5);
                float2 node_7729 = (mul(i.posWorld.rgb.br-node_7729_piv,float2x2( node_7729_cos, -node_7729_sin, node_7729_sin, node_7729_cos))+node_7729_piv);
                float4 _DiffuseTopDownYSpecA_var = tex2D(_DiffuseTopDownYSpecA,TRANSFORM_TEX(node_7729, _DiffuseTopDownYSpecA));
                float node_8964_ang = lerp(0,6,_RotationZ);
                float node_8964_spd = 1.0;
                float node_8964_cos = cos(node_8964_spd*node_8964_ang);
                float node_8964_sin = sin(node_8964_spd*node_8964_ang);
                float2 node_8964_piv = float2(0.5,0.5);
                float2 node_8964 = (mul(i.posWorld.rgb.rg-node_8964_piv,float2x2( node_8964_cos, -node_8964_sin, node_8964_sin, node_8964_cos))+node_8964_piv);
                float4 _DiffuseSideZSpecA_var = tex2D(_DiffuseSideZSpecA,TRANSFORM_TEX(node_8964, _DiffuseSideZSpecA));
                float3 emissive = (__DiffuseColor.rgb*(node_869.r*_DiffuseSideXSpecA_var.rgb + node_869.g*_DiffuseTopDownYSpecA_var.rgb + node_869.b*_DiffuseSideZSpecA_var.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
