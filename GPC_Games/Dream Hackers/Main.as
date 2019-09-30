/* December 2011
	Dream Hackers by Haskell (Mike) Murray
	Created for Game Prototype Challenge v10
	
	This class is what runs the entire game.  All other classes should be imported into
	this class. Scenes are used to switch between levels. */

package 
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;
	import flash.text.TextField;

	//used for collision checking.
	
	public class Main extends MovieClip
	{
		var npc:NPC = new NPC("Wolfwood");
		var player:Player = new Player();
		
		var npcText:TextField = new TextField();
		var actionButtonPressed:Boolean = false;
		var textTime:int = 0;
		

		public function Main() 
		{
			
			gotoAndStop(1, "Field");
	
			
			//set player position
			player.x = 691;
			player.y = 647;
			
			//set NPC position
			npc.x = 290;
			npc.y = 267;
			
			//set NPC dialogue
			npc.SetDialogue(0, "Hello!");
			//npc.SetDialogue(1, "Hello again!");
			
			//npcText = new TextField();
			npcText.textColor = 0xFFFFFF;
			//npcText.defaultTextFormat = _winFormat;
				
			//text borders
			npcText.width = 800;
			npcText.height = 80;
				
			//position the text in the black border
			npcText.x = stage.stageWidth / 3;
			npcText.y = stage.stageHeight / 2;
			
			//add objects
			addChild(npc);
			addChild(player);
			
			
			//event listeners
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
			
		}
		
		public function onKeyDown(event:KeyboardEvent):void
		{
			/******Player Controls********/
			//Walk left
			if (event.keyCode == Keyboard.A)
			{
				
				player.SetVelocityX(-3);
			}
				
			//Walk right
			if (event.keyCode == Keyboard.D)
			{
				player.SetVelocityX(3);
				
			}
								
			//Walk down
			if (event.keyCode == Keyboard.S)
			{
				
				player.SetVelocityY(3);
			}
			
			//Walk up
			if (event.keyCode == Keyboard.W)
			{
				
				player.SetVelocityY(-3);
			}
			
			//Action button
			if (event.keyCode == Keyboard.SPACE)
			{
				actionButtonPressed = true;
			}
		}
		
		/* This function checks when a key is released. Player stops moving when
			no key is pressed*/
		public function onKeyUp(event:KeyboardEvent):void
		{
			
			/******Player Controls********/
			if(event.keyCode == Keyboard.A || event.keyCode == Keyboard.D)
			{
				player.SetVelocityX(0);
			}
										
			if (event.keyCode == Keyboard.S || event.keyCode == Keyboard.W)
			{
				player.SetVelocityY(0);
			}
			
			//Action button
			if (event.keyCode == Keyboard.SPACE)
			{
				actionButtonPressed = false;
			}
				
		}
		
		function onEnterFrame(event:Event):void
		{
			//SetNextLevel();
			player.x += player.GetVelocityX();
			player.y += player.GetVelocityY();
			
			//check if player is talking to an NPC.
			if (player.hitTestObject(npc) && actionButtonPressed)
			{
				npcText.text = npc.DisplayDialogue();
				addChild(npcText);
				textTime = 120;
			}
			
			
		}


	}
	
}
