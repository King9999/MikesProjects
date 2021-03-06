﻿/* This is the superclass for all playable characters in the game. This class contains only what's
common between all characters. 

candyAmount = amount of candy collected.  Required to win the game.
cooldown = how much time must pass before ability can be used. time is measured in frames, and there is 
60 frames per second.
collectRate = how much candy is collected per second. The Princess can collect 2x more candy. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	
	public class Player extends MovieClip
	{
		protected var _playerName:String;
		protected var _candyAmount:int;		//total candy
		protected var _cooldown:int;		//number of frames before ability can be used.
		protected var _candyTaken:int;		//amount of candy taken from a house per tick.
		protected var _dropAmount:int;		//amount of candy player drops when hit. default is 5
		protected var _vx:Number;			//movement on x axis
		protected var _vy:Number;			//movement on y axis
		protected var _moveSpeed:Number;
		protected var _accelX:Number;		//acceleration
		protected var _accelY:Number;
		

		public function Player() 
		{
			// constructor code
		}
		
		public function GetName():String
		{
			return _playerName;
		}
		
		public function AddCandy(amount:int):void
		{
			//add candy to the selected player.
			_candyAmount += amount;
			if (_candyAmount > 999)
			{
				_candyAmount = 999;	//not likely to get to this amount in 3 minutes
			}
		}
		
		public function DropCandy(amount:int):void
		{
			//drop a number of pieces of candy
			if (_candyAmount > 0 && _candyAmount >= amount)
			{
				_candyAmount -= amount;
			}
			
		}
		
		public function GetCandyAmount():int
		{
			return _candyAmount;
		}
		
		public function SetCandyAmount(amount:int):void
		{
			_candyAmount = amount;
		}
		
		public function GetCandyTaken():int
		{
			return _candyTaken;
		}
		
		public function GetDropAmount():int
		{
			return _dropAmount;
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
		
		
		public function IsAttackActive():Boolean
		{
			//return (_actionTimer > 0);
			return false;
		}
		
		public function UseAction():void
		{
			/* The Knight has a sword attack that will knock the opponent back and cause him to
			drop some candy.  This attack is fairly fast.
			
			when the action is performed, a rectangle is created in front of the Knight. Any target that
			collides with it gets knocked back and loses candy.
			*/
		}

	}
	
}
