package graphic;

import org.joml.Vector3f;

public class SpotLight {
    private PointLight pl;
    private float angle;
    private Vector3f direction;

    public SpotLight(PointLight pl, float angle, Vector3f direction) {
        this.pl = pl;
        this.angle = angle;
        this.direction = direction;
    }

    public PointLight getPl() {
        return pl;
    }

    public float getAngle() {
        return angle;
    }

    public Vector3f getDirection() {
        return direction;
    }

    public void setPl(PointLight pl) {
        this.pl = pl;
    }

    public void setAngle(float angle) {
        this.angle = angle;
    }

    public void setDirection(Vector3f direction) {
        this.direction = direction;
    }
}
