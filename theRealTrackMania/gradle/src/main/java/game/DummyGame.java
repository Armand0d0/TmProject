package game;


import engine.*;
import graphic.PointLight;
import graphic.*;
import org.joml.*;
import org.lwjgl.system.CallbackI;

import java.lang.Math;
import java.time.Year;
import java.util.ArrayList;
import java.util.List;
import java.util.Vector;

import static org.lwjgl.glfw.GLFW.*;

//import static org.lwjgl.glfw.GLFW.GLFW_KEY_;


public class DummyGame implements IGameLogic {

	
	
	 
	private final Renderer renderer;
	//private final MouseInput mouseInput;
	 
	 private GameItem[] gameItems;
	 private float random = (float)Math.random()*10;
	 private final Camera camera;
	 private Vector3f cameraInc;
	 private float speed = 100f;
	 private float runningSpeed = 1f;
	 private float noRunningSpeed = 0.5f;
	 private PointLight[] pointLights;
	 private DirectionalLight[] directionalLights;
	 private SpotLight[] spotLights;
	private Hud hud ;
	private  Mesh mesh;
	private  Mesh mesh2;
	private float Xmouse;
	private float GIspeed = 1f;
	 private static final float MOUSE_SENSITIVITY = 0.1f;
	 private Vector3f ambientLight;
	private float MotoSpeed =0;
	private float MotoAcceleration =0.3f;
	private float freeWheeling =0.9994f;
	private float maxSpeed = 2f;
	private Vector3f MotoDir = new Vector3f(0,0,0);
	private float MSensivity = 1;
	private float maxAngle = 150;
	private float MotoBreak = 0.2f;
	public static final Vector3f v0 = new Vector3f(0,0,0);
	public static Vector3f red = new Vector3f(1,0,0);
    public static Vector3f green = new Vector3f(0,1,0);
    public static Vector3f blue = new Vector3f(0,0,1);
    public static Quaternionf q0 = new Quaternionf(0,0,0,0);
	private static float xdiv =5,zdiv=1000;
	private static float resetTime=0;
	private static boolean reset = false;
	private static int frames = -1;
	Quaternionf[] Rots ;
	Vector3f[] Pos ;
	int j=0,k=0;

	public DummyGame() throws Exception{
		 renderer = new Renderer();
		camera = new Camera();
		cameraInc = new Vector3f(0f,0f,0f);

				
		
	 }
	@Override
	public void init(Window win) throws Exception {
	    renderer.init(win);
//	    camera.setPosition(0, 5, 7);
//        camera.setRotation(30, 0, 0);
//	    Mesh Tonus = OBJLoader.loadMesh("/Tonus.obj");
//	    Mesh Panda = OBJLoader.loadMesh("/MonPanda3D.obj");
		/*float[] pos = new float[]{0,0,0.01f,0,0,-0.01f,0,0.01f,0}; int[] indices = new int[]{1,2,3};float[] tex = new float[]{0,0,1,1,0,1};float[] norm = new float[]{0,0,0,0,0,0,0,0,0};
		Mesh Point = new Mesh(pos,indices,tex,norm);
	    Mesh Cube = OBJLoader.loadMesh("/cubeT.obj");
	    Mesh LightTests = OBJLoader.loadMesh("/LightTests.obj");
	   // Mesh Bugatti = OBJLoader.loadMesh("/bugatti.obj");
	    Mesh Bunny = OBJLoader.loadMesh("/bunny.obj");

		Mesh Sphere2 = OBJLoader.loadMesh("/boule.obj");
	    Mesh Singe = OBJLoader.loadMesh("/singe.obj");
		Mesh Lapin = OBJLoader.loadMesh("/lapin.obj");
		Mesh Plan = OBJLoader.loadMesh("/plan.obj");


	   // Mesh mesh = X3DLoader.loadMesh("/cubeT.x3d");
        Texture CubeT = new Texture("/damier.png");
        Texture PandaT = new Texture("/Panda.png");
        Texture BlackT = new Texture("/Black.png");
        Texture RainbowT = new Texture("/Rainbow.png");
        Texture TriangleT = new Texture("/Triangle.png");
        Texture CarreT = new Texture("/Carre.png");

        Texture SingeT = new Texture("/singe.png");



        Material cube = new Material(CubeT,1);

		Material rainbow = new Material(RainbowT,1);
		Material triangle = new Material(TriangleT,1);


//        Tonus.setMaterial(rainbow); 
//        Panda.setMaterial(panda);
		//Lapin.setMaterial(white);
		Point.setMaterial(rainbow);
        Sphere.setMaterial(triangle);
        Sphere2.setMaterial(white);
        Cube.setMaterial(cube);
        //Singe.setMaterial(SingeT);
        LightTests.setMaterial(white);
      Bunny.setMaterial(rainbow);
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
      GameItem BunnyGI = new GameItem(Bunny);
		PointGI.setScale(1000);
//      PandaGI.setPosition(0,1.5f,-6);

        CubeGI.setScale(10);
		CubeGI.setPosition(0,-11,0);
        SphereGI.setScale(2f);
		SphereGI.setPosition(-10,0,0);

		SphereGI2.setScale(2f);
		SphereGI2.setPosition(10,-5,15);

		LightTestsGI.setScale(4);
        LightTestsGI.setPosition(10,5,0);

		Texture MotoT = new Texture("/Moto.png");
		Mesh moto = OBJLoader.loadMesh("/moto.obj");
		Material Moto = new Material(MotoT,4);
		moto.setMaterial(Moto);
				GameItem MotoGI = new GameItem(moto);
				MotoGI.setScale(0.1f);
				MotoGI.setRotation(0,0,0);
				Plan.setMaterial(new Material(CubeT , 3));
				GameItem PlanGI =  new GameItem(Plan);
				PlanGI.setScale(1000);

		mesh = Bunny;
		mesh2 = Cube;
		CubeGI.setPosition(0,100,0);


//*/

		Texture CubeT = new Texture("/damier.png");
		Material cube = new Material(CubeT,1);
		Mesh Plan = OBJLoader.loadMesh("/plan.obj");
		Plan.setMaterial(cube);
		GameItem PlanGI = new GameItem(Plan);
		PlanGI.setScale(1000);
		PlanGI.setPosition(0,-5,0);
		Texture White = new Texture("/white.png");
		Material white = new Material(White,1);
		Mesh Car = OBJLoader.loadMesh("/Crysler_new_yorker_1980.obj");
		Car.setMaterial(white);
		GameItem CarGI = new GameItem(Car);
		CarGI.setScale(0.01f);
		Mesh Car2 = OBJLoader.loadMesh("/Crysler_new_yorker_1980.obj");
		Car2.setMaterial(new Material(White, red,red,red,5));
		GameItem CarGI2 = new GameItem(Car2);
		CarGI2.setScale(0.01f);

		Material Red = new Material(White,red,red,red,5);
		Mesh Vector = OBJLoader.loadMesh("/Vector.obj");
		Vector.setMaterial(Red);
		GameItem VectorGI = new GameItem(Vector);
		VectorGI.setScale(5);

        Mesh Sphere = OBJLoader.loadMesh("/sphere.obj");
        Sphere.setMaterial(new Material(White,blue,blue,blue,5));
        GameItem SphereGI = new GameItem(Sphere);

        gameItems = new GameItem[] {
				CarGI	,PlanGI ,CarGI2 //, VectorGI,SphereGI
		};

		gameItems[0].setRotationType(1);
		gameItems[0].setQrot(new Quaternionf(0,1,0,1).normalize());
		gameItems[2].setRotationType(1);
		gameItems[2].setQrot(new Quaternionf(0,1,0,1).normalize());
		Rots = getQuaternionRot();
		Pos = getCarPos();
		//generate();

//	    gameItems = new GameItem[50];
//	    int x = 0,z = 0;
//	    for(int i=0;i<50;i++) {
//	    	gameItems[i] = new GameItem(Cube);
//	    	gameItems[i].setPosition(x, 0, z);
//	    	x+=4;
//	    	if(x==10) {
//	    		x=0;
//	    		z+=4;
//	    	}
//	    }
	    	PointLight pl = new PointLight(new Vector3f(-10,0,0),new Vector3f(1,0,1),10);
		pointLights = new PointLight[]{
				//pl,
		};
			DirectionalLight dl = new DirectionalLight(new Vector3f(1,1,0),new Vector3f(1,1,1),0.3f);
		directionalLights = new DirectionalLight[]{
		dl,
		};
		SpotLight sl = new SpotLight(pl,15,new Vector3f(0,-1,0));
		spotLights = new SpotLight[]{
				//sl
		};

		hud = new Hud(10);

		//hud.setImage(2,CubeT);
		//hud.getGameItems()[2].setPosition(win.getWidth()/2,win.getHeight()/2,0);

		camera.setPosition(5.068E+2f,  2.550E+1f,  2.927E+2f);
		camera.setRotation(0,  0, 0);

	}
	public Quaternionf[] getQuaternionRot() throws Exception {

		List<String> lines = new ArrayList<>();
		lines = Utils.readAllLines("/rotationACC.txt");
		int n = lines.size();
		frames = n;
		//Vector3f[] AnglesRot= new Vector3f[n];
		Quaternionf[] Qrots = new Quaternionf[n];
		for (int i=0;i<n;i++) {
			String line = lines.get(i);
			String[] tokens = line.split("\\s+");			///1 0 2
			float angle = (Float.parseFloat(tokens[0]))	;//	-(float)Math.PI;
			float axisHeading = (Float.parseFloat(tokens[1]))	;
			float axisPitch = (Float.parseFloat(tokens[2]))	;
			/*
			AnglesRot[i] = new Vector3f(
					deg(Float.parseFloat(tokens[1]))-90		,
					deg(Float.parseFloat(tokens[0]))-45	,
					deg(Float.parseFloat(tokens[2]))
			);//*/
			Quaternionf q = new Quaternionf(cosr(angle),
					sinr(angle)*cosr(axisPitch)*cosr(axisHeading),
					sinr(angle)*cosr(axisPitch)*sinr(axisHeading),
					sinr(angle)*sinr(axisPitch));
			//AnglesRot[i] = deg(ToEulerAngles(q));
			//Qrots[i] = new Quaternionf(q.x,q.y,q.z,q.w);
			Vector3f v = new Vector3f(q.x,q.y,q.z).rotateZ((float) Math.PI);
			Qrots[i] = new Quaternionf(q.w,v.x,v.y,v.z);
		}

		/*Matrix3f[] QuaternionRotMat = new Matrix3f[n];
		Vector3f[] RotFromMat = new Vector3f[n];
		for (int i=0;i<n;i++) {
			float a = QuaternionRot[i].x,
					b = QuaternionRot[i].y,
					c = QuaternionRot[i].z,
					d = QuaternionRot[i].w;
			QuaternionRotMat[i] = new Matrix3f(
					new Vector3f(a*a+b*b-c*c-d*d,  2*b*c+2*a*d, 2*b*d-2*a*c),
					new Vector3f(2*b*c-2*a*d,  a*a-b*b+c*c-d*d, 2*c*d+2*a*b),
					new Vector3f(2*b*d+2*a*c,  2*c*d-2*a*b, a*a-b*b-c*c+d*d)
			);
			float z = (QuaternionRotMat[i].m02);
			float y = (QuaternionRotMat[i].m01);
			float w = (QuaternionRotMat[i].m12);
			//System.out.println("beta: "+Beta +"  m20: "+ (2*b*d-2*a*c));
			float Beta = -asinr(-z);
			float Alpha = asinr(y/cosr(Beta));
			float Gama = asinr(w/cosr(Beta));
			RotFromMat[i] = new Vector3f((Gama), (Beta), (Alpha));
		}//*/

		return Qrots;
	}
	public Vector3f[] getCarPos() throws Exception {

		List<String> lines = new ArrayList<>();
		lines = Utils.readAllLines("/00.txt");
		int n = lines.size();
		Vector3f[] pos = new Vector3f[n];
		for (int i = 0; i < n; i++) {
			String line = lines.get(i);
			String[] tokens = line.split("\\s+");

			pos[i] = new Vector3f(
					(Float.parseFloat(tokens[1]))/2	,
					(Float.parseFloat(tokens[2]))/2	,
					(Float.parseFloat(tokens[3]))/2
			);//*/
//			System.out.println(Pos[i]);
		}
		return pos;
	}
public void generate() throws Exception {
	Mesh road = OBJLoader.loadMesh("/plan.obj"); road.setMaterial(new Material(new Texture("/Road.png"),0.2f));
	Mesh roadTurn = OBJLoader.loadMesh("/plan.obj"); roadTurn.setMaterial(new Material(new Texture("/Road_Turn.png"),0.2f));


	float scale = 25;
	float d = scale*2;


	GameItem[] RgameItems = new GameItem[100];

	Vector3f pos = new Vector3f(0,0,0);
	Vector3f dir = new Vector3f(1,0,0);
	boolean turn = false;
	boolean turn2 = false;




	float r = 90;
	float r2 = 00;
	float Zdir = 0;

	for(int i = 0;i<RgameItems.length;i++){


		if(turn2){
			dir.x = dir.x == 0 ? 1 : 0;
			if(dir.x == 0) dir.z = Zdir;
			else{dir.z = 0;}
			turn = true;
		}else{ turn = false; }

		r += turn ? 90 : 0;

		pos.x += dir.x*d;
		pos.z += dir.z*d;


		if(random(1.5)&& !turn){

			 Zdir = random12() ? 1 : -1;

			if(dir.z == 1){ r2 =180;}else if(dir.z == -1){r2 = 270;}
			else if(Zdir == -1){r2 = 90;}else{r2 = 0;}
			RgameItems[i] =  new GameItem(roadTurn).setPos(pos).setRot(0,r2,0).SetScale(scale);


			turn2 = true;
				}
		else{

			RgameItems[i] = new GameItem(road).setPos(pos).setRot(0,r,0).SetScale(scale);
			turn2 = false;
		}




	}

	 gameItems =  add(gameItems, RgameItems);
}
public GameItem[] add(GameItem[] arr1 , GameItem[] arr2){
		 GameItem[] result = new GameItem[arr1.length + arr2.length];
		for(int i=0;i<arr1.length;i++){
			result[i] = arr1[i];
		}
	for(int i=0;i<arr2.length;i++){
		result[i+arr1.length] = arr2[i];
	}
		return result;
}
public boolean random(double d){
boolean f;
	if(Math.random() <= 1d/d){
		f = false;
	}else {
		f = true;
	}
		return f;
}
public boolean random12(){
		return Math.random()<=0.5 ? true : false;
}
public void setTrajectory(float raceTime,float Time){

	//Vector3f[] Pos = getPos();
	boolean continu = true;
	if (continu) {
		float t = (Time / (raceTime / (frames-1)));
		k = (int) t;
		if (k < frames-1) {


			Quaternionf dif = sub(Rots[k + 1],Rots[k]);
			Vector3f Pdif = sub(Pos[k + 1],Pos[k]);
			float factor = (t - k);
			//System.out.println(" f= " + factor);
			//gameItems[0].setQrot(add(Rots[k],mul(dif,factor)).normalize());
			gameItems[0].setPosition(add(Pos[k],Pdif.mul(factor)));
		}else{
			System.out.println((gameItems[2].getPosition().x-Pos[0].x )/(gameItems[0].getPosition().x-Pos[0].x));
		}
	}else{
		if(j%30==0&& k<frames-1) {//
			gameItems[0].setQrot(Rots[k]);
			gameItems[0].setPosition(Pos[k]);
			k++;

		}
		//	System.out.println(gameItems[0].getQrot());
	}

}

public void setAPos(float Time,float fps){
		float a=  8.3796280690966382e-003f,b= -1.0108465617493734f,c=-1.3946130289053729f,d=9.8969449606549722e+002f;
		float p =a*Time*Time*Time +b*Time*Time+c *Time ;//+ d;
		float v = 3*a*Time*Time + 2*b*Time + c;
		v/= 100;
		Vector3f p0 = Pos[0];
	gameItems[2].setPosition(p0);
	gameItems[2].movePosition(p/2.4101908f,0,0);
//	System.out.println(gameItems[2].getPosition() + "   " + p);

}
	@Override
	public void input(Window window, MouseInput mouseInput) throws Exception {
		
	    cameraInc.set(0, 0, 0);
		 // && gameItems[0].getRotation().z > -90  && gameItems[0].getRotation().z  < 90
			Xmouse = (float)mouseInput.getCurrentPos().x / window.getWidth()  ;

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
	    	//gameItems[0].setPosition(0,0,0);
	    	MotoSpeed =0;
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_X)) {
	    	window.PolygonMode();
	    }else {
	    	window.UnPolygonMode();
	    }
	    if(window.getInput().isKeyDown(GLFW_KEY_KP_ADD)){
	    	xdiv++;
		}
		if(window.getInput().isKeyDown(GLFW_KEY_KP_SUBTRACT)){
			xdiv--;
		}
		if(window.getInput().isKeyDown(GLFW_KEY_R)){
			reset = true;
		}else{
			reset = false;
		}
	}


	@Override
	public void update(float time, float fps, Window window, MouseInput mouseInput) throws Exception {
		if(reset){
			resetTime= time;

		}
		float Time = time - resetTime;

		hud.setText(0, "FPS: " + fps);
		hud.setText(1, "Time: " + time);
		hud.setText(2, "Race time: " + Time);
		//hud.setText(2,"Xdiv = "+ xdiv + "  Zdiv="+zdiv);
		hud.getGameItems()[0].setPosition(0, 0, 0);
		hud.getGameItems()[1].setPosition(0, 30, 0);
		hud.getGameItems()[2].setPosition(0,60,0);
		//hud.getGameItems()[2].setPosition(0, 60, 0);



		camera.movePosition(cameraInc.x * speed, cameraInc.y * speed, cameraInc.z * speed);
			if (mouseInput.isLeftButtonPressed() || mouseInput.isRightButtonPressed()) {
				Vector2f rotVec = mouseInput.getDisplVec();
				camera.moveRotation(rotVec.y * MOUSE_SENSITIVITY, rotVec.x * MOUSE_SENSITIVITY, 0);
			}

	setTrajectory(14.25f,Time);
	setAPos(Time,fps);

		//System.out.println(camera.getPosition());

		//Vector3f CarPos = gameItems[0].getPosition();
		//camera.setPosition(new Vector3f( CarPos.x+15 ,CarPos.y+ 5,CarPos.z+ 5));

		j++;

	}
	Quaternionf PosToRot(Quaternionf q){
		Vector3f v = new Vector3f(q.y,q.z,q.w).normalize();
		Vector3f v1 = new Vector3f(0,0,1);
		Vector3f axis = add(v,v1).normalize();//.rotateX((float) Math.PI);
		Quaternionf qr = new Quaternionf((float)1,axis.x,axis.y,axis.z).normalize();
		return qr;
	}
	Quaternionf add(Quaternionf q, Quaternionf q2){
		return new Quaternionf(q.x+q2.x,q.y+q2.y,q.z+q2.z,q.w+q2.w);
	}
	Quaternionf sub(Quaternionf q, Quaternionf q2){
		return new Quaternionf(q.x-q2.x,q.y-q2.y,q.z-q2.z,q.w-q2.w);
	}
	Vector3f add(Vector3f v, Vector3f v2){
		return new Vector3f(v.x+v2.x,v.y+v2.y,v.z+v2.z);
	}
	Vector3f sub(Vector3f v, Vector3f v2){
		return new Vector3f(v.x-v2.x,v.y-v2.y,v.z-v2.z);
	}
	Quaternionf mul(Quaternionf q, float f){
		return new Quaternionf(q.x*f,q.y*f,q.z*f,q.w*f);
	}
	Vector3f ToEulerAngles(Quaternionf q) {										//q.w -> q.x
		Vector3f angles = new Vector3f(0,0,0);													//q.x -> q.y
																				//q.y -> q.z
		// roll (x-axis rotation)												//q.z -> q.w
		float sinr_cosp = 2 * (q.x * q.y + q.z * q.w);
		float cosr_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
		angles.x = (float)Math.atan2(sinr_cosp, cosr_cosp);

		// pitch (y-axis rotation)
		float sinp = 2 * (q.x * q.z - q.w * q.y);
		if (Math.abs(sinp) >= 1)
		angles.y = (float)Math.copySign(Math.PI / 2, sinp); // use 90 degrees if out of range
    else
		angles.y = (float)Math.asin(sinp);

		// yaw (z-axis rotation)
		float siny_cosp = 2 * (q.x * q.w + q.y * q.z);
		float cosy_cosp = 1 - 2 * (q.z * q.z + q.w * q.w);
		angles.z = (float)Math.atan2(siny_cosp, cosy_cosp);

		return new Vector3f(angles.x/0.8f,(angles.y)/1,angles.z/.2f);
	}

	public float cos(float f){
		return (float) Math.cos(rad(f));
		}
	public float cosr(float f){
		return (float) Math.cos((f));
	}
	public float acos(float f){
		return (float) Math.acos(rad(f));
	}
	public float sin(float f){
		return (float) Math.sin(rad(f));
	}
	public float sinr(float f){
		return (float) Math.sin((f));
	}
	public float asin(float f){
		return (float) Math.asin(rad(f));
	}
	public float asinr(float f){
		return (float) Math.asin((f));
	}
	public float rad(float f){
		return (float) Math.toRadians(f);
	}
	public float deg(float f){
		return (float) Math.toDegrees(f);
	}
	public Vector3f deg(Vector3f v){

		return new Vector3f((float) Math.toDegrees(v.x),(float) Math.toDegrees(v.y),(float) Math.toDegrees(v.z));
	}

	@Override
	public void render(Window window) {

		try {
			renderer.render(window,camera,gameItems,pointLights,directionalLights,spotLights , hud);
		} catch (Exception e) {e.printStackTrace();}

	}
	@Override
	public void cleanup() {
		renderer.cleanup();
		 for
		  (GameItem  gameItem : gameItems) {
		 	if(gameItem !=null)
	           gameItem.getMesh().cleanUp();
	}
	}

}
