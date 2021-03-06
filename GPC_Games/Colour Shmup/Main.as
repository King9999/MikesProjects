﻿/* Colour Shmup for Game Prototype Challenge v16 by Haskell "Mike" Murray - February 2013
This is a space shooter that involves shooting enemies of a colour opposite the player and avoiding those same enemies.  Enemies of a 
like colour can be absorbed by touching them; doing so fills up a rainbow gauge that is used to devastate everything in sight while invulnerable.*/

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
	[SWF(width = "1024",height = "768", backgroundColor = "#000050", frameRate = "60")]

	public class Main extends MovieClip
	{
		trace ("testing");
		
		const START:uint = 0;	//used to identify the buttons in the vector
		const ABOUT:uint = 1;
		var currentMenu:uint = START;	//used to change the screen.
		
		//screens
		//var titleScreen:TitleScreen = new TitleScreen();
		var aboutScreen:AboutScreen;
		var gameScreen:GameScreen;
		
		//title screen elements
		var title1:Title1 = new Title1();
		var title2:Title2 = new Title2();
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
		
		public function Main()
		{
			/******TITLE***********/
			title1.x = -title1.width;	//starts offscreen.
			title1.y = 120;
			
			title2.x = stage.stageWidth;
			trace ("X: " + title2.x);
			title2.y = 220;
			
			cursor.x = 350;
			cursor.y = 500;
			cursor.visible = false;
			
			
			/*****Text*************/
			//gameTextFormat = new TextFormat();
			gameTextFormat.size = 50;
			gameTextFormat.bold = true;
			gameTextFormat.font = "Calibri";
			
			gameText = new Vector.<TextField>();
			gameText.push(new TextField());		//Start
			gameText.push(new TextField());		//About
			
			//menu setup
			gameText[START].defaultTextFormat = gameTextFormat;
			gameText[START].text = "Start";
			gameText[START].textColor = 0xFFFFFF;
			gameText[START].width = 160;
			gameText[START].height = 80;
			gameText[START].x = cursor.x + 100;
			gameText[START].y = 500;
			gameText[START].selectable = false;
			gameText[START].visible = false;
			
			gameText[ABOUT].defaultTextFormat = gameTextFormat;
			gameText[ABOUT].text = "About";
			gameText[ABOUT].textColor = 0xFFFFFF;
			gameText[ABOUT].width = 160;
			gameText[ABOUT].height = 80;
			gameText[ABOUT].x = cursor.x + 100;
			gameText[ABOUT].y = 560;
			gameText[ABOUT].selectable = false;
			gameText[ABOUT].visible = false;
		
	
			/******Adding all children ***********/
			addChild(title1);
			addChild(title2);
			addChild(cursor);
			addChild(gameText[START]);
			addChild(gameText[ABOUT]);
			
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
				case Keyboard.S:
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
						
				case Keyboard.W:
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
						stage.removeEventListener(KeyboardEvent.KEY_UP, onKeyUp);
						trace ("Opened game screen");
					}
					else
					{
						aboutScreen = new AboutScreen(stage);
						addChild(aboutScreen);
						stage.removeEventListener(KeyboardEvent.KEY_UP, onKeyUp);
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
			if (title1.x < center)
			{
				title1.x += 15;
			}
			
			if (title2.x > center)
			{
				title2.x -= 15;
			}
			
			//when the titles reach the middle, a sound should play and the menu should appear.
			if (title1.x >= center || title2.x <= center)
			{
				gameText[ABOUT].visible = true;
				gameText[START].visible = true;
				cursor.visible = true;
			}
			
			//check if a child screen should close. If we enter this statement, the child will be terminated
			if (!aboutScreenOff)
			{
				removeChild(aboutScreen);
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