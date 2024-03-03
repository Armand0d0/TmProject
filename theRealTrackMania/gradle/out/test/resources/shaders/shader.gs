#version 330

layout ( triangles ) in;
layout ( triangle_strip ,max_vertices = 30) out;


in vec2 outTexCoord_[];
in vec3 mvVertexNormal_[];
in vec3 mvVertexPos_[];


out vec2 outTexCoord;
out vec3 mvVertexNormal;
out vec3 mvVertexPos;
out vec2 texcoords;



void TriVertex(int index){
outTexCoord = outTexCoord_[index];
	mvVertexNormal = mvVertexNormal_[index];
	mvVertexPos = mvVertexPos_[index];
	gl_Position = gl_in[index].gl_Position;
	EmitVertex();
}
void Vertex(vec4 pos,vec2 tex){
		gl_Position = pos;
		outTexCoord = tex;
		EmitVertex();
}
void main(void){

		TriVertex(0);
		TriVertex(1);
		TriVertex(2);

	EndPrimitive();
/*
		float x =0;
		float rectSize = 0.05f;


		Vertex(gl_in[0].gl_Position+vec4(-rectSize,-rectSize,0,0),vec2(x,0.1));
		Vertex(gl_in[0].gl_Position+vec4(-rectSize,rectSize,0,0),vec2(x,0));
		Vertex(gl_in[0].gl_Position+vec4(rectSize,rectSize,0,0),vec2(x+0.1,0));

		Vertex(gl_in[0].gl_Position+vec4(rectSize,rectSize,0,0),vec2(x+0.1,0));
		Vertex(gl_in[0].gl_Position+vec4(rectSize,-rectSize,0,0),vec2(x+0.1,0.1));
		Vertex(gl_in[0].gl_Position+vec4(-rectSize,-rectSize,0,0),vec2(x,0.1));

	EndPrimitive();
*/
}