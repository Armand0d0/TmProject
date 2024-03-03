package graphic;

import org.joml.Vector3f;
import org.joml.Vector4f;

public class Material {
    private Texture texture;
    private Vector3f ambientC;
    private Vector3f diffuseC;
    private Vector3f specularC;
    private float reflectance;
    Vector3f defaultColor = new Vector3f(1,1,1);
    public Material(Texture texture,float reflectance){
        this.texture = texture;
        this.ambientC = defaultColor;
        this.diffuseC = defaultColor;
        this.specularC = defaultColor;
        this.reflectance = reflectance;
    }


    public Material(Texture texture, Vector3f ambientC, Vector3f diffuseC, Vector3f specularC,float reflectance) {
        this.texture = texture;
        this.ambientC = ambientC;
        this.diffuseC = diffuseC;
        this.specularC = specularC;
        this.reflectance = reflectance;
    }

    public float getReflectance() {
        return reflectance;
    }

    public void setReflectance(float reflectance) {
        this.reflectance = reflectance;
    }

    public Texture getTexture() {
        return texture;
    }

    public Vector3f getAmbientC() {
        return ambientC;
    }

    public Vector3f getDiffuseC() {
        return diffuseC;
    }

    public Vector3f getSpecularC() {
        return specularC;
    }

    public void setTexture(Texture texture) {
        this.texture = texture;
    }

    public void setAmbientC(Vector3f ambientC) {
        this.ambientC = ambientC;
    }

    public void setDiffuseC(Vector3f diffuseC) {
        this.diffuseC = diffuseC;
    }

    public void setSpecularC(Vector3f specularC) {
        this.specularC = specularC;
    }
    public void setColour(Vector4f C) {
        this.specularC = new Vector3f(C.x,C.y,C.z);
        this.ambientC =  new Vector3f(C.x,C.y,C.z);
        this.diffuseC =  new Vector3f(C.x,C.y,C.z);


    }

}
