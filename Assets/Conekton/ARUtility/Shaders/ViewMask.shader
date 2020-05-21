Shader "ARUtility/ViewMask"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Width("Width", Float) = 10.0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Direction;
            float _Width;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 uv = abs(i.uv * 2.0 - 1.0);

                float2 u = _Width / _ScreenParams.xy * 0.5;

                u = smoothstep(0, u, 1.0 - uv);

                col = col * u.x * u.y;

                return col;
            }
            ENDCG
        }
    }
}

