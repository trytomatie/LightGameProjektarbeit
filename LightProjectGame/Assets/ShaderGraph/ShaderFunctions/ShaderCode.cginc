float CheckMaskSpehere(matrix3 LightPos,float radius)
{
float3 dist1 = LightPos[0] - PixelPos;
float3 dist2 = LightPos[1] - PixelPos;
float3 dist3 = LightPos[2] - PixelPos;
if (length(dist1) < Radius || length(dist2) < Radius || length(dist3) < Radius)
{
	Out = 1;
}
else
{
	Out = 0;
}
}
	