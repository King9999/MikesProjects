/* The Ghost has the ability to "scare" instead of attack. Scaring the enemy causes them to drop more 
candy than normal. The downside is that scaring has a longer cooldown than attacking.  Targets affected
by the action drops 2x the amount of candy.

STATS
move speed = normal (1x move speed)
candy drop amt = normal (5 pieces of candy per hit)
cooldown = high (10 seconds / 600 frames) */

package 
{
	
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;	//used for collision checking.
	
	public class Ghost extends Player
	{
		private var _attackArea:Sprite;	//action's hit box.
		private var _actionTimer:int;	//how long the action lasts for
		private var _attackUsed:Boolean;
		private var _booBubble:BooBubble;
 
		public function Ghost() 
		{
			
			//addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			_playerName = "Ghost";
			_candyAmount = 0;
			_cooldown = 0;		//10 seconds to recharge scare attack
			_dropAmount = 5;
			_candyTaken = 1;
			_vx = 0;
			_vy = 0;
			_moveSpeed = 1;
			_actionTimer = 0;		//scare lasts for 2 seconds
			_attackUsed = false;
			
			_attackArea = new Sprite(); 
			//_attackArea.graphics.beginFill(0xFF0000);		//used to draw radius. will be hidden for real game
			_attackArea.graphics.drawCircle(25, 25, 40);	//attack radius
			//_attackArea.graphics.endFill();
			
			//"BOO!" graphic.
			_booBubble = new BooBubble();
			_booBubble.x = -10;
			_booBubble.y = -10;
			_booBubble.height = 60;
			
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		

		public override function UseAction():void
		{
			/* The Ghost has the ability to scare his target and make them drop more candy than usual.
			
			What this function does is create a collision circle around the Ghost so that
			all surrounding targets that touches it will take damage.  Powerful action, but long cooldown.
			*/
			if (_cooldown <= 0)
			{
				addChild(_attackArea);
				addChild(_booBubble);
				_actionTimer = 60;		//action lasts for 1 second, ie 60 frames
				_attackUsed = true;
				_cooldown = 600;
			}
			
		}
		
		public override function IsAttackActive():Boolean
		{
			return (_actionTimer > 0);
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
					removeChild(_booBubble);
					_attackUsed = false;
				}
			}
			else		//don't move the player until action is over
			{
				_vx = 0;
				_vy = 0;
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

	} //end class
	
}
