﻿/* The about screen contains credits as well as how to play the game. */

package 
{
	import flash.display.MovieClip;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.events.Event;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flashx.textLayout.formats.BackgroundColor;
	import flash.display.Stage;
	
	public class AboutScreen extends MovieClip
	{
		var quitText:TextField;
		var quitTextFormat:TextFormat;
		
		var creditText:TextField;
		
		var instructionText:TextField;
		var instructionFormat:TextFormat;
		var bg:AboutScreenBG = new AboutScreenBG();
		var bgVector:Vector.<AboutScreenBG> = new Vector.<AboutScreenBG>();
		var stageRef:Stage;	//this is important to have. With this, I can gain control of the keyboard event listener

		public function AboutScreen(stageRef:Stage) 
		{
			
			this.stageRef = stageRef;
			
			//text
			quitTextFormat = new TextFormat();
			quitTextFormat.size = 30;
			//quitTextFormat.bold = true;
			quitTextFormat.font = "Arial";
			
			quitText = new TextField();
			quitText.defaultTextFormat = quitTextFormat;	
			quitText.text = "Press [SPACE] to return to title";
			quitText.textColor = 0xFFFFFF;
			quitText.width = 500;
			quitText.height = 80;
			quitText.x = 350;
			quitText.y = 600;
			quitText.selectable = false;
			
			
			
			instructionFormat = new TextFormat();
			instructionFormat.size = 20;
			//instructionFormat.bold = true;
			instructionFormat.font = "Arial";
			
			creditText = new TextField();
			creditText.defaultTextFormat = instructionFormat;	
			creditText.text = "\t\t\t\tTrapper! \nCreated for Game Prototype Challenge v17 \n"
								+ "Game Design & Programming by Mike Murray \n"
								+ "@MikeADMurray • mmking9999.com";
			creditText.textColor = 0xFFFF00;
			creditText.width = 500;
			creditText.height = 100;
			creditText.x = 300;
			creditText.y = 100;
			creditText.selectable = false;
			
			instructionText = new TextField();
			instructionText.defaultTextFormat = instructionFormat;	
			instructionText.text = "Visit various islands and capture the local creatures for...research. \n" +
									"The creatures only move when you do, and in the opposite direction. Avoid \n" +
									"contact with the creatures and your traps! \n\n" +
									"CONTROLS: WASD to move researcher";
			instructionText.textColor = 0xFFFFFF;
			instructionText.width = 850;
			instructionText.height = 400;
			instructionText.x = 150;
			instructionText.y = 250;
			instructionText.selectable = false;
			
			bgVector.push(new AboutScreenBG());
			bgVector.push(new AboutScreenBG());
			
			bgVector[1].x = bgVector[0].width;
			
			//addChild(bg);
			for each(var b:AboutScreenBG in bgVector)
				addChild(b);
			addChild(creditText);
			addChild(instructionText);
			addChild(quitText);
			
			stageRef.addEventListener(KeyboardEvent.KEY_UP, closeScreen);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		function closeScreen(event:KeyboardEvent):void
		{
			switch(event.keyCode)
			{
				case Keyboard.SPACE:
					//close screen
					stageRef.removeEventListener(KeyboardEvent.KEY_UP, closeScreen);
					removeEventListener(Event.ENTER_FRAME, onEnterFrame);
					Main.aboutScreenOff = false;		//this causes the child to be removed in main, closing the screen
					Main.fadeScreen.alpha = 1;
					trace ("Closed about screen");
					break;
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			/* reduce fadescreen alpha. */
			if (Main.fadeScreen.alpha > 0)
				Main.fadeScreen.alpha -= 0.02;
			
			//scroll background.
			for each(var b:AboutScreenBG in bgVector)
			{
				b.x--;
				if (b.x + b.width < 0)
					b.x = stage.stageWidth - 1;
			}
			
		}

	}
	
}
