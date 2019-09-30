/* The Clock class sets the timer for the game.  The default is 3 minutes, or 10,800 frames. */

package 
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	public class Clock extends MovieClip
	{
		private var _minutes:int;
		private var _seconds:int;
		private var _milliseconds:int;	//runs at same speed as frames
		
		public function Clock(seconds:int) 
		{
			//takes a parameter in seconds and converts it to minutes and seconds.
			
			//get the amount of minutes and seconds
			_minutes = seconds / 60;
			_seconds = seconds % 60;
			_milliseconds = 59;
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public function GetMinutes():int
		{
			return _minutes;
		}
		
		public function GetSeconds():int
		{
			return _seconds;
		}
		
		public function SetTime(seconds:int)
		{
			_minutes = seconds / 60;
			_seconds = seconds % 60;
			_milliseconds = 59;
		}
		
		public function TimeUp():Boolean
		{
			return (_minutes == 0 && _seconds == 0 && _milliseconds == 0);
		}
		
		public function Countdown():void
		{
			//countdown the clock.
			_milliseconds--;
			if (_milliseconds < 0)
			{
				//reduce seconds by 1
				_seconds--;
				_milliseconds = 59;
			}
			
			if (_seconds < 0)
			{
				//reduce minutes by 1
				_minutes--;
				_seconds = 59;
			}
			
			if (_minutes < 0)
			{
				//if this happens, then that means time has run out.
				_minutes = 0;
				_seconds = 0;
				_milliseconds = 0;
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			//Countdown();
		}

	}
	
}
