package graphic;

import org.joml.Vector3f;


public class SpotLight {

	
		PointLight pl;
	    private Vector3f direction;
	    private float angle;
	    
	    

	    public SpotLight(PointLight pl ,Vector3f direction,float angle) {
	        this.pl = pl;
	        this.direction = direction;
	        this.angle = angle;
	    }

	    public SpotLight(SpotLight spotLight) {
	        this(spotLight.getPl(),spotLight.getDirection(),spotLight.getAngle());
	    }

	    public PointLight getPl() {
			return pl;
		}

		public void setPl(PointLight pl) {
			this.pl = pl;
		}


	    public Vector3f getDirection() {
			return direction;
		}

		public void setDirection(Vector3f direction) {
			this.direction = direction;
		}

		public float getAngle() {
			return angle;
		}

		public void setAngle(float angle) {
			this.angle = angle;
		}

		
	    

}
