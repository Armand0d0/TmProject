package graphic;

import org.joml.Vector3f;

public class DirectionalLight {

	private Vector3f colour;
	private Vector3f direction;
	private float intensity;
	public DirectionalLight() {
		
	}
	public DirectionalLight(Vector3f colour ,Vector3f direction,float intensity) {
		this.colour = colour;
		this.direction = direction;
		this.intensity = intensity;
		
	}
	public DirectionalLight(DirectionalLight light) {
		this(new Vector3f(light.getColor()), new Vector3f(light.getDirection()), light.getIntensity());
		
	}
	public Vector3f getColor() {
		return colour;
	}
	public void setColor(Vector3f colour) {
		this.colour = colour;
	}
	public Vector3f getDirection() {
		return direction;
	}
	public void setDirection(Vector3f direction) {
		this.direction = direction;
	}

	public float getIntensity() {
		return intensity;
	}
	public void setIntensity(float intensity) {
		this.intensity = intensity;
	}

}
