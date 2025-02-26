// source: https://godotshaders.com/shader/infinite-ground-grid/
// license: CC0
shader_type spatial;
render_mode blend_mix,depth_draw_opaque, unshaded, cull_disabled;

uniform vec4 gridColor: source_color;
uniform vec4 checkerColor: source_color;
uniform float fadeStart = 0.0;
uniform float fadeEnd = 10.0;
uniform float unitSize = 1.0;
uniform int subdivisions: hint_range(1, 10) = 5;
uniform float majorLineThickness = 2.0;
uniform float minorLineThickness = 1.0;
uniform float minorLineAlpha: hint_range(0.0, 1.0) = .3;
uniform vec3 centerOffset = vec3(0.0, 0.0, 0.0);

// calculate line mask, usning a bit of fwidth() magic to make line width not affected by perspective
float grid(vec2 pos, float unit, float thickness){
	vec2 threshold = fwidth(pos) * thickness * .5 / unit;
	vec2 posWrapped = pos / unit;
	vec2 line = step(fract(-posWrapped), threshold) + step(fract(posWrapped), threshold);
	return max(line.x, line.y);
}

// calculate checkerboard mask
float checker(vec2 pos, float unit){
	float square1 = step(.5, fract(pos.x / unit *.5));
	float square2 = step(.5, fract(pos.y / unit *.5));
	return max(square1,square2) - square1 * square2;
}

void fragment() {
	// ray from camera to fragemnt in wrold space
	vec3 rayWorld = normalize(mat3(INV_VIEW_MATRIX) * VIEW) ;

	// calculate fragment position in world space
	vec3 posWorld;
	float t = -(CAMERA_POSITION_WORLD.y - centerOffset.y) / (rayWorld.y );
	posWorld.y = -centerOffset.y;
	posWorld.xz = CAMERA_POSITION_WORLD.xz + t * rayWorld.xz + centerOffset.xz;

	// calculate planar distance from camera to fragment (used for fading)
	//float distPlanar = distance(posWorld.xz, centerOffset.xz);
	float distPlanar = distance(posWorld, CAMERA_POSITION_WORLD);

	// grid
	float line = grid(posWorld.xz, unitSize, majorLineThickness);
	line += grid(posWorld.xz, unitSize / float(subdivisions), minorLineThickness) * minorLineAlpha;
	line = clamp(line, 0.0, 1.0);

	// checkerboard
	float chec = checker(posWorld.xz, unitSize);

	// distance fade factor
	float fadeFactor = 1.0 - clamp((distPlanar - fadeStart) / (fadeEnd - fadeStart), 0.0, 1.0);

	// write ground plane depth into z buffer
	vec4 pp = (PROJECTION_MATRIX * (VIEW_MATRIX * vec4(posWorld, 1.0)));
	DEPTH = pp.z / pp.w;

	// final alpha
	float alphaGrid = line * gridColor.a;
	float alphaChec = chec * checkerColor.a;
	ALPHA = clamp(alphaGrid + alphaChec, 0.0, 1.0) * fadeFactor;
	// eliminate grid above the horizon
	ALPHA *= step(t, 0.0);

	// final color (premultiplied alpha blend)
	ALBEDO = (checkerColor.rgb * alphaChec) * (1.0 - alphaGrid) + (gridColor.rgb * alphaGrid);

}