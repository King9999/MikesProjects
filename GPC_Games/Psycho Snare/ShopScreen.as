/* This is the shop screen, where the player can upgrade their equipment and purchase
various items.
 */

package  
{
	
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.display.Sprite;
	
	public class ShopScreen extends MovieClip 
	{
		private var _costumeText:Vector.<TextField> = new Vector.<TextField>();
		private var _textFormat:TextFormat;
		private var _playerSelected:Vector.<Boolean> = new Vector.<Boolean>();	//checks if character was picked and exits loop
		private var _current:int;				//vector iterator
		private var _currentPlayer:int;
		
		private var _selectIcon:Vector.<Sprite> = new Vector.<Sprite>();
		
		public function ShopScreen() 
		{
		
			_current = 0;
			_currentPlayer = 0;
			
			_textFormat = new TextFormat();
			_textFormat.size = 22;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			
		
			for (var j = 0; j < _selectIcon.length; j++)
			{
				_selectIcon[j].x = 180;
				_selectIcon[j].y = 350;
				_playerSelected[j] = false;
			}
			
			for (var i = 0; i < 4; i++)
			{
				_costumeText[i] = new TextField();
				_costumeText[i].textColor = 0xFFFFFF;
				_costumeText[i].defaultTextFormat = _textFormat;
				_costumeText[i].wordWrap = true;
					
				//text borders
				_costumeText[i].x = 400;
				_costumeText[i].y = 600;
				_costumeText[i].width = 300;
				_costumeText[i].height = 150;
				_costumeText[i].multiline = true;	//allows for line breaks
			}
			
			//set up the costume information to give to players
			_costumeText[0].text = "GHOST: Has the ability to scare any opponents in range, " +
				"causing lots of candy to drop!";
				
			_costumeText[1].text = "KNIGHT: Although slow-moving, the Knight won't get " +
			"knocked back much by attacks, and doesn't drop as much candy as his opponents.";
			
			_costumeText[2].text = "PRINCESS: Houses will always give out more candy " + 
			"when you wear a cute costume! Don't try to use an action, though.";
			
			_costumeText[3].text = "WITCH: With her magic wand, she can use actions from " + 
			"a safe distance. Once you run out of charges though, that's it!";
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public function PlayerSelectOK(player:int):Boolean
		{
			return (_playerSelected[player] == true);
		}
		
				
		public function IterateLeft():void
		{
			removeChild(_costumeText[_current]);
				if (_current <= 0)
				{
					_current = _costumeText.length - 1;	//move to end of vector
					_selectIcon[_currentPlayer].x = 810;	//move cursor
				}
				else
				{
					_current--;
					_selectIcon[_currentPlayer].x -= 210;
				}
				
		}
		
		public function IterateRight():void
		{
			removeChild(_costumeText[_current]);
			if (_current >= _costumeText.length - 1)
				{
					_current = 0;	//move to front of vector
					_selectIcon[_currentPlayer].x = 180;
				}
				else
				{
					_current++;
					_selectIcon[_currentPlayer].x += 210;
				}
				
		}
		
		public function SetNextIcon(player:int):void
		{
			_currentPlayer = player;
		}
		
		public function PlayerSelected(player:int)
		{
			//remove old icon
			removeChild(_selectIcon[_currentPlayer]);
			removeChild(_costumeText[_current]);
			_playerSelected[player] = true;
			//reset icon position
			_selectIcon[player].x = 180;
			_current = 0;
		}
		
		public function Update():void
		{
			
			//addChild(_costumeText[_current]);
//			addChild(_selectIcon[_currentPlayer]);
		}
		
		public function GetCostumePick():int
		{
			/*Use this function to decide who the character chose.
			0 = Ghost
			1 = Knight
			2 = Princess
			3 = Witch */
			return _current;
		}
		function onEnterFrame(event:Event):void
		{
			/* The select screen will run continuously until the space bar is pressed. 
			The information on the screen will update if the left and right keys are pressed. */
			
			
			Update();

			
			
		}
	}
	
}
