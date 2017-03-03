Shader "2D/ElectricDefeat"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			//"RenderType" = "Transparent"
			//"IgnoreProjector" = "True"
		}
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.color = v.color;

				return o;
			}
			
			sampler2D _MainTex;
			float4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float4 c = i.color;
				c.a = col.a;
				c *= _Color;

				//col = 1 - col;
				return c;
			}
			ENDCG
		}
	}
}
