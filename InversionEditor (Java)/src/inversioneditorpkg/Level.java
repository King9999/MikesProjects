/* Players can interact with any objects in this class.  Levels come in three sizes: small (single screen),
 * medium (two screens, either horizontal or vertical), and large (four screens, horizontal & vertical).
 * 
 * Levels consist of 2D arrays, and each object is represented by an ID from 0 to 127. The following
 * are what the IDs represent:
 * 0 = passable tile (i.e, background)
 * 1 = walkable tile (foreground)
 * 2 = checkpoint
 * 3 = spike (instant death)
 * 
 * When one level is created, an inverted version is automatically generated, with the 1s becoming 0s and vice versa.
 * When a level is inverted, the 0 becomes a BLOCKED tile, while 1 is PASSABLE.
 */

package inversioneditorpkg;

import java.awt.Color;
import java.awt.Graphics;
import java.util.ArrayList;

public class Level 
{
	private final byte MAX_TILES = 2;
	private final byte FOREGROUND = 0;	//used to choosing which tile gets drawn
	private final byte BACKGROUND = 1;	//used to choosing which tile gets drawn
	private Tile[] tile = new Tile[MAX_TILES];
	private final byte TILESIZE = 32;
	private ArrayList<CollisionBox> boxList;	//holds locations of all collision boxes in level.
	private ArrayList<CollisionBox> invBoxList;	//collision boxes for the inverse level
	private ArrayList<CollisionSlope> slopeList;
	
	//level variables
	private boolean levelInverted;		//determines which version of the map to draw on screen.
	private boolean tileOnePassState, tileTwoPassState;	//checks if player can pass through a block. true means they can
	private int row, col;
	private byte[][] normalLevel;
	private byte[][] inverseLevel;
	private byte[][] currentLevel;		//used to draw either the normal or inverse level to screen
	private Color[] levelColors;		//contains colours for foreground and background. Max is 2.
	
	public enum LevelSize 
	{
		SMALL, MEDIUM_H, MEDIUM_V, LARGE;
	}
	
	
	LevelSize levelSize;			//tracks what level was selected.
	
	public Level(LevelSize levelSize)
	{
		levelColors = new Color[MAX_TILES];
		levelColors[FOREGROUND] = new Color(22, 131, 100);
		levelColors[BACKGROUND] = new Color(255, 203, 15);
		levelInverted = false;
		tileOnePassState = true;
		tileTwoPassState = false;
		
		//set up tiles
		tile[FOREGROUND] = new Tile(levelColors[FOREGROUND], tileOnePassState);		//light green
		tile[BACKGROUND] = new Tile(levelColors[BACKGROUND], tileTwoPassState);	//yellowish
		boxList = new ArrayList<CollisionBox>();
		invBoxList = new ArrayList<CollisionBox>();
		//slopeList = new ArrayList<CollisionSlope>();
		
		//levels are generated based on the parameter given.
		this.levelSize = levelSize;
		switch(this.levelSize)
		{
		case SMALL:
			row = 24;	//screen width divided by tile width
			col = 32;	//screen height divided by tile height
			break;
		}
		
		//set up the new level
		normalLevel = new byte[row][col];
		inverseLevel = new byte[row][col];
		currentLevel = new byte[row][col];
		
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (i < row / 2)
				{
					int lastRowIndex = row - 1;
					int lastColIndex = col - 1;
					normalLevel[i][j] = 0;
					inverseLevel[i][j] = 1;
					//invBoxList.add(new CollisionBox(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE));
					//invBoxList.add(new CollisionBox((lastColIndex - j) * TILESIZE, (lastRowIndex - i) * TILESIZE, TILESIZE, TILESIZE));
				}
				else
				{
					normalLevel[i][j] = 1;
					inverseLevel[i][j] = 0;
					//boxList.add(new CollisionBox(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE));
					//slopeList.add(new CollisionSlope(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE));
				}
				
				//generate collision boxes.
				if (normalLevel[i][j] == 1)
					boxList.add(new CollisionBox(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE));
				if (inverseLevel[i][j] == 0)	//remember, this array is inverted, so 0 is a blocked tile!
					invBoxList.add(new CollisionBox(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE));
				
				System.out.print(inverseLevel[i][j]);
			}
			System.out.println("");
		}
		
		//copy normalLevel array to currentLevel. must use clone(), otherwise both arrays will share the same
		//reference.
		currentLevel = normalLevel.clone();
		
		
		//generate the inverse level
		/*int lastRowIndex = row - 1;
		int lastColIndex = col - 1;
		
		System.out.println("Inverse Level:");
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (i <= row / 2)
				{
					inverseLevel[i][j] = 0;	//starting from index [0][0] and moving up from there
					invBoxList.add(new CollisionBox((lastColIndex - j) * TILESIZE, (lastRowIndex - i) * TILESIZE, TILESIZE, TILESIZE));
				}
				else
				{
					inverseLevel[i][j] = 1;
					//slopeList.add(new CollisionSlope(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE));
				}
				System.out.print(inverseLevel[i][j]);
			}
			System.out.println("");
		}*/
	}
	
	/* Changes the level.  Level is flipped upside down, and the background becomes the foreground. */
	public void invertLevel()
	{
		levelInverted = !levelInverted;
		tileOnePassState = !tileOnePassState;
		tileTwoPassState = !tileTwoPassState;
		
		Color foregroundColor, backgroundColor;
		if (!levelInverted)
		{
//			foregroundColor = levelColors[FOREGROUND];
//			backgroundColor = levelColors[BACKGROUND];
			//currentLevel = new byte[row][col]; 
			currentLevel = normalLevel.clone();
		}
		else
		{
//			foregroundColor = levelColors[BACKGROUND];
//			backgroundColor = levelColors[FOREGROUND];
			
			int lastRowIndex = row - 1;
			int lastColIndex = col - 1;
			
			currentLevel = inverseLevel.clone();
//			for (int i = 0; i < row; i++)
//				for (int j = 0; j < col; j++)	
//					currentLevel[i][j] = inverseLevel[lastRowIndex - i][lastColIndex - j];	
		}
		
//		tile[FOREGROUND] = new Tile(foregroundColor, tileOnePassState);
//		tile[BACKGROUND] = new Tile(backgroundColor, tileTwoPassState);
		
		//draw the maps as a test
		//checking to see what's being drawn in the array.
		String output = "";
		String output2 = "";
		
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{	
				output += normalLevel[i][j];
				output2 += inverseLevel[i][j];
			}
			output += "\n";
			output2 += "\n";
		}
		System.out.println("Normal Level");
		System.out.println(output);
		System.out.println("Inverse Level");
		System.out.println(output2);
		
	}
	
	public void paint(Graphics g)
	{
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (currentLevel[i][j] == 0)
					tile[BACKGROUND].paint(g, j * TILESIZE, i * TILESIZE);
				else if (currentLevel[i][j] == 1)
					tile[FOREGROUND].paint(g, j * TILESIZE, i * TILESIZE);	//note the i and j
			}
		}
		//draw tiles based on whether the level is inverted.
		/*if (!levelInverted)
		{
			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < col; j++)
				{
					if (currentLevel[i][j] == 0)
						tile[BACKGROUND].paint(g, j * TILESIZE, i * TILESIZE);
					else if (currentLevel[i][j] == 1)
						tile[FOREGROUND].paint(g, j * TILESIZE, i * TILESIZE);	//note the i and j
				}
			}

		}
		else	//level is inverted
		{
			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < col; j++)
				{
					if (currentLevel[i][j] == 0)
						tile[BACKGROUND].paint(g, j * TILESIZE, i * TILESIZE);
					else if (currentLevel[i][j] == 1)
						tile[FOREGROUND].paint(g, j * TILESIZE, i * TILESIZE);	//note the i and j
				}
			}
		}*/
		
		//draw grid over the level for easier placement of tiles
		//drawGrid(g);
	}
	
	//used to place collision boxes
	public void update()
	{
		/*for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (normalLevel[i][j] == '1')
					tile[FOREGROUND].paint(g, j * TILESIZE, i * TILESIZE);	//note the i and j
			}
		}*/
	}
	
	public ArrayList<CollisionBox> getBoxList() {
		return boxList;
	}

	public ArrayList<CollisionBox> getInvBoxList() {
		return invBoxList;
	}

	public ArrayList<CollisionSlope> getSlopeList() {
		return slopeList;
	}

	private void drawGrid(Graphics g)
	{
		g.setColor(new Color(0, 0, 0, 100));
		for (int i = 0; i < row; i++)
			for (int j = 0; j < col; j++)
				g.drawRect(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE);
	}
	
	public void drawCollision(Graphics g)
	{
		if (!levelInverted)
		{
			for (CollisionBox c : boxList)
				c.paint(g);
		}
		else
		{
			for (CollisionBox c : invBoxList)
				c.paint(g);
		}
	
	}
	
	public void drawTile(byte tileType, int mouseX, int mouseY)
	{
		//using the mouse position, we can get the index in the level array that needs to change.
		int rowPos = mouseY / TILESIZE;
		int colPos = mouseX / TILESIZE;
		
		final byte PASSABLE = 0;
		final byte BLOCK = 1;
		
		//ensure row and col don't go out of bounds
		if (rowPos > row)
			rowPos = row;
		if (colPos > col)
			colPos = col;
		
		normalLevel[rowPos][colPos] = tileType;
		
		int lastRowIndex = row - 1;
		int lastColIndex = col - 1;
		if (tileType == BLOCK)
		{
			boxList.add(new CollisionBox(colPos * TILESIZE, rowPos * TILESIZE, TILESIZE, TILESIZE));
			inverseLevel[lastRowIndex - rowPos][colPos] = 1;	//passable tile in inverse world
			
			//remove any collision boxes in the inverse level that needs to be removed.
			int index = 0;
			boolean boxFound = false;
			while (!boxFound && index < invBoxList.size())
			{
				CollisionBox box = invBoxList.get(index);
				if (box.xPos == colPos * TILESIZE && box.yPos == (lastRowIndex - rowPos) * TILESIZE)
					boxFound = true;
				else
					index++;
				
			}
			if (boxFound)
				invBoxList.remove(index);
			/*for (CollisionBox box : invBoxList)
			{
				if (box.xPos == colPos * TILESIZE && box.yPos == (lastRowIndex - rowPos) * TILESIZE)
					invBoxList.remove(invBoxList.get(index));
				else
					index++;
			}*/
			//inverseLevel[rowPos][colPos] = 1;
		}
		else if (tileType == PASSABLE)
		{
			//inverseLevel[rowPos][colPos] = 0;	//blocked tile in the inverse world
			inverseLevel[lastRowIndex - rowPos][colPos] = 0;
			invBoxList.add(new CollisionBox(colPos * TILESIZE, (lastRowIndex - rowPos) * TILESIZE, TILESIZE, TILESIZE));
		}
		
	
	}

	public boolean isLevelInverted() {
		return levelInverted;
	}

	public void setLevelInverted(boolean levelInverted) {
		this.levelInverted = levelInverted;
	}
}
