/* Home Run is a game where you must fill up a meter within the time limit by mashing buttons
as quickly as you can.  Once time is up, a ball is struck and sent flying. Its distance
depends on the power meter. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.events.MouseEvent;
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
		//timer variables
		var _timer:Clock;
		var _timerText:TextField;
		var _timerFormat:TextFormat;
		var _secs:uint = 10;	//clock time
				
		var _pBarWidth:Number = 0;	//increases as buttons are pressed, decreases otherwise
		
		//physics
		const GRAVITY:Number = 0.3;
		const FRICTION:Number = 0.97;
		var _frictionLevel:Number = 0;
		var _ballVelocityX:Number = 0;
		var _ballVelocityY:Number = 0;
		var _bounceVelocity:Number = 0;	//should be 70% of the ball's Y velocity
		var _ballKicked:Boolean = false; //if true, ball is airborne
		var _ballAirborne:Boolean = false;	//used to bounce ball
		var _meters:Number = 0;				//ball distance.
		
		//graphics
		var _player:Player = new Player();
		var _ball:Ball = new Ball();
		var _sky:Sky = new Sky();
		var _skyCopy:Sky = new Sky();
		var _emptyMeter:EmptyMeter = new EmptyMeter();
		var _powerMeter:PowerMeter = new PowerMeter();	//fills up as you mash buttons
		var _ground1:Ground1 = new Ground1();
		var _ground1Copy:Ground1 = new Ground1();
		
		var _ground2:Ground2 = new Ground2();
		var _ground2Copy:Ground2 = new Ground2();
		
		var _ground3:Ground3 = new Ground3();
		var _ground3Copy:Ground3 = new Ground3();
		
		var _ground4:Ground4 = new Ground4();
		var _ground4Copy:Ground4 = new Ground4();
		
		
		//HUD text
		var _instructionText:TextField;	//tells player what must be done
		var _barLabel:TextField;		//gauge label
		var _timeLabel:TextField;		//timer label
		var _resultText:TextField = new TextField();
		
		//boundaries. If the ball hits this, then certain elements scroll to the left.
		var _boundaryLine:Shape = new Shape();
		
		//debug info
		var _debugWindow:Shape = new Shape();
		var _dbugFormat:TextFormat;
		var _dbugText:TextField = new TextField();
		
		var _gameOver:Boolean = false;

		public function Main() 
		{
			//timer information
			_timer = new Clock(_secs);
			
			_timerFormat = new TextFormat();
			_timerFormat.size = 50;
			_timerFormat.bold = true;
			_timerFormat.font = "Calibri";
			
			/*_dbugFormat = new TextFormat();
			_dbugFormat.size = 20;
			_dbugFormat.font = "Fixedsys";*/
			
			_timerText = new TextField();
			_timerText.defaultTextFormat = _timerFormat;
			_timerText.textColor = 0x0000F0;
			_timerText.width = 160;
			_timerText.height = 80;
			_timerText.x = 450;
			_timerText.y = 200;
			_timerText.selectable = false;
			
			/*_dbugText.defaultTextFormat = _dbugFormat;
			_dbugText.textColor = 0xffffff;
			_dbugText.width = 400;
			_dbugText.height = 400;
			_dbugText.multiline = true;
			_dbugText.wordWrap = true;
			_dbugText.x = 400;
			_dbugText.y = 550;*/
			
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
			_instructionText.height = 150;
			_instructionText.multiline = true;
			_instructionText.wordWrap = true;
			_instructionText.text = "Mash the A and D keys to gather power!";
			_instructionText.selectable = false;
			
			//results
			_resultText.textColor = 0x0000ff;
			_resultText.defaultTextFormat = _timerFormat;
			_resultText.x = 100;
			_resultText.y = 50;
			_resultText.width = 800;
			_resultText.height = 150;
			_resultText.multiline = true;
			_resultText.wordWrap = true;
			_resultText.visible = false;
			_resultText.selectable = false;
			
			//player
			_player.x = 100;
			_player.y = 520;
			
			//gauge
			_powerMeter.x = 55;
			_powerMeter.y = 665;
			_powerMeter.width = 0;
			
			//empty power gauge
			_emptyMeter.x = 50;
			_emptyMeter.y = 660;
			
			//boundary line. Only visible for debugging
			_boundaryLine.graphics.beginFill(0xff0000);
			_boundaryLine.graphics.drawRect(0, 0, 5, stage.stageHeight);
			_boundaryLine.x = 900;
			_boundaryLine.y = 0;
			_boundaryLine.graphics.endFill();
			
			
			//ball
			_ball.x = 180;
			_ball.y = 589;	//same position as ground1
			
			//set up the sky for scrolling
			_sky.x = 0;
			_skyCopy.x = Number(stage.stageWidth);
			
			//set up ground
			_ground1.x = 0;
			_ground1.y = 589;
			_ground1Copy.x = Number(stage.stageWidth);
			_ground1Copy.y = 589;
			
			_ground2.x = 0;
			_ground2.y = 459;
			_ground2Copy.x = Number(stage.stageWidth);
			_ground2Copy.y = 459;
			
			_ground3.x = 0;
			_ground3.y = 365;
			_ground3Copy.x = Number(stage.stageWidth);
			_ground3Copy.y = 365;
			
			_ground4.x = 0;
			_ground4.y = 297;
			_ground4Copy.x = Number(stage.stageWidth);
			_ground4Copy.y = 297;
			
			//power gauge label
			_barLabel = new TextField();
			_barLabel.defaultTextFormat = _timerFormat;
			_barLabel.text = "POWER";
			_barLabel.textColor = 0xdddd00;
			_barLabel.x = 70;
			_barLabel.y = 690;
			_barLabel.width = 200;
			_barLabel.height = 70;
			
			//debug window
			/*_debugWindow.graphics.beginFill(0x000000);
			_debugWindow.graphics.drawRect(0, 0, 400, 200);
			_debugWindow.x = 400;
			_debugWindow.y = 550;
			_debugWindow.graphics.endFill();*/
			

			//add everything
			addChild(_sky);
			addChild(_skyCopy);
			
			addChild (_ground1);
			addChild(_ground1Copy);
			
			addChild (_ground2);
			addChild(_ground2Copy);
			
			addChild (_ground3);
			addChild(_ground3Copy);
			
			addChild(_ground4);
			addChild(_ground4Copy);
			
			//addChild(_boundaryLine);
			
			addChild(_timerText);
			addChild(_instructionText);
			addChild(_resultText);
			addChild(_barLabel);
			
			
			addChild(_emptyMeter);
			addChild(_powerMeter);
			
			
			addChild(_ball);
			addChild(_player);
			
			//addChild(_debugWindow);
			//addChild(_dbugText);
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
		}
		
		
		
		public function onKeyUp(event:KeyboardEvent):void
		{
			/*when the A or D keys are pressed, the power gauge fills up slowly. The gauge 
			decreases if nothing is pressed.*/
			
			switch (event.keyCode)
			{
				case Keyboard.A: case Keyboard.D:
					if (!_timer.TimeUp())
					{
						_pBarWidth += 1.3;
						if (_pBarWidth > 200)
						{
							_pBarWidth = 200;
						}
					}
					break;
			}	
		}
		
		function onEnterFrame(event:Event):void
		{
			//update debug window
			//UpdateDebug();
			
			
			//update the sky
			ScrollImage(_sky, _skyCopy, -0.3);
			//trace ("Sky: " + _sky.x + " Sky Copy: " + _skyCopy.x);
			
			//If the ball reaches the screen edge, the ground will scroll.
			if (_ball.x > _boundaryLine.x - _ball.width - 60)
			{
				_ball.x = _boundaryLine.x - _ball.width - 60;
				
				 //The ground scrolls at different speeds to create depth. 
				ScrollImage(_ground1, _ground1Copy, -3);
				ScrollImage(_ground2, _ground2Copy, -2.7);
				ScrollImage(_ground3, _ground3Copy, -2.4);
				ScrollImage(_ground4, _ground4Copy, -2.1);
				
				//scroll the player and HUD
				_player.x -= 3;
				_timerText.x -= 3;
				_instructionText.x -= 3;
			}
			
			//the power gauge continually decreases
			if (!_timer.TimeUp())
			{
				_pBarWidth -= 0.1;
				if (_pBarWidth < 0)
				{
					_pBarWidth = 0;
				}
			}
			
			//update power gauge
			_powerMeter.width = _pBarWidth;
			
			/* Time continually decreases until it reaches 0 or player completes all words. */
			_timer.Countdown();
			UpdateTimerText();
			
			
			/* When the timer reaches 0, the ball is struck and sent flying. Calculate how far the ball
			goes based on the power */
			if (_timer.TimeUp() && !_ballKicked)
			{
				//send the ball flying. The ball will slow down as _pBarWidth decreases each frame.
				_ballVelocityX = _pBarWidth * 0.50;
				_ballVelocityY = -(_pBarWidth * 0.35);
				_bounceVelocity = _ballVelocityY * 0.60;
				_ballKicked = true;
				_ballAirborne = true;
			}
			
			
			//apply physics
			_ballVelocityY += GRAVITY;
			if (_ballVelocityY > 15)
			{
				_ballVelocityY = 15;
			}
			
			_ball.x += _ballVelocityX;
			_ball.y += _ballVelocityY;
			
			if (_ballVelocityX > 0)
			{
				_meters++;
			}
			
			
			if (_ball.y > _ground1.y)
			{
				_ball.y = _ground1.y;
				
				//check if ball was airborne
				if (_ballAirborne)
				{
					//bounce the ball
					_ballVelocityY = _bounceVelocity;
					_bounceVelocity *= 0.55;	//reduces bounce
					_frictionLevel = 1;
					
					//once the bounce reaches a certain point, ball should be grounded.
					if (_bounceVelocity > -1)
					{
						_ballAirborne = false;
					}
					
				}
				else
				{
					//ball's on the ground; apply friction
					_frictionLevel = FRICTION;
				}
				
			}
			else
			{
				_frictionLevel = 1;
			}
			
			//apply friction
			_ballVelocityX *= _frictionLevel;
			
			if (_ballVelocityX < 0.5)	//if this check isn't done, the ball moves forever.
			{
				_ballVelocityX = 0;
			}
			
			if (_ball.x > stage.stageWidth)
			{
				_ball.x = 0;
				
			}
			
			//check if game is over.  Game is over when meters is > 0 and ball is stationary.
			if (_timer.TimeUp() && _ballVelocityX == 0)
			{
				_resultText.visible = true;
				_instructionText.visible = false;
				_timerText.visible = false;
				_resultText.text = "Nice! The ball travelled " + _meters + "m!"
										+ "\nClick the mouse button to try again!";
			
				stage.addEventListener(MouseEvent.MOUSE_UP, ResetGame);
			}
			
			
		}
		
		//infinitely scrolls an image and its copy at a specified rate
		function ScrollImage(image1:DisplayObject, image2:DisplayObject, rate:Number):void
		{
			
			image1.x += rate;
			image2.x += rate;
			
			/*I check to see if x is less than the negative value of the stage width because
			the origin is at 0. The moment the left side of the sky touches the screen edge,
			the sky flips to the opposite end, leaving empty space. */
			
			if (image1.x < -image1.width)
			{
				image1.x = image1.width + rate;
			}
			
			if (image2.x < -image2.width)
			{
				image2.x = image2.width + rate;
			}
			
			
		}
		
		function UpdateDebug():void
		{
			_dbugText.text = "Ball X Vel.: " + _ballVelocityX
							+"\nBall Y Vel.: " + _ballVelocityY
							+"\nBounce Velocity: " + _bounceVelocity
							+ "\nDistance: " + _meters + "m"
							+ "\nPower: " + _pBarWidth
							+"\nFriction: " + _frictionLevel
							+"\nAirborne State: " + _ballAirborne;
							
		}
		
		function UpdateTimerText():void
		{
			if (_timer.Seconds() < 10 && _timer.Milliseconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":0" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() < 10 && _timer.Milliseconds() > 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() > 10 && _timer.Milliseconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":0" + _timer.Milliseconds();
			}
			else
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
		}
		
		function ResetGame(event:MouseEvent):void
		{
			
			//player
			_player.x = 100;
			_player.y = 520;
			
			//ball
			_ball.x = 180;
			_ball.y = 589;	//same position as ground1
			
			//set up ground
			_ground1.x = 0;
			_ground1Copy.x = Number(stage.stageWidth);
			
			_ground2.x = 0;
			_ground2Copy.x = Number(stage.stageWidth);
			
			_ground3.x = 0;
			_ground3Copy.x = Number(stage.stageWidth);
			
			_ground4.x = 0;
			_ground4Copy.x = Number(stage.stageWidth);
			
			
			//reset instructions
			_instructionText.x = 100;
			_instructionText.text = "Mash the A and D keys to gather power!";
			_instructionText.visible = true;
			_timerText.visible = true;
			_resultText.visible = false;
			
			//reset gauge
			_pBarWidth = 0;
			
			//reset distance & bools
			_ballKicked = false;
			_meters = 0;
			
			//reset clock
			_timerText.x = 450;
			_timer.SetTime(10);
			
			//display time and append zeroes where necessary
			if (_timer.Seconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			
			stage.removeEventListener(MouseEvent.MOUSE_UP, ResetGame);
		}

	}
	
}
