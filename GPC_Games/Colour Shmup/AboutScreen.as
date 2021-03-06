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
		var bg:Background = new Background();
		var stageRef:Stage;	//this is important to have. With this, I can gain control of the keyboard event listener

		public function AboutScreen(stageRef:Stage) 
		{
			
			this.stageRef = stageRef;
			
			//text
			quitTextFormat = new TextFormat();
			quitTextFormat.size = 30;
			quitTextFormat.bold = true;
			quitTextFormat.font = "Calibri";
			
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
			instructionFormat.bold = true;
			instructionFormat.font = "Calibri";
			
			creditText = new TextField();
			creditText.defaultTextFormat = instructionFormat;	
			creditText.text = "Colour Shmup \nCreated for Game Prototype Challenge v16 \n"
								+ "Game Design & Programming by Mike Murray \n"
								+ "Twitter: @MikeADMurray • mmking9999.com";
			creditText.textColor = 0x00FF00;
			creditText.width = 500;
			creditText.height = 100;
			creditText.x = 300;
			creditText.y = 100;
			creditText.selectable = false;
			
			instructionText = new TextField();
			instructionText.defaultTextFormat = instructionFormat;	
			instructionText.text = "Absorb enemies with the same colour as you by touching them.  Absorbing enemies fills up your \n" +
									"Rainbow Gauge.  The Rainbow Gauge keeps you alive and decimates your enemies (when full)! \n\n" +
									"SURVIVAL NOTES\n" +
									"**Touching an enemy who is not your colour results in damage to your ship and a loss of \n" +
									"energy from the Rainbow Gauge. \n" +
									"**Touching an enemy of an OPPOSING colour results in MORE damage!\n" +
									"**If you touch an enemy not of your colour while the gauge is empty, you die. \n" + 
									"**Destroy enemies of an opposing colour to receive a power up! \n\n" +
									"CONTROLS: WASD to move ship, SPACE to shoot, directional keys to change colour";
			instructionText.textColor = 0xFFFFFF;
			instructionText.width = 850;
			instructionText.height = 400;
			instructionText.x = 150;
			instructionText.y = 250;
			instructionText.selectable = false;
			
			addChild(bg);
			addChild(creditText);
			addChild(instructionText);
			addChild(quitText);
			
			stageRef.addEventListener(KeyboardEvent.KEY_UP, closeScreen);
		}
		
		function closeScreen(event:KeyboardEvent):void
		{
			switch(event.keyCode)
			{
				case Keyboard.SPACE:
					//close screen
					stageRef.removeEventListener(KeyboardEvent.KEY_UP, closeScreen);
					Main.aboutScreenOff = false;		//this causes the child to be removed in main, closing the screen
					trace ("Closed about screen");
					break;
			}
		}

	}
	
}
