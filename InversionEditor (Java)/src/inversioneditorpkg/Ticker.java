/* Ticker is used to display context-sensitive information at the bottom of the screen. It
 * tells you what each button does. Every time a message is set, the message starts back at the centre.
 */

package inversioneditorpkg;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;

public class Ticker 
{
	private int xPos, yPos;			//ticker box position
	private int stringX, stringY;	//ticker message position
	private int width, height;
	private Font tickerFont;
	private String msg;
	private boolean isMsgSet;		//if true, then scroll timer is set.
	private int scrollDelayTimer;
	
	public Ticker()
	{
		xPos = 0;
		yPos = 745;
		width = 1024;
		height = 20;
		stringX = width / 4;
		stringY = yPos + 15;
		tickerFont = new Font("Calibri", Font.BOLD, 17);
		msg = "";
		isMsgSet = false;
		scrollDelayTimer = 0;
	}
	
	public void update()
	{
		if (msg != "")
		{
			if (scrollDelayTimer++ > 70)
				scrollDelayTimer = 70;
		}
		
		if (scrollDelayTimer >= 70)
		{
			//start scrolling message
			stringX--;
			//System.out.println((msg.length()));
			if (stringX + msg.length() < xPos - (msg.length() * 6))
				stringX = 1024;
		}
	}
	


	public void paint(Graphics g, String msg)
	{
		g.setColor(new Color(100, 100, 100, 100));  //dark gray
		g.fillRect(xPos, yPos, width, height);		//ticker background
		g.setColor(Color.WHITE);
		g.setFont(tickerFont);
		g.drawString(msg, stringX, stringY);
	}
	/***********************************************************************/

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

	public int getWidth() {
		return width;
	}

	public void setWidth(int width) {
		this.width = width;
	}

	public int getHeight() {
		return height;
	}

	public void setHeight(int height) {
		this.height = height;
	}

	public Font getTickerFont() {
		return tickerFont;
	}

	public void setTickerFont(Font tickerFont) {
		this.tickerFont = tickerFont;
	}

	public int getStringX() {
		return stringX;
	}

	public void setStringX(int stringX) {
		this.stringX = stringX;
	}

	public int getStringY() {
		return stringY;
	}

	public void setStringY(int stringY) {
		this.stringY = stringY;
	}

	public String getMsg() {
		return msg;
	}

	public void setMsg(String msg) 
	{
		this.msg = msg;
		if (!isMsgSet)
		{
			stringX = width / 4;
			isMsgSet = true;
			scrollDelayTimer = 0;
		}
	}
	
	public boolean isMsgSet() {
		return isMsgSet;
	}

	public void setMsgSet(boolean isMsgSet) {
		this.isMsgSet = isMsgSet;
	}

	public int getScrollDelayTimer() {
		return scrollDelayTimer;
	}

	public void setScrollDelayTimer(int scrollDelayTimer) {
		this.scrollDelayTimer = scrollDelayTimer;
	}
}
