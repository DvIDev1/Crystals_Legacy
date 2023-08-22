sampler uImage0 : register(s0);
sampler uImage1 : register(s1); // Automatically Images/Misc/Perlin via Force Shader testing option
sampler uImage2 : register(s2); // Automatically Images/Misc/noise via Force Shader testing option
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

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    uv.y = uv.y + uTime;
    float4 bg = tex2D(uImage0, coords);
    float4 colors = tex2D(uImage1, coords);
    float4 Fire = tex2D(uImage2, uv);

    colors.r = 0;
    colors.g = 0.5;
    colors.b = 0.25;
    colors.rgb = ((Fire - colors) * bg);

    return colors;

}


technique Technique1
{
    pass CursedFlamesPass
    {

        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}