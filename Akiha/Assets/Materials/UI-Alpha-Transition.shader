Shader "UI/Alpha Transition" {
  Properties {
    [PerRendererData] _MainTex ("Mask Texture", 2D) = "white" {}
    [PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
    _Ratio ("Ratio", Range(0, 1)) = 0
  }

  SubShader {
    Tags {
      "Queue"="AlphaTest"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
    }

    
    Cull Off
    Lighting Off
    ZWrite Off
    ZTest [unity_GUIZTestMode]
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
    CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma target 2.0

      #include "UnityCG.cginc"
      #include "UnityUI.cginc"

      struct appdata_t {
        float4 vertex   : POSITION;
        float4 color    : COLOR;
        float2 texcoord : TEXCOORD0;
      };

      struct v2f {
        float4 vertex   : SV_POSITION;
        fixed4 color    : COLOR;
        float2 texcoord  : TEXCOORD0;
        float4 worldPosition : TEXCOORD1;
      };

      fixed4 _Color;
      fixed4 _TextureSampleAdd;

      v2f vert(appdata_t v) {
        v2f OUT;
        OUT.worldPosition = v.vertex;
        OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);
        OUT.texcoord = v.texcoord;
        #ifdef UNITY_HALF_TEXEL_OFFSET
          OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1, 1);
        #endif
        OUT.color = v.color * _Color;
        return OUT;
      }

      sampler2D _MainTex;
      sampler2D _MaskTex;
      float _Ratio;

      fixed4 frag(v2f v) : SV_Target {
        half4 color = _Color;
        half mask = tex2D(_MaskTex, v.texcoord).a - (_Ratio * 2 - 1);
        color.a = mask;
        clip(mask - 0.001);
        return color;
      }
    ENDCG
    }
  }
}
