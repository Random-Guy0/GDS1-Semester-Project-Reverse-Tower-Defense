Shader "Custom/Outline"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline color", COlor) = (0, 0, 0, 1)
        _OutlineWidth("Outline width", Range(2.0, 0.5)) = 1.0
        [HDR] _EmissionColor("Emission Color", Color) = (0, 0, 0, 1)
    }

    SubShader
    {
        //Tags{"Queue" = "Transparent"}

        Pass //render outline
        {
            ZWrite Off

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v.vertex.xyz *= _OutlineWidth;

                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }
            ENDCG
        }

        Pass //normal render
        {
            ZWrite On

            /*Lighting On

            Material
            {
                Diffuse[_Color]
                Ambient[_Color]
            }

            Lighting On

            SetTexture[_MainTex]
            {
            ConstantColor[_Color]
            }

            SetTexture[_MainTex]
            {
            Combine previous * primary DOUBLE
            }*/
            
            Tags {"LightMode"="ForwardBase"}
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "AutoLight.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1)
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal, 1));
                TRANSFER_SHADOW(o)
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _EmissionColor;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient + _EmissionColor;
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}