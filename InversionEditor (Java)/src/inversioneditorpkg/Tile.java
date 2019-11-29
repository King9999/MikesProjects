/* Provides an object for the player to stand on, and also serves as the background.  Tiles can have a single colour
 * only.
 */

package inversioneditorpkg;

import java.awt.Color;
import java.awt.Graphics;

public class Tile 
{
	private int xPos, yPos;
	private Color color;
	private boolean isForeground;	//if true, this tile is walkable
	
	public Tile(Color color, boolean isForeground)
	{
		this.color = color;
		this.isForeground = isForeground;
	}
	
	public void paint(Graphics g, int x, int y)
	{
		g.setColor(color);
		g.fillRect(x, y, 32, 32);
	}
	
	
	/********************************/
	public int getxPos() {
		return xPos;
	}
	public void setxPos(int xPos) {
		this.xPos = xPos;
	}
	public int getyPos() {
		return yPos;
	}
	public void setyPos(int yPos) {
		this.yPos = yPos;
	}
	public Color getColor() {
		return color;
	}
	public void setColor(Color color) {
		this.color = color;
	}
	public boolean isForeground() {
		return isForeground;
	}
	public void setForeground(boolean isForeground) {
		this.isForeground = isForeground;
	}
	
}
