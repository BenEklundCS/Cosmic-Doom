shader_type sky;

uniform sampler2D sky_texture : hint_default_white;
uniform float horizontal_repeat : hint_range(1.0, 20.0) = 3.0;

void sky() {
    vec2 uv = SKY_COORDS;
    
    // Repeat only in horizontal direction
    uv.x = fract(uv.x * horizontal_repeat);
    
    // Sample the texture and apply to sky
    COLOR = texture(sky_texture, uv).rgb;
}