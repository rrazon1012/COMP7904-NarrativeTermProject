// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amp_TriplanarReveal"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TopTexture("TopTexture", 2D) = "white" {}
		_TopColor("TopColor", Color) = (0.5475008,0.8679245,0.3807404,1)
		_TopTextureScale("TopTextureScale", Range( -10 , 10)) = 0.5
		_SideTexture("Side Texture", 2D) = "white" {}
		_SideColor("SideColor", Color) = (0.1561944,0.2225333,0.2830189,1)
		_SideTextureScale("SideTextureScale", Range( -10 , 10)) = 0.5
		_Noise("Noise", 2D) = "white" {}
		_NoiseTextureScale("NoiseTextureScale", Range( -10 , 10)) = 0.5
		_EdgeColor("EdgeColor", Color) = (0.1886348,0.254717,0.1988013,1)
		_EdgeWidth("Edge Width", Range( 0 , 1)) = 0.1792134
		_TopSpread("TopSpread", Range( 0.05 , 4)) = 0.3629656
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" }
		Cull Back
		Blend One Zero , SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D Texture0;
		uniform float2 NoiseScale;
		uniform float3 _Position;
		uniform float _SphereRadius;
		uniform float4 _TopColor;
		uniform float _TopSpread;
		uniform float _EdgeWidth;
		uniform sampler2D _Noise;
		uniform float _NoiseTextureScale;
		uniform sampler2D _TopTexture;
		uniform float _TopTextureScale;
		uniform float4 _EdgeColor;
		uniform float4 _SideColor;
		uniform sampler2D _SideTexture;
		uniform float _SideTextureScale;
		uniform float Float1;
		uniform float4 Color0;
		uniform float _Cutoff = 0.5;


		inline float4 TriplanarSampling10( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float4 temp_cast_0 = (0.01).xxxx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar10 = TriplanarSampling10( Texture0, ase_worldPos, ase_worldNormal, 1.0, NoiseScale, 1.0, 0 );
			float4 temp_output_12_0 = ( triplanar10 * ( 1.0 - saturate( ( distance( ase_worldPos , _Position ) / _SphereRadius ) ) ) );
			float4 temp_output_16_0 = step( temp_cast_0 , temp_output_12_0 );
			float temp_output_173_0 = ( _TopSpread + _EdgeWidth );
			float4 temp_cast_1 = (saturate( ase_worldNormal.y )).xxxx;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 appendResult123 = (float4(ase_worldPos.z , ase_worldPos.y , 0.0 , 0.0));
			float3 break112 = ase_worldNormal;
			float temp_output_138_0 = ( pow( abs( break112.x ) , 4.0 ) * 1.4 );
			float4 appendResult116 = (float4(ase_worldPos.z , ase_worldPos.x , 0.0 , 0.0));
			float temp_output_130_0 = ( pow( abs( break112.y ) , 4.0 ) * 1.4 );
			float4 appendResult119 = (float4(ase_worldPos.x , ase_worldPos.y , 0.0 , 0.0));
			float temp_output_134_0 = ( pow( abs( break112.z ) , 4.0 ) * 1.4 );
			float4 temp_output_143_0 = ( ( tex2D( _Noise, ( appendResult123 * _NoiseTextureScale ).xy ) * temp_output_138_0 ) + ( ( tex2D( _Noise, ( appendResult116 * _NoiseTextureScale ).xy ) * temp_output_130_0 ) + ( tex2D( _Noise, ( appendResult119 * _NoiseTextureScale ).xy ) * temp_output_134_0 ) ) );
			float dotResult174 = dot( temp_cast_1 , ( float4( ase_vertexNormal , 0.0 ) + ( ( temp_output_143_0 * 0.5 ) + temp_output_143_0.g ) ) );
			float4 temp_cast_16 = (0.01).xxxx;
			float4 temp_cast_17 = (Float1).xxxx;
			float4 temp_cast_18 = (0.01).xxxx;
			o.Albedo = ( ( temp_output_16_0 * ( ( ( _TopColor * ( step( temp_output_173_0 , dotResult174 ) * ( ( tex2D( _TopTexture, ( appendResult123 * _TopTextureScale ).xy ) * temp_output_138_0 ) + ( ( tex2D( _TopTexture, ( appendResult116 * _TopTextureScale ).xy ) * temp_output_130_0 ) + ( tex2D( _TopTexture, ( appendResult119 * _TopTextureScale ).xy ) * temp_output_134_0 ) ) ) ) ) + ( _EdgeColor * ( step( _TopSpread , dotResult174 ) * step( dotResult174 , temp_output_173_0 ) ) ) ) + ( _SideColor * ( ( ( tex2D( _SideTexture, ( appendResult123 * _SideTextureScale ).xy ) * temp_output_138_0 ) + ( ( tex2D( _SideTexture, ( appendResult116 * _SideTextureScale ).xy ) * temp_output_130_0 ) + ( tex2D( _SideTexture, ( appendResult119 * _SideTextureScale ).xy ) * temp_output_134_0 ) ) ) * step( dotResult174 , _TopSpread ) ) ) ) ) + saturate( ( ( temp_output_16_0 * step( ( temp_output_12_0 - temp_cast_17 ) , temp_cast_18 ) ) * Color0 ) ) ).xyz;
			o.Alpha = 1;
			clip( temp_output_16_0.x - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
330;1498;1880;870;8729.836;3130.84;6.035373;True;True
Node;AmplifyShaderEditor.CommentaryNode;110;-5688.254,420.2664;Inherit;False;1355.03;743.9213;World Normal Axis Decomposition (xyz);13;138;134;132;130;128;127;124;122;120;118;114;112;111;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;111;-5599.201,599.7685;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;113;-6473.664,-1197.122;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;112;-5330.317,604.0714;Inherit;True;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.AbsOpNode;114;-5060.352,655.6379;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-5286.81,856.1625;Inherit;False;Constant;_WorldPow;WorldPow;3;0;Create;True;0;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;119;-6188.662,-737.8228;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;120;-5063.969,745.4041;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;115;-5101.672,-1307.57;Inherit;False;1424.92;782.0477;noisetexture;9;143;140;139;137;136;135;133;129;121;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;116;-6191.518,-1075.331;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-5716.982,-1235.725;Inherit;False;Property;_NoiseTextureScale;NoiseTextureScale;9;0;Create;True;0;0;0;False;0;False;0.5;0.1;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-5381.024,-1014.568;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;121;-5051.672,-1251.89;Inherit;True;Property;_Noise;Noise;8;0;Create;True;0;0;0;False;0;False;09f8c5f70b5707b40ab3cdaf39c87491;09f8c5f70b5707b40ab3cdaf39c87491;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PowerNode;127;-4929.425,763.5666;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;128;-5063.549,569.6973;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;123;-6180.344,-1399.545;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;122;-4928.941,655.0817;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-5378.96,-776.1634;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-5063.269,885.7451;Inherit;False;Constant;_NormalSaturation;NormalSaturation;8;0;Create;True;0;0;0;False;0;False;1.4;0.15;0;1.4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;129;-4791.82,-773.7674;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;132;-4927.722,555.7097;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-5381.695,-1237.09;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;133;-4782.046,-1018.749;Inherit;True;Property;_NoiseTexture;NoiseTexture;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-4695.118,689.1779;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-4693.552,914.0974;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;-4443.051,-1018.55;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;136;-4782.562,-1257.57;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-4693.953,474.1758;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-4448.771,-779.5245;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-4159.25,-839.347;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-4442.589,-1247.149;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;141;-3605.583,-889.7749;Inherit;False;691.6182;526.6148;(noisetexture.y + (noisetexture * 0.5));4;156;150;147;145;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;143;-3911.753,-985.6815;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-3555.583,-719.0867;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;2;-2942.13,425.7858;Inherit;False;Global;_Position;_Position;0;0;Create;True;0;0;0;False;0;False;0,0,-1;4.000001,4.411825,-73.44601;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;142;-5101.565,-2141.552;Inherit;False;1413.77;786.0975;toptexture;9;184;178;175;172;166;163;158;154;149;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-2979.198,223.8256;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;144;-5724.318,-2062.847;Inherit;False;Property;_TopTextureScale;TopTextureScale;4;0;Create;True;0;0;0;False;0;False;0.5;0.1;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-5363.872,-1608.889;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;146;-3602.426,-1458.375;Inherit;False;781.9852;548.5261;worldNormalDotNoise;5;174;170;165;162;157;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-5365.936,-1847.295;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-5749.692,-365.7169;Inherit;False;Property;_SideTextureScale;SideTextureScale;7;0;Create;True;0;0;0;False;0;False;0.5;0.1;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;4;-2694.716,224.0709;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;149;-5051.565,-2086.162;Inherit;True;Property;_TopTexture;TopTexture;1;0;Create;True;0;0;0;False;0;False;8f48b8bc62ffc4b408d4cb7b15899364;dedff298ed2121c46b72d88135f8b95b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.BreakToComponentsNode;150;-3389.011,-617.1597;Inherit;True;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;3;-2680.979,502.3287;Inherit;False;Global;_SphereRadius;_SphereRadius;0;0;Create;True;0;0;0;False;0;False;3.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-3399.836,-839.7747;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;152;-5127.814,-441.6924;Inherit;False;1472.929;790.1393;sidetexture;9;191;183;181;179;177;176;168;167;159;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;159;-5077.816,-391.6922;Inherit;True;Property;_SideTexture;Side Texture;5;0;Create;True;0;0;0;False;0;False;dedff298ed2121c46b72d88135f8b95b;dedff298ed2121c46b72d88135f8b95b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WorldNormalVector;162;-3553.905,-1389.188;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;156;-3148.967,-748.0748;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;-2450.252,224.3416;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;-5415.425,95.46535;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;158;-4747.38,-1840.425;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-5417.489,-142.9404;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;-5366.607,-2069.817;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;154;-4751.865,-1613.953;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;157;-3552.425,-1127.451;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;-4423.627,-1839.224;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;6;-2683.919,45.2813;Float;False;Global;NoiseScale;NoiseScale;2;0;Create;True;0;0;0;False;0;False;0.3,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-4427.605,-1609.458;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;163;-4745.137,-2091.552;Inherit;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;7;-2705.745,-175.6775;Inherit;True;Global;Texture0;Texture 0;2;0;Create;True;0;0;0;False;0;False;94cd92fed0415f5408340f1dc6885c2f;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SaturateNode;8;-2179.563,223.7489;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;171;-2723.652,-1124.767;Inherit;False;Property;_TopSpread;TopSpread;12;0;Create;True;0;0;0;False;0;False;0.3629656;0;0.05;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;170;-3300.116,-1151.074;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-2720.002,-989.3407;Inherit;False;Property;_EdgeWidth;Edge Width;11;0;Create;True;0;0;0;False;0;False;0.1792134;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;168;-4749.07,99.11308;Inherit;True;Property;_TextureSample7;Texture Sample 7;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-5418.16,-365.4625;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;167;-4751.173,-127.999;Inherit;True;Property;_TextureSample5;Texture Sample 5;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;165;-3282.887,-1380.98;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;-4397.352,-134.1465;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TriplanarNode;10;-2345.766,-40.54346;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-4144.054,-1793.369;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;-4392.571,94.44692;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;174;-3066.242,-1343.883;Inherit;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;-4426.419,-2087.207;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;9;-1950.594,223.7489;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-2434.586,-1070.01;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;179;-4754.608,-377.0567;Inherit;True;Property;_TextureSample6;Texture Sample 6;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;180;-2213.461,-1243.958;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;182;-2211.161,-1011.323;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1724.783,340.9081;Inherit;False;Global;Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1774.375,-39.33506;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;184;-3922.792,-1853.773;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;185;-2265.988,-1661.011;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-4126.119,-64.95296;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-4399.801,-367.2924;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;-2050.656,-1661.706;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;188;-1981.488,-1247.715;Inherit;False;Property;_EdgeColor;EdgeColor;10;0;Create;True;0;0;0;False;0;False;0.1886348,0.254717,0.1988013,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;191;-3889.885,-158.0746;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-1974.547,-1026.649;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;186;-2052.969,-1845.541;Inherit;False;Property;_TopColor;TopColor;3;0;Create;True;0;0;0;False;0;False;0.5475008,0.8679245,0.3807404,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1540.156,47.61602;Inherit;False;Constant;_RadiusNoiseOffset;RadiusNoiseOffset;2;0;Create;True;0;0;0;False;0;False;0.01;1;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;14;-1507.674,266.2238;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StepOpNode;187;-2307.871,-403.0366;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;15;-1260.003,267.7893;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;195;-2114.886,-610.8809;Inherit;False;Property;_SideColor;SideColor;6;0;Create;True;0;0;0;False;0;False;0.1561944,0.2225333,0.2830189,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-1739.745,-1128.525;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;16;-1230.396,-148.9233;Inherit;True;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;-2083.223,-401.5515;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-1806.171,-1684.217;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;197;-1860.787,-433.8081;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1042.473,268.6338;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;20;-1034.089,505.4574;Inherit;False;Global;Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;0.1803922,0.1803922,0.1803922,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;196;-1508.118,-1150.99;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;198;-1263.63,-571.3027;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-820.1955,266.4354;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;23;-594.7308,263.4785;Inherit;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-659.6547,-153.0661;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-376.0216,23.86344;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,-243.0016;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;amp_TriplanarReveal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;112;0;111;0
WireConnection;114;0;112;1
WireConnection;119;0;113;1
WireConnection;119;1;113;2
WireConnection;120;0;112;2
WireConnection;116;0;113;3
WireConnection;116;1;113;1
WireConnection;125;0;116;0
WireConnection;125;1;117;0
WireConnection;127;0;120;0
WireConnection;127;1;118;0
WireConnection;128;0;112;0
WireConnection;123;0;113;3
WireConnection;123;1;113;2
WireConnection;122;0;114;0
WireConnection;122;1;118;0
WireConnection;126;0;119;0
WireConnection;126;1;117;0
WireConnection;129;0;121;0
WireConnection;129;1;126;0
WireConnection;132;0;128;0
WireConnection;132;1;118;0
WireConnection;131;0;123;0
WireConnection;131;1;117;0
WireConnection;133;0;121;0
WireConnection;133;1;125;0
WireConnection;130;0;122;0
WireConnection;130;1;124;0
WireConnection;134;0;127;0
WireConnection;134;1;124;0
WireConnection;135;0;133;0
WireConnection;135;1;130;0
WireConnection;136;0;121;0
WireConnection;136;1;131;0
WireConnection;138;0;132;0
WireConnection;138;1;124;0
WireConnection;137;0;129;0
WireConnection;137;1;134;0
WireConnection;139;0;135;0
WireConnection;139;1;137;0
WireConnection;140;0;136;0
WireConnection;140;1;138;0
WireConnection;143;0;140;0
WireConnection;143;1;139;0
WireConnection;153;0;119;0
WireConnection;153;1;144;0
WireConnection;151;0;116;0
WireConnection;151;1;144;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;150;0;143;0
WireConnection;147;0;143;0
WireConnection;147;1;145;0
WireConnection;156;0;147;0
WireConnection;156;1;150;1
WireConnection;5;0;4;0
WireConnection;5;1;3;0
WireConnection;160;0;119;0
WireConnection;160;1;148;0
WireConnection;158;0;149;0
WireConnection;158;1;151;0
WireConnection;155;0;116;0
WireConnection;155;1;148;0
WireConnection;161;0;123;0
WireConnection;161;1;144;0
WireConnection;154;0;149;0
WireConnection;154;1;153;0
WireConnection;166;0;158;0
WireConnection;166;1;130;0
WireConnection;172;0;154;0
WireConnection;172;1;134;0
WireConnection;163;0;149;0
WireConnection;163;1;161;0
WireConnection;8;0;5;0
WireConnection;170;0;157;0
WireConnection;170;1;156;0
WireConnection;168;0;159;0
WireConnection;168;1;160;0
WireConnection;164;0;123;0
WireConnection;164;1;148;0
WireConnection;167;0;159;0
WireConnection;167;1;155;0
WireConnection;165;0;162;2
WireConnection;176;0;167;0
WireConnection;176;1;130;0
WireConnection;10;0;7;0
WireConnection;10;3;6;0
WireConnection;178;0;166;0
WireConnection;178;1;172;0
WireConnection;177;0;168;0
WireConnection;177;1;134;0
WireConnection;174;0;165;0
WireConnection;174;1;170;0
WireConnection;175;0;163;0
WireConnection;175;1;138;0
WireConnection;9;0;8;0
WireConnection;173;0;171;0
WireConnection;173;1;169;0
WireConnection;179;0;159;0
WireConnection;179;1;164;0
WireConnection;180;0;171;0
WireConnection;180;1;174;0
WireConnection;182;0;174;0
WireConnection;182;1;173;0
WireConnection;12;0;10;0
WireConnection;12;1;9;0
WireConnection;184;0;175;0
WireConnection;184;1;178;0
WireConnection;185;0;173;0
WireConnection;185;1;174;0
WireConnection;181;0;176;0
WireConnection;181;1;177;0
WireConnection;183;0;179;0
WireConnection;183;1;138;0
WireConnection;190;0;185;0
WireConnection;190;1;184;0
WireConnection;191;0;183;0
WireConnection;191;1;181;0
WireConnection;189;0;180;0
WireConnection;189;1;182;0
WireConnection;14;0;12;0
WireConnection;14;1;11;0
WireConnection;187;0;174;0
WireConnection;187;1;171;0
WireConnection;15;0;14;0
WireConnection;15;1;13;0
WireConnection;192;0;188;0
WireConnection;192;1;189;0
WireConnection;16;0;13;0
WireConnection;16;1;12;0
WireConnection;193;0;191;0
WireConnection;193;1;187;0
WireConnection;194;0;186;0
WireConnection;194;1;190;0
WireConnection;197;0;195;0
WireConnection;197;1;193;0
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;196;0;194;0
WireConnection;196;1;192;0
WireConnection;198;0;196;0
WireConnection;198;1;197;0
WireConnection;22;0;19;0
WireConnection;22;1;20;0
WireConnection;23;0;22;0
WireConnection;24;0;16;0
WireConnection;24;1;198;0
WireConnection;25;0;24;0
WireConnection;25;1;23;0
WireConnection;0;0;25;0
WireConnection;0;10;16;0
ASEEND*/
//CHKSM=E7D25FDDA7B9C502961F414DF7208A0F5DD9A049