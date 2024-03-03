package graphic;

import org.joml.Matrix4f;
import org.joml.Quaternionf;
import org.joml.Vector3f;

public class Transformation {
	private final Matrix4f projectionMatrix;
	private final Matrix4f viewMatrix;
	private final Matrix4f modelViewMatrix;
	private final Matrix4f modelMatrix;
	private final Matrix4f orthoMatrix;


	public Transformation() {
        projectionMatrix = new Matrix4f();
        viewMatrix = new Matrix4f();
        modelViewMatrix = new Matrix4f();
		modelMatrix = new Matrix4f();
		orthoMatrix = new Matrix4f();
	}
	public final Matrix4f getOrthoProjectionMatrix(float left, float right, float bottom, float top) {
		orthoMatrix.identity();
		orthoMatrix.setOrtho2D(left, right, bottom, top);
		return orthoMatrix;
	}
	
	
	public final Matrix4f getProjectionMatrix(float fov, float width, float height, float zNear, float zFar) {
        float aspectRatio = width / height;        
        projectionMatrix.identity();
        projectionMatrix.perspective(fov, aspectRatio, zNear, zFar);
        return projectionMatrix;
    }
	public Matrix4f getViewMatrix(Camera camera) {
	    Vector3f cameraPos = camera.getPosition();
	    Vector3f rotation = camera.getRotation();
		Vector3f PrePos = camera.getPrePos();
		Vector3f PreRot = camera.getPreRot();
	    viewMatrix.identity()


				.rotate((float)Math.toRadians(PreRot.x), new Vector3f(1, 0, 0))
				.rotate((float)Math.toRadians(PreRot.y), new Vector3f(0, 1, 0))
				.rotate((float)Math.toRadians(PreRot.z), new Vector3f(0, 0, 1))
				.translate(-PrePos.x,-PrePos.y,-PrePos.z)


		.rotate((float)Math.toRadians(rotation.x), new Vector3f(1, 0, 0))
		.rotate((float)Math.toRadians(rotation.y), new Vector3f(0, 1, 0))
		.rotate((float)Math.toRadians(rotation.z), new Vector3f(0, 0, 1))
				.translate(-cameraPos.x, -cameraPos.y, -cameraPos.z)
		;
	    return viewMatrix;
	}
	/*public Matrix4f getModelViewMatrix(GameItem gameItem, Matrix4f viewMatrix) {
	    Vector3f rotation = gameItem.getRotation();
	    modelViewMatrix
				.identity()
				.translate(gameItem.getPosition())
				.rotateX((float)Math.toRadians(-rotation.x))
				.rotateY((float)Math.toRadians(-rotation.y))
				.rotateZ((float)Math.toRadians(-rotation.z))
				.scale(gameItem.getScale());
	    Matrix4f viewCurr = new Matrix4f(viewMatrix);
	    return viewCurr.mul(modelViewMatrix);
	}*/
	public Matrix4f getModelViewMatrix(Matrix4f modelMatrix, Matrix4f viewMatrix) {

		Matrix4f viewCurr = new Matrix4f(viewMatrix);
		return viewCurr.mul(modelMatrix);
	}
	public Matrix4f getModelMatrix(GameItem gameItem) {
		if(gameItem.getRotationType() == 0) {
			Vector3f rotation = gameItem.getRotation();
			modelMatrix.identity().translate(gameItem.getPosition()).
					rotateX((float) Math.toRadians(-rotation.x)).
					rotateY((float) Math.toRadians(-rotation.y)).
					rotateZ((float) Math.toRadians(-rotation.z)).
					scale(gameItem.getScale());
		}else{
			Quaternionf rotation = gameItem.getQrot();
			modelMatrix.identity().translate(gameItem.getPosition()).
					rotate(rotation).
					scale(gameItem.getScale());
		}
		return modelMatrix;
	}

	public Matrix4f getOrtoProjModelMatrix(GameItem gameItem, Matrix4f ortho) {
		Vector3f rotation = gameItem.getRotation();
		Matrix4f modelMatrix = new Matrix4f();
		modelMatrix.identity().translate(gameItem.getPosition()).
				rotateX((float)Math.toRadians(-rotation.x)).
				rotateY((float)Math.toRadians(-rotation.y)).
				rotateZ((float)Math.toRadians(-rotation.z)).
				scale(gameItem.getScale());
		Matrix4f orthoMatrixCurr = new Matrix4f(ortho);
		orthoMatrixCurr.mul(modelMatrix);
		return orthoMatrixCurr;
	}
}
