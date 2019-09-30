/* Train Dodge involves the player avoiding an incoming train as late as possible. The later
they avoid the train, the more points they get. Getting hit by the train is a game over.
The train will appear at random times, and it comes in FAST. 


Becaue this is a prototype, the train will be a large rectangle, and the player a small rect.*/

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
    import flash.display.JointStyle;
    import flash.display.LineScaleMode;
    import flash.display.Shape;
    import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	public class Main extends MovieClip
	{
		var _player:Shape = new Shape();
		var _train:Shape = new Shape();
		var _pColor:uint = 0x00F000;
		var _tColor:uint = 0x000044;
		
		
		var _score:int = 0;				//score
		var _gameplayOn:Boolean = true;	//if false, goes to score screen.
		var _timer:int;					//once this reaches 0, train starts coming
		var _timerSet:Boolean = false;			//controls timer in update loop
		var _flashInterval:int;			//used to flash the indicator text
		var _intervalCopy:int;	//used to reset interval
		var _cooldown:int;		//used to delay the time it takes for indicator to appear
			
		
		var _indicator:String;		//warning indicator
		var _indicatorFormat:TextFormat; 
		var _indicatorText:TextField;

		public function Main() 
		{
			//create player
			_player.graphics.beginFill(_pColor);
			_player.graphics.drawRect(500, 600, 50, 50);
			_player.graphics.endFill();
			
			//create train. The train should always start off the screen
			_train.graphics.beginFill(_tColor);
			_train.graphics.drawRect(370, -500, 300, 500);
			_train.graphics.endFill();
			
			//indicator information
			//_indicator = "DING";
			_indicatorFormat = new TextFormat();
			_indicatorFormat.size = 50;
			_indicatorFormat.bold = true;
			_indicatorFormat.font = "Calibri";
			
			_indicatorText = new TextField();
			_indicatorText.defaultTextFormat = _indicatorFormat;
			_indicatorText.textColor = 0xFF0000;
			_indicatorText.width = 120;
			_indicatorText.height = 80;
			_indicatorText.x = 200;
			_indicatorText.y = 0;
			_indicatorText.text = "DING";	//this will flash in intervals
			
			
			
			addChild(_player);
			addChild(_train);
			addChild(_indicatorText);
			
			//event listeners
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			//stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public function onKeyDown(event:KeyboardEvent):void
		{
			//the player can only move left or right. Once a button is pressed, the game is over
			//and the score is calculated.
			if (event.keyCode == Keyboard.LEFT && _gameplayOn)
			{
				_player.x -= 200;
			}
			
			if (event.keyCode == Keyboard.RIGHT && _gameplayOn)
			{
				_player.x += 200;
			}
			
			//game is over once button is pressed
			_gameplayOn = false;

		}
		
		
		function onEnterFrame(event:Event):void
		{
			/* The train will appear at random times, but there will be an indicator
			that alerts the player that the train is coming. The indicator will flash;
			the quicker it flashes, the sooner the train arrives.  The later the player moves,
			the more points is awarded. Basically, the game checks the distance between 
			the train and the player. If the player moves before the train is on screen,
			no points are awarded.*/
			
			
			switch (_gameplayOn)
			{
				case true:	//game is running
				{
					if (!_timerSet)
					{
						//set timer to random number between 5 and 15 seconds,
						//or 300 and 900 frames
						_timer = Math.floor(Math.random() * 600) + 300;
						_timerSet = true;
						
						//depending on the timer, the flash interval will be higher
						//or lower. 
						if (_timer <= 300)
							_flashInterval = 30;
						else if (_timer <= 500)
							_flashInterval = 90;
						else if (_timer <= 700)
							_flashInterval = 150;
						else if (_timer <= 900)
							_flashInterval = 210;
							
						_intervalCopy = _flashInterval;
						
					}
					else
					{
						_timer--;
						_flashInterval--;
						_cooldown--;
						
						//if cooldown > 0, then the indicator disappears
						if (_cooldown < 0)
						{
							_cooldown = 0;
							_indicatorText.visible = true;
						}
						else
						{
							_indicatorText.visible = false;
						}
						
						//set cooldown so the indicator disappears momentarily
						if (_flashInterval < 0)
						{
							_cooldown = 15;
							//_flashInterval = _intervalCopy;
							
							//check the timer and decrease interval as the train approaches
							if (_timer <= 100)
								_flashInterval = 20;
							else if (_timer <= 300)
								_flashInterval = 30;
							else if (_timer <= 500)
								_flashInterval = 90;
							else if (_timer <= 700)
								_flashInterval = 150;
							else if (_timer <= 900)
								_flashInterval = 210;
							
							//trace("Copy: " + _intervalCopy);
						}
							
							
						//trace("Interval: " + _flashInterval);
						
						
						
						if (_timer < 0)
						{
							//train begins moving; increase its Y value
							_timer = 0;
							_train.y += 35;
							trace ("Train distance: " + _train.y);
							
							//if the Y values of the train and the player intersect,
							//then there's collision.
							if (_train.y > 600)
								_gameplayOn = false;
						}
					}
				}
				
				case false:	//calculate score
				{
					
				}
			}
		}

	}
	
}
