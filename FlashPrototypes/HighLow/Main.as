/* High Low is a number guessing game. You and the AI roll dice. You must guess
if your number is lower or higher than their number. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
    import flash.display.JointStyle;
    import flash.display.LineScaleMode;
    import flash.display.Shape;
    import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.events.MouseEvent;
	
	
	public class Main extends MovieClip
	{
						
				
		var _die:Vector.<int> = new Vector.<int>();		//dice
		var _playerNum:int = 0;								//player's number from 2 to 12
		var _opponentNum:int = 0;
		var _diceRolled:Boolean = false;
		
				
		//HUD text
		var _textFormat:TextFormat;
		var _instructionText:TextField;	//tells player what must be done
		var _playerNumText:TextField;
		var _oppNumText:TextField;
		var _highText:TextField;
		var _lowText:TextField;
		var _rollText:TextField;
		var _resultText:TextField;
		
		var _highButton:Shape;
		var _lowButton:Shape;
		var _rollButton:Shape;

		public function Main() 
		{
			
			
			_textFormat = new TextFormat();
			_textFormat.size = 50;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			

			//instruction
			_instructionText = new TextField();

			_instructionText.textColor = 0x000000;
			_instructionText.defaultTextFormat = _textFormat;
			_instructionText.x = 100;
			_instructionText.y = 50;
			_instructionText.width = 800;
			_instructionText.height = 150;
			_instructionText.multiline = true;
			_instructionText.wordWrap = true;
			_instructionText.text = "High or Low?";
			
			_playerNumText = new TextField();
			_playerNumText.defaultTextFormat = _textFormat;
			_playerNumText.textColor = 0xff0000;
			_playerNumText.x = 100;
			_playerNumText.y = 500;
			_playerNumText.width = 400;
			_playerNumText.height = 60;
			_playerNumText.text = "Your number: " + _playerNum;
			
			_oppNumText = new TextField();
			_oppNumText.defaultTextFormat = _textFormat;
			_oppNumText.textColor = 0x0000ff;
			_oppNumText.x = 500;
			_oppNumText.y = 500;
			_oppNumText.width = 400;
			_oppNumText.height = 60;
			_oppNumText.text = "Their number: ??";
			
			//buttons
			_highButton = new Shape();
			_highButton.graphics.beginFill(0x000088);
			_highButton.graphics.drawRect(100, 600, 120, 80);
			_highButton.graphics.endFill();
			
			_lowButton = new Shape();
			_lowButton.graphics.beginFill(0x000088);
			_lowButton.graphics.drawRect(500, 600, 120, 80);
			_lowButton.graphics.endFill();
			
			_rollButton = new Shape();
			_rollButton.graphics.beginFill(0x880000);
			_rollButton.graphics.drawRect(300, 600, 120, 80);
			_rollButton.graphics.endFill();
			
			_highText = new TextField();
			_highText.defaultTextFormat = _textFormat;
			_highText.textColor = 0x007777;
			_highText.x = 100;
			_highText.y = 600;
			_highText.width = 120;
			_highText.height = 60;
			_highText.selectable = false;
			_highText.text = "HIGH";
			
			_lowText = new TextField();
			_lowText.defaultTextFormat = _textFormat;
			_lowText.textColor = 0x007777;
			_lowText.x = 500;
			_lowText.y = 600;
			_lowText.width = 120;
			_lowText.height = 60;
			_lowText.selectable = false;
			_lowText.text = "LOW";
			
			_rollText = new TextField();
			_rollText.defaultTextFormat = _textFormat;
			_rollText.textColor = 0x007777;
			_rollText.x = 300;
			_rollText.y = 600;
			_rollText.width = 120;
			_rollText.height = 60;
			_rollText.selectable = false;
			_rollText.text = "ROLL";
			
			//results message
			_resultText = new TextField();
			_resultText.defaultTextFormat = _textFormat;
			_resultText.textColor = 0x009900;
			_resultText.x = 400;
			_resultText.y = 700;
			_resultText.width = 500;
			_resultText.height = 60;
			//_resultText.text = "LOW";
			
			
			_highText.addEventListener(MouseEvent.MOUSE_UP, highButtonClicked);
			_lowText.addEventListener(MouseEvent.MOUSE_UP, lowButtonClicked);
			_rollText.addEventListener(MouseEvent.MOUSE_UP, rollButtonClicked);
			
						
			
			
			//create dice
			for (var j = 0; j < 2; j++)
			{
				_die.push(new int);
			}
			
			
			
			//add everything
			addChild(_instructionText);
			addChild(_playerNumText);
			
			addChild(_playerNumText);
			addChild(_oppNumText);
			
			addChild(_highButton);
			addChild(_lowButton);
			addChild(_rollButton);
			addChild(_highText);
			addChild(_lowText);
			addChild(_rollText);
			addChild(_resultText);
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		function highButtonClicked(event:MouseEvent):void
		{
			_oppNumText.text = "Their number: " + _opponentNum;
			if (_playerNum > _opponentNum)
			{
				_resultText.text = "High is correct!";
				
			}
			else
			{
				_resultText.text = "Nope!";
			}
			_resultText.visible = true;
		}
				
		function lowButtonClicked(event:MouseEvent):void
		{
			_oppNumText.text = "Their number: " + _opponentNum;
			if (_playerNum <= _opponentNum)
			{
				_resultText.text = "Low is correct!";
			}
			else
			{
				_resultText.text = "Nope!";
			}
			_resultText.visible = true;
		}
		
		function rollButtonClicked(event:MouseEvent):void
		{
			//re-roll
			_diceRolled = false;
			_resultText.text = "Re-rolled";
			_oppNumText.text = "Their number: ??";
			_resultText.visible = true;
		}
		
		function onEnterFrame(event:Event):void
		{
			if (!_diceRolled)
			{
				_playerNum = 0;
				_opponentNum = 0;
				
				/* roll dice */
				for (var i = 0; i < 2; i++)
				{
					_die[i] = Math.floor(Math.random() * 6) + 1;
					_playerNum += _die[i];
					_playerNumText.text = "Your number: " + _playerNum;
				}
				
				//roll again for opponent
				for (var j = 0; j < 2; j++)
				{
					_die[j] = Math.floor(Math.random() * 6) + 1;
					_opponentNum += _die[j];
				}
				
				_diceRolled = true;
			}
			
			
		}

	}
	
}
