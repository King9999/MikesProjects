/* In Fly Swat, you must destroy random flies. They move erratically, so they may be hard to swat! */

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
						
		//consts to determine maximum objects on screen
		const MAX_FLIES:int = 20;
		
		var _fly:Vector.<Sprite> = new Vector.<Sprite>();		//flies
		var _flyCounter:int = 0;	//controls number of flies on screen
		
		var _swatter:Sprite;									//used to hit flies
		
		var _spawnTimer:int;
		
				
		//HUD text
		var _textFormat:TextFormat;
		var _instructionText:TextField;	//tells player what must be done
		var _killCount:int = 0;
		var _killCountText:TextField;

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
			_instructionText.text = "Smack all of the flies!";
			
			//kill counter
			_killCountText = new TextField();
			_killCountText.textColor = 0x990000;
			_killCountText.defaultTextFormat = _textFormat;
			_killCountText.x = 100;
			_killCountText.y = 700;
			_killCountText.width = 300;
			_killCountText.height = 60;
			_killCountText.text = "Kill Count: " + _killCount;
			
			//swatter
			_swatter = new Sprite();
			_swatter.graphics.beginFill(0x0000cc);
			_swatter.graphics.drawRect(300, 500, 30, 30);
			_swatter.graphics.endFill();
			
			_swatter.addEventListener(MouseEvent.MOUSE_MOVE, mouseMoved);
			_swatter.addEventListener(MouseEvent.MOUSE_UP, buttonReleased);
			
			//swatter should be drawn on top of the flies
			stage.addChild(_swatter);
									
			//create flies
			/*for (var j = 0; j < MAX_FLIES; j++)
			{
				_fly.push(new Sprite());
				_fly[j].graphics.beginFill(0x00ff00);
				_fly[j].graphics.drawCircle(Math.floor(Math.random() * 850) + 50,
										   Math.floor(Math.random() * 650) + 50, 8);
				_fly[j].graphics.endFill();
			}*/
			
			
			
			//add everything
			addChild(_instructionText);
			addChild (_killCountText);
			
			
			addEventListener(Event.ENTER_FRAME, onEnterFrame);
		}
		
		function mouseMoved(event:MouseEvent):void
		{
			event.target.startDrag();
		}
				
		function buttonReleased(event:MouseEvent):void
		{
			//check if the swatter is on top of a fly
			for (var i = 0; i < _fly.length; i++)
			{
				if (_swatter.hitTestObject(_fly[i]))
				{
					//kill the fly
					removeChild(_fly[i]);
					_killCount++;
				}
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			/* A timer controls when flies appear on screen. */
			_spawnTimer--;
			if (_spawnTimer < 0)
			{
				//spawn a fly
				if (_flyCounter < MAX_FLIES)
				{
					_fly.push(new Sprite());
					_fly[_flyCounter].graphics.beginFill(0x00ff00);
					_fly[_flyCounter].graphics.drawCircle(Math.floor(Math.random() * 850) + 50,
											   Math.floor(Math.random() * 650) + 50, 8);
					_fly[_flyCounter].graphics.endFill();
					addChild(_fly[_flyCounter]);
					_flyCounter++;
					
					//reset timer
					_spawnTimer = Math.floor(Math.random() * 120) + 60;
				}
				else
				{
					_spawnTimer = 0;
				}
			}
			
			/* For all existing flies, they should move erratically */
			for (var i = 0; i < _fly.length; i++)
			{
				_fly[i].x = Math.floor(Math.random() * _fly[i].x + (Math.random() * -30) + 50);
				_fly[i].y = Math.floor(Math.random() * _fly[i].y + (Math.random() * -10) + 30);
			}
			
			//update killcount
			_killCountText.text = "Kill Count: " + _killCount;
		}

	}
	
}
