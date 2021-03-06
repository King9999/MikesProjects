﻿/* Towers of Hanoi test  **DUE APRIL 9 AT 8PM**

-Have 3 vertical bars and 4 discs of increasing size
-Move all 4 discs from the first bar to the 3rd bar
-Can only move one disc at a time
-Can only stack a disc if it's smaller than the one below it
-Must have a solve button that plays the game and completes it with the minimum number of moves, which
should be 15.
*/

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.events.MouseEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
    import flash.display.Shape;
    import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	
	public class Main extends MovieClip
	{
		
		const MAX_PEGS:uint = 3;
		const MAX_DISKS:uint = 4;
		const TARGET:uint = 2;	//the peg index# that 4 disks must be on to win the game
		
		//game states
		const NORMAL:uint = 0;	//no disc is picked up
		const PICKED_UP_DISC:uint = 1;	//disc was selected with mouse
		
		
		var _diskSize:uint;		//size of disk. Cannot be more than 4
		var _pegs:uint;			//number of pegs
		var _numDisks:uint;		//number of disks
		
		
		//Pegs. Using a vector of vectors, hope this doesn't get confusing.
		var _peg:Vector.<Vector.<int>> = new Vector.<Vector.<int>>();
		var _pegShape:Vector.<Shape> = new Vector.<Shape>(); 
		var _prevPeg:int;		//tracks which peg a previous disc was on
		
		//Discs.
		var _disc:Vector.<Sprite> = new Vector.<Sprite>();
		var _dColor:Vector.<uint> = new Vector.<uint>();
		var _tempDisc:Sprite;		//used for placing discs in certain places.
		var _tempDiscID:int;		//corresponds to selected disc
		
				
		//HUD elements
		var _textFormat:TextFormat;
		var _debugTextFormat:TextFormat;
		var _instructionText:TextField;	//tells player what must be done
		var _debugText:TextField;
		var _titleText:TextField;		//displays name of game
		var _debugWindow:Shape;			//used to show peg contents and number of turns
		var _turnCount:uint = 0;			//number of turns.  Hidden from player.
		
		var _debugButton:Shape;			//toggles the debug screen on and off.
		var _dBugButtonText:TextField;
		var _solveButton:Shape;			//AI solves puzzle. **NOTE: not mandatory**
		
		var _winText:TextField;			//displayed when player wins game

		public function Main() 
		{
						
			_textFormat = new TextFormat();
			_textFormat.size = 30;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			
			_debugTextFormat = new TextFormat();
			_debugTextFormat.size = 20;
			//_debugTextFormat.bold = true;
			_debugTextFormat.font = "Fixedsys";
			
			//title
			_titleText = new TextField();
			_titleText.textColor = 0x770000;
			_titleText.defaultTextFormat = _textFormat;
			_titleText.x = 350;
			_titleText.y = 10;
			_titleText.width = 800;
			_titleText.height = 150;
			_titleText.text = "**TOWERS OF HANOI**";
									
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
			_instructionText.text = "Move the four discs to the 3rd peg." + 
			"  You can only move the topmost disc, and cannot stack larger discs" +
			" on top of smaller discs.";
			
			
			
			//Pegs
			for (var i = 0; i < MAX_PEGS; i++)
			{
				_peg.push(new Vector.<int>);
				
				//graphics
				_pegShape.push(new Shape());
				_pegShape[i].graphics.beginFill(0xE0A877);
				_pegShape[i].graphics.drawRect(300 + (i * 250), 250, 30, 250);
				_pegShape[i].graphics.endFill();
				addChild(_pegShape[i]);
			}
			
			//set up disc colors
			for (var p = 0; p < MAX_DISKS; p++)
			{
				_dColor.push(new uint);
			}
			
			_dColor[0] = 0xff0000;
			_dColor[1] = 0x00ff00;
			_dColor[2] = 0x0000ff;
			_dColor[3] = 0xffff00;
			
			//Disc setup
			for (var j = 0; j < MAX_DISKS; j++)
			{
				/* Stack the discs on the first peg */
				_peg[0].push(j+1);	//inserts numbers 1 to 4, representing disc size
				
				//create graphic representation of a disc
				_disc.push(new Sprite());
				_disc[j].graphics.beginFill(_dColor[j]);
				_disc[j].graphics.drawRect(265 - (j * 25), (j * 50) + 300, (j * 50) + 100, 50);
				_disc[j].graphics.endFill();
				addChild(_disc[j]);
				
				//add event listeners for the discs
				_disc[j].addEventListener(MouseEvent.MOUSE_DOWN, buttonClicked);
				_disc[j].addEventListener(MouseEvent.MOUSE_UP, buttonReleased);
			}
			
			//debug window
			_debugWindow = new Shape();
			_debugWindow.graphics.beginFill(0x000088);
			_debugWindow.graphics.drawRect(10, 600, 300, 150);
			_debugWindow.graphics.endFill();
			
			//debug button
			_debugButton = new Shape();
			_debugButton.graphics.beginFill(0x444444);
			_debugButton.graphics.drawRect(350, 710, 120, 45);
			_debugButton.graphics.endFill();
			
			_dBugButtonText = new TextField();
			_dBugButtonText.textColor = 0xff0000;
			_dBugButtonText.defaultTextFormat = _debugTextFormat;
			_dBugButtonText.x = 375;
			_dBugButtonText.y = 710;
			_dBugButtonText.width = 100;
			_dBugButtonText.height = 80;
			_dBugButtonText.multiline = true;
			_dBugButtonText.selectable = false;
			_dBugButtonText.text = "Toggle \n Debug";
			
			//debug text
			_debugText = new TextField();
			_debugText.textColor = 0xffffff;
			_debugText.defaultTextFormat = _debugTextFormat;
			_debugText.x = 15;
			_debugText.y = 615;
			_debugText.width = 200;
			_debugText.height = 150;
			_debugText.multiline = true;
			_debugText.wordWrap = true;
			_debugText.selectable = false;
			//UpdateDebugInfo();
			
			_winText = new TextField();
			_winText.textColor = 0x008800;
			_winText.defaultTextFormat = _textFormat;
			_winText.x = 300;
			_winText.y = 500;
			_winText.width = 300;
			_winText.height = 150;
			_winText.multiline = true;
			_winText.wordWrap = true;
			//_winText.visible = false;	//invisible by default until player wins
			
			
	
			//display HUD elements
			addChild(_titleText);
			addChild(_instructionText);
			addChild(_debugWindow);
			addChild(_debugText);
			//addChild (_debugButton);
			//addChild(_dBugButtonText);
			addChild(_winText);
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			//stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			//stage.addEventListener(MouseEvent.MOUSE_DOWN, onMouseDown);
			//stage.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
		}
		
		//The game only uses a mouse to play
		function buttonClicked(event:MouseEvent):void
		{
			/*Upon clicking the mouse button, the game checks which disc the cursor is resting on,
			and chooses the appropriate action based on the result.*/
			
			//make sure the player clicks on a valid disc, i.e. should not be able to select a disc underneath
			//another one
			
			
			//set a temporary sprite to control the clicked disc
			_tempDisc = Sprite(event.target);
			
			
			//this line will move whatever object is clicked by the mouse if the object has a listener attached
			//_tempDisc.startDrag();
			
			//and this line will set the draw order to the highest priority while the object is dragged
			stage.addChild(_tempDisc);
			

			//check which disc was clicked and assign the disc ID, which will be used for adding and 
			//removing discs.
			switch (_tempDisc)
			{
				case _disc[0]:
					_tempDiscID = 1;
					break;
				case _disc[1]:
					_tempDiscID = 2;
					break;
				case _disc[2]:
					_tempDiscID = 3;
					break;
				case _disc[3]:
					_tempDiscID = 4;
					break;
				default:	//nothing happens
					break;
			}
			//trace ("ID " + _tempDiscID);
			
			//check which peg the current disc is on
			for (var i = 0; i < MAX_PEGS; i++)
			{
				
				if (_peg[i].length > 0 && _tempDiscID == _peg[i][0])
				{
					//allow the player to pick up the disc. 
					//they will be unable to select a disc underneath a smaller disc.
					_prevPeg = i;
					_tempDisc.startDrag();
				}
					
			}
			//trace ("Prev. Peg: " + _prevPeg);
			
		}
		
		function buttonReleased(event:MouseEvent):void
		{
			/*When the mouse is released, must check where the disc is resting. If it's atop a peg, then
			assume that the player wants to drop a disc on that peg. Make sure the move is valid and 
			adjust the disc's position. */
			_tempDisc = Sprite(event.target);
			_tempDisc.stopDrag();
			//_tempDisc.x = _disc[1].x;
			//_tempDisc.y = _disc[1].y;
			
			//check if the disc is resting on a peg
			for (var i = 0; i < MAX_PEGS; i++)
			{
				if (_tempDisc.hitTestObject(_pegShape[i]))
				{
					//trace ("Hit Peg " + i);
					//check if there are any other discs on this peg and compare with the current disc
					if (_peg[i].length == 0)
					{
						//just add the disc
						_peg[i].push(_tempDiscID);
						
						//remove disc from previous peg. The removed disc should always match
						//the value of _tempDiscID
						_peg[_prevPeg].shift();
						
						//increment turn count
						_turnCount++;
					}
					else if (_tempDiscID < _peg[i][0])
					{
						/* The disc goes to the front of the vector */
						_peg[i].push(_tempDiscID);
						_peg[_prevPeg].shift();
						
						//sort the vector
						_peg[i].sort(Array.NUMERIC);
						
						//increment turn count
						_turnCount++;
					}
					
				}
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			
			//if (!GameWon())
//			{
				//Update debug data
				UpdateDebugInfo();
				
				
			//}
			if (GameWon())
			{
				/*display win message */
				_winText.text = "Nice! You solved the puzzle in " + _turnCount + " turns!";
			}
		}
		
		
		/* Various functions */
		private function UpdateDebugInfo():void
		{
			//update debug info
			_debugText.text = "GAME STATUS\n" + 
								"============\n" + 
								"Peg 0: " + _peg[0] + "\n" +
								"Peg 1: " + _peg[1] + "\n" +
								"Peg 2: " + _peg[2] + "\n" +
								"Turn: " + _turnCount;
		}
		
		private function GameWon():Boolean
		{
			//the game can only be won if all 4 pegs are on the 3rd peg
			return (_peg[TARGET].length == MAX_DISKS);
		}

	}
	
}
