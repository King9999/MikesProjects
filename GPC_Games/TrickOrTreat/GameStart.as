/* When the game starts, the players must select their costume.  The Main class will take
their selection as parameters. */

package  
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.display.Sprite;
	import flash.sampler.Sample;
	
	
	public class GameStart extends MovieClip
	{
		private var _selectScreen:SelectScreen = new SelectScreen;

		public function GameStart() 
		{
			//gotoAndStop(1, "Select Screen");
			//addChild(_selectScreen);
			
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public function onKeyDown(event:KeyboardEvent):void
		{
			//move candy cursor left. Position it to the last character if it can't go left any further.
			if (event.keyCode == Keyboard.LEFT)
			{
				_selectScreen.IterateLeft();
			}
				
			//move candy cursor right. Position it to the first character if it can't go right any further.
			if (event.keyCode == Keyboard.RIGHT)
			{
				_selectScreen.IterateRight();
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			/* The select screen will run continuously until the space bar is pressed. 
			The information on the screen will update if the left and right keys are pressed. */
			
			_selectScreen.Update();
		}

	}
	
}
