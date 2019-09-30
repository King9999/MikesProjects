package  
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.display.Sprite;
	
	public class Slime extends Enemy
	{

		public function Slime() 
		{
			  hitPoints = 20;
			  power = 3;
			  moveSpeed = 0.60;
			  orbs = 2;
			  vx = 3;		//movement
			  vy = 3;
			  
			  //event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		function onEnterFrame(event:Event):void
		{
			
			Action();
		}
		
		public override function Action():void
		{
			/*Slimes move around the field slowly towards the player and attack occasionally. */
			this.x += GetVelocityX();
			this.y += GetVelocityY();
		}

	}
	
}
