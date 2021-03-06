﻿/* The Knight has armor which prevents him from getting knocked back from an enemy action. Also,
he drops less candy when hit. However, movement speed is slightly slower.

STATS
move speed = slow (0.75x move speed)
candy drop amt = low (2 pieces of candy per hit)
cooldown = normal (5 seconds / 300 frames) */

package 
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;	//used for collision checking.
	
	public class Knight extends Player
	{
		private var _attackArea:Sprite;	//action's hit box.
		private var _actionTimer:int;	//how long the action lasts for
		private var _attackUsed:Boolean;
		private var _slash:SwordSlash;
		private var _faceLeft:Boolean;
		private var _faceRight:Boolean;	//used to check player's direction
		private var _faceUp:Boolean;
		private var _faceDown:Boolean;
		
		//sword ranges
		private var _horiRange:Number;
		private var _vertRange:Number;
		

		public function Knight() 
		{
			_playerName = "Knight";
			_candyAmount = 0;
			_cooldown = 0;		//2.5 seconds to recharge sword attack
			_dropAmount = 2;
			_candyTaken = 1;
			_vx = 0;
			_vy = 0;
			_moveSpeed = 0.60;
			_actionTimer = 0;		//sword lasts for 1 second
			_attackUsed = false;
			_horiRange = 80;
			_vertRange = 20;
			
			_attackArea = new Sprite(); 
			//_attackArea.graphics.beginFill(0xFF0000);		//used to draw range. will be hidden for real game
			//_attackArea.graphics.drawRect(25, 25, _horiRange, _vertRange);	//attack range
			//_attackArea.graphics.endFill();
			
			//sword slash graphic.
			_slash = new SwordSlash();
			_slash.x = 25;
			_slash.y = 25;
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		
		
		public override function UseAction():void
		{
			/* The Knight has a sword attack that will knock the opponent back and cause him to
			drop some candy.  This attack is fairly fast.
			
			when the action is performed, a rectangle is created in front of the Knight. Any target that
			collides with it gets knocked back and loses candy.
			*/
			if (_cooldown <= 0)
			{
				//create the attack depending on which direction is being pressed
				//TODO: stop the rects from being drawn more than once after changing directions
				
				if (_faceLeft)
				{
					//_attackArea.graphics.beginFill(0xFF0000);
					_attackArea.graphics.drawRect(-50, 25, _horiRange, _vertRange);
					//_attackArea.graphics.endFill();
					_slash.x = -50;
					_slash.y = 25;
					//set scale in case it got reversed
					_slash.scaleX = 1;
					trace("Attacking left");
				}
				else if (_faceRight)
				{
					//_attackArea.graphics.beginFill(0xFF0000);
					_attackArea.graphics.drawRect(25, 25, _horiRange, _vertRange);
					//_attackArea.graphics.endFill();
					_slash.x = 100;	//we set to 100 to compensate for the change in scale
					_slash.y = 25;
					//reverse the slash image.
					_slash.scaleX = -1;
				}
				
				else if (_faceUp)
				{
					//_attackArea.graphics.beginFill(0xFF0000);
					_attackArea.graphics.drawRect(25, -50, _vertRange, _horiRange);
					//_attackArea.graphics.endFill();
					_slash.x = 25;	
					_slash.y = 25;
					_slash.scaleX = 1;
					_slash.rotationX = 30;
				}
				else if (_faceDown)
				{
					//_attackArea.graphics.beginFill(0xFF0000);
					_attackArea.graphics.drawRect(25, 50, _vertRange, _horiRange);
					//_attackArea.graphics.endFill();
					_slash.x = 25;	
					_slash.y = 25;
					_slash.scaleX = 1;
				}
			
				addChild(_attackArea);
				addChild(_slash);
				_actionTimer = 15;		//action lasts for 0.25 seconds
				_attackUsed = true;
				_cooldown = 100;
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
				//trace (_faceLeft);
//				trace (_faceRight);
//				trace (_faceUp);
//				trace (_faceDown);
			}
			
			if (_vx > 0)	//moving right
			{
				_faceLeft = false;
				_faceRight = true;
				_faceUp = false;
				_faceDown = false;
				//trace ("Knight is facing right");
			}
			
			if (_vy < 0)	//moving up
			{
				_faceLeft = false;
				_faceRight = false;
				_faceUp = true;
				_faceDown = false;
				//trace ("Knight is facing up");
			}
			
			if (_vy > 0)	//moving down
			{
				_faceLeft = false;
				_faceRight = false;
				_faceUp = false;
				_faceDown = true;
				//trace ("Knight is facing down");
			}
			
			
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
					removeChild(_slash);
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
