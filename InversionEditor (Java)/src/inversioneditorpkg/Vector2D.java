/* This class is used to create vectors for math/physics, not the data structure.
 * 
 */

package inversioneditorpkg;

public class Vector2D 
{
	private float x1, y1;
	private float x2, y2;
	private float magnitude;	//length of vector.
	
	public Vector2D(float x1, float y1, float x2, float y2)
	{
		this.x1 = x1;
		this.x2 = x2;
		this.y1 = y1;
		this.y2 = y2;
	}

	public float getX1() {
		return x1;
	}

	public void setX1(float x1) {
		this.x1 = x1;
	}

	public float getY1() {
		return y1;
	}

	public void setY1(float y1) {
		this.y1 = y1;
	}

	public float getX2() {
		return x2;
	}

	public void setX2(float x2) {
		this.x2 = x2;
	}

	public float getY2() {
		return y2;
	}

	public void setY2(float y2) {
		this.y2 = y2;
	}

	public float getMagnitude() 
	{
		//use Pythagorean Theorem to find magnitude
		float a, b, c;
		a = (x2 - x1) * (x2 - x1);
		b = (y2 - y1) * (y2 - y1);
		c = (float)Math.sqrt(a + b);
		
		return c;
	}
	
	public float getVx()
	{
		return x2 - x1;
	}
	
	public float getVy()
	{
		return y2 - y1;
	}
}
