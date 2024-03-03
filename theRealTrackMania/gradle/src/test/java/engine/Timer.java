package engine;

public class Timer {
	
	 private double lastLoopTime;
	    
	    public void init() {
	        lastLoopTime = getTime();
	    }

	   

	    public float getElapsedTime() {
	        double time = getTime();
	        float elapsedTime = (float) (time - lastLoopTime);
	        lastLoopTime = time;
	        return elapsedTime;
	    }

	    public double getLastLoopTime() {
	        return lastLoopTime;
	}
	public double getTime() {
		return (double)System.nanoTime() / (double)1000000000L;
	}

	
	public void Wait(long t){
		
		try {
			Thread.sleep(t);
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
		
	}
}
