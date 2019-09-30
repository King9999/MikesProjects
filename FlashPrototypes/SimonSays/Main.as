/*Simon Says is a memory game. letters and numbers appear on the screen briefly, and you must type
them correctly.*/

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
	
	
	public class Main extends MovieClip
	{

		var _timerText:TextField;
		var _textFormat:TextFormat;
		var _targetWord:String;	//contains words to win game
		var _randomChar:uint;	//selects random letters based on its ASCII code
		var _wordCount:uint;	//number of words needed to complete level.
		var _currentChar:uint;	//iterator.
		var _wordText:TextField;	//current word
		var _wordCreated:Boolean;	//true when word is generated
		var _timer:int;				//used to transition to next letter in sequence
		
		
		var _typeField:TextField;	//used to type in words
		
		
		//HUD text
		var _instructionText:TextField;	//tells player what must be done
		

		public function Main() 
		{
			_wordCreated = false;			
						
			_textFormat = new TextFormat();
			_textFormat.size = 50;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			
			_timerText = new TextField();
			_timerText.defaultTextFormat = _textFormat;
			_timerText.textColor = 0x0000F0;
			_timerText.width = 160;
			_timerText.height = 80;
			_timerText.x = 450;
			_timerText.y = 200;
			
			
			//Text field
			_typeField = new TextField();
			_typeField.type = "input";	//need this to be able to input text
			_typeField.border = true;	//need this to be able to input text
			_typeField.borderColor = 0xFF0000;
			_typeField.textColor = 0x000000;
			_typeField.defaultTextFormat = _textFormat;
			_typeField.background = true;
			_typeField.backgroundColor = 0xcccccc;
			_typeField.x = 350;
			_typeField.y = 600;
			_typeField.width = 300;
			_typeField.height = 60;
			
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
			//_instructionText.text = "Choose a difficulty level";	//displayed on main screen
			_instructionText.text = "Type the characters in order."
						
			//words
			_currentChar = 0;
						
			_wordText = new TextField();
			_wordText.defaultTextFormat = _textFormat;
			//_wordText.text = _targetWord;
			_wordText.textColor = 0x004400;
			_wordText.x = 440;
			_wordText.y = 500;
			_wordText.width = 200;
			_wordText.height = 70;
			

			
			
			addChild(_instructionText);
			addChild(_wordText);
			addChild(_typeField);
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
		}
		
		public function onKeyDown(event:KeyboardEvent):void
		{
			/*When ENTER is pressed, the game checks what was typed against
			the target word.  If correct, the next word is selected.
			Otherwise, the text box is cleared.*/
			
			switch (event.keyCode)
			{
				case Keyboard.ENTER:
					/* Take what the player entered in the input box and compare
					with the target word.*/
					if (_typeField.text == _targetWord)
					{
						trace("Correct!");
						//move to next word
						_wordCreated = false;
						_currentChar = 0;
						
					}
					else	//do nothing
					{
						trace("Wrong!");
					}
						
					_typeField.text = "";
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			
			/* words are dynamically created, and then each character is displayed one at a time */
								
			
			if (!_wordCreated)
			{
				var p = 0;
				_targetWord = "";	
				while (p < 5)	//5 = number of letters per word
				{
					_targetWord += String.fromCharCode(Math.floor(Math.random() * 25) + 97);
					p++;
				}		
				_wordCreated = true;		
				trace (_targetWord);
			}
			
			//display each char momentarily
			_timer--;
			if (_timer < 0 && _currentChar != _targetWord.length)
			{
				
				_wordText.text = _targetWord.charAt(_currentChar);
				
				_currentChar++;
				_timer = 45;
					
			}
			
			
			
		}

	}
	
}
