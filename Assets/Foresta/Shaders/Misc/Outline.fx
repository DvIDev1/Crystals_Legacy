sampler uImage0 : register(s0);
sampler uImage1 : register(s1); 
sampler uImage2 : register(s2); 
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float2 uShaderSpecificData;
float2 uWorldPosition;

float4 edge(float2 coords : TEXCOORD0) : COLOR0
{

    float offset = 0.00001;
    float4 col = tex2D(uImage0, coords);
    if (col.a > 0.5)
        return col;
    else
    {
        float a = tex2D(uImage0, float2
        (coords.x + offset, coords.y)).a +

        tex2D(uImage0, float2
        (coords.x, coords.y - offset)).a +

        tex2D(uImage0, float2
        (coords.x - offset, coords.y)).a +

        tex2D(uImage0, float2
        (coords.x, coords.y + offset)).a;
        
        if (col.a < 1.0 && a > 0.0)
        {
            col.r = 1;
            col.b = 1;
            col.g = 0;
            col.a = 1;
        }
        else
            return col.rgba=0,0,0,0;

    }
    return col;
}

technique Technique1
{
    pass Edge
    {
        PixelShader = compile ps_2_0 edge();
    }
}