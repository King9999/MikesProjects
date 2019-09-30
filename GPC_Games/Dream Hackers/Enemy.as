/* This is the main class for all enemies in the game. Here are the stats each enemy has:
	Hit Points = enemy life
	Power = how much damage they deal.
	Speed = how fast they move
	Orbs = how many orbs they drop when killed
	*/

package  
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.display.Sprite;	
	
	public class Enemy extends MovieClip
	{
		protected var hitPoints:int;
		protected var power:int;
		protected var moveSpeed:Number;
		protected var orbs:int;
		protected var vx:Number;		//movement
		protected var vy:Number;

		public function Enemy()
		{
			// constructor code
		}
		
		public function GetHitPoints():int
		{
			return hitPoints;
		}
		
		public function GetPower():int
		{
			return power;
		}
		
		public function SetMoveSpeed(amount:Number):void
		{
			moveSpeed = amount;
		}
		
		public function GetVelocityX():Number
		{
			return vx * moveSpeed;
		}
		
		public function GetVelocityY():Number
		{
			return vy * moveSpeed;
		}
		
		/*This function determines the unique actions each enemy will have */
		public function Action():void
		{
			
		}

	}
	
}
