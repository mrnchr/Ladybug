Shader "Ladybug/BlitLineBrush"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
        _SegmentStart ("Segment Start", Vector) = (0,0,0,0)
        _SegmentEnd ("Segment End", Vector) = (0,0,0,0)
        _BrushRadius ("Brush Radius", Float) = 0.05
        _BrushColor ("Brush Color", Color) = (0,0,0,0)
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

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.texcoord;
                return o;
            }

            sampler2D _MainTex;
            float4 _SegmentStart;
            float4 _SegmentEnd;
            float _BrushRadius;
            float4 _BrushColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 uv = i.uv;

                float2 segStart = _SegmentStart.xy;
                float2 segEnd   = _SegmentEnd.xy;

                float2 q;
                if (distance(segStart, segEnd) < 1e-6)
                {
                    q = segStart;
                }
                else
                {
                    float2 ab = segEnd - segStart;
                    float2 ap = uv - segStart;

                    float t = dot(ap, ab) / dot(ab, ab);
                    t = clamp(t, 0.0, 1.0);

                    q = segStart + t * ab;
                }

                float dist = distance(uv, q);

                if (dist < _BrushRadius)
                {
                    col = _BrushColor;
                }

                return col;
            }
            ENDCG
        }
    }
}