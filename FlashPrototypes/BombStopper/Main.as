﻿/* Bomb Stopper is a timed game where the player must stop a bomb from exploding by
typing random codes onscreen.  The more difficult the game, the longer the words will be/
higher target number of correct codes. 

The game gets random words from an XML document.  The number of words collected depends on difficulty level*/

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
    import flash.display.Shape;
    import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.media.Sound;
	import flash.media.SoundChannel;
	import flash.net.URLRequest;
	import flash.media.SoundTransform;
	
	
	//set up stage
	[SWF(width="1024", height="768", backgroundColor="#99ddff", frameRate="60")]
	
	
	
	
	public class Main extends MovieClip
	{
		var _timer:Clock;	//the time before bomb detonates. Uses my Clock class from Trick or Treat.
		var _timerText:TextField;
		var _timerFormat:TextFormat;
		var _targetWords:Vector.<String> = new Vector.<String>();	//contains words to win game
		var _randomChar:uint;		//selects random letters based on its ASCII code
		var _wordCount:uint;		//number of words needed to complete level.
		var _currentWord:uint;		//iterator.
		var _wordText:TextField;	//current word
		
		var _difficultyLevel:Vector.<TextField> = new Vector.<TextField>();	//used to select challenge
		var _mode:int;			//matches the current difficulty level.
		const EASY:int = 0;
		const NORMAL:int = 1;
		const HARD:int = 2;
		
		const MAX_WORDS:int = 50;		//max # of words in a list per difficulty
		
		//sound variables
		var _gameMusic:Sound = new Sound(new URLRequest("Defusethebomb.mp3"));	//Skullgirls tutorial theme
		var _musicChannel:SoundChannel = new SoundChannel();
		var _volume:SoundTransform = new SoundTransform(0.5);	//sets volume to 50%
		
		/*var _buzzer:Sound = new Sound(new URLRequest("Buzzer.wav"));
		var _buzzerChannel:SoundChannel = new SoundChannel();
		
		var _ding:Sound = new Sound(new URLRequest("Ding.wav"));*/
		
		//bomb variables
		var _secs:uint = 60;	//clock time
		var _bomb:Shape;
		var _fuse:Sprite;	//width corresponds to amount of time.		
		var _fuseWidth:Number = Number(_secs) * 60;	//60 is frame rate
		var _fuseCounter:Number = _fuseWidth;	//used to decrease fuse and keep consistent size
		
		//HUD 
		var _instructionText:TextField;	//tells player what must be done
		var _typeField:TextField;	//used to type in words
		var _resultText:TextField;	//feedback for when player enters a word correctly or otherwise
		var _resultTimer:int;	//number of frames the result text is displayed
		
		//white screen simulates an explosion
		var _whiteScreen:Shape = new Shape();
		
		//screen manager
		var _screenState:int;	//screen manager
		const MAIN_SCREEN:int = 0;	//used to select difficulty
		const GAME_SCREEN:int = 1;	//gameplay screen
		const RESULT_SCREEN:int = 2;	//results screen.
		var _menuCleared:Boolean = false;		//used to eliminate children from main menu
		
		
		/********Achievements******/
		const ACHIEVEMENTS:int = 7;
		var achStageComplete:Boolean = false;		//player always gets this.
		var achPerfectRun:Boolean = true;			//no mistakes made
		var achSpeedHack:Boolean = false;			//completed stage in <= 30 secs
		var achNoEraser:Boolean = true;			//backspace/delete key is never pressed
		var achDrama:Boolean = false;				//level completed in <= 1 sec
		var achCheater:Boolean = false;				//level completed in <= 10 secs
		
				
		var achievementText:Vector.<TextField> = new Vector.<TextField>();
		
		
		/************embed the xml doc*************/
		[Embed(source = "dictionary.xml", mimeType = "application/octet-stream")]
		var dictionary:Class;	//used to access the file contents
		var temp:Object = new dictionary();
		
		//cast temp into an XML object and assign it a new variable.
		var wordlist = XML(temp);
		
		
		/******************MAIN******************/
		public function Main() 
		{
			
			//initialize main screen
			_screenState = MAIN_SCREEN;

			
			//timer information
			_timer = new Clock(_secs);
			
			_timerFormat = new TextFormat();
			_timerFormat.size = 50;
			_timerFormat.bold = true;
			_timerFormat.font = "Calibri";
			
			_timerText = new TextField();
			_timerText.defaultTextFormat = _timerFormat;
			_timerText.textColor = 0x0000F0;
			_timerText.width = 160;
			_timerText.height = 80;
			_timerText.x = 450;
			_timerText.y = 200;
			
			//display time and append zeroes where necessary
			if (_timer.Seconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			
			//Text field
			_typeField = new TextField();
			_typeField.type = "input";	//need this to be able to input text
			_typeField.border = true;	//need this to be able to input text
			_typeField.borderColor = 0xFF0000;
			_typeField.textColor = 0x000000;
			_typeField.defaultTextFormat = _timerFormat;
			_typeField.background = true;
			_typeField.backgroundColor = 0xcccccc;
			_typeField.x = 350;
			_typeField.y = 600;
			_typeField.width = 300;
			_typeField.height = 60;
			
			
			//Result text
			_resultText = new TextField();
			_resultText.defaultTextFormat = _timerFormat;
			_resultText.textColor = 0xfb7b09;
			_resultText.x = 420;
			_resultText.y = 660;
			_resultText.width = 300;
			_resultText.height = 60;
			
			
			//set up difficulty modes
			_difficultyLevel.push(new TextField());		//subscript 0
			_difficultyLevel.push(new TextField());		//1
			_difficultyLevel.push(new TextField());		//2
			
			//difficulty text setup
			for (var i = 0; i < _difficultyLevel.length; i++)
			{
				_difficultyLevel[i].textColor = 0x0000ff;
				_difficultyLevel[i].defaultTextFormat = _timerFormat;
				_difficultyLevel[i].x = 200;
				_difficultyLevel[i].y = (i * 50) + 250;
				_difficultyLevel[i].width = 800;
				_difficultyLevel[i].height = 60;
				_difficultyLevel[i].selectable = false;
				
				//when player mouses over the text, it will highlight in a different colour
				_difficultyLevel[i].addEventListener(MouseEvent.MOUSE_OVER, onHover);
				
				//game will set up differently depending on what's clicked
				_difficultyLevel[i].addEventListener(MouseEvent.MOUSE_UP, mouseClicked);
				
			}
			
			_difficultyLevel[0].text = "Easy (5-letter words, 10 words)";
			_difficultyLevel[1].text = "Normal (5-8 letter words, 15 words)";
			_difficultyLevel[2].text = "Hard (9-12 letter words, 20 words)";
			
			
			//instruction
			_instructionText = new TextField();

			_instructionText.textColor = 0x000000;
			_instructionText.defaultTextFormat = _timerFormat;
			_instructionText.x = 100;
			_instructionText.y = 50;
			_instructionText.width = 800;
			_instructionText.height = 350;
			_instructionText.multiline = true;
			_instructionText.wordWrap = true;
			_instructionText.selectable = false;
			
			//achievement setup
			for (var k = 0; k < ACHIEVEMENTS; k++)
			{
				achievementText.push(new TextField());
				achievementText[k].textColor = 0x007700;
				achievementText[k].defaultTextFormat = _timerFormat;
				achievementText[k].width = 950;
				achievementText[k].height = 60;
				achievementText[k].x = 50;
				
				//the y position varies, so it's not defined here.
			}
			
			//set up names
			achievementText[0].text = "Hero: defused the bomb!";
			achievementText[1].text = "Perfect Run: made no mistakes";
			achievementText[2].text = "Speed Run: finished in less than 30 secs";
			achievementText[3].text = "No Eraser: never used Backspace/Delete";
			achievementText[4].text = "Drama Queen: 1 sec remaning on the clock!";
			achievementText[5].text = "Cheater?: finished in less than 10 secs.";
			achievementText[6].text = "Master Defuser: Completed Hard mode";
			
			
			//bomb
			_bomb = new Shape();
			_bomb.graphics.beginFill(0xcc0000);
			_bomb.graphics.drawRect(330, 300, 300, 100);
			_bomb.graphics.endFill();
			
			//fuse
			_fuse = new Sprite();
			_fuse.graphics.beginFill(0x000000);
			_fuse.graphics.drawRect(0, 0, (_fuseCounter / _fuseWidth) * 120, 20);
			_fuse.x = 630;
			_fuse.y = 340;
			_fuse.graphics.endFill();
			
			
			//white screen
			_whiteScreen.graphics.beginFill(0xffffff);
			_whiteScreen.graphics.drawRect(0, 0, stage.stageWidth, stage.stageHeight);
			_whiteScreen.alpha = 0;
			_whiteScreen.graphics.endFill();
			
			//words
			_currentWord = 0;
			
			
			_wordText = new TextField();
			_wordText.defaultTextFormat = _timerFormat;
			_wordText.textColor = 0x004400;
			_wordText.x = 440;
			_wordText.y = 500;
			_wordText.width = 700;
			_wordText.height = 70;
			
			//add instructions.  Will be displayed on all screens
			addChild(_instructionText);
			
			//add white screen.
			stage.addChild(_whiteScreen);
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
		}
		
		function onHover(event:MouseEvent):void
		{
			//changes colour of text when cursor is resting on it
			switch (event.target)
			{
				case _difficultyLevel[0]:
					_difficultyLevel[0].textColor = 0xff0000;
					_difficultyLevel[1].textColor = 0x0000ff;
					_difficultyLevel[2].textColor = 0x0000ff;
					break;
				case _difficultyLevel[1]:
					_difficultyLevel[1].textColor = 0xff0000;
					_difficultyLevel[2].textColor = 0x0000ff;
					_difficultyLevel[0].textColor = 0x0000ff;
					break;
				case _difficultyLevel[2]:
					_difficultyLevel[2].textColor = 0xff0000;
					_difficultyLevel[0].textColor = 0x0000ff;
					_difficultyLevel[1].textColor = 0x0000ff;
					break;
				
			}
			
			
		}
		
		function mouseClicked(event:MouseEvent):void
		{
			/* Upon clicking a difficulty, the game checks the XML document for the timer and word count
			attributes. Next, it will randomly pull words from the list appropriate to the difficulty. */
			
			//check what was clicked and set the difficulty
			switch(event.target)
			{
				case _difficultyLevel[0]:	//easy
					_mode = EASY;
					break;
				case _difficultyLevel[1]:	//normal
					_mode = NORMAL;
					break;
				case _difficultyLevel[2]:	//hard
					_mode = HARD;
					break;
			}
			
			
			
			//set up variables
			//_secs = wordlist.difficulty[_mode].attributes()[1];
			_wordCount = wordlist.difficulty[_mode].attributes()[2];
			
			_timer = new Clock(_secs);
			
			
			//set up word list
			var rand:int;
			for (var i = 0; i < _wordCount; i++)
			{
				_targetWords.push(new String());
				
				//pull random words from the dictionary file
				rand = Math.floor(Math.random() * MAX_WORDS);
				//trace (rand);
				_targetWords[i] = wordlist.difficulty[_mode].list[rand];
				
				//trace(_targetWords[i]);
				//trace("Word# " + i + ": " + _targetWords[i]);
				
			}
			
			_currentWord = 0;
			_wordText.text = _targetWords[_currentWord];
			
			//change screen state
			_screenState = GAME_SCREEN;
			
			//play music
			_musicChannel = _gameMusic.play();
			_musicChannel.soundTransform = _volume;
			
		}
		
		function ExitResultScreen(event:MouseEvent):void
		{
			/* When the mouse is clicked on the result screen, it will go back to the main menu. */
			
			//remove achievements from screen
			if (achStageComplete)
			{
				removeChild(achievementText[0]);	//player always gets this
			}
			
			if (achPerfectRun)
			{
				removeChild(achievementText[1]);
			}
			
			if (achSpeedHack)
			{
				removeChild(achievementText[2]);
			}
			
			if (achNoEraser)
			{
				removeChild(achievementText[3]);
			}
			
			if (achDrama)
			{
				removeChild(achievementText[4]);
			}
			
			if (achCheater)
			{
				removeChild(achievementText[5]);
			}
			
			ResetGame();
			
			stage.removeEventListener(MouseEvent.MOUSE_UP, ExitResultScreen);
		}
		
		function onKeyDown(event:KeyboardEvent):void
		{
			/*When ENTER is pressed, the game checks what was typed against
			the target word.  If correct, the next word is selected.
			Otherwise, the text box is cleared.*/
			
			switch (event.keyCode)
			{
				case Keyboard.ENTER:
					/* Take what the player entered in the input box and compare
					with the target word.*/
					var str:String = _typeField.text;	
					if (str.toLowerCase() == _wordText.text) //convert typed text to lower case
					{
						//play sound effect and move to next word
						_resultText.text = "Correct!";
						_resultText.visible = true;
						_resultTimer = 45;
						//_ding.play();	
						_currentWord++;
						
						//this line prevents out of range errors
						if (_currentWord < _wordCount)
						{
							_wordText.text = _targetWords[_currentWord];
						}
					}
					else
					{
						_resultText.text = "Wrong...";
						_resultText.visible = true;
						_resultTimer = 45;
						//wrong word, play sound
						//_buzzerChannel = _buzzer.play();
						//_buzzer.play();
						
						//lose achievement
						achPerfectRun = false;
					}
						
					_typeField.text = "";
					break;
					
				case Keyboard.BACKSPACE: case Keyboard.DELETE:
					//if either of these keys are pressed, the player can't get the 
					//"No Eraser" achievement
					achNoEraser = false;
					break;
					
			}
		}
		
		
		
		function onEnterFrame(event:Event):void
		{
			
			switch (_screenState)
			{
				case MAIN_SCREEN:
				{
					
					/*_whiteScreen.alpha -= 0.01;
					if (_whiteScreen.alpha < 0)
					{
						_whiteScreen.alpha = 0;
					}*/
					/* On this screen, the player should be able to select a difficulty */
					_instructionText.text = "***BOMB STOPPER***"
											+ "\n Defuse the bomb by typing in X number"
											+" of words in 60 seconds!";
					
					for (var i = 0; i < _difficultyLevel.length; i++)
					{
						addChild(_difficultyLevel[i]);
					}
					break;
				}
				
				case GAME_SCREEN:
				{
					if (!_menuCleared)
					{
						//clear menu screen elements
						for (var i = 0; i < _difficultyLevel.length; i++)
						{
							removeChild(_difficultyLevel[i]);
						}
						_menuCleared = true;
					}
					_instructionText.text = "Type the word you see below." + 
					"Press ENTER when you're done!";
					
					//text 
					addChild(_timerText);
					addChild(_wordText);
					addChild(_typeField);
					addChild(_resultText);
					
					//bomb graphics
					addChild(_fuse);
					addChild(_bomb);
					
					
					//check if the game is won before resuming the game
					if (_currentWord == _wordCount)
					{
						//check for certain achievements
						if (_timer.Seconds() >= 30)
						{
							achSpeedHack = true;
						}
						
						if (_timer.Seconds() >= 50)
						{
							achCheater = true;
						}
						
						if (_timer.Seconds() <= 1)
						{
							achDrama = true;
						}
						
						//go to results screen
						achStageComplete = true;
						_screenState = RESULT_SCREEN;
						
						//remove game screen graphics
						removeChild(_timerText);
						removeChild(_wordText);
						removeChild(_typeField);
						removeChild(_resultText);
						removeChild(_fuse);
						removeChild(_bomb);
					}
					else if (_timer.TimeUp())
					{
						/*if (_whiteScreen.alpha == 1)
						{
							removeChild(_whiteScreen);
							ResetGame();
						}
						else
						{
							Flash();
						}*/
						
						
						//game over! reset game
						removeChild(_timerText);
						removeChild(_wordText);
						removeChild(_typeField);
						removeChild(_resultText);
						removeChild(_fuse);
						removeChild(_bomb);
						ResetGame();
						Flash();
					}
					else
					{
						//update the game screen
						Update();
					}
					break;
				}
				
				case RESULT_SCREEN:	//show results
					ShowResults();
					break;			
			}
		}
		
		//update game screen
		function Update():void
		{
			/* Time continually decreases until it reaches 0 or player completes all words. */
			_timer.Countdown();
			_resultTimer--;
			if (_resultTimer < 0)
			{
				_resultTimer = 0;
				_resultText.visible = false;
			}
			//trace (_timer.Seconds());
			
			//decrease fuse to give the player a visual of urgency.
			_fuseCounter--;
			//trace("Fuse Counter: " + _fuseCounter / _fuseWidth * 120);
			_fuse.width = (_fuseCounter/ _fuseWidth) * 120;
				
			//update timer display
			if (_timer.Seconds() < 10 && _timer.Milliseconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":0" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() < 10 && _timer.Milliseconds() >= 10)
			{
				_timerText.text = _timer.Minutes() + ":0" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() >= 10 && _timer.Milliseconds() >= 10)
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":" + _timer.Milliseconds();
			}
			else if (_timer.Seconds() >= 10 && _timer.Milliseconds() < 10)
			{
				_timerText.text = _timer.Minutes() + ":" + _timer.Seconds() + ":0" + _timer.Milliseconds();
			}
		}
		
		//reset function
		function ResetGame():void
		{
			
			/* send player back to main menu and reset all variables */
			_screenState = MAIN_SCREEN;
			_menuCleared = false;
			_resultTimer = 0;
			_resultText.visible = false;
			_typeField.text = "";
			_currentWord = 0;
			
				
			//reset achievements
			achStageComplete = false;		
			achPerfectRun = true;			
			achSpeedHack = false;			
			achNoEraser = true;				
			achDrama = false;
			achCheater = false;
			
							
			//reset fuse size and redraw
			_fuseCounter = _fuseWidth;
			
			//stop music
			_musicChannel.stop();
			
			//remove white screen
			//removeChild(_whiteScreen);
			
				
		}
		
		//results screen
		function ShowResults():void
		{
			_instructionText.text = "Good job defusing that bomb!"
											+ "\nHere are your medals!";
					
			//show all applicable achievements and their rewards.
			achievementText[0].y = 250;
			if (achStageComplete)
			{
				addChild(achievementText[0]);	//player always gets this
			}
			
			if (achPerfectRun)
			{
				achievementText[1].y = achievementText[0].y + 50;
				addChild(achievementText[1]);
			}
			
			if (achSpeedHack)
			{
				achievementText[2].y = achievementText[0].y + 100;
				addChild(achievementText[2]);
			}
			
			if (achNoEraser)
			{
				achievementText[3].y = achievementText[0].y + 150;
				addChild(achievementText[3]);
			}
			
			if (achDrama)
			{
				achievementText[4].y = achievementText[0].y + 200;
				addChild(achievementText[4]);
			}
			
			if (achCheater)
			{
				achievementText[5].y = achievementText[0].y + 250;
				addChild(achievementText[5]);
			}
			
			stage.addEventListener(MouseEvent.MOUSE_UP, ExitResultScreen);
		
		}
		
		//makes the screen flash white. Should go into the update() function.
		function Flash():void
		{
			
			//increase the alpha
			//while (_whiteScreen.alpha != 1)
//			{
				_whiteScreen.alpha += 0.05;
			//}
			
		}

	}
	
}
