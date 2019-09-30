/* This is the class that defines the player character. The player has a number of lives,
cash, and weapons.  The player will lose a life if touched by a patient or a thrown object.


cooldown = how much time must pass before ability can be used. time is measured 
in frames, and there is 60 frames per second.
collectRate = how much candy is collected per second. The Princess can collect 2x more candy. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;
	
	public class Player extends MovieClip
	{
		protected var _playerName:String;
		protected var _cash:int;			//player's money
		protected var _cooldown:int;		//number of frames before ability can be used.
		protected var _vx:Number;			//movement on x axis
		protected var _vy:Number;			//movement on y axis
		protected var _moveSpeed:Number;
		protected var _accelX:Number;		//acceleration
		protected var _accelY:Number;
		private var _attackArea:Sprite;		//action's hit box.
		private var _actionTimer:int;		//how long the action lasts for
		private var _attackUsed:Boolean;
		

		public function Player() 
		{
			_playerName = "";
			_cash = 0;
			_cooldown = 0;		//only 1 shot can be fired at a time
			_vx = 0;
			_vy = 0;
			_moveSpeed = 1;
			_actionTimer = 0;		//scare lasts for 2 seconds
			_attackUsed = false;
			
			_attackArea = new Sprite(); 
			//_attackArea.graphics.beginFill(0xFF0000);		//used to draw radius. will be hidden for real game
			_attackArea.graphics.drawCircle(25, 25, 40);	//attack radius
			//_attackArea.graphics.endFill();
					
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public function Name():String
		{
			return _playerName;
		}
		
		public function AddCash(amount:int):void
		{
			//add candy to the selected player.
			_cash += amount;
			if (_cash > 999999)
			{
				_cash = 999999;
			}
		}
		
		public function RemoveCash(amount:int):void
		{
			//deduct cash from funds
			if (_cash >= amount)
			{
				_cash -= amount;
			}
			
		}
		
		public function GetCash():int
		{
			return _cash;
		}
		
		public function SetCash(amount:int):void
		{
			_cash = amount;
		}
		
				
		public function SetVelocityX(amount:Number):void
		{
			_vx = amount;
		}
		
		public function SetVelocityY(amount:Number):void
		{
			_vy = amount;
		}
		
		public function GetVelocityX():Number
		{
			return _vx * _moveSpeed;
		}
		
		public function GetVelocityY():Number
		{
			return _vy * _moveSpeed;
		}
		
		public function GetCooldown():int
		{
			return _cooldown;
		}
		
		public function SetCooldown(amount:int):void
		{
			_cooldown = amount;
		}
		/* Check for collision between player and walls and enemies.  */
		public function CollisionCheck(obj:MovieClip):void
		{
			//Collision detection
			if (hitTestObject(obj))
			{
				//collided; block player
				this.x -= _vx * _moveSpeed;
				this.y -= _vy * _moveSpeed;
				//trace("Hit something");
			}
		}
		
		public function IsAttackActive():Boolean
		{
			return (_actionTimer > 0);
		}
		
		public function UseGun():void
		{
			/* Code for firing gun should go here.
			*/
		}
		
		public function UseStimulant():void
		{
			/* Code for using stimulants should go here.
			*/
		}
		
		function onEnterFrame(event:Event):void
		{
			
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
					removeChild(_attackArea);	//action ended
					_attackUsed = false;
				}
			}
			else		//don't move the player until action is over
			{
				_vx = 0;
				_vy = 0;
			}
			
			
			/****check boundaries. "This" refers to the player. *****/
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
