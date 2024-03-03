package engine;


public class GameEngine implements Runnable {

	public static final int TARGET_FPS = 75;

	public static final int TARGET_UPS = 75;

	public static final int MIN_UPS = TARGET_UPS - 3;


	public void start() {
	    String osName = System.getProperty("os.name");
	    if ( osName.contains("Mac") ) {
	    	System.out.println("Mac");
			System.setProperty("java.awt.headless", "true");
	        gameLoopThread.run();
	       
	    } else {
	        gameLoopThread.run();
	        
	    }
	}
	
	@Override
	public void run() {
		try{
		init();
		loop();
		}catch(Exception e){
			e.printStackTrace();
		}finally {
			cleanup();
		}
	}


	private final Thread gameLoopThread;
	private IGameLogic gameLogic;
	private  Window window = new Window();
	private final Timer timer;
	private MouseInput mouseInput = new MouseInput();
	
	public GameEngine(String windowTitle, int width, int height,boolean fullScreen, boolean vSync, IGameLogic gameLogic) throws Exception {
		gameLoopThread = new Thread(this, "GAME_LOOP_THREAD");
		   window = new Window(windowTitle, width, height, fullScreen,vSync);
		   mouseInput = new MouseInput();
		 this.gameLogic = gameLogic;
		 timer = new Timer();
	}
	protected void init() throws Exception{
		window.init();
		timer.init();
		mouseInput.init(window);
		gameLogic.init(window);
	}
	protected void input() throws Exception{
		Window.input();
		mouseInput.input(window);
		gameLogic.input(window,mouseInput);
	}
	protected void update(float time,float fps) throws Exception{
		gameLogic.update(time,fps,window,mouseInput);
		
		}
	protected void render() throws Exception{
		gameLogic.render(window);
		window.update();
	}
	
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
private void loop() throws Exception {
	 float elapsedTime;
     float accumulator = 0f;
     float interval = 1f / TARGET_UPS;
	float fps =0;
	float prevT = timer.getInitTime();


	boolean running = true;
     while (running && !window.WinShouldClose()) {



         elapsedTime = timer.getElapsedTime();
accumulator += elapsedTime;
input();


	while (accumulator >= interval) {
    update((float)timer.getTime()-timer.getInitTime(),fps);
    accumulator -= interval;
}
		 float time = (float)timer.getTime();

	render();
		 fps = 1/(time - prevT) ;
		 prevT = time;
	if ( window.isvSync()) {
        sync();
    }

}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


}protected void cleanup() {
         gameLogic.cleanup();
 }
     private void sync() {
         float loopSlot = 1f / TARGET_FPS;
         double endTime = timer.getLastLoopTime() + loopSlot;
         while (timer.getTime() < endTime) {
           timer.Wait(1);
         }
 }




	
	
	
}
