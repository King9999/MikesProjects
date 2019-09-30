/* Player.as by Haskell (Mike) Murray
	This class contains the player-controlled dream hacker. Players can earn points in the form of
	collectible orbs, which can be used to upgrade various abilities.*/
	
package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;	//used for collision checking.
	
	public class Player extends MovieClip
	{
		private var _dreamPoints:int;	//amount of health
		private var _maxDreamPoints:int;//max health
		private var _psychMeter:Number; //meter used for attacking and other abilities
		private const MAX_METER:Number = 100;	//max meter is always 100, never changes
		private var _rechargeRate:Number;	//how fast the meter recharges
		private var _consumeRate:Number;//attacks and abilities take a portion of the meter
		private var _consumeMod:Number;	//a modifier of how much meter gets consumed
		private var _cooldown:int;		//number of frames before ability can be used.
		private var _psychPower:int;	//amount of psych power. Affects damage
		private var _hackLevel:int;		//player's level. Has gameplay applications
		private var _orbs:int;			//amount of orbs collected
		private var _nextLevel:int;		//amount of orbs needed for next level
		private var _actionTimer:int;	//duration of an attack in frames
		
		/*** movement variables ***/
		private var _vx:Number;			//movement on x axis
		private var _vy:Number;			//movement on y axis
		private var _projVx:Number;
		private var _projVy:Number;		//projectile velocity
		private var _moveSpeed:Number;
		private var _accelX:Number;		//acceleration
		private var _accelY:Number;
		

		public function Player() 
		{
			_maxDreamPoints = 120;
			_dreamPoints = _maxDreamPoints;
			_psychMeter = 100;
			_rechargeRate = 0.55;	//the rate per frame at which the meter recharges.
			_consumeRate = 1;
			_consumeMod = 1;
			_cooldown = 15;
			_psychPower = 10;
			_hackLevel = 1;
			_orbs = 0;
			_nextLevel = 100;
			_actionTimer = 0;
			_moveSpeed = 1.0;
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			//stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
//			addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
		}
		
		
		/**** Setters ****/
		public function SetVelocityX(amount:Number):void
		{
			_vx = amount;
		}
		
		public function SetVelocityY(amount:Number):void
		{
			_vy = amount;
		}
		
		public function SetProjVelocityX(amount:Number):void
		{
			_projVx = amount;
		}
		
		public function SetProjVelocityY(amount:Number):void
		{
			_projVy = amount;
		}
		
		public function SetCooldown(amount:int):void
		{
			_cooldown = amount;
		}
		
		public function AddMaxDreamPoints(amount:int):void
		{
			_maxDreamPoints += amount;
		}
		
		public function SetNextLevel():void
		{
			/* Every time this function is called, a formula is used to
			determine what the next level should be. It uses the current 
			XP as a base to determine the next requirement. */
			
			//raise hacker level
			_hackLevel++;
			
			//set the next level
			_nextLevel = Math.sqrt(_nextLevel * 3 + (_nextLevel * 2)) + _nextLevel * 1.4;
			//trace("Hack Level: " + _hackLevel + " XP To Next Level: " + _nextLevel);
		}
		
		/**** Getters ****/
		public function GetVelocityX():Number
		{
			return _vx * _moveSpeed;
		}
		
		public function GetVelocityY():Number
		{
			return _vy * _moveSpeed;
		}
		
		public function GetProjVelocityX():Number
		{
			return _projVx;
		}
		
		public function GetProjVelocityY():Number
		{
			return _projVy;
		}
		
		public function GetCooldown():int
		{
			return _cooldown;
		}
		
		public function GetDreamPoints():int
		{
			return _dreamPoints;
		}
		
		public function GetNextLevel():int
		{
			return _nextLevel;
		}
		
		
		/* Check for collision between the front of the house or another player. 
		The collision is not terribly precise, but in the interest of time I wish to have
		the game fully functional. */
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
		
		
		public function AttackIsActive():Boolean
		{
			return (_actionTimer > 0);
		}
		
		public function UseAction():void
		{
			
		}
		
		//public function onKeyDown(event:KeyboardEvent):void
//		{
//			//*****Player 1 Controls*******
//			//P1 Walk left
//			if (event.keyCode == Keyboard.A)
//			{
//				
//				SetVelocityX(-3);
//			}
//				
//			//P1 Walk right
//			if (event.keyCode == Keyboard.D)
//			{
//				SetVelocityX(3);
//				
//			}
//								
//			//P1 Walk down
//			if (event.keyCode == Keyboard.S)
//			{
//				
//				SetVelocityY(3);
//			}
//			
//			//P1 Walk up
//			if (event.keyCode == Keyboard.W)
//			{
//				
//				SetVelocityY(-3);
//			}
//		}
//		
//		 //This function checks when a key is released 
//		public function onKeyUp(event:KeyboardEvent):void
//		{
//			
//			//*****Player 1 Controls*******
//			if(event.keyCode == Keyboard.A || event.keyCode == Keyboard.D)
//			{
//				SetVelocityX(0);
//			}
//										
//			if (event.keyCode == Keyboard.S || event.keyCode == Keyboard.W)
//			{
//				SetVelocityY(0);
//			}
//				
//		}
		
		function onEnterFrame(event:Event):void
		{
			//SetNextLevel();
			//this.x += _vx;
//			this.y += _vy;
		}

	}
	
}
