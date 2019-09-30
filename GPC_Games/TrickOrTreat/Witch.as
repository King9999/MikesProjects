/* The Witch has a magic wand that allows ranged attacks.  It has a limited number of charges;
once the charges are used up, she will be unable to attack.

STATS
move speed = normal (1x move speed)
candy drop amt = normal (5 pieces of candy per hit)
cooldown = normal (5 seconds / 300 frames) */

package 
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	
	public class Witch extends Player
	{
		private var _magicShot:Magic;
		private var _charges:int;	//number of charges for ranged attack
		private var _attackUsed:Boolean;
		private var _actionTimer:int;	//how long the action lasts for
		private var _shotVelocityX, _shotVelocityY;	//velocity of the magic beam.
		private var _faceLeft:Boolean;
		private var _faceRight:Boolean;	//used to check player's direction
		private var _faceUp:Boolean;
		private var _faceDown:Boolean;

		public function Witch() 
		{
			
			//addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			_playerName = "Witch";
			_candyAmount = 0;
			_cooldown = 0;		//5 seconds to recharge scare attack
			_dropAmount = 5;
			_candyTaken = 1;
			_vx = 0;
			_vy = 0;
			_moveSpeed = 1;
			_charges = 10;
			
			_shotVelocityX = 6;
			_shotVelocityY = 6;
			
			//magic shot
			_magicShot = new Magic();
			_magicShot.x = 0;	//local coordinates
			_magicShot.y = 0;
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public override function UseAction():void
		{
			/* The Witch has a ranged attack called the magic shot. It is a fast attack, but
			she has a limited amount and cannot get any more until the game ends.
			*/
			if (_cooldown <= 0)
			{
				
				addChild(_magicShot);
				
				
				//fire a shot in a direction the witch is facing.
				if (_faceLeft)
				{
					_magicShot.x -= _shotVelocityX;
				}
				else if (_faceRight)
				{
					_magicShot.x += _shotVelocityX;
				}
				else if (_faceUp)
				{
					_magicShot.y -= _shotVelocityY;
				}
				else if (_faceDown)
				{
					_magicShot.y += _shotVelocityY;
				}
				
				_actionTimer = 60;		//action lasts for 1 second, ie 60 frames
				_attackUsed = true;
				_cooldown = 120;
			}
			
		}
		
		public override function IsAttackActive():Boolean
		{
			return (_actionTimer > 0);
		}
		
				
		function onEnterFrame(event:Event):void
		{
			
			//check which direction player is facing
			if (_vx < 0)	//moving left
			{
				_faceLeft = true;
				_faceRight = false;
				_faceUp = false;
				_faceDown = false;
				
			}
			
			if (_vx > 0)	//moving right
			{
				_faceLeft = false;
				_faceRight = true;
				_faceUp = false;
				_faceDown = false;
				
			}
			
			if (_vy < 0)	//moving up
			{
				_faceLeft = false;
				_faceRight = false;
				_faceUp = true;
				_faceDown = false;
				
			}
			
			if (_vy > 0)	//moving down
			{
				_faceLeft = false;
				_faceRight = false;
				_faceUp = false;
				_faceDown = true;
				
			}
			
			//update magic shot
			_magicShot.x += _shotVelocityX;
			
			//adjust the cooldown and action timers
			_cooldown--;
			if (_cooldown < 0)
			{
				_cooldown = 0;
			}
			
			_actionTimer--;
			if (_actionTimer <= 0)
			{
				_actionTimer = 0;
				if (_attackUsed)
				{
					
					removeChild(_magicShot);
					_attackUsed = false;
				}
			}
			
			
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

