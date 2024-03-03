package graphic;

import org.joml.Quaternionf;
import org.joml.Vector3f;
import org.joml.Vector4f;
import org.lwjgl.system.CallbackI;

public class GameItem {
	private Mesh mesh;
	private final Vector3f position;
    private float scale;
    private final Vector3f rotation;
    private int rotType;
    private Quaternionf Qrot;



    public GameItem() {
        //this.mesh = null;
        this.position =  new Vector3f(0, 0, 0);
        this.scale = 1;
        this.rotation =  new Vector3f(0, 0, 0);
        this.rotType = 0;
        this.Qrot = new Quaternionf(0,0,0,0);

    }

    public GameItem(Mesh mesh) {
    	
    	this.mesh = mesh;
    	position = new Vector3f(0, 0, 0);
    	scale = 1;
    	rotation = new Vector3f(0, 0, 0);
        this.rotType = 0;
        this.Qrot = new Quaternionf(0,0,0,0);
    }
    public void moveRelativePos(float Z){
            if ( Z != 0 ) {
                position.x += (float)Math.sin(Math.toRadians(rotation.y)) * -1.0f * Z;

                position.z += (float)Math.cos(Math.toRadians(rotation.y)) * Z;
            }
//            if ( offsetX != 0) {
//                position.x += (float)Math.sin(Math.toRadians(rotation.y - 90)) * -1.0f * offsetX;
//
//                position.z += (float)Math.cos(Math.toRadians(rotation.y - 90)) * offsetX;
//
//            }
            //position.y += offsetY;
    }

    public Vector3f getPosition() {
        return position;
    }
    public void setPosition(float x, float y, float z) {
        this.position.x = x;
        this.position.y = y;
        this.position.z = z;
    }
    public void setPosition(Vector3f v) {
        this.position.x = v.x;
        this.position.y = v.y;
        this.position.z = v.z;
    }
    public void movePosition(float offsetX, float offsetY, float offsetZ) {
        this.position.x += offsetX;
        this.position.y += offsetY;
        this.position.z += offsetZ;
    }
    public void movePosition(Vector3f v) {
        this.position.x += v.x;
        this.position.y += v.y;
        this.position.z += v.z;
    }
    public float getScale() {
        return scale;
    }
    public void setScale(float scale) {
        this.scale = scale;
    }
    public void moveScale(float offset) {
        this.scale += offset;
    }
    public Vector3f getRotation() {
        return rotation;
    }
    public void setRotation(float x, float y, float z) {
        this.rotation.x = x;
        this.rotation.y = y;
        this.rotation.z = z;
    }
    public void setRotation(Vector3f v) {
        this.rotation.x = v.x;
        this.rotation.y = v.y;
        this.rotation.z = v.z;
    }
    public void moveRotation(float x, float y, float z) {
        this.rotation.x += x;
        this.rotation.y += y;
        this.rotation.z += z;
    }
    public void moveRotation(Vector3f v) {
        this.rotation.x += v.x;
        this.rotation.y += v.y;
        this.rotation.z += v.z;
    }


    public void setRotationX(float x){
        this.rotation.x = x;
    }
    public void setRotationY(float y){
        this.rotation.y = y;
    }
    public void setRotationZ(float z){
        this.rotation.z = z;
    }

    public Mesh getMesh() {
        return mesh;
    }
    public void setMesh(Mesh mesh) {
        this.mesh = mesh;
    }
    public GameItem setPos(Vector3f v){
        setPosition(v);
        return this;
    }
    public GameItem setRot(float x,float y,float z){
        setRotation(x,y,z);
        return this;
    }
    public GameItem SetScale(float s){
        setScale(s);
        return this;
    }
    public void setRotationType(int rotationType){
        this.rotType = rotationType;
    }
    public int getRotationType(){
        return rotType;
    }
    public Quaternionf getQrot() {
        return Qrot;
    }

    public void setQrot(Quaternionf qrot) {
        this.Qrot.x = qrot.x;
        this.Qrot.y = qrot.y;
        this.Qrot.z = qrot.z;
        this.Qrot.w = qrot.w;
    }

}
