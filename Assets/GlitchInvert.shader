Shader "Custom/GlitchInvert"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Glitch Intensity", Float) = 1.0
        _Offset ("Offset", Float) = 0.01
        _TimeSpeed ("Time Speed", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Intensity;
            float _Offset;
            float _TimeSpeed;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float time = _Time.y * _TimeSpeed;

                float glitch = (sin(i.uv.y * 300.0 + time * 10.0) * _Offset) * _Intensity;
                float2 uv = i.uv + float2(glitch, 0);

                fixed4 col = tex2D(_MainTex, uv);
                col.rgb = 1.0 - col.rgb;
                col.rgb *= (0.9 + 0.1 * sin(i.uv.y * 800.0 + time * 50.0));
                return col;
            }
            ENDCG
        }
    }
}
