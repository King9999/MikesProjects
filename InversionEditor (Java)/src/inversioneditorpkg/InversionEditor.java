/* March 2013
 * Inversion Editor by Mike Murray
 * ©2013 Mike Murray, all rights reserved.
 * 
 * This is the editor used to create levels for the Inversion game.  The editor uses the full screen to display levels.
 * The UI consists of a few buttons and a single menu bar that's hidden unless the mouse hovers in the top corner of the
 * screen.  My goal is to create a simple but slick editor.
 * 
 * FEATURES
 * ============
 * Create levels of various sizes
 * Test play mode
 * Ability to use any two colours (will not be reflected in game)
 * Place starting point, exit point, traps, enemies
 * Save and load levels
 * Generate an XML document containing level data
 * 
 * NOTE: The Editor runs in 60 fps, but on Android, the game must run at 30 fps. 
 */


package inversioneditorpkg;
import inversioneditorpkg.Level.LevelSize;

import java.applet.Applet;
import java.awt.Color;
import java.awt.Font;
import java.awt.Frame;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.MouseInfo;
import java.awt.Point;
import java.awt.PointerInfo;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.image.BufferedImage;
import java.net.URL;
import java.util.ArrayList;

import javax.imageio.ImageIO;
//this is here so I can use the enum. Enums are treated like classes in java

public class InversionEditor extends Applet implements Runnable, KeyListener, MouseListener
{
	
	private Image image;		//used for double buffering
	private Graphics second;	//used for double buffering
	private Image playerImg;	//player graphic
	private Image originImg;
	private URL playerImgLocation;	//location of image file
	private URL originImgLocation;
	
	private boolean jumpPressed = false;	//used to prevent multiple jumps by holding down jump key
	private boolean jumpKeyReleased = false;//used to prevent player from staying in the air repeatedly by tapping the jump key
	//player
	private Player player;
	private CollisionBox playerHitbox, shadowHitbox;
	private Shadow shadow;
	
	//mouse pointer 
	PointerInfo pointer;
	Point mousePoint;
	
	//FPS counter
	private int fps;
	private long currentTime;	//time in millisecs
	private long previousTime;
	
	/*** Buttons - images, url's, IDs & Vector ***/
	private Image tileIcon;
	private Image dBugIcon;
	private Image invertIcon;
	private Image testIcon;
	private Image startIcon;
	private Image exitIcon;
	private Image colorIcon;
	
	private URL tileUrl;
	private URL dBugUrl;
	private URL invertUrl;
	private URL testUrl;
	private URL startUrl;
	private URL exitUrl;
	private URL colorUrl;
	
	private final int TILE = 0;
	private final int INVERT = 1;
	private final int TEST = 2;
	private final int START = 3;
	private final int EXIT = 4;
	private final int COLOR = 5;
	private final int DEBUG = 6;
	
	
	ArrayList<Button> buttonList = new ArrayList<Button>();
	
	/****Level ****/
	private Level level;
	private CollisionBox box1, box2;
	private CollisionSlope slope;
	private Vector2D vector;
	
	//ticker
	private Ticker ticker;
	
	@Override
	public void init() 
	{
		//applet setup
		this.setSize(1024, 768);	//window size
		Color darkBlue = new Color(0, 0, 50);
		this.setBackground(darkBlue);
		this.setFocusable(true);	//so you don't have to click the applet to use it
		Frame frame = (Frame) this.getParent().getParent();
		frame.setTitle("Inversion Editor by Mike Murray");
		addKeyListener(this);
		addMouseListener(this);
		
		//ticker set up
		ticker = new Ticker();
		
		//level set up
		level = new Level(LevelSize.SMALL);
		box1 = new CollisionBox(100, 270, 50, 50);
		box2 = new CollisionBox(400, 200, 50, 50);
		//slope = new CollisionSlope(150, 380, 50, 50);
		vector = new Vector2D(box1.getxPos(), box1.getyPos(), box2.getxPos(), box2.getyPos());
		
		//fps set up
		fps = 0;
		currentTime = 0;
		previousTime = 0;
		
		//image set up
		try
		{
			playerImgLocation = getDocumentBase();
			//img = ImageIO.read(playerImgLocation);
			originImgLocation = getDocumentBase();
			dBugUrl = getDocumentBase();
			tileUrl = getDocumentBase();
			invertUrl = getDocumentBase();
			testUrl = getDocumentBase();
			startUrl = getDocumentBase();
			exitUrl = getDocumentBase();
			colorUrl = getDocumentBase();
		}
		catch (Exception e)
		{
			System.out.println("Unable to locate file!");
		}
		
		playerImg = getImage(playerImgLocation, "images/Avatar.png");
		originImg = getImage(originImgLocation, "images/origin.png");
		dBugIcon = getImage(dBugUrl, "images/debugicon.png");
		tileIcon = getImage(tileUrl, "images/tileicon.png");
		invertIcon = getImage(invertUrl, "images/inverticon.png");
		testIcon = getImage(testUrl, "images/testicon.png");
		startIcon = getImage(startUrl, "images/starticon.png");
		exitIcon = getImage(exitUrl, "images/exiticon.png");
		colorIcon = getImage(colorUrl, "images/coloricon.png");
		
		//player setup
		player = new Player(100, 300, (float)playerImg.getWidth(this), (float)playerImg.getHeight(this));
		shadow = new Shadow(player.posX(), player.posY(), (float)playerImg.getWidth(this), (float)playerImg.getHeight(this));
		
		playerHitbox = new CollisionBox(player.posX(), player.posY(), 18, 32);	//TODO: Figure out why player width and height is returning -1 so I don't have to use hard numbers
		shadowHitbox = new CollisionBox(shadow.posX(), shadow.posY(), 18, 32);
		
		//System.out.println(playerImg.getWidth(this));
		
		//set up player width and height for the origin
//		player.setPlayerWidth(playerImg.getWidth(this));
//		player.setPlayerHeight(playerImg.getHeight(this));
		
		//set origin point so the player can't fall past it
//		player.setOriginX(player.posX() + player.width() * 0.5f);
//		player.setOriginY(player.posY() + player.height() - originImg.getHeight(this));
		
		/***Buttons ****/
		int defaultPosX = 10;
		int defaultPosY = 700;
		buttonList.add(new Button(tileIcon, defaultPosX, defaultPosY, false));
		buttonList.add(new Button(invertIcon, defaultPosX + 60, defaultPosY, false));
		buttonList.add(new Button(testIcon, defaultPosX + 120, defaultPosY, false));
		buttonList.add(new Button(startIcon, defaultPosX + 180, defaultPosY, false));
		buttonList.add(new Button(exitIcon, defaultPosX + 240, defaultPosY, false));
		buttonList.add(new Button(colorIcon, defaultPosX + 300, defaultPosY, false));
		buttonList.add(new Button(dBugIcon, defaultPosX + 360, defaultPosY, false));
		
		
	}
	
	@Override
	public void start() 
	{
		Thread thread = new Thread(this);
		thread.start();
	}
	
	@Override
	public void stop() {
		// TODO Auto-generated method stub
		
	}
	
	@Override
	public void destroy() {
		// TODO Auto-generated method stub
		
	}
	
	
	@Override
	public void run() 
	{
		//game loop.
		while (true)
		{
			player.update();
			shadow.update();
			ticker.update();
			
			//get FPS
			currentTime = System.currentTimeMillis();	
			fps = (int)(1000 / (currentTime - previousTime));
			previousTime = currentTime;
			
			//System.out.println("Frame Count: " + player.getFrameCount());
			
			/*capture mouse pointer's location.  We subtract getLocationOnScreen() so that we get the pointer's
			 * position relative to the applet's screen rather than the monitor's screen. 
			 */
			pointer = MouseInfo.getPointerInfo();
			int x = pointer.getLocation().x - this.getLocationOnScreen().x;
			int y = pointer.getLocation().y - this.getLocationOnScreen().y;
			
			//pointer boundary checks
			if (x < 0)
				x = 0;
			if (x > this.getWidth())
				x = this.getWidth();
			if (y < 0)
				y = 0;
			if (y > this.getHeight())
				y = this.getHeight();
			
			mousePoint = new Point(x, y);
			
			
			/* Check where the mouse is resting. */
			for (Button b : buttonList)
			{
				if (buttonHovered(b) && b.getCurrentColor() != b.getCOLOR_ON())
				{
					b.setCurrentColor(b.getCOLOR_HOVER());
					//change ticker message depending on which button is touched
					switch(buttonList.indexOf(b))
					{
					case TILE:
						ticker.setMsg("Place a tile on the screen. Left click to drop color 1, right click for color 2.");
						break;
					case INVERT:
						ticker.setMsg("Turns the level upside down, and the background becomes the foreground.");
						break;
					case TEST:
						ticker.setMsg("Test play the current level.  There must be a starting point!");
						break;
					case START:
						ticker.setMsg("Place a starting point for the player.  Only one per level is allowed.");
						break;
					case EXIT:
						ticker.setMsg("Place an exit warp. After placing one, you must specify where it takes you.  More than one warp can be dropped.");
						break;
					case COLOR:
						ticker.setMsg("Sets the colors for the foreground and background.  Both colors cannot be the same.");
						break;
					case DEBUG:
						ticker.setMsg("Displays debug information, including collision boxes.");
						break;
					}
				}
				else if (!buttonHovered(b) && b.getCurrentColor() != b.getCOLOR_ON())
				{
					b.setCurrentColor(b.getCOLOR_OFF());
					//ticker.setMsg("");
					//ticker.setScrollDelayTimer(0);
					ticker.setMsgSet(false);
				}
			}
			
			/* collision checking. Must check collision for both the normal and inverse level. */
			playerHitbox.setxPos(player.posX());
			playerHitbox.setyPos(player.posY());
			shadowHitbox.setxPos(shadow.posX());
			shadowHitbox.setyPos(shadow.posY());
			
			if (!level.isLevelInverted())
			{
				for (CollisionBox c : level.getBoxList())
				{
					if (playerHitbox.intersects(c))
					{
						player.setPosX(playerHitbox.getxPos());
						player.setPosY(playerHitbox.getyPos());
						
						//TODO:Figure out how to prevent improper movement & collision checking when this line is called.
	//					if (playerHitbox.isOnSide())	
	//					{
	//						player.stopMoving();
	//					}
						
						if (playerHitbox.isOnTop())
						{
							player.setJumping(false);
							player.setDoubleJumping(false);
							player.setSpeedY(0);
							//System.out.println("Jump Height: " + player.getJumpHeight());
						}
						
						if (playerHitbox.isOnBottom())	//player bumps head; fall slightly faster
						{
							player.setSpeedY(player.getSpeedY() * -0.6f);
							shadow.setSpeedY(shadow.getSpeedY() * -0.6f);
							player.setDoubleJumping(true);	//disable double jump if player hits their head.
							shadow.setDoubleJumping(true);
						}
					}
				}
				
				//inverse collision checking.
				/*for (CollisionBox c : level.getInvBoxList())
				{
					if (playerHitbox.intersects(c))
					{
						player.setPosX(playerHitbox.getxPos());
						player.setPosY(playerHitbox.getyPos());
						
						//TODO:Figure out how to prevent improper movement & collision checking when this line is called.
	//					if (playerHitbox.isOnSide())	
	//					{
	//						player.stopMoving();
	//					}
						
						if (playerHitbox.isOnTop())
						{
							player.setJumping(false);
							player.setDoubleJumping(false);
							player.setSpeedY(0);
							//System.out.println("Jump Height: " + player.getJumpHeight());
						}
						
						if (playerHitbox.isOnBottom())	//player bumps head; fall slightly faster
						{
							player.setSpeedY(player.getSpeedY() * -0.6f);
							shadow.setSpeedY(shadow.getSpeedY() * -0.6f);
							player.setDoubleJumping(true);	//disable double jump if player hits their head.
							shadow.setDoubleJumping(true);
						}
					}
				}*/
			}
			else		//level is inverted, check collision against inverted boxes.
			{
				for (CollisionBox c : level.getInvBoxList())
				{
					if (playerHitbox.intersects(c))
					{
						player.setPosX(playerHitbox.getxPos());
						player.setPosY(playerHitbox.getyPos());
						
						//TODO:Figure out how to prevent improper movement & collision checking when this line is called.
	//					if (playerHitbox.isOnSide())	
	//					{
	//						player.stopMoving();
	//					}
						
						if (playerHitbox.isOnTop())
						{
							player.setJumping(false);
							player.setDoubleJumping(false);
							player.setSpeedY(0);
							//System.out.println("Jump Height: " + player.getJumpHeight());
						}
						
						if (playerHitbox.isOnBottom())
						{
							player.setSpeedY(player.getSpeedY() * -0.6f);
							shadow.setSpeedY(shadow.getSpeedY() * -0.6f);
							player.setDoubleJumping(true);	//disable double jump if player hits their head.
							shadow.setDoubleJumping(true);
						}
					}
				}
			}
			
			//set shadow's position
			shadow.setPosX(player.posX());
			if (shadow.posY() < player.posY() + playerImg.getHeight(this))
			{
				shadow.setPosY(player.posY() + playerImg.getHeight(this));
				shadow.setSpeedY(0);
				shadow.setJumping(false);
				shadow.setDoubleJumping(false);
			}
			//System.out.println("Player Height: " + playerImg.getHeight(this));
//			shadow.setPosY(player.posY() + 35);
			
			//TODO: Figure out how to make slopes!
		/*	if (slope.intersects(playerHitbox))
			{
				player.setPosX(playerHitbox.getxPos());
				player.setPosY(playerHitbox.getyPos());
				player.setJumping(false);
			}*/
			
			
			repaint();
			
			try
			{
				Thread.sleep(17);	// 1000/60 = 17. In other words, the editor updates 60 times a second.
			}
			catch(InterruptedException e)
			{
				e.printStackTrace();
			}
		}
		
	}
	
	/** Checks if a button was clicked **/
	public boolean buttonHovered(Button b)
	{
		return (mousePoint.x >= b.getButtonX() && mousePoint.x <= b.getButtonX() + b.getWidth()
				&& mousePoint.y >= b.getButtonY() && mousePoint.y <= b.getButtonY() + b.getHeight());
	}
	
	public void update(Graphics g)
	{
		//double buffering method. Use to prevent flickering of images
		if (image == null)
		{
			image = createImage(this.getWidth(), this.getHeight());
			second = image.getGraphics();
		}
		
		second.setColor(getBackground());
		second.fillRect(0, 0, getWidth(), getHeight());
		second.setColor(getForeground());
		paint(second);
		g.drawImage(image, 0, 0, this);
		
	
		
	}
	
	public void paint(Graphics g)
	{
		
		//draw level
		//level.paint(g);
//		box1.paint(g);
//		box2.paint(g);
//		slope.paint(g);
		
	
		
		//check if debug needs to be drawn
		Button b = buttonList.get(DEBUG);
		if (b.isButtonOn())
		{
			level.drawCollision(g);
			playerHitbox.paint(g);
			drawDebug(g);
		}
		else
			level.paint(g);
		
		//draw player
		g.drawImage(playerImg, (int)player.posX(), (int)player.posY(), this);
		g.drawImage(playerImg, (int)shadow.posX(), (int)shadow.posY(), this);
		
//		g.setColor(Color.BLACK);
//		g.drawLine((int)box1.getxPos() + (int)(box1.getWidth() * 0.5), (int)box1.getyPos() + (int)(box2.getHeight() * 0.5), 
//				(int)box2.getxPos() + (int)(box2.getWidth() * 0.5), (int)box2.getyPos() + (int)(box2.getHeight() * 0.5));
		
		//ticker
		ticker.paint(g, ticker.getMsg());
				
		for (Button button : buttonList)
			button.paint(g, this);
		
		
	}
	
	/*** Debug Data ***/
	public void drawDebug(Graphics g)
	{
		Font dBugFont = new Font("Courier New", Font.PLAIN, 16);
		g.setColor(Color.WHITE);
		g.setFont(dBugFont);
		int defaultPosY = 100;	//positions the text
		int defaultPosX = 700;
		
		g.drawString(fps + " FPS", 10, 20);
		g.drawString("Mouse Pointer: (" + mousePoint.x + ", " + mousePoint.y + ")", defaultPosX, defaultPosY - 20);
		g.drawString("Player Pos: <" + player.posX() + ", " + player.posY() + ">", defaultPosX, defaultPosY);
		g.drawString("Walking: " + player.isMoving(), defaultPosX, defaultPosY + 20);
		g.drawString("Running: " + player.isRunning(), defaultPosX, defaultPosY + 40);
		g.drawString("Jumping: " + player.isJumping(), defaultPosX, defaultPosY + 60);
		g.drawString("D. Jumping: " + player.isDoubleJumping(), defaultPosX, defaultPosY + 80);
		g.drawString("Level Inverted: " + player.isLevelInverted(), defaultPosX, defaultPosY + 100);
		g.drawString("Acceleration: " + player.getAcceleration(), defaultPosX, defaultPosY + 120);
		g.drawString("SpeedX: " + player.getSpeedX(), defaultPosX, defaultPosY + 140);
		g.drawString("SpeedY: " + player.getSpeedY(), defaultPosX, defaultPosY + 160);
		g.drawString("Friction: " + player.getFriction(), defaultPosX, defaultPosY + 180);
		g.drawString("Level Inverted: " + level.isLevelInverted(), defaultPosX, defaultPosY + 200);
	}
	
	/***Keyboard Events***/
	@Override
	public void keyPressed(KeyEvent key) 
	{
		switch(key.getKeyCode())
		{
		case KeyEvent.VK_W:		//Editor: scroll up; Game: jump, double jump (while in midair)
			if (!jumpPressed)
			{
				if (!player.isJumping() || !player.isDoubleJumping())
				{
					player.jump();
					shadow.jump();
					jumpKeyReleased = false;
				}
				else
					jumpPressed = true;
			}
			break;
			
		case KeyEvent.VK_S:		//Editor: scroll down; Game: N/A
			break;
			
		case KeyEvent.VK_A:		//Editor: scroll left; game: move left
			player.moveLeft();
			break;
			
		case KeyEvent.VK_D:		//Editor scroll right; Game: move right
			player.moveRight();
			break;
		}
		
	}

	@Override
	public void keyReleased(KeyEvent key)
	{
		if (key.getKeyCode() == KeyEvent.VK_D || key.getKeyCode() == KeyEvent.VK_A)
		{
	
//			if (player.getFrameCount() >= 2)
				player.stopMoving();
		}
		
		if(key.getKeyCode() == KeyEvent.VK_W)
		{
			
			jumpPressed = false;
			
			//this piece of code allows the player to control the height of their first jump, but not the double
			//jump.
			if (!jumpKeyReleased && !player.isDoubleJumping())
			{
				player.setSpeedY(player.getSpeedY() * -0.1f);
				shadow.setSpeedY(shadow.getSpeedY() * -0.1f);
				jumpKeyReleased = true;
			}
		}
	}

	@Override
	public void keyTyped(KeyEvent m) {
		// TODO Auto-generated method stub
		
	}

	/***Mouse Events***/
	@Override
	public void mouseClicked(MouseEvent m) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void mouseEntered(MouseEvent m) 
	{
		
		
	}

	@Override
	public void mouseExited(MouseEvent m) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void mousePressed(MouseEvent m) 
	{
		Button b = buttonList.get(TILE);
		if (b.isButtonOn())
		{
			//drop a tile on the screen.
			byte num = 1;
			level.drawTile(num, mousePoint.x, mousePoint.y);
		}
		
		
	}

	@Override
	public void mouseReleased(MouseEvent m)
	{
		//Check which button was clicked.
		checkButtons();
		
	}
	
	//used to check which button was pressed.
	private void checkButtons()
	{
		Button b = buttonList.get(TILE);
		if (buttonHovered(b))
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
				b.setCurrentColor(b.getCOLOR_ON());
			else
				b.setCurrentColor(b.getCOLOR_OFF());
			//System.out.println("Clicked Tile");
			
		}
		
		b = buttonList.get(INVERT);
		if (buttonHovered(b))
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
			{
				b.setCurrentColor(b.getCOLOR_ON());
				level.invertLevel();
//				player.setPosX(shadow.posX());
//				player.setPosY(shadow.posY());
			}
			else
				b.setCurrentColor(b.getCOLOR_OFF());
			//System.out.println("Clicked Invert");
		}
		
		b = buttonList.get(TEST);
		if (buttonHovered(b))
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
				b.setCurrentColor(b.getCOLOR_ON());
			else
				b.setCurrentColor(b.getCOLOR_OFF());
			//System.out.println("Clicked Test");
		}
		
		b = buttonList.get(START);
		if (buttonHovered(b))
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
				b.setCurrentColor(b.getCOLOR_ON());
			else
				b.setCurrentColor(b.getCOLOR_OFF());
			//System.out.println("Clicked Start");
		}
		
		b = buttonList.get(EXIT);
		if (buttonHovered(b))
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
				b.setCurrentColor(b.getCOLOR_ON());
			else
				b.setCurrentColor(b.getCOLOR_OFF());
			//System.out.println("Clicked Exit");
		}
		
		b = buttonList.get(COLOR);
		if (buttonHovered(b))
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
				b.setCurrentColor(b.getCOLOR_ON());
			else
				b.setCurrentColor(b.getCOLOR_OFF());
			//System.out.println("Clicked Color");
		}
		
		b = buttonList.get(DEBUG);
		if (buttonHovered(b))		//debug button
		{
			b.setButtonOn(!b.isButtonOn());
			if (b.isButtonOn())
				b.setCurrentColor(b.getCOLOR_ON());
			else
				b.setCurrentColor(b.getCOLOR_OFF());	
		}
	}
}
