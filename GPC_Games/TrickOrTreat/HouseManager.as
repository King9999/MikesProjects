/* This class manages all of the House objects. It controls whether a house has candy, and how much
candy it has. 

-At the start of the game, there will be a 50% chance that a house will have candy.
-At each frame, the game will check if a house is empty. If it is, then there's a 0.00002% chance
it'll get restocked.
-Houses normally have 1-10 pieces of candy. Each time a house is stocked, there will be a 20% chance 
that a house will have 20 pieces of candy.
-There will be 10 houses in the game.
*/



package  
{
	import flash.display.MovieClip;
	import House;
	import Clock;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	public class HouseManager extends MovieClip
	{
		private var _houseArray:Array = new Array(); 
		private var _houseVector:Vector.<House> = new Vector.<House>();
		private var _candyText:Vector.<TextField> = new Vector.<TextField>();
		private var _textFormat:TextFormat;
		private var _clockText:TextField;
		private var _clockFormat:TextFormat;
		var clock:Clock = new Clock(60);

		public function HouseManager() 
		{
			/*******CLOCK SETUP ************/
			//set up clock text
			_clockFormat = new TextFormat();
			_clockFormat.size = 30;
			_clockFormat.bold = true;
			_clockFormat.font = "Calibri";
			
			_clockText = new TextField();
			_clockText.textColor = 0xFFFFFF;
			_clockText.defaultTextFormat = _clockFormat;
				
			//text borders
			_clockText.width = 80;
			_clockText.height = 80;
				
			//position the text in the black border
			_clockText.x = 512;
			_clockText.y = 0;
			
			if (_seconds < 10)
			{
				//append a zero in front of the seconds
				_clockText.text = _minutes.toString() + ":0" + _seconds.toString();
			}
			else
			{
				_clockText.text = _minutes.toString() + ":" + _seconds.toString();
			}
			
			//display clock
			addChild(_clockText);
			
			/*******HOUSE SETUP ***************/
			
			//Set up each house in the vector.				
			_houseVector.push(house0);
			_houseVector.push(house1);
			_houseVector.push(house2);
			_houseVector.push(house3);
			_houseVector.push(house4);
			_houseVector.push(house5);
			_houseVector.push(house6);
			_houseVector.push(house7);
			_houseVector.push(house8);
			_houseVector.push(house9);
			
			//text formatting for displaying the candy amounts
			_textFormat = new TextFormat();
			_textFormat.size = 30;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			
			for (var i = 0; i < 10; i++)
			{
				//text information
				_candyText[i] = new TextField();
				_candyText[i].textColor = 0xFFFFFF;
				_candyText[i].defaultTextFormat = _textFormat;
				
				//text borders
				_candyText[i].width = 40;
				_candyText[i].height = 40;
				
				//position the text on the house.
				_candyText[i].x = _houseVector[i].x - 10;
				_candyText[i].y = _houseVector[i].y - 50;
			
			
				//Randomly determine which houses start with candy
				var chance:int;
			
				chance = Math.floor(Math.random() * 100) + 1;
				if (chance <= 50)
				{
					_houseVector[i].StockUp();
					_candyText[i].text = _houseVector[i].GetCandyAmount().toString();
					addChild(_candyText[i]);	//draw text only when candy is available
				}
				//trace ("House " + i + " candy amount: " + _houseVector[i].GetCandyAmount());
			}
		
			
			//we need this here so that updates can happen each frame.
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			
		}
		
		
		function onEnterFrame(event:Event):void
		{
			//The only thing this function does is check if a house will get stocked
			//with candy.  There's a 0.00002% chance this will happen.  The number is low
			//because the game is checking 60 times a second.  Don't want the houses
			//to fill up too quickly.
			var chance:Number;
			
			for (var i = 0; i < _houseVector.length; i++)
			{
				if (!_houseVector[i].HasCandy())
				{
					chance = Math.floor(Math.random() * 100);
					if (chance <= 0.00002)
					{
						_houseVector[i].StockUp();
						_candyText[i].text = _houseVector[i].GetCandyAmount().toString();
						addChild(_candyText[i]);	//draw text only when candy is available
					}
				}
			}
			
		}

	}
	
}
