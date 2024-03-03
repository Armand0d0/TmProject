package graphic;

import org.joml.Vector3f;

public class DirectionalLight {
    private Vector3f pos;
    private Vector3f colour;
    private float intensity;

    public DirectionalLight(Vector3f pos, Vector3f colour, float intensity) {
        this.pos = pos;
        this.colour = colour;
        this.intensity = intensity;
    }

    public Vector3f getPos() {
        return pos;
    }

    public Vector3f getColour() {
        return colour;
    }

    public float getIntensity() {
        return intensity;
    }

    public void setPos(Vector3f pos) {
        this.pos = pos;
    }
    public void setPos(float x,float y,float z) {
        this.pos.x = x;
        this.pos.y = y;
        this.pos.z = z;

    }

    public void setColour(Vector3f colour) {
        this.colour = colour;
    }

    public void setIntensity(float intensity) {
        this.intensity = intensity;
    }
}
