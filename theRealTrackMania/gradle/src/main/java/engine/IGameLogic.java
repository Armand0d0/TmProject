package engine;

public interface IGameLogic {
	
    void init(Window win) throws Exception;
    
    void input(Window window, MouseInput mouseInput) throws Exception;


    void update(float time,float fps, Window window, MouseInput mouseInput) throws Exception;

    void render(Window window);
    
    void cleanup();

}
