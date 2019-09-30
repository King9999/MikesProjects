/* God vs. God is a two-player game where players compete to be the one true god by eliminating their opponent.
Winning means destroying the humans who believe in the other god; a god is only as strong as the people 
perceive him to be. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
    import flash.display.Shape;
    import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.media.Sound;
	import flash.media.SoundChannel;
	import flash.net.URLRequest;
	import flash.media.SoundTransform;
	
	
	//set up stage.  I'm using 30 FPS because this is going on the Web.
	[SWF(width="1024", height="768", backgroundColor="#000000", frameRate="30")]
	
	
	
	
	public class Main extends MovieClip
	{
		var _timer:Clock;	//time remaining before combat begins
		var _timerText:TextField;
		var _timerFormat:TextFormat;
		var _secs:int = 10;
		
		var turn:int = 1;		//rounds
		
		//players
		var player:Vector.<God> = new Vector.<God>();
		
		//sound variables
		//var _gameMusic:Sound = new Sound(new URLRequest("Defusethebomb.mp3"));	//Skullgirls tutorial theme
		//var _musicChannel:SoundChannel = new SoundChannel();
		//var _volume:SoundTransform = new SoundTransform(0.5);	//sets volume to 50%
				
		//HUD 
		var _instructionText:TextField;	//tells player what must be done

		
		//white screen simulates an explosion
		var _whiteScreen:Shape = new Shape();
		
		//screen manager
		var _screenState:int;	//screen manager
		const MAIN_SCREEN:int = 0;	//used to select difficulty
		const GAME_SCREEN:int = 1;	//gameplay screen
		const RESULT_SCREEN:int = 2;	//results screen.
		var _menuCleared:Boolean = false;		//used to eliminate children from main menu
	
				
		/******************MAIN******************/
		public function Main() 
		{
			
			//initialize main screen
			_screenState = MAIN_SCREEN;

			
			//timer information
			_timer = new Clock(_secs);
			
			_timerFormat = new TextFormat();
			_timerFormat.size = 50;
			_timerFormat.bold = true;
			_timerFormat.font = "Calibri";
			
			_timerText = new TextField();
			_timerText.defaultTextFormat = _timerFormat;
			_timerText.textColor = 0x0000F0;
			_timerText.width = 160;
			_timerText.height = 80;
			_timerText.x = 450;
			_timerText.y = 200;
			
			//display time and append zeroes where necessary
			if (_timer.Seconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			
			
			//instruction
			_instructionText = new TextField();

			_instructionText.textColor = 0x000000;
			_instructionText.defaultTextFormat = _timerFormat;
			_instructionText.x = 100;
			_instructionText.y = 50;
			_instructionText.width = 800;
			_instructionText.height = 350;
			_instructionText.multiline = true;
			_instructionText.wordWrap = true;
			_instructionText.selectable = false;
			
			
			
			
			//white screen
			_whiteScreen.graphics.beginFill(0xffffff);
			_whiteScreen.graphics.drawRect(0, 0, stage.stageWidth, stage.stageHeight);
			_whiteScreen.alpha = 0;
			_whiteScreen.graphics.endFill();
			
			
			//add instructions.  Will be displayed on all screens
			addChild(_instructionText);
			
			//add white screen.
			stage.addChild(_whiteScreen);
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
		}
		
		
		
		function mouseClicked(event:MouseEvent):void
		{
			
			switch(event.target)
			{
				
			}
			//change screen state
			_screenState = GAME_SCREEN;
			
			//play music
			//_musicChannel = _gameMusic.play();
			//_musicChannel.soundTransform = _volume;
			
		}
		
		function ExitResultScreen(event:MouseEvent):void
		{
			
			
			stage.removeEventListener(MouseEvent.MOUSE_UP, ExitResultScreen);
		}
		
		function onKeyDown(event:KeyboardEvent):void
		{
			
			
			switch (event.keyCode)
			{
				case Keyboard.ENTER:
					break;
					
				case Keyboard.BACKSPACE: case Keyboard.DELETE:
					break;
					
			}
		}
		
		
		
		function onEnterFrame(event:Event):void
		{
			
			switch (_screenState)
			{
				case MAIN_SCREEN:
				{
					
					break;
				}
				
				case GAME_SCREEN:
				{
					
					break;
				}
				
				case RESULT_SCREEN:	//show results
					ShowResults();
					break;			
			}
		}
		
		//update game screen
		function Update():void
		{
				
			//update timer display
			if (_timer.Seconds() < 10 && _timer.Milliseconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":0" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() < 10 && _timer.Milliseconds() >= 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() >= 10 && _timer.Milliseconds() >= 10)
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() >= 10 && _timer.Milliseconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":0" + _timer.Milliseconds();
			}
		}
		
		//reset function
		function ResetGame():void
		{
			
			/* send player back to main menu and reset all variables */
			_screenState = MAIN_SCREEN;
			_menuCleared = false;
			
			
			//stop music
			//_musicChannel.stop();
						
				
		}
		
		//results screen
		function ShowResults():void
		{
			
			stage.addEventListener(MouseEvent.MOUSE_UP, ExitResultScreen);
		
		}
		
		//makes the screen flash white. Should go into the update() function.
		function Flash():void
		{

			_whiteScreen.alpha += 0.05;

		}

	}
	
}
