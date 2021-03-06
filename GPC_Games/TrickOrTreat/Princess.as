﻿/* The Princess is able to collect more candy from houses than any other character, and she can move
slightly faster than other characters.  However, the Princess cannot perform an action.

STATS
move speed = fast (1.2x move speed)
candy drop amt = normal (5 pieces of candy per hit)
cooldown = N/A  */

package 
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;	
	
	public class Princess extends Player
	{
		private var _question:QuestionMark;
		private var _attackUsed:Boolean;
		private var _actionTimer:int;	//how long the action lasts for
		

		public function Princess() 
		{
			
			_playerName = "Princess";
			_candyAmount = 0;
			_cooldown = 0;		
			_dropAmount = 5;
			_candyTaken = 2;
			_vx = 0;
			_vy = 0;
			_moveSpeed = 1.1;
			_actionTimer = 0;		
			_attackUsed = false;
			
			_question = new QuestionMark();
			_question.x = 20;
			_question.y = -20;
			//_question.height = 60;
			

			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public override function UseAction():void
		{
			/* The Princess has no action. A "?" will be displayed when the action is used
			*/
			if (_cooldown <= 0)
			{
				
				addChild(_question);
				_actionTimer = 30;		//action lasts for 0.5 seconds, ie 30 frames
				_attackUsed = true;
				_cooldown = 70;
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
					
					removeChild(_question);
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

