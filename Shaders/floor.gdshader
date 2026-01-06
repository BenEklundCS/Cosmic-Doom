shader_type spatial;

uniform sampler2D floor_texture;

void fragment() {
    vec4 pixel_color = texture(floor_texture, UV * 10.0);
    ALBEDO = pixel_color.rgb;
    ALPHA = pixel_color.a;
}