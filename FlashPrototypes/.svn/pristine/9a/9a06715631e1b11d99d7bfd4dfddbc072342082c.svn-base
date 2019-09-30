/* Pollen Panic is based on my GPC game. Eliminate flowers using your weapon. */

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
		const MAX_FLOWERS:int = 20;
		
		var _flower:Vector.<Sprite> = new Vector.<Sprite>();		//flowers
		var _flowerCounter:int = 0;	//controls number of flies on screen
		
		var _player:Sprite;									//player
		var _bullet:Vector.<Sprite> = new Vector.<Sprite>();
		var _fireRate:int = 0;			//controls bullets
		var _bulletCounter:int = 0;
		
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
			_instructionText.text = "Eliminate deadly flowers!";
			
			//kill counter
			_killCountText = new TextField();
			_killCountText.textColor = 0x990000;
			_killCountText.defaultTextFormat = _textFormat;
			_killCountText.x = 100;
			_killCountText.y = 700;
			_killCountText.width = 300;
			_killCountText.height = 60;
			_killCountText.text = "Kill Count: " + _killCount;
			
			//player
			_player = new Sprite();
			_player.graphics.beginFill(0x0000cc);
			_player.graphics.drawRect(300, 500, 50, 50);
			_player.graphics.endFill();
			
			_player.addEventListener(MouseEvent.MOUSE_MOVE, mouseMoved);
			_player.addEventListener(MouseEvent.MOUSE_UP, buttonReleased);
			
			//swatter should be drawn on top of the flies
			stage.addChild(_player);
									
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
			//fire bullet
			if (_fireRate <= 0)
			{
				//create a bullet
				_bullet.push(new Sprite());
				addChild(_bullet[_bulletCounter]);
				_bullet[_bulletCounter].graphics.beginFill(0x008800);
				_bullet[_bulletCounter].graphics.drawCircle(mouseX, mouseY, 5);
				_bullet[_bulletCounter].graphics.endFill();
				
				_bulletCounter++;
				
				_fireRate = 12;
			}
		}
		
		function onEnterFrame(event:Event):void
		{
			//reduce fire rate
			_fireRate--;
			if (_fireRate < 0)
			{
				_fireRate = 0;
			}
			
			
			/* A timer controls when flies appear on screen. */
			_spawnTimer--;
			if (_spawnTimer < 0)
			{
				//spawn a fly
				if (_flowerCounter < MAX_FLOWERS)
				{
					_flower.push(new Sprite());
					_flower[_flowerCounter].graphics.beginFill(0xff0000);
					_flower[_flowerCounter].graphics.drawCircle(Math.floor(Math.random() * 850) + 50,
											   Math.floor(Math.random() * 650) + 50, 12);
					_flower[_flowerCounter].graphics.endFill();
					addChild(_flower[_flowerCounter]);
					_flowerCounter++;
					
					//reset timer
					_spawnTimer = Math.floor(Math.random() * 120) + 60;
				}
				else
				{
					_spawnTimer = 0;
				}
			}
			
			
			
			/* flowers shoot pollen */
			for (var i = 0; i < _flower.length; i++)
			{
				//_fly[i].x = Math.floor(Math.random() * _fly[i].x + (Math.random() * -30) + 50);
//				_fly[i].y = Math.floor(Math.random() * _fly[i].y + (Math.random() * -10) + 30);
			}
			
			//check existing bullets
			if (_bullet.length > 0)
			{
				for (var j = 0; j < _bullet.length; j++)
				{
					_bullet[j].y += -8;
					/*if (_bullet[j].y < 0)
					{
						//kill the bullet
						//removeChild(_bullet[j]);
						
						_bulletCounter--;
					}*/
					
					for (var k = 0; k < _flower.length; k++)
					{
						if (_bullet[j].hitTestObject(_flower[k]))
						{
							//removeChild(_flower[k]);
							//removeChild(_bullet[j]);
							//_flowerCounter--;
							//_bulletCounter--;
						}
					}
				}
			}
			
			//update killcount
			_killCountText.text = "Kill Count: " + _killCount;
		}

	}
	
}
