/* This class is a subclass of CollisionBox.  Slopes can be created, which will improve the level variety. */

package inversioneditorpkg;

import java.awt.Color;

public class CollisionSlope extends CollisionBox
{
	
	public CollisionSlope(float xPos, float yPos, float width, float height) 
	{
		super(xPos, yPos, width, height);	//takes parameters from its parent.
		
		color = new Color(200, 200, 255);	//making this blue to separate the collision box colour from the more common red.
		
	}
	
	public boolean intersects(CollisionBox box)
	{
		boolean isColliding = false;
		
		/* This function operates similarly to CollisionBox's function, except collision does not result in the player
		 * being pushed out of the box's area.  Instead, collision results in increased movement on the Y axis.
		 * Since slopes are 45 degree angles, we can use the linear equation y = mx + b to move the player.
		 * The b constant is not a factor, so it will always be 0.  The m constant will be either 1 or -1.
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
				
				float m = 1;	//slope
				
				
				//compare the overlap on X & Y.  Whichever has the SMALLER overlap is where the collision is occurring.
				if (overlapX <= overlapY)
				{
					//check which side of the rectangle the collision is occurring. We use the vector to check.
					//If I have trouble understanding, a diagram will help.
					if (vector.getVx() > 0)
					{
						//collision is occurring on the left side; raise player
						//this.xPos = this.xPos - overlapX;
						//box.yPos = m * box.xPos;
						float leftSide = xPos - width;
						box.yPos = this.yPos + halfHeightA - ((box.xPos + box.width) - leftSide) - halfHeightB;
						System.out.println("Collision with slope");
						
					}
//					else
//					{
//						this.xPos = this.xPos + overlapX;
//						//System.out.println("Collision on right side");
//					
//					}
					
				}
				else
				{
					//collision is occurring on y axis
					if (vector.getVy() > 0)
					{
						//collision is occuring on the top of the rectangle; push the box out to the opposite direction
						this.yPos = this.yPos - overlapY;
						//System.out.println("Collision on top");
						
					}
					else
					{
						this.yPos = this.yPos + overlapY;
						//System.out.println("Collision on bottom");
					
					}
				}
			}
		}
		return isColliding;
	}
	
}
