﻿/* Block Jump is a game where you are in control of two blocks, and you use them to
move the player's avatar around the level. The blocks are moved using the mouse, and
the player moves with the directional keys. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.MouseEvent;
	import flash.events.Event;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
    import flash.display.Shape;
    import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	public class Main extends MovieClip
	{
		var _player:Shape = new Shape();
		//var _playerPoint:Point;
		var _platform:Vector.<Sprite> = new Vector.<Sprite>();
		var _pColor:uint = 0x00F000;
		var _platColor:Vector.<uint> = new Vector.<uint>();	//corresponds to number of platforms
		
		
		var _score:int = 0;				//score
		var _gameplayOn:Boolean = true;	//if false, goes to score screen.
		var _timer:int;					//once this reaches 0, train starts coming
		var _timerSet:Boolean = false;			//controls timer in update loop
		var _flashInterval:int;			//used to flash the indicator text
		var _intervalCopy:int;	//used to reset interval
		var _cooldown:int;		//used to delay the time it takes for indicator to appear
		var _vx:Number = 0;
		var _vy:Number = 0;
		var _jumpVel:Number = -20;
		
		const GRAVITY:Number = 0.4;
		const FRICTION:Number = 0.65;
		var _frictionValue:Number;
		
		var _indicator:String;		//warning indicator
		var _indicatorFormat:TextFormat; 
		var _indicatorText:TextField;

		public function Main() 
		{
			//create player
			_player.graphics.beginFill(_pColor);
			_player.graphics.drawRect(500, 300, 20, 20);
			_player.graphics.endFill();
			
			//_playerPoint = new Point(_player.x, _player.y);
			//_playerPoint = _player.localToGlobal(_playerPoint);
			//_player.x = _playerPoint.x;
			//_player.y = _playerPoint.y;
			
			//create 2 platforms.
			_platform.push(new Sprite());
			_platform.push(new Sprite());
			_platColor.push(new uint);
			_platColor.push(new uint);
			
			_platColor[0] = 0xFF0000;
			_platColor[1] = 0x0000FF;
			
			for (var i = 0; i < _platform.length; i++)
			{
				//the platforms will appear in random places.
				_platform[i].graphics.beginFill(_platColor[i]);
				_platform[i].graphics.drawRect(Math.floor(Math.random() * 900) + 100,
											   Math.floor(Math.random() * 600) + 100, 
											   100, 40);
				_platform[i].graphics.endFill();
				
				_platform[i].addEventListener(MouseEvent.MOUSE_DOWN, buttonClicked);
				_platform[i].addEventListener(MouseEvent.MOUSE_UP, buttonReleased);
				addChild(_platform[i]);
			}
			
			
			//indicator information
			
			//_indicatorFormat = new TextFormat();
//			_indicatorFormat.size = 50;
//			_indicatorFormat.bold = true;
//			_indicatorFormat.font = "Calibri";
//			
//			_indicatorText = new TextField();
//			_indicatorText.defaultTextFormat = _indicatorFormat;
//			_indicatorText.textColor = 0xFF0000;
//			_indicatorText.width = 120;
//			_indicatorText.height = 80;
//			_indicatorText.x = 200;
//			_indicatorText.y = 0;
//			_indicatorText.text = "DING";	//this will flash in intervals
			
			
			
			addChild(_player);
			
			//addChild(_indicatorText);
			
			//event listeners
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			
		}
		
		function buttonClicked(event:MouseEvent):void
		{
			event.target.startDrag();
		}
		
		function buttonReleased(event:MouseEvent):void
		{
			event.target.stopDrag();
		}
		
		public function onKeyDown(event:KeyboardEvent):void
		{
			//the player can only move left or right. Once a button is pressed, the game is over
			//and the score is calculated.
			if (event.keyCode == Keyboard.LEFT)
			{
				_vx = -4;
				_frictionValue = 1;
			}
			
			if (event.keyCode == Keyboard.RIGHT )
			{
				_vx = 4;
				_frictionValue = 1;
			}
			
			/*if (event.keyCode == Keyboard.SPACE )
			{
				_vy += _jumpVel;
			}*/
			
			//game is over once button is pressed
			_gameplayOn = false;

		}
		
		public function onKeyUp(event:KeyboardEvent):void
		{
			if (event.keyCode == Keyboard.LEFT || event.keyCode == Keyboard.RIGHT)
			{
				_frictionValue = FRICTION;
			}
						
			if (event.keyCode == Keyboard.SPACE )
			{
				_vy += _jumpVel;
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			/* Whenever the player is airborne, gravity kicks in and the player starts to fall.
			If the player goes off the screen, he appears at the opposite end.*/
			
			//apply physics
			_vy += GRAVITY;
			if (_vy > 8)
			{
				_vy = 8;
			}
			
			_vx *= _frictionValue;
			
			
			if (_player.y > stage.stageHeight)
			{
				//_playerPoint.y = 0;
				//_player.localToGlobal(_playerPoint);
				_player.y = -299;
			}
			
			if (_player.x > stage.stageWidth)
			{
				_player.x = 0;
			}
			if (_player.x < 0)
			{
				_player.x = stage.stageWidth;
			}
			
			//check if the player collides with a platform
				if (_player.hitTestObject(_platform[0]) || _player.hitTestObject(_platform[1]))
				{
					_vy += _jumpVel;
				}
				/*else
				{
					_vy = 4;
				}*/
			
			//apply movement after all other calculations
			_player.x += _vx;
			_player.y += _vy;
					
			//move the platform if it's clicked with a mouse.
			
			
			//switch (_gameplayOn)
//			{
//				case true:	//game is running
//				{
//					
//				}
//				
//				case false:	//calculate score
//				{
//					
//				}
//			}
		}

	}
	
}