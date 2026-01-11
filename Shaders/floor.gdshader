shader_type spatial;

uniform sampler2D floor_texture;
uniform float tiling_factor;

void fragment() {
    vec4 pixel_color = texture(floor_texture, UV * tiling_factor);
    ALBEDO = pixel_color.rgb;
    ALPHA = pixel_color.a;
}