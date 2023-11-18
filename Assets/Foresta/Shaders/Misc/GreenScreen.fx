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
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = tex2D(uImage0, coords);

    float2 uv = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 centreCoords = (coords - uv) * (uScreenResolution / uScreenResolution.y);
    float4 d = length(centreCoords);
    

    d.rb = sin(d.rb - uTime);
    d.g = 1.5f + uOpacity;
    col.rgb *= d.rgb;
    return col;
}

technique Technique1
{
    pass GreenScreen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}