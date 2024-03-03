package engine;

import org.lwjgl.glfw.GLFWErrorCallback;
import org.lwjgl.glfw.GLFWVidMode;
import org.lwjgl.opengl.GL;

import static org.lwjgl.glfw.GLFW.*;
import static org.lwjgl.opengl.GL11.*;
import static org.lwjgl.system.MemoryUtil.NULL;

public class Window {
	private long win = 0;
	private int w , h;
	private boolean fullScreen;
	private String title;
	private static Input i;
	private boolean vSync;

	private boolean isResized;
	
	public Window(String windowTitle, int h,int w,boolean fullScren,boolean vSync) {
		setSize(h,w);
		setFullScreen(fullScren);
		this.w=w  ;
		this.h=h  ;
		this.vSync = vSync;
		  title = windowTitle;
		
	}
	public Window() {}
	public void init(){
		
		 GLFWErrorCallback.createPrint(System.err).set();
		if(!glfwInit()) {
			throw new IllegalStateException("GLFW not Initialized");
		}
			
		  glfwDefaultWindowHints(); // optional, the current window hints are already the default
	        glfwWindowHint(GLFW_VISIBLE, GL_FALSE); // the window will stay hidden after creation
	        glfwWindowHint(GLFW_RESIZABLE, GL_TRUE); // the window will be resizable
	        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
	        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
		win = glfwCreateWindow(w,h,title,fullScreen ? glfwGetPrimaryMonitor() :NULL,NULL);
		if(win == 0){
			throw new IllegalStateException("rip win");
		}
	

		
		glfwSetFramebufferSizeCallback(win, (window, width, height) -> {
		    this.w = width;
		   this.h = height;
		   
		    Window.this.setResized(true);
		});
		
		GLFWVidMode vidmode = glfwGetVideoMode(glfwGetPrimaryMonitor());
		 glfwSetWindowPos(win,(vidmode.width() - w) / 2,(vidmode.height() - h) / 2 +20);
		setSize(h,w);
		
		glfwMakeContextCurrent(win);
		if(isvSync()) {
			glfwSwapInterval(1);
		}
		
		glfwShowWindow(win);
		GL.createCapabilities();
		glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		depthTest(true);
		
	
	i = new Input(win);

		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	
	}
	public void depthTest(boolean b) {
		if(b) {
			glEnable(GL_DEPTH_TEST);

		}else {
			glDisable(GL_DEPTH_TEST);

		}
	}
	
	public void PolygonMode() {
		glPolygonMode( GL_FRONT_AND_BACK, GL_LINE );
		}
	public void UnPolygonMode() {
		glPolygonMode( GL_FRONT_AND_BACK, GL_FILL );
		}
	public boolean WinShouldClose(){
		return glfwWindowShouldClose(win) !=false;
		
	}

	public void setSize(int h,int w){
		this.h = h;
		this.w = w;
	}
	public void setFullScreen(boolean fullScreen){
		this.fullScreen = fullScreen;
	}
	
	public void update(){
	glfwSwapBuffers(win);
		
		glfwPollEvents();

	}
	public static void input() {
		i.update();
	
	}
	public int getWidth(){
		return w;
	}
	public int getHeight(){
		return h;
	}
	public boolean isFullSceen(){
		return fullScreen;
	}
	public    long getWin(){
		return win;
	}
	public Input getInput(){

		return i;
	}
	public void setResized(boolean b) {
		isResized = b;
	}
	public boolean isResized() {
		
		return isResized;
	}
	public void setClearColor(float color, float color2, float color3, float f) {
		setClearColor(color, color2, color3, f);
	}
	public boolean isvSync() {
        return vSync;
        
}
	public void setvSync(boolean vSync) {
        this.vSync = vSync;
}
	
	
}
