/* This is the player class.  Gods have a belief meter, skills and the ability to create land. */

package  
{
	import flash.display.MovieClip;
	import flash.display.Sprite;
	
	public class God extends MovieClip
	{
		var playerName:String;		//player's name
		var population:int;			//number of humans belonging to player
		var health:Number; 			//belief
		var landCount:int;			//number of lands owned by player

		public function God(pName:String) 
		{
			playerName = pName;
			population = 0;
			health = 0;
			landCount = 0;
		}
		
		public function Name():String { return playerName; }
		public function Population():int { return population; }
		public function Belief():Number { return health; }
		public function Lands():int { return landCount; }
		

	}
	
}
