/* This class creates buttons for use in the editor. Buttons operate as such:
 * By default, buttons are off, and shown as half transparent.
 * When the mouse hovers on a button, the button gradually becomes fully opaque. When the mouse leaves, it goes transparent again.
 * Clicking on a button changes its colour to indicate that it's on. While on, the button is opaque.
 */

package inversioneditorpkg;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.image.ImageObserver;
import java.net.URL;

public class Button
{
	private boolean buttonOn;	//if true, button is clicked.
	private URL iconUrl;		//the location of a button icon
	private Image icon;		//button icon
	private String urlString;
	private final Color COLOR_ON = new Color(255, 0, 0);
	private final Color COLOR_OFF = new Color(38, 132, 255);	//light blue
	private final Color COLOR_HOVER = new Color(0, 220, 220);	//yellow
	private int buttonX, buttonY;
	private Color currentColor;		//alternates between on and off.
	private int width, height;
	
	public Button(Image icon, int buttonX, int buttonY, boolean buttonOn)
	{
		this.icon = icon;
		this.buttonOn = buttonOn;
		this.buttonX = buttonX;
		this.buttonY = buttonY;
		currentColor = COLOR_OFF;
		
		width = 40;
		height = 40;
	}
	
	
	//updates the button state, including color and transitioning from transparent to opaque
	public void update()
	{
		
	}
	
	public void paint(Graphics g, ImageObserver observer)
	{
		g.setColor(currentColor);
		g.fillRoundRect(buttonX, buttonY, width, height, 40, 20);
		g.drawImage(icon, buttonX + width / 4, buttonY + height / 4, icon.getWidth(observer), icon.getHeight(observer), 
				currentColor, observer);
	}
	
	/********Getters/Setters********/
	public boolean isButtonOn() {
		return buttonOn;
	}

	public void setButtonOn(boolean buttonOn) {
		this.buttonOn = buttonOn;
	}

	public URL getIconUrl() {
		return iconUrl;
	}

	public void setIconUrl(URL iconUrl) {
		this.iconUrl = iconUrl;
	}

	public Image getIcon() {
		return icon;
	}


	public void setIcon(Image icon) {
		this.icon = icon;
	}

	public String getUrlString() {
		return urlString;
	}

	public void setUrlString(String urlString) {
		this.urlString = urlString;
	}

	public Color getCOLOR_ON() {
		return COLOR_ON;
	}

	public Color getCOLOR_OFF() {
		return COLOR_OFF;
	}


	public Color getCOLOR_HOVER() {
		return COLOR_HOVER;
	}


	public Color getCurrentColor() {
		return currentColor;
	}


	public void setCurrentColor(Color currentColor) {
		this.currentColor = currentColor;
	}


	public int getButtonX() {
		return buttonX;
	}


	public void setButtonX(int buttonX) {
		this.buttonX = buttonX;
	}


	public int getButtonY() {
		return buttonY;
	}


	public void setButtonY(int buttonY) {
		this.buttonY = buttonY;
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

}
