// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "fx/displ"
{
	Properties
	{
	   _MainTex("MainTex", 2D) = "white" {}
	   _DisplTex("DisplTex", 2D) = "white" {}
	   _DisplScale("DisplScale", Range(0, 1)) = 0.25
	}
		SubShader
	   {
			Tags { "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

		  Pass
		  {
			 CGPROGRAM

			 #pragma vertex vert  
			 #pragma fragment frag

			 uniform sampler2D _MainTex;
			 uniform sampler2D _DisplTex;
			 uniform float4 _MainTex_ST;
			 uniform float4 _DisplTex_ST;
			 uniform float _DisplScale;

			 struct vertexInput
			 {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			 };
			 struct vertexOutput
			 {
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
			 };

			 vertexOutput vert(vertexInput input)
			 {
				vertexOutput output;

				output.tex = input.texcoord;
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			 }

			 float4 frag(vertexOutput input) : COLOR
			 {
				float dipl = (tex2D(_DisplTex, input.tex.xy *_DisplTex_ST.xy + _DisplTex_ST.zw).a - 0.5) * 2 * _DisplScale;

				return tex2D(_MainTex, _MainTex_ST.xy * input.tex.xy + _MainTex_ST.zw + float2(dipl.x, 0));
			 }

			 ENDCG
		  }
	   }
}