/* The HUD displays health bars and the charge meter */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;	
	import flash.geom.Rectangle;
	
	public class HUD extends MovieClip
	{
		var lifebarWidth:Number = 120;
		var redbarWidth:Number = 120;
		var blackbarWidth:Number = 120;
		
		var lifebar:Rectangle;
		var redbar:Rectangle;
		var blackbar:Rectangle;

		public function HUD() 
		{
			// constructor code
			lifebar = new Rectangle(50, 0, lifebarWidth, 20);
			redbar = new Rectangle(50, 0, redbarWidth, 20);
			blackbar = new Rectangle(50, 0, blackbarWidth, 20);
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}

	}
	
}
