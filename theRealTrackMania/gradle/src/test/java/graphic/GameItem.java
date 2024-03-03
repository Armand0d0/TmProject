package graphic;

import org.joml.Vector3f;

public class GameItem {
	private final Mesh mesh;
	private final Vector3f position;
    private float scale;
    private final Vector3f rotation;
	
    public GameItem(Mesh mesh) {
    	
    	this.mesh = mesh;
    	position = new Vector3f(0, 0, 0);
    	scale = 1;
    	rotation = new Vector3f(0, 0, 0);
    	
    	
    }
	
    public Vector3f getPosition() {
        return position;
    }
    public void setPosition(float x, float y, float z) {
        this.position.x = x;
        this.position.y = y;
        this.position.z = z;
    }
    public void movePosition(float offsetX, float offsetY, float offsetZ) {
        this.position.x += offsetX;
        this.position.y += offsetY;
        this.position.z += offsetZ;
    }
    public float getScale() {
        return scale;
    }
    public void setScale(float scale) {
        this.scale = scale;
    }
    public Vector3f getRotation() {
        return rotation;
    }
    public void setRotation(float x, float y, float z) {
        this.rotation.x = x;
        this.rotation.y = y;
        this.rotation.z = z;
    }
    public void moveRotation(float x, float y, float z) {
        this.rotation.x += x;
        this.rotation.y += y;
        this.rotation.z += z;
    }
    public Mesh getMesh() {
        return mesh;
    }

}
