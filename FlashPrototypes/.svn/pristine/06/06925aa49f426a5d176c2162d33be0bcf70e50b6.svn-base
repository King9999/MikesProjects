/* Trampoline is a simple game where you must keep a ball in the air for as long as possible by using a
trampoline. */

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
						
		
		//physics
		const FRICTION:Number = 0.90;	//used to slow down and stop movement
		var _frictionLevel:Number;		//1 if moving, otherwise it's 0.9
		
		const GRAVITY:Number = 0.1;
		var _gravityOn:Boolean = true;
		
		//ball variables
		var _ball:Sprite;
		var _ballVx:Number = 0;
		var _ballVy:Number;
		var _bounceVy:Number;	//bounce height
		var _direction:Number = 1;	//controls horizontal distance of ball.
		
		//trampoline vars
		var _trampoline:Sprite;	//player-controlled
		var _trampVx:Number;
		var _trampVy:Number;
		
				
		//HUD text
		var _textFormat:TextFormat;
		var _instructionText:TextField;	//tells player what must be done

		public function Main() 
		{
			_ballVy = 1;
			_ballVx = 0;
			_bounceVy = -2;
			
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
			_instructionText.text = "Keep the ball in the air using the trampoline!";
			
									
			_ball = new Sprite();			
			_ball.graphics.beginFill(0xff0000);
			_ball.graphics.drawCircle(Math.floor(Math.random() * 880) + 20, 0, 20);
			_ball.graphics.endFill();
			addChild(_ball);
			
			//trampoline
			_trampoline = new Sprite();
			_trampoline.graphics.beginFill(0x0000ff);
			_trampoline.graphics.drawRect(300, 600, 100, 10);
			
			stage.addEventListener(KeyboardEvent.KEY_DOWN, keyPressed);
			stage.addEventListener(KeyboardEvent.KEY_UP, keyReleased);
			addChild(_trampoline);	
			
			
			//add everything
			addChild(_instructionText);
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		function keyPressed(event:KeyboardEvent):void
		{
			//no friction applied
			_frictionLevel = 1;
			
			switch(event.keyCode)
			{
				case Keyboard.LEFT:
					_trampVx = -3;
					break;
					
				case Keyboard.RIGHT:
					_trampVx = 3;
					break;
			}
		}
				
		function keyReleased(event:KeyboardEvent):void
		{
			if (event.keyCode == Keyboard.LEFT || event.keyCode == Keyboard.RIGHT)
			{
				//_trampVx = 0;
				_frictionLevel = FRICTION;
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			/* The ball is constantly in motion and always begins from top of screen*/
			
			
			//apply physics
			_trampVx *= _frictionLevel;
			
			
			_ballVy += GRAVITY;
			if (_ballVy > 10)
			{
				_ballVy = 10;
			}
			
			
			_trampoline.x += _trampVx;
			
			
			if (_ball.hitTestObject(_trampoline))
			{
				_ballVy += _bounceVy;
				_ballVx = 3 * _direction;
			}
			
			//check boundaries
			if (_ball.y > stage.stageHeight + 50)
			{
				_ball.y = -10;
			}
			
			if (_ball.x > stage.stageWidth || _ball.x < 0)
			{
				_direction *= -1;
				_ballVx *= _direction;
			}
			
			
			_ball.y += _ballVy;
			_ball.x += _ballVx;
			trace (_ball.x);
			
			
			
		}

	}
	
}
