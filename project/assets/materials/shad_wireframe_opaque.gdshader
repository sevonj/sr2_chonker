// source: https://godotshaders.com/shader/wireframe-shader/
// license: CC0
shader_type spatial;
render_mode cull_disabled, unshaded;

uniform vec4 modelColor : source_color;
uniform vec4 wireframeColor : source_color;

const vec3 vectors[3] = {
	vec3(1.0, 0.0 ,0.0),
	vec3(0.0, 1.0 ,0.0),
	vec3(0.0, 0.0 ,1.0)
};

varying vec3 baryCoord;

void vertex() {
	baryCoord = vectors[VERTEX_ID % 3];
}

void fragment() {
	vec3 dBaryCoordX = dFdx(baryCoord);
	vec3 dBaryCoordY = dFdy(baryCoord);
	vec3 dBaryCoord  = sqrt(dBaryCoordX*dBaryCoordX + dBaryCoordY*dBaryCoordY);

	vec3 remap = smoothstep(
		vec3(0.0),
		dBaryCoord,
		baryCoord
	);
	
	float closestEdge = min(min(remap.x, remap.y), remap.z);

	ALBEDO = mix(wireframeColor, modelColor, closestEdge).xyz;
}