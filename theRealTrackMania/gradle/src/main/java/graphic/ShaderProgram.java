package graphic;


import org.joml.Matrix4f;
import org.joml.Vector3f;
import org.joml.Vector4f;
import org.lwjgl.system.MemoryStack;

import java.nio.FloatBuffer;
import java.util.HashMap;
import java.util.Map;

import static org.lwjgl.opengl.GL20.*;
import static org.lwjgl.opengl.GL32.GL_GEOMETRY_SHADER;


public class ShaderProgram {

	private	final int programId;
	private int vertexShaderId;
	private int geometryShaderId;
	private int fragmentShaderId;
	private final Map<String, Integer> uniforms = new HashMap<>();
	 
	public ShaderProgram()throws Exception{
		programId = glCreateProgram();
		if(programId == 0) {
			throw new IllegalStateException("ProgramId = 0");
		}
	}
	public void createVertexShader(String shaderCode) throws Exception {
		 vertexShaderId = createShader(shaderCode, GL_VERTEX_SHADER);
	}
	public void createGeometryShader(String shaderCode) throws Exception {
		geometryShaderId = createShader(shaderCode, GL_GEOMETRY_SHADER);
	}
	public void createFragmentShader(String shaderCode) throws Exception {
		fragmentShaderId = createShader(shaderCode, GL_FRAGMENT_SHADER);
	}
	protected int createShader(String shaderCode, int shaderType) throws Exception {
		int shaderId = glCreateShader(shaderType);
		if (shaderId == 0) {
			throw new Exception("Error creating shader. Type: " + shaderType);
		}
		glShaderSource(shaderId,shaderCode);
		glCompileShader(shaderId);
		if (glGetShaderi(shaderId, GL_COMPILE_STATUS) == 0) {
			throw new Exception("Error compiling Shader code: " + glGetShaderInfoLog(shaderId, 1024) + "<<<<<<<<<<");
		}
		
		glAttachShader(programId, shaderId);
		
		return shaderId;
	}
	public void link()throws Exception{
		glLinkProgram(programId);
		if (glGetProgrami(programId, GL_LINK_STATUS) == 0) {
			throw new Exception("Error linking Shader code: " + glGetProgramInfoLog(programId, 1024));
		}
		
		if (vertexShaderId != 0) {
			 glDetachShader(programId, vertexShaderId);
		}
		if (vertexShaderId != 0) {
			glDetachShader(programId, geometryShaderId);
		}
		if (fragmentShaderId != 0) {
			glDetachShader(programId, fragmentShaderId);
		}
		glValidateProgram(programId);
		if(glGetProgrami(programId, GL_VALIDATE_STATUS) == 0) {
			System.err.println("Warning validating Shader code: " + glGetProgramInfoLog(programId, 1024));
		}
	}
	
	
	public void bind() {
		glUseProgram(programId);
	}
	
	public void unbind() {
		glUseProgram(0);
	}
	public void createUniform(String uniformName) throws Exception {
	    int uniformLocation = glGetUniformLocation(programId, uniformName);
	        
	    if (uniformLocation < 0) {
	        throw new Exception("Could not find uniform:" + uniformName);
	            
	    }
	    uniforms.put(uniformName, uniformLocation);
	}
	
	public void setUniform(String uniformName, Matrix4f value) {
	    // Dump the matrix into a float buffer
	    try (MemoryStack stack = MemoryStack.stackPush()) {
	        FloatBuffer fb = stack.mallocFloat(16);
	        value.get(fb);
	        glUniformMatrix4fv(uniforms.get(uniformName), false, fb);
	    }
	}
	  public void setUniform(String uniformName, int value) {
	        glUniform1i(uniforms.get(uniformName), value);
	    }

	    public void setUniform(String uniformName, Vector3f value) {
	        glUniform3f(uniforms.get(uniformName), value.x,value.y,value.z);
	}
	    public void setUniform(String uniformName, float value) {
	        glUniform1f(uniforms.get(uniformName), value);
	    }
	    public void setUniform(String uniformName, Vector4f value) {
	        glUniform4f(uniforms.get(uniformName), value.x, value.y, value.z, value.w);
	    }
	    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void createPointLightUniform(String uniformName) throws Exception {
		createUniform(uniformName + ".pos");
		createUniform(uniformName + ".colour");
		createUniform(uniformName + ".intensity");
	}
	public void setUniform(String uniformName, PointLight pointLight) {
		setUniform(uniformName + ".pos", pointLight.getPos());
		setUniform(uniformName + ".colour", pointLight.getColour() );
		setUniform(uniformName + ".intensity", pointLight.getIntensity());
	}
	public void createPointLightListUniform(String uniformName, int size) throws Exception {
		for (int i = 0; i < size; i++) {
			createPointLightUniform(uniformName + "[" + i + "]");
		}
	}
	public void setUniform(String uniformName, PointLight[] pointLights) {
		int numLights = pointLights != null ? pointLights.length : 0;
		for (int i = 0; i < numLights; i++) {
			setUniform(uniformName, pointLights[i], i);
		}

	}
	public void setUniform(String uniformName, PointLight pointLight, int pos) {
		setUniform(uniformName + "[" + pos + "]", pointLight);
	}
	//////////////////////////////////////////
	public void createDirLightUniform(String uniformName) throws Exception {
		createUniform(uniformName + ".pos");
		createUniform(uniformName + ".colour");
		createUniform(uniformName + ".intensity");
	}
	public void setUniform(String uniformName, DirectionalLight directionalLight) {
		setUniform(uniformName + ".pos", directionalLight.getPos());
		setUniform(uniformName + ".colour", directionalLight.getColour() );
		setUniform(uniformName + ".intensity", directionalLight.getIntensity());
	}
	public void createDirLightListUniform(String uniformName, int size) throws Exception {
		for (int i = 0; i < size; i++) {
			createDirLightUniform(uniformName + "[" + i + "]");
		}
	}
	public void setUniform(String uniformName, DirectionalLight[] directionalLights) {
		int numLights = directionalLights != null ? directionalLights.length : 0;
		for (int i = 0; i < numLights; i++) {
			setUniform(uniformName, directionalLights[i], i);
		}

	}
	public void setUniform(String uniformName, DirectionalLight directionalLight, int pos) {
		setUniform(uniformName + "[" + pos + "]", directionalLight);
	}
/////////////////////////////////////////////////
public void createSpotLightUniform(String uniformName) throws Exception {
	createPointLightUniform(uniformName + ".pl");
	createUniform(uniformName + ".angle");
	createUniform(uniformName + ".direction");
}
	public void setUniform(String uniformName, SpotLight spotLight) {
		setUniform(uniformName + ".pl", spotLight.getPl());
		setUniform(uniformName + ".angle", spotLight.getAngle() );
		setUniform(uniformName + ".direction", spotLight.getDirection());
	}
	public void createSpotLightListUniform(String uniformName, int size) throws Exception {
		for (int i = 0; i < size; i++) {
			createSpotLightUniform(uniformName + "[" + i + "]");
		}
	}
	public void setUniform(String uniformName, SpotLight[] spotLights) {
		int numLights = spotLights != null ? spotLights.length : 0;
		for (int i = 0; i < numLights; i++) {
			setUniform(uniformName, spotLights[i], i);
		}

	}
	public void setUniform(String uniformName, SpotLight spotLight, int pos) {
		setUniform(uniformName + "[" + pos + "]", spotLight);
	}
//////////////////////////////////////
public void createMaterialUniform(String uniformName) throws Exception {
	createUniform(uniformName + ".ambient");
	createUniform(uniformName + ".diffuse");
	createUniform(uniformName + ".specular");
	createUniform(uniformName + ".reflectance");
}
	public void setUniform(String uniformName, Material material) {
		setUniform(uniformName + ".ambient", material.getAmbientC());
		setUniform(uniformName + ".diffuse", material.getDiffuseC());
		setUniform(uniformName + ".specular", material.getSpecularC());
		setUniform(uniformName + ".reflectance", material.getReflectance());
	}
	public void cleanup() {
        unbind();
        if (programId != 0) {
            glDeleteProgram(programId);
        }
}
	
	
}
