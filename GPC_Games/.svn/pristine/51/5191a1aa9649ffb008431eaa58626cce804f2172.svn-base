/* This class is used for collision between itself and the player. If there is collison,
then the player will start gathering candy. The pads should be placed by the BASE of the houses,
i.e. in front. */

package  
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.display.Sprite;	//used for collision checking.
	
	public class LandingPad extends MovieClip
	{
		private var _rect:Sprite;

		public function LandingPad(xPos:Number, yPos:Number) 
		{
			
			_rect = new Sprite(); 
			_rect.graphics.beginFill(0xFF0000);		//used to draw radius. will be hidden for real game
			_rect.graphics.drawRect(xPos, yPos, 130, 20);
			_rect.graphics.endFill();
			//addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		//Check collision against 
		//public function testMarioCollisionWithEnemy(obj1:MovieClip, obj2:MovieClip):void
//		{
//			obj1 = mario;
//			obj2 = enemy;
//			//Collision detection
//			if (obj1.hitTestObject(obj2))
//			{
//				//Check if Mario is small.  If true, then he dies.
//				//Otherwise, Mario's state changes to small Mario
//				if (marioState == SM_MARIO)
//				{
//					mario.gotoAndStop(46);	//Call "dead" frame
//				}
//				else
//					marioState == SM_MARIO;	//Mario becomes small
//				
//			}
//		}
		
		//function onEnterFrame(event:Event):void
//		{
//			//check for collision between player and box
//			
//			
//		}

	}
	
}
