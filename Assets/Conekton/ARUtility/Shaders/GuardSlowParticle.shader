Shader "ARUtility/GuardSlowParticle"
{
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            Cull Back
            Zwrite Off
            ZTest Off

            ColorMask 0
        }
    }
}
