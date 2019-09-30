/* Lunar Ball is a pool game with a twist. There are walls that will prevent the player from making
precise shots.  Also, the pockets won't necessarily be in the corners. */

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
	import flash.events.MouseEvent;
	
	
	public class Main extends MovieClip
	{
						
		//consts to determine maximum objects on screen
		const MAX_BALLS:int = 8;
		const MAX_POCKETS:int = 6;
		const MAX_WALLS:int = 3;
		
		//physics
		const FRICTION:Number = 0.70;	//used to slow down and stop movement
		
		
		
		var _ball:Vector.<Sprite> = new Vector.<Sprite>();		//pool balls
		var _pocket:Vector.<Sprite> = new Vector.<Sprite>();	//pockets
		var _wall:Vector.<Sprite> = new Vector.<Sprite>();		//blocks balls
		var _cue:Sprite;										//used to hit balls
		
				
		//HUD text
		var _textFormat:TextFormat;
		var _instructionText:TextField;	//tells player what must be done

		public function Main() 
		{
			
			
			_textFormat = new TextFormat();
			_textFormat.size = 50;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			

			//instruction
			_instructionText = new TextField();

			_instructionText.textColor = 0x000000;
			_instructionText.defaultTextFormat = _textFormat;
			_instructionText.x = 100;
			_instructionText.y = 50;
			_instructionText.width = 800;
			_instructionText.height = 150;
			_instructionText.multiline = true;
			_instructionText.wordWrap = true;
			_instructionText.text = "Hit the balls using the cue.";
			
			//pool cue
			_cue = new Sprite();
			_cue.graphics.beginFill(0x0000cc);
			_cue.graphics.drawRect(100, 500, 20, 90);
			_cue.graphics.endFill();
			
			_cue.addEventListener(MouseEvent.MOUSE_DOWN, buttonClicked);
			_cue.addEventListener(MouseEvent.MOUSE_UP, buttonReleased);
			
						
			//create 6 pockets
			for (var k = 0; k < MAX_POCKETS; k++)
			{
				_pocket.push(new Sprite());
				_pocket[k].graphics.beginFill(0x333333);
				_pocket[k].graphics.drawCircle(Math.floor(Math.random() * 850) + 50,
										   Math.floor(Math.random() * 650) + 50, 22);
				_pocket[k].graphics.endFill();
				addChild(_pocket[k]);
			}
			
			//create 8 balls and display them randomly on the screen
			for (var i = 0; i < MAX_BALLS; i++)
			{
				_ball.push(new Sprite());
				_ball[i].graphics.beginFill(0xff0000);
				_ball[i].graphics.drawCircle(Math.floor(Math.random() * 880) + 20,
											 Math.floor(Math.random() * 680) + 20, 20);
				_ball[i].graphics.endFill();
				addChild(_ball[i]);
			}
			
			//create 3 walls
			for (var j = 0; j < MAX_WALLS; j++)
			{
				_wall.push(new Sprite());
				_wall[j].graphics.beginFill(0x00ff00);
				_wall[j].graphics.drawRect(Math.floor(Math.random() * 850) + 50,
										   Math.floor(Math.random() * 650) + 50, 10, 70);
				_wall[j].graphics.endFill();
				addChild(_wall[j]);
			}
			
			
			
			//add everything
			addChild(_instructionText);
			addChild(_cue);
			
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
		
		function onEnterFrame(event:Event):void
		{
			/* The game will check for collision between the following;
			-cue and balls
			-cue and wall
			-balls with each other
			-balls and wall
			-balls and pocket */
			
			for (var i = 0; i < MAX_BALLS; i++)
			{
				if (_cue.hitTestObject(_ball[i]))
				{
					//send the ball flying in the same direction as the cue
					_ball[i].x++;
				}
			}
		}

	}
	
}
