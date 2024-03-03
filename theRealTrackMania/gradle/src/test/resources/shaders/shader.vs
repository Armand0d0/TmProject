#version 330

layout (location=0) in vec3 position;
layout (location=1) in vec2 texCoord;
layout (location=2) in vec3 vertexNormal;


out vec2 outTexCoord_;
out vec3 mvVertexNormal_;
out vec3 mvVertexPos_;

uniform mat4 modelViewMatrix;
uniform mat4 projectionMatrix;


void main()
{
		vec4 mvPos = modelViewMatrix * vec4(position, 1);
    gl_Position = projectionMatrix * mvPos;
    outTexCoord_ = texCoord;
    mvVertexNormal_ =  normalize(modelViewMatrix * vec4(vertexNormal, 0.0)).xyz;
    mvVertexPos_ = mvPos.xyz;
    
}