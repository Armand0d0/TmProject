package game;


import engine.IGameLogic;
import engine.MouseInput;
import engine.Window;
import graphic.*;
import org.joml.Vector2f;
import org.joml.Vector3f;

import static org.lwjgl.glfw.GLFW.*;

//import static org.lwjgl.glfw.GLFW.GLFW_KEY_;


public class DummyGame implements IGameLogic {

	
	
	 
	private final Renderer renderer;
	//private final MouseInput mouseInput;
	 
	 private GameItem[] gameItems;
	 private float random = (float)Math.random()*10;
	 private final Camera camera;
	 private Vector3f cameraInc;
	 private float speed = 2f;
	 private float runningSpeed = 0.2f;
	 private float noRunningSpeed = 0.1f;

	 private float GIspeed = 0.04f;
	 private float lightAngle = 0;
	 private boolean islightMoving = false;
	 private static final float MOUSE_SENSITIVITY = 0.1f;
	 private Vector3f ambientLight;
	 private PointLight[] pointLights;
	 private DirectionalLight[] directionalLights;
	 private SpotLight[] spotLights;
	 
	 public DummyGame() throws Exception{
		 renderer = new Renderer();
		camera = new Camera();
		cameraInc = new Vector3f(0f,0f,0f);
		 lightAngle = 0;
				
				
		
	 }
	@Override
	public void init(Window win) throws Exception {
	    renderer.init(win);
//	    camera.setPosition(0, 5, 7);
//        camera.setRotation(30, 0, 0);
	    float reflectance = 10f;
//	    Mesh Tonus = OBJLoader.loadMesh("/Tonus.obj");
//	    Mesh Panda = OBJLoader.loadMesh("/MonPanda3D.obj");
		float[] pos = new float[]{0,0,0.01f,0,0,-0.01f,0,0.01f,0}; int[] indices = new int[]{1,2,3};float[] tex = new float[]{0,0,1,1,0,1};float[] norm = new float[]{0,0,0,0,0,0,0,0,0};
		Mesh Point = new Mesh(pos,indices,tex,norm);
	    Mesh Cube = OBJLoader.loadMesh("/cubeT.obj");
	    Mesh LightTests = OBJLoader.loadMesh("/LightTests.obj");
	   // Mesh Bugatti = OBJLoader.loadMesh("/bugatti.obj");
	   // Mesh Bunny = OBJLoader.loadMesh("/bunny.obj");
	    Mesh Sphere = OBJLoader.loadMesh("/sphere.obj");
	    Mesh Sphere2 = OBJLoader.loadMesh("/boule.obj");
	    Mesh Singe = OBJLoader.loadMesh("/singe.obj");
		Mesh Lapin = OBJLoader.loadMesh("/lapin.obj");


	   // Mesh mesh = X3DLoader.loadMesh("/cubeT.x3d");
        Texture CubeT = new Texture("damier.png");
        Texture PandaT = new Texture("/Panda.png");
        Texture BlackT = new Texture("/Black.png");
        Texture RainbowT = new Texture("/Rainbow.png");
        Texture TriangleT = new Texture("/Triangle.png");
        Texture CarreT = new Texture("/Carre.png");
        Texture White = new Texture("/white.png");
        Texture SingeT = new Texture("/singe.png");

        Material rainbow = new Material(RainbowT, reflectance);
        Material panda = new Material(PandaT, reflectance);
        Material cube = new Material(CubeT, 10);
        Material sphere = new Material(TriangleT, 0);
        Material sphere2 = new Material(White, 5);
        Material singe = new Material(SingeT, 30);
		Material lapin = new Material(White, 30);

        
//        Material cube = new Material(new Vector4f(0,0,1,0), reflectance);

//        Tonus.setMaterial(rainbow); 
//        Panda.setMaterial(panda);
		Lapin.setMaterial(lapin);
		Point.setMaterial(rainbow);
        Sphere.setMaterial(sphere);
        Sphere2.setMaterial(sphere2);
        Cube.setMaterial(cube);
        Singe.setMaterial(singe);
        LightTests.setMaterial(sphere2);
//      Bunny.setMaterial(rainbow);
//      Bugatti.setMaterial(cube);
		//GameItem LapinGI = new GameItem(Lapin);
        GameItem SphereGI = new GameItem(Sphere);
		GameItem PointGI = new GameItem(Point);
        GameItem SphereGI2 = new GameItem(Sphere2);
        GameItem SingeGI = new GameItem(Singe);
//      GameItem TonusGI= new GameItem(Tonus);
//      GameItem PandaGI = new GameItem(Panda);
        GameItem CubeGI= new GameItem(Cube);
        GameItem LightTestsGI = new GameItem(LightTests);
//      GameItem BugattiGI = new GameItem(Bugatti);
//      GameItem BunnyGI = new GameItem(Bunny);
		PointGI.setScale(10);
      	SphereGI.setPosition(0,0,0);
      	SphereGI2.setPosition(5,0,0);
//      PandaGI.setPosition(0,1.5f,-6);

        CubeGI.setScale(3);
		//CubeGI.setPosition(0,-10,0);
        SphereGI.setScale(2f);
		SphereGI.setPosition(-10,0,0);

		SphereGI2.setScale(2f);
		SphereGI2.setPosition(0,0,15);

		LightTestsGI.setScale(4);
        LightTestsGI.setPosition(10,0,0);



        	    gameItems = new GameItem[] {
						CubeGI,SphereGI,SphereGI2,LightTestsGI,PointGI
        	    };

//	    gameItems = new GameItem[1000];
//	    int x = 0,z = 0;
//	    for(int i=0;i<1000;i++) {
//	    	gameItems[i] = new GameItem(Cube);
//	    	gameItems[i].setPosition(x, 0, z);
//	    	x+=4;
//	    	if(x==400) {
//	    		x=0;
//	    		z+=4;
//	    	}
//	    }
	    
	  
	    ambientLight = new Vector3f(0.3f, 0.3f, 0.3f);
	   // ambientLight = new Vector3f(0f, 0f, 0f);
        Vector3f lightColour = new Vector3f(1f,0f, 0f);
        Vector3f lightPosition = new Vector3f(0,0,0);
        float lightIntensity =5.0f;
        PointLight pointLight = new PointLight(lightColour, lightPosition, lightIntensity);
        PointLight.Attenuation att = new PointLight.Attenuation(1f, 0f, 1f);
        pointLight.setAttenuation(att);
        
        Vector3f SpotLightColour = new Vector3f(0f,1f, 1f);
        Vector3f SpotLightPosition = new Vector3f(0,0, 0);
        float SpotLightIntensity = 10.0f;
        PointLight SpotPointLight = new PointLight(SpotLightColour,SpotLightPosition,SpotLightIntensity);
        PointLight.Attenuation Spotatt = new PointLight.Attenuation(1f, 0f, 1f);
        pointLight.setAttenuation(Spotatt);
        Vector3f SpotLightDirection = new Vector3f(0,1,0);
        float SpotLightAngle = 30;
        SpotLight spotLight = new SpotLight(SpotPointLight,SpotLightDirection,SpotLightAngle);
       
       
        
        
       
        lightPosition = new Vector3f(0, 1,0);
        lightColour = new Vector3f(0, 1, 0);
        DirectionalLight directionalLight = new DirectionalLight(lightColour, lightPosition, 1);
       
        
        pointLights = new PointLight[] {
       		//SpotPointLight,
        	pointLight,
        };
        directionalLights = new DirectionalLight[] {
      		//directionalLight
        };
        spotLights = new SpotLight[] {
        		//spotLight
        };
        camera.setPosition(0,0,0);
}   	

	@Override
	public void input(Window window, MouseInput mouseInput) {
		
	    cameraInc.set(0, 0, 0);
	    if (window.getInput().isKeyDown(GLFW_KEY_W)) {//z
	        cameraInc.z = -1;
	    }
	    if (window.getInput().isKeyDown(GLFW_KEY_S)) {//s
	        cameraInc.z = 1;
	    }
	    if (window.getInput().isKeyDown(GLFW_KEY_A)) {//q
	        cameraInc.x = -1;

	    }
	    if (window.getInput().isKeyDown(GLFW_KEY_D)||window.getInput().isKeyDown(GLFW_KEY_V)) {//d
	        cameraInc.x = 1;

	    }
	    if (window.getInput().isKeyDown(GLFW_KEY_LEFT_SHIFT)) {//w
	        cameraInc.y = -1;

	    }
	    if (window.getInput().isKeyDown(GLFW_KEY_SPACE)) {//x
	        cameraInc.y = 1;

	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_C)) {
	    	speed = runningSpeed;
	    }else {
	    	speed = noRunningSpeed;
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_0)) {
	    	camera.setPosition(0, 0, 0);
	    	camera.setRotation(0, 0, 0);

	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_X)) {
	    	window.PolygonMode();
	    }else {
	    	window.UnPolygonMode();
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_6)) {
	    	pointLights[0].movePosition(-GIspeed,0f,0f);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_4)) {
			pointLights[0].movePosition(GIspeed,0,0);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_8)) {
			pointLights[0].movePosition(0,GIspeed,0);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_2)) {
			pointLights[0].movePosition(0,-GIspeed,0);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_7)) {
			pointLights[0].movePosition(0,0,GIspeed);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_1)) {
			pointLights[0].movePosition(0,0,-GIspeed);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_3)) {
	    //	gameItems[0].moveRotation(0,0,1);
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_N)) {
	    	islightMoving = true;
	    }else if(window.getInput().isKeyDown(GLFW_KEY_B)) {
	    	islightMoving = false;
	    }
	}

	@Override
	public void update(float interval,Window window, MouseInput mouseInput) {

		    camera.movePosition(cameraInc.x * speed, cameraInc.y * speed, cameraInc.z * speed);
		    if(mouseInput.isLeftButtonPressed()) {
		    	Vector2f rotVec = mouseInput.getDisplVec();
		        camera.moveRotation(rotVec.x * MOUSE_SENSITIVITY , rotVec.y * MOUSE_SENSITIVITY, 0); 
		    }
			gameItems[4].setPosition(pointLights[0].getPosition().x,pointLights[0].getPosition().y,pointLights[0].getPosition().z);
			//pointLights[0].setPosition(new Vector3f(camera.getPosition().x,camera.getPosition().y,camera.getPosition().z));
		    
		    /*if(islightMoving) {
		    lightAngle += 0.5f;
		    }
		    if (lightAngle > 90) {
		        directionalLights[0].setIntensity(0);
		        if (lightAngle >= 360) {
		            lightAngle = -90;
		        }
		    } else if (lightAngle <= -80 || lightAngle >= 80) {
		        float factor = 1 - (float)(Math.abs(lightAngle) - 80)/ 10.0f;
		        directionalLights[0].setIntensity(factor);
		        directionalLights[0].getColor().y = Math.max(factor, 0.9f);
		        directionalLights[0].getColor().z = Math.max(factor, 0.5f);
		    } else {
		    	directionalLights[0].setIntensity(1);
		    	directionalLights[0].getColor().x = 1;
		    	directionalLights[0].getColor().y = 1;
		    	directionalLights[0].getColor().z = 1;
		    }
		    double angRad = Math.toRadians(lightAngle);
		    directionalLights[0].getDirection().x = (float) Math.sin(angRad);
		    directionalLights[0].getDirection().y = (float) Math.cos(angRad);*/

	}

	@Override
	public void render(Window window) {
		try {
			renderer.render(window,camera,gameItems,ambientLight,pointLights,directionalLights,spotLights);
		} catch (Exception e) {e.printStackTrace();}
			
	}
	@Override
	public void cleanup() {
		renderer.cleanup();
		 for
		  (GameItem  gameItem : gameItems) {
	           gameItem.getMesh().cleanUp();
	}
	}

}
