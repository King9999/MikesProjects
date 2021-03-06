﻿/* October 2011 
TRICK OR TREAT
A game for Game Prototype Challenge v9
By Haskell (Mike) Murray

This class manages all of the different game components. The main game loop is here.

HOUSES
-At the start of the game, there will be a 50% chance that a house will have candy.
-At each frame, the game will check if a house is empty. If it is, then there's a 0.00002% chance
it'll get restocked.
-Houses normally have 1-10 pieces of candy. Each time a house is stocked, there will be a 10% chance 
that a house will have 20 pieces of candy.

CLOCK
Each game is 3 minutes. This cannot be changed at present.

PLAYER
-The player selects their costume, while the CPU controls the others.
-The AI players always go for the best houses.

*/



package  
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import flash.display.Sprite;
	import flash.sampler.Sample;
	import flash.utils.getTimer;

	//used for collision checking.
	
	public class Main extends MovieClip
	{
		
		private var _houseVector:Vector.<House> = new Vector.<House>();
		private var _candyText:Vector.<TextField> = new Vector.<TextField>();
		private var _textFormat:TextFormat;
		private var _clockText:TextField;
		private var _clockFormat:TextFormat;
		
		
		var clock:Clock;
		var selectScreen:SelectScreen = new SelectScreen();
		
		private var _collectBox:Vector.<Sprite> = new Vector.<Sprite>();
		
		//when houses are emptied, they undergo cooldown before they get a chance
		//to restock again.
		private var _restockTime:Vector.<int> = new Vector.<int>();
		
		//player variables
		var player:Vector.<Player> = new Vector.<Player>();
		private var _yell:Vector.<YellBubble> = new Vector.<YellBubble>();
		private var _yellTimer:Vector.<int> = new Vector.<int>();	//duration of yell
		private var _isYellActive:Vector.<Boolean> = new Vector.<Boolean>();
		private var _invulTimer:Vector.<int> = new Vector.<int>();
		
		private var _collectTimer:Vector.<int> = new Vector.<int>;	//used to control the rate of collecting candy
		
		//HUD variables
		private var _playerHUD:Vector.<Sprite> = new Vector.<Sprite>();	//icons for HUD
		private var _candyHUD:Vector.<Sprite> = new Vector.<Sprite>();	//candy icon
		private var _candyCountHUD:Vector.<TextField> = new Vector.<TextField>();	//number of candy
		private var _hudFormat:TextFormat;
		private var _actionHUD:Vector.<Sprite> = new Vector.<Sprite>();
		
		//candy variables
		private var _candy:Vector.<CandyIcon> = new Vector.<CandyIcon>();
		private var _isPickedUp:Vector.<Boolean> = new Vector.<Boolean>();	//prevents candy from being picked up multiple times
		
		//Win text
		private var _winText:TextField = new TextField();
		private var _winFormat:TextFormat = new TextFormat();
		private var _winTextTimer:int;
		private var _winTextDisplayed:Boolean;
		
		//select screen state
		private var _isSelectScreenOn:Boolean;
		private var _playerPick:Vector.<int> = new Vector.<int>;
		
		
		

		public function Main() 
		{
			_isSelectScreenOn = true;	//start with the select screen displayed
			
			/*************PLAYER SETUP**************/
			
			//player[0] = new Ghost();	//player 1
//			player[1] = new Knight();	//player 2
			
			
			//Set P1 & P2 position
			//player[0].x = 78;
//			player[0].y = 262;
//			
//			player[1].x = 747;
//			player[1].y = 564;
			
			_invulTimer[0] = 0;
			_invulTimer[1] = 0;
			
			//addChild(player[0]);
//			addChild(player[1]);
			
			/************HUD Setup************/
			//Add player Icons
			//these are the default icons. I'm leaving this here because I don't want to mess with the code
			//too much.
			_playerHUD[0] = new GhostIcon();
			_playerHUD[0].x = 50;
			_playerHUD[0].y = 20;
			
			_playerHUD[1] = new KnightIcon();
			_playerHUD[1].x = 800;
			_playerHUD[1].y = 20;
					
			//addChild(_playerHUD[0]);
			//addChild(_playerHUD[1]);
			
			//candy and action prompt info
			_hudFormat = new TextFormat();
			_hudFormat.size = 22;
			_hudFormat.bold = true;
			_hudFormat.font = "Calibri";
			
			for (var i = 0; i < 2; i++)
			{
				_candyHUD[i] = new CandyIcon();
				_candyHUD[i].x = _playerHUD[i].x + 60;
				_candyHUD[i].y = _playerHUD[i].y + 5;
				
				_candyCountHUD[i] = new TextField();
				_candyCountHUD[i].textColor = 0xFFFFFF;
				_candyCountHUD[i].defaultTextFormat = _hudFormat;
					
				//text borders
				_candyCountHUD[i].width = 60;
				_candyCountHUD[i].height = 40;
					
				//position the text in the black border
				_candyCountHUD[i].x = _candyHUD[i].x + 25;
				_candyCountHUD[i].y = _candyHUD[i].y;
				//_candyCountHUD[i].text = "x " + player[i].GetCandyAmount().toString();
				
				//action icon
				_actionHUD[i] = new ActionIcon();
				_actionHUD[i].x = _candyHUD[i].x + 50;
				_actionHUD[i].y = _candyHUD[i].y + 50;
				
				//add everything
				addChild(_candyHUD[i]);
				addChild(_candyCountHUD[i]);
				addChild(_actionHUD[i]);
			}
			
			//set up player-used variables
			//yell bubble. Used only at certain points.
			for (var p = 0; p < 2; p++)
			{
				_yell.push(new YellBubble());
				_yellTimer.push(0);
				_isYellActive.push(false);
				_collectTimer.push(0);
			}
			
			
			
			
			/*************WIN TEXT SETUP ************/
			
			
			//_winFormat = new TextFormat();
			_winFormat.size = 50;
			_winFormat.bold = true;
			_winFormat.font = "Baveuse";
			
			//_winText = new TextField();
			_winText.textColor = 0xFFFFFF;
			_winText.defaultTextFormat = _winFormat;
				
			//text borders
			_winText.width = 800;
			_winText.height = 80;
				
			//position the text in the black border
			_winText.x = stage.stageWidth / 3;
			_winText.y = stage.stageHeight / 2;
			
			
			
			
			
			/*************CLOCK SETUP ************/
			//set up clock
			clock = new Clock(120);		//2 minutes
			_clockFormat = new TextFormat();
			_clockFormat.size = 50;
			_clockFormat.bold = true;
			_clockFormat.font = "Calibri";
			
			_clockText = new TextField();
			_clockText.textColor = 0xFFFFFF;
			_clockText.defaultTextFormat = _clockFormat;
				
			//text borders
			_clockText.width = 100;
			_clockText.height = 80;
				
			//position the text in the black border
			_clockText.x = stage.stageWidth / 2.25;
			_clockText.y = 0;
			
			if (clock.GetSeconds() < 10)
			{
				//append a zero in front of the seconds
				_clockText.text = clock.GetMinutes().toString() + ":0" + clock.GetSeconds().toString();
			}
			else
			{
				_clockText.text = clock.GetMinutes().toString() + ":" + clock.GetSeconds().toString();
			}
			
			//display clock
			addChild(_clockText);
			
			/******************HOUSE SETUP ***************/
			
			//Set up each house in the vector.				
			_houseVector.push(house0);
			_houseVector.push(house1);
			_houseVector.push(house2);
			_houseVector.push(house3);
			_houseVector.push(house4);
			_houseVector.push(house5);
			_houseVector.push(house6);
			_houseVector.push(house7);
			_houseVector.push(house8);
			_houseVector.push(house9);
			
			
			
			//text formatting for displaying the candy amounts
			_textFormat = new TextFormat();
			_textFormat.size = 30;
			_textFormat.bold = true;
			_textFormat.font = "Calibri";
			
			
			/*************this massive for loop initializes many things.*************/
			for (var i = 0; i < _houseVector.length; i++)
			{
				//text information
				_candyText[i] = new TextField();
				_candyText[i].textColor = 0xFFFFFF;
				_candyText[i].defaultTextFormat = _textFormat;
				
				//text borders
				_candyText[i].width = 40;
				_candyText[i].height = 40;
				
				//position the text on the house.
				_candyText[i].x = _houseVector[i].x + 52;
				_candyText[i].y = _houseVector[i].y + 15;
			
			
				//Randomly determine which houses start with candy
				var chance:int;
			
				chance = Math.floor(Math.random() * 100) + 1;
				if (chance <= 50)
				{
					_houseVector[i].StockUp();
					_candyText[i].text = _houseVector[i].GetCandyAmount().toString();
					addChild(_candyText[i]);	//draw text only when candy is available
				}
				
				
				//set collision boxes for collecting candy
				_collectBox[i] = new Sprite();
				//_collectBox[i].graphics.beginFill(0xFF0000);
				_collectBox[i].graphics.drawRect( _houseVector[i].x+40, _houseVector[i].y+130, 50, 20);
				//_collectBox[i].graphics.endFill();
				addChild(_collectBox[i]);
				
				//cooldown timer
				_restockTime[i] = 0;
			}
		
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
				
				player[0].SetVelocityX(-3);
			}
				
			//P1 Walk right
			if (event.keyCode == Keyboard.D)
			{
				player[0].SetVelocityX(3);
				
			}
								
			//P1 Walk down
			if (event.keyCode == Keyboard.S)
			{
				
				player[0].SetVelocityY(3);
			}
			
			//P1 Walk up
			if (event.keyCode == Keyboard.W)
			{
				
				player[0].SetVelocityY(-3);
			}
			
			//P2 Action button
			if (event.keyCode == Keyboard.C)
			{
				//perform the player-specific action
				player[0].UseAction();
			}
			
			
			/******Player 2 Controls********/
			//P2 Walk left
			if (event.keyCode == Keyboard.LEFT)
			{
				if (_isSelectScreenOn)
				{
					selectScreen.IterateLeft();
				}
				else
				{
					player[1].SetVelocityX(-3);
				}
			}
				
			//P2 Walk right
			if (event.keyCode == Keyboard.RIGHT)
			{
				if (_isSelectScreenOn)
				{
					selectScreen.IterateRight();
				}
				else
				{
					player[1].SetVelocityX(3);
				}
				
			}
								
			//P2 Walk down
			if (event.keyCode == Keyboard.DOWN)
			{
				
				player[1].SetVelocityY(3);
			}
			
			//P2 Walk up
			if (event.keyCode == Keyboard.UP)
			{
				
				player[1].SetVelocityY(-3);
			}
			
			//P2 Action button
			if (event.keyCode == Keyboard.SHIFT)
			{
				//perform the player-specific action
				player[1].UseAction();
			}
			
			/*******Select Screen-only key *******/
			//the space bar handles costume selection.
			if (event.keyCode == Keyboard.SPACE && _isSelectScreenOn)
			{
				if (!selectScreen.PlayerSelectOK(0))	//player 1
				{
					//record the player's pick
					_playerPick[0] = selectScreen.GetCostumePick();
					selectScreen.PlayerSelected(0);
					//change icon to next player
					selectScreen.SetNextIcon(1);
					
				}
				else if (!selectScreen.PlayerSelectOK(1))	//player 2
				{
					_playerPick[1] = selectScreen.GetCostumePick();
					selectScreen.PlayerSelected(1);
				}
				
				//if both players have selected, determine the characters then close the screen
				if (selectScreen.PlayerSelectOK(0) && selectScreen.PlayerSelectOK(1))
				{
					for (var d = 0; d < 2; d++)
					{
						if (_playerPick[d] == 0)	//ghost
						{
							player[d] = new Ghost();
							_playerHUD[d] = new GhostIcon();
						}
						else if (_playerPick[d] == 1)	//knight
						{
							player[d] = new Knight();
							_playerHUD[d] = new KnightIcon();
						}
						else if (_playerPick[d] == 2)	//princess
						{
							player[d] = new Princess();
							_playerHUD[d] = new PrincessIcon();
						}
						else //witch
						{
							player[d] = new Witch();
							_playerHUD[d] = new WitchIcon();
						}
						
						
						addChild(player[d]);
			
					}
					
					_playerHUD[0].x = 50;
					_playerHUD[0].y = 20;
						
					_playerHUD[1].x = 800;
					_playerHUD[1].y = 20;
					//Add icons to screen			
					addChild(_playerHUD[0]);
					addChild(_playerHUD[1]);
					
					//Set P1 & P2 position
					player[0].x = 78;
					player[0].y = 262;
					
					
					player[1].x = 747;
					player[1].y = 564;
					
					//close screen
					removeChild(selectScreen);
					_isSelectScreenOn = false;
				}
			}
			
		}
		
		/* This function checks when a key is released */
		public function onKeyUp(event:KeyboardEvent):void
		{
			if (!_isSelectScreenOn)
			{
				/******Player 1 Controls********/
				if(event.keyCode == Keyboard.A || event.keyCode == Keyboard.D)
				{
					player[0].SetVelocityX(0);
				}
										
				if (event.keyCode == Keyboard.S || event.keyCode == Keyboard.W)
				{
					player[0].SetVelocityY(0);
				}
				
				
				/******Player 2 Controls********/
				if(event.keyCode == Keyboard.LEFT || event.keyCode == Keyboard.RIGHT)
				{
					player[1].SetVelocityX(0);
				}
										
				if (event.keyCode == Keyboard.DOWN || event.keyCode == Keyboard.UP)
				{
					player[1].SetVelocityY(0);
				}
			}
		}
		
		
		/* This is the heart of the game, where all the updates happen and games are won. */
		function onEnterFrame(event:Event):void
		{
			
			/**************Select Screen***********/
			if (_isSelectScreenOn)
			{
				//the game will not begin until we leave this condition
				addChild(selectScreen);
			}
			else
			{
				clock.Countdown();
				/*******Has the game ended? ***********/
				
				if (clock.TimeUp())
				{
					//who won the game?
					if (player[0].GetCandyAmount() > player[1].GetCandyAmount())
					{
						//player 1 won
						_winText.text = "Player 1 wins!!";
					}
					else if (player[1].GetCandyAmount() > player[0].GetCandyAmount())
					{
						_winText.text = "Player 2 wins!!";
					}
					else	//draw game
					{
						_winText.text = "It's a draw...";
					}
					
					addChild(_winText);
					 _winTextTimer = 120;
					 _winTextDisplayed = true;
					//reset game
					ResetGame();
				}
				
				
				/***********HUD Update***************/
				//update the HUD
				for (var k = 0; k < player.length; k++)
				{
					_candyCountHUD[k].text = "x " + player[k].GetCandyAmount().toString();
					addChild(_candyCountHUD[k]);
				}
				
				
				/**********check all timers**********/
				 _winTextTimer--;
				 if (_winTextTimer < 0)
				 {
					  _winTextTimer = 0;
					  if (_winTextDisplayed)
					  {
						removeChild(_winText);
						_winTextDisplayed = false;
					  }
				 }
				 
				for (var m = 0; m < player.length; m++)
				{
					//yell bubble timer
					_yellTimer[m]--;
					if (_yellTimer[m] < 0)
					{
						_yellTimer[m] = 0;
						if (_isYellActive[m])
						{
							removeChild(_yell[m]);
							_isYellActive[m] = false;
						}
					}
					
					//candy collection rate
					_collectTimer[m]--;
					if (_collectTimer[m] < 0)
					{
						_collectTimer[m] = 0;
					}
					
					
					//invulnerabiility
					_invulTimer[m]--;
					if (_invulTimer[m] < 0)
					{
						_invulTimer[m] = 0;
					}
					
					
					//if there's no cooldown on player abilities, then display action icon
					if (player[m].GetCooldown() <= 0)
					{
						_actionHUD[m].visible = true;
					}
					else
					{
						_actionHUD[m].visible = false;
					}
					/**********update player movement*************/
					player[m].x += player[m].GetVelocityX();
					player[m].y += player[m].GetVelocityY();
				}
				
				
				
				/************ House Restocking **************/
				/*Check if a house will get stocked with candy.  There's a 0.00002% chance this will happen.
				The number is low because the game is checking 60 times a second.  Don't want the houses
				to fill up too quickly.*/
				var chance:Number;
				
				for (var a = 0; a < _houseVector.length; a++)
				{
					//update restock time
					_restockTime[a]--;
					if (_restockTime[a] < 0)
					{
						_restockTime[a] = 0;
					}
					
					if (!_houseVector[a].HasCandy() && _restockTime[a] <= 0)
					{
						chance = Math.floor(Math.random() * 100);
						if (chance <= 0.00002)
						{
							_houseVector[a].StockUp();
							_candyText[a].text = _houseVector[a].GetCandyAmount().toString();
							addChild(_candyText[a]);	//draw text only when candy is available
						}
					}
				}
				
				/**************update the clock*****************/
				if (clock.GetSeconds() < 10)
				{
					//append a zero in front of the seconds
					_clockText.text = clock.GetMinutes().toString() + ":0" + clock.GetSeconds().toString();
				}
				else
				{
					_clockText.text = clock.GetMinutes().toString() + ":" + clock.GetSeconds().toString();
				}
				addChild(_clockText);
				
				
				/************** COLLISION CHECKING**************/
				
				//House stuff
				for (var j = 0; j < _houseVector.length; j++)
				{
		
					for (var k = 0; k < player.length; k++)
					{
						//check collision against all houses
						player[k].CollisionCheck(_houseVector[j]);
						
						
						//check if player is standing in front of house. Player must stand in front and stay
						//there to collect candy.
						if (player[k].hitTestObject( _collectBox[j]) && _houseVector[j].HasCandy())
						{
							//player yells "Trick or Treat" and begins collecting candy
							_yell[k].x = player[k].x - 10;
							_yell[k].y = player[k].y - 50;
							addChild(_yell[k]);
							_isYellActive[k] = true;
							_yellTimer[k] = 10;	//0.25 seconds
							
							
							//start collecting candy. This is not instantaneous!
							if (_collectTimer[k] <= 0)
							{
								player[k].AddCandy(player[k].GetCandyTaken());
								_houseVector[j].ReduceCandy(player[k].GetCandyTaken());
								_candyText[j].text = _houseVector[j].GetCandyAmount().toString();
								
								
								
								//if there's no more candy, then don't display a number and set cooldown
								if (_houseVector[j].HasCandy())
								{
									addChild(_candyText[j]);
								}
								else
								{
									removeChild(_candyText[j]);
									_restockTime[j] = 300;	//5 seconds of restock time
								}
								
								_collectTimer[k] = 45;		//0.75 seconds
							}
							//trace("House " + j + " touched");
						}
						
					}
				}
				
				//collision vs. Knight
				if (player[0].hitTestObject(player[1]) && player[0].IsAttackActive() 
					&& _invulTimer[1] <= 0)
				{
					//set invul timer so that candy isn't rapidly decreased.
					_invulTimer[1] = 60;
					
					var rand:Number; //random number for dropping candy
					//since the opponent is a knight, he does not get knocked back as far, and
					//he doesn't drop so much candy.
					for (var j = 0; j < player[1].GetDropAmount(); j++)
					{
						if (player[1].GetCandyAmount() > 0)
						{
							player[1].DropCandy(1);
							//drop candy onto the field.
							_candy.push(new CandyIcon());
							addChild(_candy[_candy.length-1]);	//always access the latest candy in vector
							
							_isPickedUp.push(new Boolean());
							_isPickedUp[_isPickedUp.length-1] = false;	//hasn't been picked up yet
							
							//spread the candy around!
							rand = Math.floor(Math.random() * -60) + 60;
							_candy[_candy.length-1].x = player[1].x + rand;
							rand = Math.floor(Math.random() * -60) + 60;
							_candy[_candy.length-1].y = player[1].y + rand;
						}
						
						//knock player back.
						if (player[1].GetVelocityX() < 0)
						{
							player[1].x += 25;
						}
						else if (player[1].GetVelocityX() > 0)
						{
							player[1].x -= 25;
						}
						else //not moving, so knock him upwards
						{
							player[1].y -= 25;
						}
					}
				}//end collsion vs.Knight
					
					
					//collision vs. Ghost
				if (player[1].hitTestObject(player[0]) && player[1].IsAttackActive() 
					&& _invulTimer[0] <= 0)
				{
					//set invul timer so that candy isn't rapidly decreased.
					_invulTimer[0] = 60;
					
					var rand:Number; //random number for dropping candy
					//since the opponent is a knight, he does not get knocked back as far, and
					//he doesn't drop so much candy.
					for (var j = 0; j < player[0].GetDropAmount(); j++)
					{
						if (player[0].GetCandyAmount() > 0)
						{
							player[0].DropCandy(1);
							//drop candy onto the field.
							_candy.push(new CandyIcon());
							addChild(_candy[_candy.length-1]);	//always access the latest candy in vector
							
							_isPickedUp.push(new Boolean());
							_isPickedUp[_isPickedUp.length-1] = false;	//hasn't been picked up yet
							
							//spread the candy around!
							rand = Math.floor(Math.random() * -60) + 60;
							_candy[_candy.length-1].x = player[0].x + rand;
							rand = Math.floor(Math.random() * -60) + 60;
							_candy[_candy.length-1].y = player[0].y + rand;
						}
						
						//knock player back.
						if (player[0].GetVelocityX() < 0)
						{
							player[0].x += 40;
						}
						else if (player[0].GetVelocityX() > 0)
						{
							player[0].x -= 40;
						}
						else //not moving, so knock him upwards
						{
							player[0].y -= 40;
						}
					}
					
				} //end collsion vs. Ghost
				
				
				//Collision vs. dropped candy
				for (var b = 0; b < _candy.length; b++)
				{
					for (var c = 0; c < player.length; c++)
					{
						if (player[c].hitTestObject(_candy[b]) && !player[c].IsAttackActive()
							&& !_isPickedUp[b])
						{
							//pick up candy
							player[c].AddCandy(1);
							removeChild(_candy[b]);
							_isPickedUp[b] = true;
							_candy[b].pop();
						}
					}
				}
			
			}
			
		} //end onEnterFrame
		
		public function ResetGame():void
		{

			/*************PLAYER SETUP**************/
			
			//Set P1 & P2 position
			player[0].x = 78;
			player[0].y = 262;
			
			player[1].x = 747;
			player[1].y = 564;
			
			/***********reset all counters**********/
			
			for (var m = 0; m < player.length; m++)
			{
				//yell bubble timer
				_yellTimer[m] = 0;
				if (_isYellActive[m])
				{
					removeChild(_yell[m]);
					_isYellActive[m] = false;
				}
				
					
				//candy collection rate
				_collectTimer[m] = 0;
				
				
				
				//invulnerabiility
				_invulTimer[m] = 0;
				
				
				//attack cooldown
				player[m].SetCooldown(0);
				
			
				player[m].SetCandyAmount(0);
				_candyCountHUD[m].text = "x " + player[m].GetCandyAmount().toString();		
					
				addChild(_candyCountHUD[m]);
					
				
			}
			
			
			
			
			
			/*************CLOCK SETUP ************/
			//set up clock
			clock.SetTime(120);
			
			if (clock.GetSeconds() < 10)
			{
				//append a zero in front of the seconds
				_clockText.text = clock.GetMinutes().toString() + ":0" + clock.GetSeconds().toString();
			}
			else
			{
				_clockText.text = clock.GetMinutes().toString() + ":" + clock.GetSeconds().toString();
			}
			
			//display clock
			addChild(_clockText);
			
			/******************HOUSE SETUP ***************/
			
			for (var i = 0; i < _houseVector.length; i++)
			{	
				_houseVector[i].SetCandyStock(0);
				_restockTime[i] = 0;
				removeChild(_candyText[i]);	//draw text only when candy is available
			
				//Randomly determine which houses start with candy
				var chance:int;
			
				chance = Math.floor(Math.random() * 100) + 1;
				if (chance <= 50)
				{
					_houseVector[i].StockUp();
					_candyText[i].text = _houseVector[i].GetCandyAmount().toString();
					addChild(_candyText[i]);	//draw text only when candy is available
				}
				
				
				
			}
			
			/**********Candy Removal*************/
			for (var b = 0; b < _candy.length; b++)
			{
				
				removeChild(_candy[b]);
				_isPickedUp[b] = false;
				_candy[b].pop();
					
			}
		
		}//end ResetGame
		

	}	//end class
	
}
