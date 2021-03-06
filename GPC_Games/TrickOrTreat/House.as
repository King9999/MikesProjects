﻿/* The House gives candy to visiting players. The amount of candy that's given, as well as whether 
the house even has candy, is determined at random.  The amount of candy will be displayed above 
the roof. The amount of candy should be anywhere from 1 to 10, with the chance of a house having
20 pieces. */


package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	public class House extends MovieClip
	{
		private var _hasCandy:Boolean;		//checks if a house has candy
		private var _candyAmount:int;
		

		public function House() 
		{
			
			_candyAmount = 0;
			this.gotoAndStop(1);		//by default, the frame should be set to "lights off."

		}
		
		public function HasCandy():Boolean
		{
			return (_candyAmount > 0);
		}
		
		public function GetCandyAmount():int
		{
			return _candyAmount;
		}
		
		public function ReduceCandy(amount:int):void
		{
			_candyAmount -= amount;
			if (!HasCandy())
			{
				_candyAmount = 0;
				this.gotoAndStop(1);	//turn off the lights, no more candy
			}

		}
		
		public function SetCandyStock(amount:int):void
		{
			_candyAmount = amount;
			if (!HasCandy())
			{
				this.gotoAndStop(1);	//turn off the lights, no more candy
			}
		}
		public function StockUp():void
		{
			var candyStock:int;		//random amount of candy
			var chance:int;			//checks if house is brimming with candy
			
			//give a house a random amount of candy, with a possible chance of getting 20 pieces.
			chance = Math.floor(Math.random() * 100) + 1;
			
			if (chance <= 10)
			{
				candyStock = 20;
			}
			else
			{
				candyStock = Math.floor(Math.random() * 10) + 1;
			}
			
			_candyAmount = candyStock;
			this.gotoAndStop(2);	//set "lights on" frame
			
			
		}
		
		public function CheckPlayerProximity():void
		{
			/* This function checks if a player is close by.  If true, then start
			giving out candy if stocked. */
		}

	}	//end class
	
}
