// Shader "Custom/ColorMaskShader"

Shader "Custom/ColorMaskShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskColor ("Mask Color", Color) = (0,1,0,0)
        _Tolerance ("Tolerance", Range(0, 1)) = 0.75
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }

        // Render the front faces
        Pass
        {
            Cull Off // Disable face culling for front faces
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MaskColor;
            float _Tolerance;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 maskColor = _MaskColor;

                // Calculate the color difference
                float diff = distance(col.rgb, maskColor.rgb);

                // If the difference is within the tolerance, make it transparent
                if (diff <= _Tolerance)
                    //col.a = 0;
                    clip(-1);

                return col;
            }
            ENDCG
        }

        // Render the back faces
        Pass
        {
            Cull Front // Cull the front faces (default behavior)
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MaskColor;
            float _Tolerance;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 maskColor = _MaskColor;

                // Calculate the color difference
                float diff = distance(col.rgb, maskColor.rgb);

                // If the difference is within the tolerance, make it transparent
                if (diff <= _Tolerance)
                    //col.a = 0;
                    clip(-1);

                return col;
            }
            ENDCG
        }
    }
}

