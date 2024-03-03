package graphic;

import org.joml.Vector3f;

public class PointLight {
    private Vector3f pos = new Vector3f(0,0,0);
    private Vector3f colour = new Vector3f(1,1,1);
    private float intensity = 1;

    public PointLight(Vector3f pos, Vector3f colour, float intensity) {
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

    public void setColour(Vector3f colour) {
        this.colour = colour;
    }

    public void setIntensity(float intensity) {
        this.intensity = intensity;
    }
}
