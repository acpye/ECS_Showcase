#version 330

in vec2 v_TexCoord;

uniform sampler2D s_texture;

out vec4 FragColor;

void main()
{
    FragColor = texture(s_texture, v_TexCoord);
}