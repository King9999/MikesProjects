﻿/* Trapper for Game Prototype Challenge v17 by Haskell "Mike" Murray - April 2013
This is a puzzle game where the objective is to trap all creatures on each island (level). The creatures only move when the player moves,
and in opposite directions.  The player must think about where to move in order to trap the creatures, and at the same avoid
contact with traps and the creatures. 

The game uses XML files to load levels.
*/

package 
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
	import flash.display.Shape;
	import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	//set up screen
	[SWF(width = "1024",height = "768", backgroundColor = "#000000", frameRate = "60")]

	public class Main extends MovieClip
	{
		trace ("testing");
		
		const START:uint = 0;	//used to identify the buttons in the vector
		const ABOUT:uint = 1;
		const INFO:uint = 2;	//tells player to use SPACE to make a selection.
		var currentMenu:uint = START;	//used to change the screen.
		
		//screens
		//var titleScreen:TitleScreen = new TitleScreen();
		var aboutScreen:AboutScreen;
		var gameScreen:GameScreen;
		static var fadeScreen:FadeScreen;		//used to transition between screens.
		
		//title screen elements
		var gameTitle:Title = new Title();
		var cursor:Cursor = new Cursor();
		
		//text
		static var gameTextFormat:TextFormat = new TextFormat();
		var gameText:Vector.<TextField>;	//contains "Start" and "About"
		
		/*//sound variables
		var gameMusic:Sound = new Sound(new URLRequest("Super Aleste - Area 1.mp3"));	//Space Megaforce area 1 music
		var musicChannel:SoundChannel = new SoundChannel();
		var _volume:SoundTransform = new SoundTransform(0.4);	//sets volume to 40%*/
		//static var clickSound:Sound = new ClickSound();		//used on buttons and tiles
		//static var beep:Sound = new FaceSelect();
		
		//bools for the different screens
		static var aboutScreenOff:Boolean = true;	//used to open/close screen
		static var gameScreenOff:Boolean = true;
		
		/************embed the xml doc*************/
		[Embed(source = "levels.xml", mimeType = "application/octet-stream")]
		private static var levelList:Class;	//used to access the file contents
		private static var temp:Object = new levelList();
		
		//cast temp into an XML object and assign it a new variable.
		static var levels = XML(temp);
		
		
		public function Main()
		{
			/******TITLE***********/
			gameTitle.x = 350;
			gameTitle.y = 120;
			
			//title2.x = stage.stageWidth;
//			trace ("X: " + title2.x);
//			title2.y = 220;
			
			cursor.x = 350;
			cursor.y = 500;
			//cursor.visible = false;
			
			
			/*****Text*************/
			//gameTextFormat = new TextFormat();
			gameTextFormat.size = 50;
			gameTextFormat.bold = true;
			gameTextFormat.font = "Calibri";
			
			gameText = new Vector.<TextField>();
			gameText.push(new TextField());		//Start
			gameText.push(new TextField());		//About
			gameText.push(new TextField());		//info
			
			//menu setup
			gameText[START].defaultTextFormat = gameTextFormat;
			gameText[START].text = "Start";
			gameText[START].textColor = 0xFFFFFF;
			gameText[START].width = 160;
			gameText[START].height = 80;
			gameText[START].x = cursor.x + 100;
			gameText[START].y = 500;
			gameText[START].selectable = false;
			
			
			gameText[ABOUT].defaultTextFormat = gameTextFormat;
			gameText[ABOUT].text = "About";
			gameText[ABOUT].textColor = 0xFFFFFF;
			gameText[ABOUT].width = 160;
			gameText[ABOUT].height = 80;
			gameText[ABOUT].x = cursor.x + 100;
			gameText[ABOUT].y = 560;
			gameText[ABOUT].selectable = false;
			
			gameText[INFO].defaultTextFormat = gameTextFormat;
			gameText[INFO].text = "Press [Space] to select";
			gameText[INFO].textColor = 0xFFFFFF;
			gameText[INFO].width = 470;
			gameText[INFO].height = 80;
			gameText[INFO].x = cursor.x;
			gameText[INFO].y = 640;
			gameText[INFO].selectable = false;
			
		
			fadeScreen = new FadeScreen();
			/******Adding all children ***********/
			
			addChild(gameTitle);
			addChild(cursor);
			addChild(gameText[START]);
			addChild(gameText[ABOUT]);
			addChild(gameText[INFO]);
			
			//screens
			/*aboutScreen = new AboutScreen(stage);
			//gameScreen = new GameScreen(stage);
			addChild(aboutScreen);
			aboutScreen.visible = false;*/
	
			//addChild(gameScreen);
			//gameScreen.visible = false;
			
			//event listeners
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			
			//play music
			//PlayMusic(musicChannel, gameMusic);
			//musicChannel = gameMusic.play(0, 10000);
			//musicChannel.soundTransform = _volume;
			
		}
	
		
		/*Plays music and loops it infinitely. Flash has no built-in way of doing this */
		/*function PlayMusic(channel:SoundChannel, sound:Sound):void
		{
			channel = sound.play();
			channel.soundTransform = _volume;
			channel.addEventListener(Event.SOUND_COMPLETE, Replay);
		}*/
		
		/*function Replay(event:Event):void
		{
			//remove the event listener and start music again.
			PlayMusic(musicChannel, gameMusic);
			SoundChannel(event.target).removeEventListener(event.type, Replay);
		}*/
		
		/* On the title screen, the only thing the keyboard does is move the cursor up and down with W or S, and allow the player to 
		make a selection with Enter. */
		function onKeyUp(event:KeyboardEvent):void
		{
			switch (event.keyCode)
			{
				case Keyboard.S: case Keyboard.DOWN:
					//move cursor down, or back to starting position if it already moved
					if (cursor.y != 560)
					{
						cursor.y = 560;
						currentMenu = ABOUT;
					}
					else
					{
						cursor.y = 500;
						currentMenu = START;
					}
					break;
						
				case Keyboard.W: case Keyboard.UP:
					//move cursor up
					if (cursor.y != 500)
					{
						cursor.y = 500;
						currentMenu = START;
					}
					else
					{
						cursor.y = 560;
						currentMenu = ABOUT;
					}
					break;
					
				case Keyboard.SPACE:
					//check which menu is highlighted and change screen
					if (currentMenu == START)
					{
						gameScreen = new GameScreen(stage);
						addChild(gameScreen);
						fadeScreen.alpha = 1;
						addChild(fadeScreen);
						stage.removeEventListener(KeyboardEvent.KEY_UP, onKeyUp);
						trace ("Opened game screen");
					}
					else
					{
						aboutScreen = new AboutScreen(stage);
						addChild(aboutScreen);
						fadeScreen.alpha = 1;
						addChild(fadeScreen);
						stage.removeEventListener(KeyboardEvent.KEY_UP, onKeyUp);
						//set cursor back to default
						cursor.y = 500;
						currentMenu = START;
						trace ("Opened about screen");
					}
					break;
				
			}
			trace (currentMenu);
		}
		
		function onEnterFrame(event:Event):void
		{
			//The title will begin off-screen and quickly scroll to the middle. Title 1 comes from the left,
			//and title 2 from the right.
			var center:Number = 350;		//centre of the screen.
			//if (gameTitle.x < center)
//			{
//				gameTitle.x += 15;
//			}
//			
//			if (title2.x > center)
//			{
//				title2.x -= 15;
//			}
//			
//			//when the titles reach the middle, a sound should play and the menu should appear.
//			if (gameTitle.x >= center || title2.x <= center)
//			{
//				gameText[ABOUT].visible = true;
//				gameText[START].visible = true;
//				cursor.visible = true;
//			}
			
			//check if a child screen should close. If we enter this statement, the child will be terminated
			if (!aboutScreenOff)
			{
				removeChild(aboutScreen);
				removeChild(fadeScreen);
				aboutScreenOff = true;
				//restore keyboard control to the title screen
				stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			}
			
			if (!gameScreenOff)
			{
				removeChild(gameScreen);
				gameScreenOff = true;
				//restore keyboard control to the title screen
				stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			}
			
		}

	}

}