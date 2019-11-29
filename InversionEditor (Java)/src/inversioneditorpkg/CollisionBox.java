/* This class is used to check collision between objects.  It uses the separating axis theorem (SAT). */

package inversioneditorpkg;

import java.awt.Color;
import java.awt.Graphics;

public class CollisionBox 
{
	protected float xPos, yPos;
	protected float width, height;
	protected Color color;			//shows boundaries
	protected float overlap;		//checks for overlap between two boxes
	private float distance;		//distance between two boxes' half-dimensions
	private boolean isOnTop;	//checks to see if a box is standing on top of another box. Used by player.
	private boolean isOnBottom;
	private boolean isOnSide;
	
	public CollisionBox(float xPos, float yPos, float width, float height)
	{
		this.xPos = xPos;
		this.yPos = yPos;
		this.width = width;
		this.height = height;
		
		color = new Color(255, 0, 0);
	}
	
	public boolean intersects(CollisionBox box)
	{
		boolean isColliding = false;
		
		/*check distance between two boxes. If there's no overlap on the x axis, then there's no collision.
		 * If there's overlap on the x axis, then check for overlap on the y axis; if there's overlap there as well,
		 * then there is collision.
		 * 
		 *  If there's collision, then check where the collision is occurring by using a vector and getting the
		 *  direction it's facing.  The direction tells us which side of the box the collision is happening.
		 */
		
		//checking if box1's x value is within box2's range
		if (xPos + width >= box.xPos && xPos <= box.xPos + box.width)
		{	
			if (yPos + height >= box.yPos && yPos <= box.yPos + box.height)
			{
				isColliding = true;
				//if we get here, then there's collision. Check for overlap by first getting half-dimensions of both boxes.
				float halfWidthA = this.width * 0.5f;
				float halfHeightA = this.height * 0.5f;
				float halfWidthB = box.width * 0.5f;
				float halfHeightB = box.width * 0.5f;
				
				Vector2D vector = new Vector2D(xPos + halfWidthA, yPos + halfHeightA, box.xPos + halfWidthB, box.yPos + halfHeightB);
				
				//get the overlap on both x and y axis
				float total = (xPos + width + box.xPos + box.width) * 0.5f;
				float overlapX = (halfWidthA + halfWidthB) - Math.abs(vector.getVx());	//vx & vy can't be negative
				float overlapY = (halfHeightA + halfHeightB) - Math.abs(vector.getVy());
				
				
				//compare the overlap on X & Y.  Whichever has the SMALLER overlap is where the collision is occurring.
				if (overlapX <= overlapY)
				{
					//check which side of the rectangle the collision is occurring. We use the vector to check.
					//If I have trouble understanding, a diagram will help.
					if (vector.getVx() > 0)
					{
						//collision is occuring on the left side; push the box out to the left
						this.xPos = this.xPos - overlapX;
						//System.out.println("Collision on left side");
						isOnTop = false;
						isOnBottom = false;
						isOnSide = true;
					}
					else
					{
						this.xPos = this.xPos + overlapX;
						//System.out.println("Collision on right side");
						isOnTop = false;
						isOnBottom = false;
						isOnSide = true;
					}
					
				}
				else
				{
					//collision is occurring on y axis
					if (vector.getVy() > 0)
					{
						//collision is occuring on the top of the rectangle; push the box out to the opposite direction
						this.yPos = this.yPos - overlapY;
						//System.out.println("Collision on top");
						isOnTop = true;
						isOnBottom = false;
						isOnSide = false;
					}
					else
					{
						this.yPos = this.yPos + overlapY;
						//System.out.println("Collision on bottom");
						isOnTop = false;
						isOnBottom = true;
						isOnSide = false;
					}
				}
			}
		}
		return isColliding;
	}
	
	
	public void paint(Graphics g)
	{
		g.setColor(color);
		g.drawRect((int)xPos, (int)yPos, (int)width, (int)height);
	}
	
	
	/*****************************/
	public float getxPos() {
		return xPos;
	}

	public void setxPos(float xPos) {
		this.xPos = xPos;
	}

	public float getyPos() {
		return yPos;
	}

	public void setyPos(float yPos) {
		this.yPos = yPos;
	}

	public float getWidth() {
		return width;
	}

	public void setWidth(float width) {
		this.width = width;
	}

	public float getHeight() {
		return height;
	}

	public void setHeight(float height) {
		this.height = height;
	}

	public Color getColor() {
		return color;
	}

	public void setColor(Color color) {
		this.color = color;
	}

	public float getOverlap() {
		return overlap;
	}

	public void setOverlap(float overlap) {
		this.overlap = overlap;
	}

	public float getDistance() {
		return distance;
	}

	public void setDistance(float distance) {
		this.distance = distance;
	}

	public boolean isOnTop() {
		return isOnTop;
	}

	public void setOnTop(boolean isOnTop) {
		this.isOnTop = isOnTop;
	}

	public boolean isOnBottom() {
		return isOnBottom;
	}

	public void setOnBottom(boolean isOnBottom) {
		this.isOnBottom = isOnBottom;
	}

	public boolean isOnSide() {
		return isOnSide;
	}

}
