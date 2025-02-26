// Unshaded vertex color lines. Used for editor axis lines
// license: CC0
shader_type spatial;
render_mode unshaded;

const float fadeStart = 1440.0;
const float fadeEnd = 2048.0;

void fragment() {
	float depth = FRAGCOORD.z;
	vec2 frag_ndc = ((FRAGCOORD.xy / VIEWPORT_SIZE) * 2.0) - 1.0; 
	vec4 frag_view_space_position = INV_PROJECTION_MATRIX * vec4(frag_ndc, depth, 1.0);
	frag_view_space_position /= frag_view_space_position.w;
	float dist = length(INV_VIEW_MATRIX * frag_view_space_position);
	float fadeFactor = 1.0 - clamp((dist - fadeStart) / (fadeEnd - fadeStart), 0.0, 1.0);
	ALPHA = fadeFactor;
	
	ALBEDO = COLOR.rgb;
}