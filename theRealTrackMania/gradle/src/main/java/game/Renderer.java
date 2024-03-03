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
	 ShaderProgram shaderProgram;
	ShaderProgram hudShaderProgram;

	public Renderer() {
		 transformation = new Transformation();
	}
	 int vaoId,vboId;
	 
	    private static final float FOV = (float) Math.toRadians(60.0f);
	    private static final float Z_NEAR = 0.001f;
	    		
	  private static final float Z_FAR = 10000.f;
	   private Matrix4f projectionMatrix;
	   	private int MAX_PL =5;
		private int MAX_DL =5;
		private int MAX_SL =5;

	private void setupHudShader() throws Exception {
		hudShaderProgram = new ShaderProgram();
		hudShaderProgram.createVertexShader(Utils.getShader("/hud_shader.vs"));
		hudShaderProgram.createFragmentShader(Utils.getShader("/hud_shader.fs"));
		hudShaderProgram.link();

		// Create uniforms for Ortographic-model projection matrix and base colour

		hudShaderProgram.createUniform("projModelMatrix");
		hudShaderProgram.createUniform("colour");
	}

	public void init(Window win)throws Exception{
		
		
		
			shaderProgram = new ShaderProgram();
			shaderProgram.createVertexShader(Utils.getShader("/shader.vs"));
		shaderProgram.createGeometryShader(Utils.getShader("/shader.gs"));
		shaderProgram.createFragmentShader(Utils.getShader("/shader.fs"));
		shaderProgram.link();
		shaderProgram.createUniform("projectionMatrix");
	    //shaderProgram.createUniform("modelViewMatrix");
		shaderProgram.createUniform("modelMatrix");
		shaderProgram.createUniform("viewMatrix");
		shaderProgram.createUniform("texture_sampler");
		shaderProgram.createUniform("camera_Pos");
		shaderProgram.createPointLightListUniform("pointLights",MAX_PL);
		shaderProgram.createDirLightListUniform("directionalLights",MAX_DL);
		shaderProgram.createSpotLightListUniform("spotLights",MAX_SL);
		shaderProgram.createMaterialUniform("material");

		setupHudShader();

	}
	public void clear() { 
		
       glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    


}

	
	public void render(Window win, Camera camera , GameItem[] gameItems, PointLight[] pointLights, DirectionalLight[] directionalLights, SpotLight[] spotLights , Hud hud) throws Exception{

			 clear();

			if(win.isResized()){
				glViewport(0, 0, win.getWidth(), win.getHeight());
				win.setResized(false);
			}
			 shaderProgram.bind();
		shaderProgram.setUniform("camera_Pos",camera.getPosition());

			  projectionMatrix = transformation.getProjectionMatrix(FOV, win.getWidth(), win.getHeight(), Z_NEAR, Z_FAR);
			 shaderProgram.setUniform("projectionMatrix", projectionMatrix);

			 Matrix4f viewMatrix = transformation.getViewMatrix(camera);
		shaderProgram.setUniform("viewMatrix",viewMatrix);

				renderLights(pointLights,directionalLights,spotLights);
			 shaderProgram.setUniform("texture_sampler",0);
			 for(GameItem gameItem : gameItems) {
			 	if( gameItem != null) {
					Mesh mesh = gameItem.getMesh();
					Matrix4f modelMatrix = transformation.getModelMatrix(gameItem);
					shaderProgram.setUniform("modelMatrix", modelMatrix);
					//Matrix4f modelViewMatrix = transformation.getModelViewMatrix(modelMatrix, viewMatrix);
					//shaderProgram.setUniform("modelViewMatrix", modelViewMatrix);

					shaderProgram.setUniform("material", mesh.getMaterial());
					mesh.render();
				}
			 }

			 shaderProgram.unbind();



		hudShaderProgram.bind();

		Matrix4f ortho = transformation.getOrthoProjectionMatrix(0, win.getWidth(), win.getHeight(), 0);

		for (GameItem gameItem : hud.getGameItems()) {
			if(gameItem != null){
			Mesh mesh = gameItem.getMesh();
			// Set ortohtaphic and model matrix for this HUD item
			Matrix4f projModelMatrix = transformation.getOrtoProjModelMatrix(gameItem, ortho);

			hudShaderProgram.setUniform("projModelMatrix", projModelMatrix);
			hudShaderProgram.setUniform("colour",new Vector4f(1,1,1,1));

			// Render the mesh for this HUD item
			mesh.render();
		}}

		hudShaderProgram.unbind();
		
	}
			public void renderLights(PointLight[] pointLights,DirectionalLight[] directionalLights,SpotLight[] spotLights){
			/*for(int i=0;i<pointLights.length;i++) {
				shaderProgram.setUniform("pointLights", pointLights[i], i);
			}*/

				shaderProgram.setUniform("pointLights",pointLights);
				shaderProgram.setUniform("directionalLights",directionalLights);
				shaderProgram.setUniform("spotLights",spotLights);
			}
	 public void cleanup() {
	        if (shaderProgram != null) {
	            shaderProgram.cleanup();
	}

	
	
	
	
	 }	
}
