// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amp_Triplanar"
{
	Properties
	{
		_TopTexture("TopTexture", 2D) = "white" {}
		_TopColor("TopColor", Color) = (0.5475008,0.8679245,0.3807404,1)
		_TopTextureScale("TopTextureScale", Range( -10 , 10)) = 0.6033961
		_SideTexture("Side Texture", 2D) = "white" {}
		_SideColor("SideColor", Color) = (0.1561944,0.2225333,0.2830189,1)
		_SideTextureScale("SideTextureScale", Range( -10 , 10)) = 0.6033961
		_Noise("Noise", 2D) = "white" {}
		_NoiseTextureScale("NoiseTextureScale", Range( -10 , 10)) = 0.6033961
		_EdgeColor("EdgeColor", Color) = (0.1886348,0.254717,0.1988013,1)
		_EdgeWidth("Edge Width", Range( 0 , 1)) = 0.1792134
		_TopSpread("TopSpread", Range( 0.05 , 4)) = 0.3629656
		_DecayingMin("DecayingMin", Range( 0.1 , 0.9)) = 0.1
		_DecayingMax("Decaying Max", Range( 0.1 , 0.9)) = 0.9
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldNormal;
			float3 worldPos;
		};

		uniform float4 _TopColor;
		uniform float _TopSpread;
		uniform float _EdgeWidth;
		uniform float _DecayingMin;
		uniform float _DecayingMax;
		uniform sampler2D _Noise;
		uniform float _NoiseTextureScale;
		uniform sampler2D _TopTexture;
		uniform float _TopTextureScale;
		uniform float4 _EdgeColor;
		uniform float4 _SideColor;
		uniform sampler2D _SideTexture;
		uniform float _SideTextureScale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float clampResult162 = clamp( _SinTime.x , _DecayingMin , _DecayingMax );
			float temp_output_164_0 = ( ( _TopSpread + _EdgeWidth ) * clampResult162 );
			float3 ase_worldNormal = i.worldNormal;
			float4 temp_cast_0 = (saturate( ase_worldNormal.y )).xxxx;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 ase_worldPos = i.worldPos;
			float4 appendResult15 = (float4(ase_worldPos.z , ase_worldPos.y , 0.0 , 0.0));
			float2 panner157 = ( _SinTime.y * float2( 0.05,0.01 ) + ( appendResult15 * _NoiseTextureScale ).xy);
			float3 break69 = ase_worldNormal;
			float temp_output_83_0 = ( pow( abs( break69.x ) , 4.0 ) * 1.4 );
			float4 appendResult17 = (float4(ase_worldPos.z , ase_worldPos.x , 0.0 , 0.0));
			float2 panner159 = ( _SinTime.y * float2( 0.05,0.01 ) + ( appendResult17 * _NoiseTextureScale ).xy);
			float temp_output_85_0 = ( pow( abs( break69.y ) , 4.0 ) * 1.4 );
			float4 appendResult18 = (float4(ase_worldPos.x , ase_worldPos.y , 0.0 , 0.0));
			float2 panner160 = ( _SinTime.y * float2( 0.05,0.01 ) + ( appendResult18 * _NoiseTextureScale ).xy);
			float temp_output_72_0 = ( pow( abs( break69.z ) , 4.0 ) * 1.4 );
			float4 temp_output_96_0 = ( ( tex2D( _Noise, panner157 ) * temp_output_83_0 ) + ( ( tex2D( _Noise, panner159 ) * temp_output_85_0 ) + ( tex2D( _Noise, panner160 ) * temp_output_72_0 ) ) );
			float dotResult117 = dot( temp_cast_0 , ( float4( ase_vertexNormal , 0.0 ) + ( ( temp_output_96_0 * 0.5 ) + temp_output_96_0.g ) ) );
			o.Albedo = ( ( ( _TopColor * ( step( temp_output_164_0 , dotResult117 ) * ( ( tex2D( _TopTexture, ( appendResult15 * _TopTextureScale ).xy ) * temp_output_83_0 ) + ( ( tex2D( _TopTexture, ( appendResult17 * _TopTextureScale ).xy ) * temp_output_85_0 ) + ( tex2D( _TopTexture, ( appendResult18 * _TopTextureScale ).xy ) * temp_output_72_0 ) ) ) ) ) + ( _EdgeColor * ( step( _TopSpread , dotResult117 ) * step( dotResult117 , temp_output_164_0 ) ) ) ) + ( _SideColor * ( ( ( tex2D( _SideTexture, ( appendResult15 * _SideTextureScale ).xy ) * temp_output_83_0 ) + ( ( tex2D( _SideTexture, ( appendResult17 * _SideTextureScale ).xy ) * temp_output_85_0 ) + ( tex2D( _SideTexture, ( appendResult18 * _SideTextureScale ).xy ) * temp_output_72_0 ) ) ) * step( dotResult117 , ( clampResult162 * _TopSpread ) ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
382;1494;1880;961;1342.008;2129.672;2.083391;True;True
Node;AmplifyShaderEditor.CommentaryNode;94;-2570.988,304.404;Inherit;False;1355.03;743.9213;World Normal Axis Decomposition (xyz);13;1;69;68;74;3;77;81;70;75;82;72;85;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-2481.935,483.906;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;11;-3677.49,-1310.219;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;69;-2213.051,488.209;Inherit;True;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;17;-3395.344,-1188.428;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-3392.488,-850.9191;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-3142.536,-1344.387;Inherit;False;Property;_NoiseTextureScale;NoiseTextureScale;7;0;Create;True;0;0;0;False;0;False;0.6033961;0.1;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;68;-1946.703,629.5417;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;125;-1984.406,-1423.435;Inherit;False;1424.92;782.0477;noisetexture;9;25;23;7;22;92;93;95;91;96;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2169.544,740.2999;Inherit;False;Constant;_WorldPow;WorldPow;3;0;Create;True;0;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-3384.169,-1512.642;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;74;-1943.086,539.7755;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-2806.578,-1123.23;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SinTimeNode;161;-2558.331,-1271.907;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-2804.514,-884.8249;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-1946.003,769.8826;Inherit;False;Constant;_NormalSaturation;NormalSaturation;8;0;Create;True;0;0;0;False;0;False;1.4;0.15;0;1.4;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;75;-1811.675,539.2193;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;81;-1946.283,453.8348;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;-2807.249,-1345.752;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;160;-2295.17,-868.9369;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;25;-1934.406,-1367.755;Inherit;True;Property;_Noise;Noise;6;0;Create;True;0;0;0;False;0;False;09f8c5f70b5707b40ab3cdaf39c87491;09f8c5f70b5707b40ab3cdaf39c87491;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PowerNode;70;-1812.159,647.7042;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;159;-2293.568,-1105.737;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;23;-1674.554,-889.6312;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;157;-2298.17,-1345.792;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-1576.286,798.2348;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;82;-1810.456,439.8471;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1664.78,-1134.614;Inherit;True;Property;_NoiseTexture;NoiseTexture;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-1577.852,573.3155;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-1331.505,-895.3885;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-1325.785,-1134.415;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;22;-1665.296,-1373.435;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1576.687,358.3133;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;-1041.984,-955.2109;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1325.323,-1363.014;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;123;-524.4175,-1011.339;Inherit;False;691.6182;526.6148;(noisetexture.y + (noisetexture * 0.5));4;114;113;109;110;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;127;-1984.299,-2257.417;Inherit;False;1413.77;786.0975;toptexture;9;71;89;38;86;87;36;88;37;34;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;-794.4863,-1101.546;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2607.052,-2178.712;Inherit;False;Property;_TopTextureScale;TopTextureScale;2;0;Create;True;0;0;0;False;0;False;0.6033961;0.1;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-474.4174,-840.6511;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-2248.67,-1963.16;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;126;-2010.548,-557.5563;Inherit;False;1472.929;790.1393;sidetexture;9;106;99;100;105;102;103;98;101;104;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-2632.426,-481.5807;Inherit;False;Property;_SideTextureScale;SideTextureScale;5;0;Create;True;0;0;0;False;0;False;0.6033961;0.1;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;114;-307.8444,-738.7238;Inherit;True;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2246.606,-1724.754;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-318.67,-961.3387;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;128;-540.2597,-1589.44;Inherit;False;781.9852;548.5261;worldNormalDotNoise;5;118;116;115;117;135;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;34;-1934.299,-2202.027;Inherit;True;Property;_TopTexture;TopTexture;0;0;Create;True;0;0;0;False;0;False;8f48b8bc62ffc4b408d4cb7b15899364;dedff298ed2121c46b72d88135f8b95b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-67.80035,-869.6388;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;166;195.4927,-823.3273;Inherit;False;Property;_DecayingMin;DecayingMin;12;0;Create;True;0;0;0;False;0;False;0.1;0;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;193.5013,-739.5429;Inherit;False;Property;_DecayingMax;Decaying Max;13;0;Create;True;0;0;0;False;0;False;0.9;0.9;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;272.7414,-1252.058;Inherit;False;Property;_TopSpread;TopSpread;10;0;Create;True;0;0;0;False;0;False;0.3629656;0;0.05;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;115;-490.2591,-1258.516;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;163;325.1309,-976.0535;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2249.341,-2185.682;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;108;276.3913,-1116.631;Inherit;False;Property;_EdgeWidth;Edge Width;9;0;Create;True;0;0;0;False;0;False;0.1792134;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-2300.223,-258.8037;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;118;-491.7385,-1520.253;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;37;-1630.114,-1956.29;Inherit;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;98;-1960.55,-507.5562;Inherit;True;Property;_SideTexture;Side Texture;3;0;Create;True;0;0;0;False;0;False;dedff298ed2121c46b72d88135f8b95b;dedff298ed2121c46b72d88135f8b95b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;-2298.159,-20.39764;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;38;-1634.599,-1729.819;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;129;613.5607,-1170.453;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;162;504.7842,-922.116;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-1627.871,-2207.417;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;135;-220.7212,-1512.045;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;101;-1631.804,-16.74984;Inherit;True;Property;_TextureSample7;Texture Sample 7;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1310.339,-1725.323;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1306.361,-1955.089;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-2300.894,-481.3263;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-237.9499,-1282.139;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;100;-1633.907,-243.8622;Inherit;True;Property;_TextureSample6;Texture Sample 6;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;99;-1637.342,-492.9207;Inherit;True;Property;_TextureSample5;Texture Sample 5;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-1026.788,-1909.234;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-1275.305,-21.41605;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-1280.086,-250.0098;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;117;-4.075728,-1474.948;Inherit;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;816.389,-912.2242;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1309.153,-2203.072;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-1008.852,-180.816;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-1282.535,-483.1563;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-805.5255,-1969.638;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;143;1239.446,-1354.093;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;646.7604,-1444.656;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;134;1060.434,-1771.146;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;144;1241.746,-1121.458;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;1478.36,-1136.784;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;146;1471.419,-1357.85;Inherit;False;Property;_EdgeColor;EdgeColor;8;0;Create;True;0;0;0;False;0;False;0.1886348,0.254717,0.1988013,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;1275.766,-1771.841;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;130;1018.551,-513.1702;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;-772.6193,-273.9377;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;138;1273.453,-1955.676;Inherit;False;Property;_TopColor;TopColor;1;0;Create;True;0;0;0;False;0;False;0.5475008,0.8679245,0.3807404,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;140;1211.536,-721.0145;Inherit;False;Property;_SideColor;SideColor;4;0;Create;True;0;0;0;False;0;False;0.1561944,0.2225333,0.2830189,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;1713.162,-1238.66;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;1520.251,-1794.352;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;1243.199,-511.6852;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;1465.635,-543.9418;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;137;1944.789,-1261.125;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;148;2254.739,-1156.03;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;90;2492.287,-1032.797;Inherit;False;Property;_Specular;Specular;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2672.721,-1157.586;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;amp_Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;69;0;1;0
WireConnection;17;0;11;3
WireConnection;17;1;11;1
WireConnection;18;0;11;1
WireConnection;18;1;11;2
WireConnection;68;0;69;2
WireConnection;15;0;11;3
WireConnection;15;1;11;2
WireConnection;74;0;69;1
WireConnection;151;0;17;0
WireConnection;151;1;150;0
WireConnection;152;0;18;0
WireConnection;152;1;150;0
WireConnection;75;0;74;0
WireConnection;75;1;3;0
WireConnection;81;0;69;0
WireConnection;149;0;15;0
WireConnection;149;1;150;0
WireConnection;160;0;152;0
WireConnection;160;1;161;2
WireConnection;70;0;68;0
WireConnection;70;1;3;0
WireConnection;159;0;151;0
WireConnection;159;1;161;2
WireConnection;23;0;25;0
WireConnection;23;1;160;0
WireConnection;157;0;149;0
WireConnection;157;1;161;2
WireConnection;72;0;70;0
WireConnection;72;1;77;0
WireConnection;82;0;81;0
WireConnection;82;1;3;0
WireConnection;7;0;25;0
WireConnection;7;1;159;0
WireConnection;85;0;75;0
WireConnection;85;1;77;0
WireConnection;93;0;23;0
WireConnection;93;1;72;0
WireConnection;92;0;7;0
WireConnection;92;1;85;0
WireConnection;22;0;25;0
WireConnection;22;1;157;0
WireConnection;83;0;82;0
WireConnection;83;1;77;0
WireConnection;95;0;92;0
WireConnection;95;1;93;0
WireConnection;91;0;22;0
WireConnection;91;1;83;0
WireConnection;96;0;91;0
WireConnection;96;1;95;0
WireConnection;19;0;17;0
WireConnection;19;1;8;0
WireConnection;114;0;96;0
WireConnection;20;0;18;0
WireConnection;20;1;8;0
WireConnection;109;0;96;0
WireConnection;109;1;110;0
WireConnection;113;0;109;0
WireConnection;113;1;114;1
WireConnection;16;0;15;0
WireConnection;16;1;8;0
WireConnection;155;0;17;0
WireConnection;155;1;154;0
WireConnection;37;0;34;0
WireConnection;37;1;19;0
WireConnection;156;0;18;0
WireConnection;156;1;154;0
WireConnection;38;0;34;0
WireConnection;38;1;20;0
WireConnection;129;0;107;0
WireConnection;129;1;108;0
WireConnection;162;0;163;1
WireConnection;162;1;166;0
WireConnection;162;2;167;0
WireConnection;36;0;34;0
WireConnection;36;1;16;0
WireConnection;135;0;118;2
WireConnection;101;0;98;0
WireConnection;101;1;156;0
WireConnection;71;0;38;0
WireConnection;71;1;72;0
WireConnection;87;0;37;0
WireConnection;87;1;85;0
WireConnection;153;0;15;0
WireConnection;153;1;154;0
WireConnection;116;0;115;0
WireConnection;116;1;113;0
WireConnection;100;0;98;0
WireConnection;100;1;155;0
WireConnection;99;0;98;0
WireConnection;99;1;153;0
WireConnection;86;0;87;0
WireConnection;86;1;71;0
WireConnection;104;0;101;0
WireConnection;104;1;72;0
WireConnection;103;0;100;0
WireConnection;103;1;85;0
WireConnection;117;0;135;0
WireConnection;117;1;116;0
WireConnection;164;0;129;0
WireConnection;164;1;162;0
WireConnection;88;0;36;0
WireConnection;88;1;83;0
WireConnection;105;0;103;0
WireConnection;105;1;104;0
WireConnection;102;0;99;0
WireConnection;102;1;83;0
WireConnection;89;0;88;0
WireConnection;89;1;86;0
WireConnection;143;0;107;0
WireConnection;143;1;117;0
WireConnection;165;0;162;0
WireConnection;165;1;107;0
WireConnection;134;0;164;0
WireConnection;134;1;117;0
WireConnection;144;0;117;0
WireConnection;144;1;164;0
WireConnection;145;0;143;0
WireConnection;145;1;144;0
WireConnection;136;0;134;0
WireConnection;136;1;89;0
WireConnection;130;0;117;0
WireConnection;130;1;165;0
WireConnection;106;0;102;0
WireConnection;106;1;105;0
WireConnection;147;0;146;0
WireConnection;147;1;145;0
WireConnection;139;0;138;0
WireConnection;139;1;136;0
WireConnection;131;0;106;0
WireConnection;131;1;130;0
WireConnection;141;0;140;0
WireConnection;141;1;131;0
WireConnection;137;0;139;0
WireConnection;137;1;147;0
WireConnection;148;0;137;0
WireConnection;148;1;141;0
WireConnection;0;0;148;0
ASEEND*/
//CHKSM=55D8A6ABEECFA4CE9A059B48A65DFE5A5A1C2280