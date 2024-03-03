package main;


import engine.GameEngine;
import engine.IGameLogic;
import engine.Window;
import game.DummyGame;

import static org.lwjgl.glfw.GLFW.*;


public class MainMoteur3D
{
	
	int h = 1060;
	int w = 1920;
	private boolean fulllScreen = false;
	private boolean vSync = false;
	
	public void run(){
		long win = new Window().getWin();
		try{
			 IGameLogic gameLogic = new DummyGame();
			 GameEngine gameEng = new GameEngine("Moteur-3D", h, w,fulllScreen, vSync, gameLogic);
			 gameEng.start();
			
		}catch(Exception e){
			e.printStackTrace();
			System.exit(-1);
		}
		
		
		
		glfwDestroyWindow(win);
		
		glfwTerminate();
		glfwSetErrorCallback(null).free();
		System.exit(-1);
	}
	
	
	
	
	public static void main(String[] args) 
	{
		new MainMoteur3D().run();
	}

}
