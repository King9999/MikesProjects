/* The Maskless does not wear a costume, hence cannot get candy from houses. His special ability
is stealing candy from his opponents.  When performing an action, candy does not drop from the target.
Instead, the candy goes directly to the Maskless.  The Maskless drops more candy than normal.

STATS
move speed = above average (1.2x move speed)
candy drop amt = high (10 pieces of candy per hit)
cooldown = normal (5 seconds / 300 frames) */

package 
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	
	public class Maskless extends Player
	{

		public function Maskless() 
		{
			
			//addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			_playerName = "Maskless";
			_candyAmount = 0;
			_cooldown = 300;		//5 seconds to recharge scare attack
			_dropAmount = 10;
			_candyTaken = 0;
			_vx = 0;
			_vy = 0;
			_moveSpeed = 1.2;
			
			//event listeners
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		/* Keyboard controls. The player can move up, down, left and right.  The player can perform
		an action by pressing the Spacebar. */
		private function onKeyDown(event:KeyboardEvent):void
		{
			//Walk left
			if (event.keyCode == Keyboard.LEFT)
			{
				//_accelX = -0.2;	
				_vx = -1;
			}
				
			//Walk right
			if (event.keyCode == Keyboard.RIGHT)
			{
				//_accelX = 0.2;
				_vx = 1;
			}
								
			//Walk down
			if (event.keyCode == Keyboard.DOWN)
			{
				//_accelY = 0.2;
				_vy = 1;
			}
			
			//Walk up
			if (event.keyCode == Keyboard.UP)
			{
				//_accelY = -0.2;
				_vy = -1;
			}
			
			//Action button
			if (event.keyCode == Keyboard.SPACE)
			{
				//perform the player-specific action
				UseAction();
			}
		}
		
		/* This function checks when a key is released */
		private function onKeyUp(event:KeyboardEvent):void
		{
			if(event.keyCode == Keyboard.LEFT || event.keyCode == Keyboard.RIGHT)
			{
				_vx = 0;
			}
					
			if (event.keyCode == Keyboard.SPACE)
			{
				//Nothing should happen. Once the space is pressed, the action should go into cooldown
			}
				
			if (event.keyCode == Keyboard.DOWN || event.keyCode == Keyboard.UP)
			{
				_vy = 0;
			}
		}
		
		function UseAction():void
		{
			/* The Maskless has a standard attack, but candy goes directly to him instead of it dropping
			on the ground for anyone to pick up.  He gains candy equal to the amount that would have 
			dropped from the target.  This means that the Knight is a hard counter, since he only drops
			2 pieces and doesn't get knocked back.
			
			when the action is performed, a rectangle is created in front of the Maskless. Any target that
			collides with it gets knocked back and loses candy.
			*/
		}
		
		function onEnterFrame(event:Event):void
		{
			//move player
			this.x += _vx * _moveSpeed;
			this.y += _vy * _moveSpeed;
			
			/****check boundaries*****/
			if (this.x < 0)
			{
				this.x = 0;
			}
			
			if (this.x > stage.stageWidth - this.width)
			{
				this.x = stage.stageWidth - this.width;
			}
			
			if (this.y < 60) //make sure player doesn't go up to black border where HUD is
			{
				this.y = 60;
			}
			
			if (this.y > stage.stageHeight - this.height)
			{
				this.y = stage.stageHeight - this.height;
			}
		}

	}
	
}

