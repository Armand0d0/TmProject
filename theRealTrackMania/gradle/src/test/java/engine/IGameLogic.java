package engine;

public interface IGameLogic {
	
    void init(Window win) throws Exception;
    
    void input(Window window, MouseInput mouseInput);

    void update(float interval, Window window, MouseInput mouseInput);
    
    void render(Window window);
    
    void cleanup();

}
