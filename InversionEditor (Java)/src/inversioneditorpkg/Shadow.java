/* This is the player's shadow.  When inverting a level, the shadow becomes the character and the character becomes the
 * shadow.
 * 
 * The shadow has a collision box, but collision is only checked against boxes in the inverted world. If there is no
 * collision to check, the shadow will stick to the player.
 * 
 */

package inversioneditorpkg;

public class Shadow extends Player
{

	public Shadow(float xPos, float yPos, float width, float height) 
	{
		super(xPos, yPos, width, height);
		
		yPos += 32;		//TODO: 32 is the height of the avatar. For some reason the height is undefined upon init.
		jumpHeight = 10;
	}
	
	@Override
	public void jump()
	{
		if (!isJumping)
		{
			isJumping = true;
			speedY += jumpHeight;
		}
		else if (isJumping && !isDoubleJumping)
		{
			isDoubleJumping = true;
			speedY += jumpHeight + 5;
		}
		
	}
	
	@Override
	public void update()
	{
		speedX += acceleration;
	
		if (acceleration > 0 && speedX > speedLimit)
			speedX = speedLimit;
		if (acceleration < 0 && speedX < speedLimit)	//moving left
			speedX = speedLimit;
				
		//apply friction.
		speedX *= currentFriction;
		if (Math.abs(speedX) < 0.1)
			speedX = 0;
		
		//apply gravity. Works a little bit different for the shadow than it does for the player.
		speedY -= GRAVITY;
		
		if (speedY < -DROP_SPEED)
			speedY = -DROP_SPEED;
		if (speedY > DROP_SPEED)
			speedY = DROP_SPEED;
		
		
		xPos += speedX;
		yPos += speedY;
		
		
	}

}
