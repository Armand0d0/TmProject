package game;


import engine.Utils;
import engine.Window;
import graphic.*;
import org.joml.Matrix4f;
import org.joml.Vector3f;
import org.joml.Vector4f;

import static org.lwjgl.opengl.GL11.*;


public class Renderer
{
	
	Transformation transformation;
	 private float specularPower;
	 ShaderProgram shaderProgram;
	public Renderer() {
		 transformation = new Transformation();
		 specularPower = 5f;
	}
	 int vaoId,vboId;
	 
	    private static final float FOV = (float) Math.toRadians(60.0f);
	    private static final float Z_NEAR = 0.001f;
	    		
	  private static final float Z_FAR = 10000.f;
	   private Matrix4f projectionMatrix;
	   
	public void init(Window win)throws Exception{
		
		
		
			shaderProgram = new ShaderProgram();
			shaderProgram.createVertexShader(Utils.getShader("/shader.vs"));
		shaderProgram.createGeometryShader(Utils.getShader("/shader.gs"));
		shaderProgram.createFragmentShader(Utils.getShader("/shader.fs"));
		shaderProgram.link();
		shaderProgram.createUniform("projectionMatrix");	   
	    shaderProgram.createUniform("modelViewMatrix");
	    shaderProgram.createUniform("texture_sampler");
		shaderProgram.createUniform("camera_pos");

        // Create uniform for material
        shaderProgram.createMaterialUniform("material");
        // Create lighting related uniforms
       shaderProgram.createUniform("specularPower");
        shaderProgram.createUniform("ambientLight");
        shaderProgram.createPointLightListUniform("pointLights",5);
        shaderProgram.createSpotLightListUniform("spotLights",5);
        shaderProgram.createDirListUniform("directionalLights",5);


	}
	public void clear() { 
		
       glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    


}

	
	public void render(Window win,Camera camera ,GameItem[] gameItems,
			Vector3f ambientLight,PointLight[] pointLights,DirectionalLight[] directionalLights,SpotLight[] spotLights) throws Exception{
		
		 clear();
		 
		 
		if(win.isResized()){
			glViewport(0, 0, win.getWidth(), win.getHeight());
			win.setResized(false);
		}
		 shaderProgram.bind();
		//shaderProgram.setUniform("camera_pos",camera.getPosition());
		shaderProgram.setUniform("specularPower",specularPower);
		  projectionMatrix = transformation.getProjectionMatrix(FOV, win.getWidth(), win.getHeight(), Z_NEAR, Z_FAR);
		 shaderProgram.setUniform("projectionMatrix", projectionMatrix);

		 Matrix4f viewMatrix = transformation.getViewMatrix(camera);

		renderLights(viewMatrix,ambientLight,pointLights,spotLights,directionalLights);

		 shaderProgram.setUniform("texture_sampler",0);
		 for(GameItem gameItem : gameItems) {
			 Mesh mesh = gameItem.getMesh();
		        Matrix4f modelViewMatrix =transformation.getModelViewMatrix(gameItem,viewMatrix);
		        shaderProgram.setUniform("modelViewMatrix", modelViewMatrix);
		        shaderProgram.setUniform("material", mesh.getMaterial());
		            mesh.render();
		            
		 }
		 
		 shaderProgram.unbind();
		
		
	}
	public void renderLights(Matrix4f viewMatrix, Vector3f ambientLight,
			PointLight[] pointLights, SpotLight[] spotLights, DirectionalLight[] directionalLights) 
	{
		
		 shaderProgram.setUniform("ambientLight", ambientLight);
//	        shaderProgram.setUniform("specularPower", specularPower);
	        // Get a copy of the light object and transform its position to view coordinates
	        for(int i=0;i<pointLights.length;i++) {
	        	PointLight currPointLight = new PointLight(pointLights[i]);
		        Vector3f lightPos = currPointLight.getPosition();
		        Vector4f aux = new Vector4f(lightPos, 1);
		        aux.mul(viewMatrix);
		        lightPos.x = aux.x;
		        lightPos.y = aux.y;
		        lightPos.z = aux.z;
		        shaderProgram.setUniform("pointLights", pointLights[i],i);
	        }
	        shaderProgram.setUniform("pointLights", pointLights);
	        
	        for(int i=0;i<directionalLights.length;i++) {
	        	 DirectionalLight currDirLight = new DirectionalLight(directionalLights[i]);
	        Vector4f dir = new Vector4f(currDirLight.getDirection(), 0);
	        dir.mul(viewMatrix);
	        currDirLight.setDirection(new Vector3f(dir.x, dir.y, dir.z));
	        shaderProgram.setUniform("directionalLights", directionalLights[i],i);
	        }
	        shaderProgram.setUniform("directionalLights", directionalLights);

	        for(int i=0;i<spotLights.length;i++) {
	        	 SpotLight currSpotLight = new SpotLight(spotLights[i]);
	 	        Vector3f SpotLightPos = currSpotLight.getPl().getColor();
	 	        Vector4f Spotaux = new Vector4f(SpotLightPos, 1);
	 	        Spotaux.mul(viewMatrix);
	 	        SpotLightPos.x = Spotaux.x;
	 	        SpotLightPos.y = Spotaux.y;
	 	        SpotLightPos.z = Spotaux.z;
	 	       shaderProgram.setUniform("spotLights", spotLights[i],i);
	        }
	        shaderProgram.setUniform("spotLights", spotLights);

		
	}
	 public void cleanup() {
	        if (shaderProgram != null) {
	            shaderProgram.cleanup();
	}

	
	
	
	
	 }	
}
