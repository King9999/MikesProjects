/* This class contains all data about the player, including 
 * movement and jump ability. And of course, inverting the level.
 * 
 * The player actually controls two characters.  The second character is the player's "shadow," which mimics everything
 * the player does.  Upon inverting the level, control is given to the shadow, and the other character becomes
 * the shadow.
 * 
 * NOTE: Inversion can only occur if the shadow can fit within the dimensons of at least one 32x32 tile.
 */

package inversioneditorpkg;

public class Player 
{
	//states
	protected boolean isJumping = false;
	protected boolean isMoving = false;
	protected boolean isDoubleJumping = false;
	protected boolean levelInverted = false;
	protected boolean isRunning = false;
	protected boolean canInvert = false;		//if true, then player's shadow is not obstructed.
	
	//player movement
	protected float xPos = 0;
	protected float yPos = 0;
	private float originX = 0;
	private float originY = 0;
	protected float speedX = 0;
	protected float speedY = 0;
	protected float speedLimit = 0;		//changes depending on whether player is running or walking
	protected float acceleration = 0;		//gradually speeds up player while moving
	protected float jumpHeight = 0;		//must be a negative number
	protected float currentFriction = 0;	//when it's 1, player can move normally.
	
	//misc
	protected float width = 0;		//used for setting up origin
	protected float height = 0;
	protected byte frameCount = 0;	//a buffer used to prevent friction from kicking in too soon
	
	

	//constants
	protected final float GRAVITY = 0.45f;
	protected final float FRICTION = 0.77f;	//slows down player when on the ground and not moving.
	protected final float DROP_SPEED = 10f;	//controls how fast player falls.
	
	public Player(float xPos, float yPos, float width, float height)
	{
		this.xPos = xPos;
		this.yPos = yPos;
		this.width = width;
		this.height = height;
		speedX = 0;
		speedY = 0;
		speedLimit = 0;
		acceleration = 0;
		jumpHeight = -10;
		currentFriction = 0;
		
		isJumping = false;
		isMoving = false;
		isDoubleJumping = false;
		levelInverted = false;
		isRunning = false;
		frameCount = 0;
	}
	
	public void moveLeft()
	{
		speedLimit = -5f;
		acceleration = -0.3f;
		currentFriction = 1;
		isMoving = true;
		isRunning = false;
	}
	
	public void moveRight()
	{
		speedLimit = 5f;
		acceleration = 0.3f;
		currentFriction = 1;
		isMoving = true;
		isRunning = false;
	}
	
	public void runLeft()
	{
		speedLimit = -7.5f;
		currentFriction = 1;
		isMoving = true;
		isRunning = true;
	}
	
	public void runRight()
	{
		speedLimit = 7.5f;
		currentFriction = 1;
		isMoving = true;
		isRunning = true;
	}
	
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
			speedY += jumpHeight - 5;
		}
	}
	
	public void stopMoving()
	{
		acceleration = 0;
		currentFriction = FRICTION;
		isMoving = false;
		isRunning = false;
	}
	
	/* Updates player movement and actions every frame */
	public void update()
	{
//		if (!isMoving && !isRunning)
//			frameCount++;
//		else
//			frameCount = 0;
		
		speedX += acceleration;
		//speedY = jumpHeight;
		if (acceleration > 0 && speedX > speedLimit)
			speedX = speedLimit;
		if (acceleration < 0 && speedX < speedLimit)	//moving left
			speedX = speedLimit;
				
		//apply friction.
		speedX *= currentFriction;
		if (Math.abs(speedX) < 0.1)
			speedX = 0;
		
		//apply constant gravity
		//if (isJumping)
		speedY += GRAVITY;
		
		if (speedY < -DROP_SPEED)
			speedY = -DROP_SPEED;
		if (speedY > DROP_SPEED)
			speedY = DROP_SPEED;
		
		isJumping = true;
			
		/*if (yPos + height > originY)
		{
			yPos = originY - height;
			speedY = 0;
			isJumping = false;
		}*/
		
		xPos += speedX;
		yPos += speedY;
		
	}
	
	/****Getters and Setters ******/

	public float getSpeedX() {
		return speedX;
	}

	public void setSpeedX(float speedX) {
		this.speedX = speedX;
	}

	public float getSpeedY() {
		return speedY;
	}

	public void setSpeedY(float speedY) {
		this.speedY = speedY;
	}

	public boolean isJumping() {
		return isJumping;
	}

	public void setJumping(boolean isJumping) {
		this.isJumping = isJumping;
	}

	public boolean isMoving() {
		return isMoving;
	}

	public void setMoving(boolean isMoving) {
		this.isMoving = isMoving;
	}

	public boolean isDoubleJumping() {
		return isDoubleJumping;
	}

	public void setDoubleJumping(boolean isDoubleJumping) {
		this.isDoubleJumping = isDoubleJumping;
	}

	public boolean isLevelInverted() {
		return levelInverted;
	}

	public void setLevelInverted(boolean levelInverted) {
		this.levelInverted = levelInverted;
	}

	public boolean canInvert() {
		return canInvert;
	}

	public void setCanInvert(boolean canInvert) {
		this.canInvert = canInvert;
	}

	public boolean isRunning() {
		return isRunning;
	}

	public void setRunning(boolean isRunning) {
		this.isRunning = isRunning;
	}

	public float getSpeedLimit() {
		return speedLimit;
	}

	public void setSpeedLimit(float speedLimit) {
		this.speedLimit = speedLimit;
	}

	public float getAcceleration() {
		return acceleration;
	}

	public void setAcceleration(float acceleration) {
		this.acceleration = acceleration;
	}

	public float getGravity() {
		return GRAVITY;
	}

	public float getJumpHeight() {
		return jumpHeight;
	}


	public void setJumpHeight(float jumpHeight) {
		this.jumpHeight = jumpHeight;
	}
	

	public float posX() {
		return xPos;
	}
	

	public void setPosX(float xPos) {
		this.xPos = xPos;
	}

	public float posY() {
		return yPos;
	}

	public void setPosY(float yPos) {
		this.yPos = yPos;
	}
	
	public float getFriction() {
		return currentFriction;
	}

	public void setFriction(float currentFriction) {
		this.currentFriction = currentFriction;
	}

	public float originX() {
		return originX;
	}

	public void setOriginX(float originX) {
		this.originX = originX;
	}

	public float originY() {
		return originY;
	}

	public void setOriginY(float originY) {
		this.originY = originY;
	}

	public float width() {
		return width;
	}

	public void setWidth(float width) {
		this.width = width;
	}

	public float height() {
		return height;
	}

	public void setHeight(float height) {
		this.height = height;
	}
	
	public byte getFrameCount() {
		return frameCount;
	}

	public void setFrameCount(byte frameCount) {
		this.frameCount = frameCount;
	}
}
