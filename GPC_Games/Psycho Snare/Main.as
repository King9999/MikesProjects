/* Contains the main game loop. */

package  
{
	import flash.display.MovieClip;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.events.Event;
	import flash.display.Sprite;
	
	public class Main extends MovieClip
	{
		//var shopScreen:ShopScreen = new ShopScreen();
		var player:Player = new Player();

		public function Main() 
		{
			player.x = 512;
			player.y = 324;
			
			addChild(player);
			
			//event listeners
			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyDown);
			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		public function onKeyDown(event:KeyboardEvent):void
		{
			/******Player 1 Controls********/
			//P1 Walk left
			if (event.keyCode == Keyboard.A)
			{
				
				player.SetVelocityX(-3);
			}
				
			//P1 Walk right
			if (event.keyCode == Keyboard.D)
			{
				player.SetVelocityX(3);
				
			}
								
			//P1 Walk down
			if (event.keyCode == Keyboard.S)
			{
				
				player.SetVelocityY(3);
			}
			
			//P1 Walk up
			if (event.keyCode == Keyboard.W)
			{
				
				player.SetVelocityY(-3);
			}
			
			//P2 Action button
			//if (event.keyCode == Keyboard.C)
//			{
//				//perform the player-specific action
//				player.UseAction();
//			}
				
		}
		
		/* This function checks when a key is released */
		public function onKeyUp(event:KeyboardEvent):void
		{
			
				/******Player 1 Controls********/
				if(event.keyCode == Keyboard.A || event.keyCode == Keyboard.D)
				{
					player.SetVelocityX(0);
				}
										
				if (event.keyCode == Keyboard.S || event.keyCode == Keyboard.W)
				{
					player.SetVelocityY(0);
				}
	
				
			
		}
		
		function onEnterFrame(event:Event):void
		{
			/**********update player movement*************/
			player.x += player.GetVelocityX();
			player.y += player.GetVelocityY();
		}

	}
	
}
