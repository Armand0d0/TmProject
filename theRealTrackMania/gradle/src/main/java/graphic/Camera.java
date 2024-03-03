package graphic;

import org.joml.Vector3f;
import org.lwjgl.system.CallbackI;

public class Camera {
	
    private final Vector3f position;

    private final Vector3f rotation;

    private final Vector3f prePos;

    private final Vector3f preRot;

    public Camera() {
        position = new Vector3f(0,0,0);
        rotation = new Vector3f(0,0,0);
        this.prePos = new Vector3f(0,0,0);
        this.preRot = new Vector3f(0,0,0);
    }

	public Camera(Vector3f position, Vector3f rotation) {
    this.position = position;
    this.rotation = rotation;
    this.prePos = new Vector3f(0,0,0);
    this.preRot = new Vector3f(0,0,0);
	}
	
    public void setPosition(float x, float y, float z) {
        position.x = x;
        position.y = y; 
        position.z = z;
    }
    public void setPosition(Vector3f pos) {
        position.x = pos.x;
        position.y = pos.y;
        position.z = pos.z;
    }
    public void movePosition(float offsetX, float offsetY, float offsetZ) {
        if ( offsetZ != 0 ) {
            position.x += (float)Math.sin(Math.toRadians(rotation.y)) * -1.0f * offsetZ;
            
            position.z += (float)Math.cos(Math.toRadians(rotation.y)) * offsetZ;
        }
        if ( offsetX != 0) {
            position.x += (float)Math.sin(Math.toRadians(rotation.y - 90)) * -1.0f * offsetX;

            position.z += (float)Math.cos(Math.toRadians(rotation.y - 90)) * offsetX;

        }
        position.y += offsetY;
//        System.out.print("x" + position.x);
//        System.out.print("	y" + position.y);
//        System.out.print("	z" + position.z);
//        System.out.print("									Rx " + rotation.x);
//        System.out.print("	Ry " + rotation.y);
//        System.out.println("	Rz " + rotation.z);


    }
    
    public void setRotation(float x, float y, float z) {
        rotation.x = x;
        rotation.y = y;
        rotation.z = z;
    }
    public void moveRotation(float offsetX, float offsetY, float offsetZ) {
        rotation.x += offsetX;
        rotation.y += offsetY;
        rotation.z += offsetZ;
    }
    public Vector3f getPosition() {
    return position;
	}
    public Vector3f getRotation() {
        return rotation;
    }

    public Vector3f getPrePos() {
        return this.prePos;
    }
    public Vector3f getPreRot() {
        return this.preRot;
    }
    public void setPrePos(float x,float y,float z){
        this.prePos.x = x;
        this.prePos.y = y;
        this.prePos.z = z;

    }
    public void setPrePos(Vector3f v){
        setPrePos(v.x,v.y,v.z);
    }
    public void setPreRot(float x,float y,float z){
        this.preRot.x = x;
        this.preRot.y = y;
        this.preRot.z = z;

    }
    public void setPreRot(Vector3f v){
        setPreRot(v.x,v.y,v.z);
    }
    public void movePreRotation(Vector3f v){
        this.preRot.x += v.x;
        this.preRot.y += v.y;
        this.preRot.z += v.z;
    }
    public void moveRotationAround(Vector3f o,Vector3f rot){
        movePreRotation(rot);
        Vector3f pos = new Vector3f(this.position.x-o.x ,this.position.y-o.y,this.position.z-o.z);
        setPrePos(pos);
    }
    public void setRotationAround(Vector3f o,Vector3f rot){
        setPreRot(rot);
        Vector3f pos = new Vector3f(-this.position.x-o.x ,this.position.y-o.y,this.position.z-o.z);
        setPrePos(pos);
    }
}
