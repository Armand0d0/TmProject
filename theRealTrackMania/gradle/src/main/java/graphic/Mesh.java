package graphic;

import org.joml.Vector3f;
import org.lwjgl.system.MemoryUtil;

import java.nio.FloatBuffer;
import java.nio.IntBuffer;

import static org.lwjgl.opengl.GL11.*;
import static org.lwjgl.opengl.GL15.*;
import static org.lwjgl.opengl.GL20.*;
import static org.lwjgl.opengl.GL30.*;


public class Mesh {
	private  int vaoId;
	private  int PointVaoId;
	private  int PointVboId;
	private  int vboId;
	private int idxVboId;
	private int texCoordsVboId;
	private  int NvboId;

	 private  int vertexCount;
	 private Vector3f colour;
	 private Material material;


	 private static final Vector3f DEFAULT_COLOUR = new Vector3f(1.0f, 1.0f, 1.0f);
	 public Mesh(){
		 
	 }

	 public Mesh(float[] positions , int[] indices, float[] texCoords,float[] normals) {
		 FloatBuffer verticesBuffer = null;
		 IntBuffer indicesBuffer = null;
		 FloatBuffer texCoordsBuffer = null;
		 FloatBuffer vecNormalsBuffer = null;
		 try {	      
			 colour = DEFAULT_COLOUR;
			 vertexCount = indices.length;
			 
			 vaoId = glGenVertexArrays();
			 glBindVertexArray(vaoId);
			 
	            vboId = glGenBuffers();
	            verticesBuffer  = MemoryUtil.memAllocFloat(positions.length);
	            verticesBuffer.put(positions).flip();
	            glBindBuffer(GL_ARRAY_BUFFER, vboId);
	            glBufferData(GL_ARRAY_BUFFER, verticesBuffer, GL_STATIC_DRAW);
	            glVertexAttribPointer(0, 3, GL_FLOAT, false, 0, 0);
	          
	           
	            idxVboId = glGenBuffers(); 
	            indicesBuffer = MemoryUtil.memAllocInt(indices.length);
	            indicesBuffer.put(indices).flip();
	            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, idxVboId);
	            glBufferData(GL_ELEMENT_ARRAY_BUFFER, indicesBuffer, GL_STATIC_DRAW);
	           
	            
	           texCoordsVboId = glGenBuffers();
	            texCoordsBuffer = MemoryUtil.memAllocFloat(texCoords.length);
	            texCoordsBuffer.put(texCoords).flip();
	            glBindBuffer(GL_ARRAY_BUFFER, texCoordsVboId);
	            glBufferData(GL_ARRAY_BUFFER, texCoordsBuffer, GL_STATIC_DRAW);
	            glVertexAttribPointer(1, 2, GL_FLOAT, false, 0, 0);
	            
	            NvboId = glGenBuffers();
	            //vboIdList.add(vboId);
	            vecNormalsBuffer = MemoryUtil.memAllocFloat(normals.length);
	            vecNormalsBuffer.put(normals).flip();
	            glBindBuffer(GL_ARRAY_BUFFER, NvboId);
	            glBufferData(GL_ARRAY_BUFFER, vecNormalsBuffer, GL_STATIC_DRAW);
	            glVertexAttribPointer(2, 3, GL_FLOAT, false, 0, 0);
	            
	            glBindBuffer(GL_ARRAY_BUFFER, 0);
	            glBindVertexArray(0);
	            
	            
	           
		 }finally {
			 if (verticesBuffer  != null) {
	                MemoryUtil.memFree(verticesBuffer);
	                
	            }
			 if (indicesBuffer  != null) {
	               
	                MemoryUtil.memFree(indicesBuffer);
	            }
			 if (texCoordsBuffer  != null) {
	               
	                MemoryUtil.memFree(texCoordsBuffer);
	            }
			 if (vecNormalsBuffer  != null) {
	               
	                MemoryUtil.memFree(vecNormalsBuffer);
	            }
		 } 
	 }
	 		public void render() {

				if (material != null && material.getTexture() != null) {
					// Activate first texture unit
					glActiveTexture(GL_TEXTURE0);
					// Bind the texture
					glBindTexture(GL_TEXTURE_2D, material.getTexture().getId());
				}

				glBindVertexArray(getVaoId());
				glEnableVertexAttribArray(0);
				glEnableVertexAttribArray(1);
				glEnableVertexAttribArray(2);

				glDrawElements(GL_TRIANGLES, getVertexCount(), GL_UNSIGNED_INT, 0);

				glDisableVertexAttribArray(0);
				glDisableVertexAttribArray(1);
				glDisableVertexAttribArray(2);
				glBindVertexArray(0);
			}

	 		


	 	    public void setColour(Vector3f colour) {
	 	        this.colour = colour;
	 	    }

	 	    public Vector3f getColour() {
	 	        return this.colour;
	 	}
	 		public int getVaoId() {
			 return vaoId;
		    }
		    public int getVertexCount() {
		        return vertexCount;
		    }

		    public void cleanUp() {

		        glDisableVertexAttribArray(0);
		        glBindBuffer(GL_ARRAY_BUFFER, 0);
		        glDeleteBuffers(vboId);
		        glDeleteBuffers(idxVboId);
		        glDeleteBuffers(texCoordsVboId);
		        glDeleteBuffers(NvboId);
		        
		        if (material.getTexture() != null) {
					material.getTexture().cleanup();
		}

		        glBindVertexArray(0);
		        glDeleteVertexArrays(vaoId);
		        
		    }
		    public void setMaterial(Material material){
	 	this.material=material;
			}
	public Material getMaterial(){
		return this.material;
	}


	public void deleteBuffers() {
		glDisableVertexAttribArray(0);

		// Delete the VBOs
		glBindBuffer(GL_ARRAY_BUFFER, 0);
			glDeleteBuffers(vboId);
		glDeleteBuffers(NvboId);
		glDeleteBuffers(texCoordsVboId);
		glDeleteBuffers(idxVboId);


		// Delete the VAO
		glBindVertexArray(0);
		glDeleteVertexArrays(vaoId);
	}
		    
		    
		    
		    
}
