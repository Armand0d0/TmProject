#version 330

in vec2 outTexCoord;
in vec3 mvPos;
out vec4 fragColor;

uniform sampler2D texture_sample;
uniform vec4 colour;

void main()
{
    fragColor = colour * texture(texture_sample, outTexCoord);
}