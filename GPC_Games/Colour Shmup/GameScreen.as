﻿/* This is the gameplay screen. From here, the game can be paused and exited back to the title screen. */

package  
{
	import flash.display.MovieClip;
	import flash.display.Stage;
	import flash.events.Event;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
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
	import flash.geom.Point;
	import flash.display.BitmapData;
	import flash.display.Bitmap;
	
	public class GameScreen extends MovieClip
	{
		var stageRef:Stage;
		
		//UI & game states
		var score:int;
		var level:int;
		var enemyCount:uint;		//current # of enemies killed.
		var enemyTotal:uint;		//# of enemies that must be killed to go to next level
		var rainbowMeter:RainbowMeter;
		var dmgMeter:DamageMeter;
		var border:MeterBorder;
		var meterLabel:MeterLabel;
		const FULL_METER:uint = 200;		//equal to rainbow gauge width.
		var livesText:TextField;
		var enemyCountText:TextField;		//tracks how many enemies must be killed.
		var uiTextFormat:TextFormat;
		var absorbLabel:AbsorbLabel = new AbsorbLabel();	//shown whenever an enemy is absorbed.
		var showAbsorbLabel:Boolean = false;				//controls the absorb label display
		var gameOver:Boolean;
		var gameOverImg:GameOver;
		
		
		//graphics
		var blackBG:BlackBG;
		var starList:Vector.<Star>;
		
		//sound variables
		/*var musicPlayer:MusicPlayer;
		const PLAY:uint = 1;			//correspond to the frames used to play and stop music.
		const STOP:uint = 2;*/
		
		//player.  
		var player:Ship;
		var bulletList:Vector.<ShipBullet>;
		var superBulletList:Vector.<SuperBullet>;
		var bulletCount:uint;
		var superBulletCount:uint;
		const WHITE:uint = 1;
		const RED:uint = 2;			//The colour constants coincide with the keyframes of the Ship movieclip.
		const BLUE:uint = 3;		//directional controls: left = red, right = blue, up = white, down = black
		const BLACK:uint = 4;
		var currentColour:uint;
		var vx:Number;
		var vy:Number;				//movement
		var defaultSpeed:Number;
		var speedMod:Number;		//adjusts bullet and player speed.
		var bulletTimer:int;		//controls rate of fire. Timer = frame rate
		var playerDead:Boolean;		//used to make player invincible after death
		var playerInvincible:Boolean;
		var playerLives:int;
		var playerIcon:Ship;
		var invulTimer:uint;			//time, in frames, indicating how long player is invulnerable
		var superBulletEnabled:Boolean;	//when meter is full, everything dies
		
		
		//enemy
		/*var red:Enemy = new Enemy(new Point(50, 180), new Point(200, 200), 100, 40, RED);
		var blue:Enemy = new EnemyBlue(new Point(100, 260), new Point(200, 200), 100, 40, BLUE);
		var black:Enemy = new EnemyBlack(new Point(200, 340), new Point(200, 200), 100, 40, BLACK);
		var white:Enemy = new EnemyWhite(new Point(150, 420), new Point(200, 200), 100, 40, WHITE);*/
		
		var red:Enemy;
		var blue:Enemy;
		var black:Enemy;
		var white:Enemy;
		var enemySpeed:Number;
		
		var enemyList:Vector.<Enemy> = new Vector.<Enemy>();
		var totalOnScreenEnemies:uint;		//controls how many enemies are onscreen at once.
		var currentOnScreenEnemies:uint;
		
		//power ups
		var power1:PowerUp1 = new PowerUp1();	//increases bullet speed
		var power2:PowerUp2 = new PowerUp2();	//gives +20 energy to rainbow gauge. this one should occur less frequently
		var powerup1OnScreen:Boolean;
		var powerup2OnScreen:Boolean;
		var speedLabel:SpeedLabel = new SpeedLabel();		//speed powerup feedback.
		var showSpeedLabel:Boolean;
		var gaugeUpLabel:GaugeUpLabel = new GaugeUpLabel();
		var showGaugeUpLabel:Boolean;
		
		//flight paths
		var path1:BitmapData = new Flightpath1();
	
		
		//debug
		var debugText:TextField;
		var debugFormat:TextFormat;
		
		//music
		var gameMusic:Sound = new Sound(new URLRequest("Super Aleste - Area 1.mp3"));	//Space Megaforce area 1 music
		var musicChannel:SoundChannel = new SoundChannel();
		var _volume:SoundTransform = new SoundTransform(0.5);	//sets volume to 50%*/
		

		public function GameScreen(stageRef:Stage) 
		{
			this.stageRef = stageRef;
			
			
			/*****METER SETUP*******/
			rainbowMeter = new RainbowMeter();
			rainbowMeter.x = 700;
			rainbowMeter.y = 700;
			rainbowMeter.width = 0;
			
			dmgMeter = new DamageMeter();
			dmgMeter.x = rainbowMeter.x;
			dmgMeter.y = rainbowMeter.y;
			dmgMeter.width = rainbowMeter.width;
			
			border = new MeterBorder();
			border.x = rainbowMeter.x - 10;
			border.y = rainbowMeter.y - 10;
			
			meterLabel = new MeterLabel();
			meterLabel.x = border.x + 10;
			meterLabel.y = border.y - 20;
			
			/*****GRAPHICS*******/
			blackBG = new BlackBG();		
			starList = new Vector.<Star>();
			gameOverImg = new GameOver();
			gameOverImg.x = 400;
			gameOverImg.y = 300;
			
			
			/*****PLAYER*******/
			player = new Ship();
			player.x = 500;
			player.y = 600;
			player.gotoAndStop(WHITE);
			currentColour = WHITE;
			
			bulletList = new Vector.<ShipBullet>();
			superBulletList = new Vector.<SuperBullet>();
			bulletCount = 0;
			superBulletCount = 0;
			
			//movement
			vx = 0;
			vy = 0;
			defaultSpeed = 5;
			speedMod = 0;
			bulletTimer = 0;
			
			//counters
			playerDead = false;
			playerInvincible = false;
			playerLives = 3;
			playerIcon = new Ship();
			playerIcon.x = 50;
			playerIcon.y = 700;
			playerIcon.scaleX = 0.55;
			playerIcon.scaleY = 0.55;
			playerIcon.gotoAndStop(WHITE);
			
			/********ENEMIES**********/
			enemySpeed = 2;
			/*enemyList.push(red);
			enemyList.push(blue);
			enemyList.push(white);
			enemyList.push(black);*/
			
			/*******UI**********/
			enemyCount = 0;
			enemyTotal = 20;
			totalOnScreenEnemies = 4;		
			currentOnScreenEnemies = 0;
			level = 1;
			uiTextFormat = new TextFormat();
			uiTextFormat.size = 30;
			uiTextFormat.bold = true;
			uiTextFormat.font = "Calibri";
			
			livesText = new TextField();
			livesText.defaultTextFormat = uiTextFormat;
			livesText.textColor = 0xCCCCFF;
			livesText.width = 70;
			livesText.height = 40;
			livesText.x = 80;
			livesText.y = 695;
			livesText.text = "x " + playerLives;
			livesText.selectable = false;
			
			enemyCountText = new TextField();
			enemyCountText.defaultTextFormat = uiTextFormat;
			enemyCountText.textColor = 0xCCCCFF;
			enemyCountText.width = 220;
			enemyCountText.height = 80;
			enemyCountText.x = 50;
			enemyCountText.y = 30;
			enemyCountText.text = "Level " + level + "\nKill Count: " + enemyCount + "/" + enemyTotal;
			enemyCountText.selectable = false;
			
			/*******DEBUG*******/
			debugFormat = new TextFormat();
			debugFormat.size = 20;
			debugFormat.bold = true;
			debugFormat.font = "Fixedsys";
			
			debugText = new TextField();
			debugText.defaultTextFormat = debugFormat;
			debugText.textColor = 0xCCCCFF;
			debugText.width = 220;
			debugText.height = 200;
			debugText.x = 800;
			debugText.y = 50;
			debugText.selectable = false;
			 
			/*****ADD ALL ELEMENTS*****/
			addChild(blackBG);
			//create multiple stars and draw them randomly on the screen.
			for (var i:int = 0; i < 40; i++)
			{
				starList.push(new Star());
			}
			
			for each (var s:Star in starList)
			{
				s.x = Math.random() * stageRef.stageWidth;
				s.y = Math.random() * stageRef.stageHeight;
				addChild(s);
			}
			
			addChild(player);
			addChild(playerIcon);
			addChild(livesText);
			addChild (enemyCountText);
			for each(var e:Enemy in enemyList)
			{
				addChild(e);
			}
			
			addChild(border);
			addChild(meterLabel);
			addChild(dmgMeter);
			addChild(rainbowMeter);
			
			addChild(debugText);
	
			/****EVENT LISTENERS*****/
			addEventListener(Event.ENTER_FRAME, Update);
			stageRef.addEventListener(KeyboardEvent.KEY_UP, getInput);
			stageRef.addEventListener(KeyboardEvent.KEY_DOWN, moveShip);
			
			/*******play music***********/
			//musicPlayer = new MusicPlayer();
//			musicPlayer.gotoAndStop(PLAY);
			PlayMusic(musicChannel, gameMusic);
			
		}
		
		

		function PlayMusic(channel:SoundChannel, sound:Sound):void
		{
			channel = sound.play();
			channel.soundTransform = _volume;
			channel.addEventListener(Event.SOUND_COMPLETE, Replay);
		}
		
		function Replay(event:Event):void
		{
			//remove the event listener and start music again.
			PlayMusic(musicChannel, gameMusic);
			SoundChannel(event.target).removeEventListener(event.type, Replay);
		}
		
		function getInput(event:KeyboardEvent):void
		{
			switch(event.keyCode)
			{
				/*case Keyboard.SHIFT:	//pause game
					stageRef.removeEventListener(KeyboardEvent.KEY_UP, getInput);
					stageRef.removeEventListener(KeyboardEvent.KEY_DOWN, moveShip);
					
					//stop music
					musicPlayer.gotoAndStop(STOP);
					Main.gameScreenOff = false;
					//trace ("Closed game screen");
					break;*/
					
				case Keyboard.A: case Keyboard.D:
					vx = 0;
					break;
				case Keyboard.W: case Keyboard.S:
					vy = 0;
					break;
					
				//colour change	
				case Keyboard.LEFT:
					currentColour = RED;
					break;
				case Keyboard.RIGHT:
					currentColour = BLUE;
					break;
				case Keyboard.UP:
					currentColour = WHITE;
					break;
				case Keyboard.DOWN:
					currentColour = BLACK;
					break;
					
				case Keyboard.SPACE:
					if (gameOver)
						ResetGame();
						break;
					
			}
		}
		
		function moveShip(event:KeyboardEvent):void
		{
			switch(event.keyCode)
			{
				case Keyboard.A:	//move ship left
					vx = -defaultSpeed - speedMod;
					break;
				case Keyboard.D:	//right
					vx = defaultSpeed + speedMod;
					break;
				case Keyboard.W:	//up
					vy = -defaultSpeed - speedMod;
					break;
				case Keyboard.S:	//down
					vy = defaultSpeed + speedMod;
					break;
				case Keyboard.SPACE:	//fire bullets. Can hold down. TODO: find out how to move and hold down key to shoot
					if (!superBulletEnabled)
					{
						if (bulletTimer <= 0 && !playerDead)
						{
							bulletList.push(new ShipBullet());
							bulletList[bulletCount].x = player.x + 25;
							bulletList[bulletCount].y = player.y - 40;
							addChild(bulletList[bulletCount]);
							bulletCount++;
							bulletTimer = 10;
						}
					}
					else	//fire a super bullet!
					{
						superBulletList.push(new SuperBullet());
						superBulletList[superBulletCount].x = player.x + 5;
						superBulletList[superBulletCount].y = player.y - 150;
						addChild(superBulletList[superBulletCount]);
						superBulletCount++;
					}
					break;
			}
		}
		
		//player flashes while invincible
		function Invincibility(frameCount:uint)
		{
			if (frameCount > 0)
			{
				player.visible = !player.visible;	//repeated flashing
				playerInvincible = true;
			}
			else
			{
				player.visible = true;
				playerInvincible = false;
			}
		}
		
		function UpdateDebug()
		{
			var colour:String;
			switch (currentColour)
			{
				case WHITE:
					colour = "White";
					break;
				case BLUE:
					colour = "Blue";
					break;
				case BLACK:
					colour = "Black";
					break;
				case RED:
					colour = "Red";
					break;
					
			}
			
			debugText.text = "Bullets: " + bulletCount + "\n"
								+ "Colour: " + colour + "\n"
								+ "Rainbow Mtr.: " + rainbowMeter.width + "\n"
								+ "Invul Timer: " + invulTimer + "\n"
								+ "Abs. Label on: " + showAbsorbLabel + "\n"
								+ "Invincible: " + playerInvincible + "\n"
								+ "Onscreen enemy#: " + currentOnScreenEnemies;
		}
		
		/* Check for opposing colours. More damage is dealt to player if they touch an enemy with an opposing colour. */
		function PlayerIsOpposingColour(enemyColour:uint):Boolean
		{
			var coloursOpposed:Boolean = false;
			
			switch (currentColour)
			{
				case WHITE:
					coloursOpposed = (enemyColour == BLACK) ? true : false;
					break;
				case BLACK:
					coloursOpposed = (enemyColour == WHITE) ? true : false;
					break;
				case RED:
					coloursOpposed = (enemyColour == BLUE) ? true : false;
					break;
				case BLUE:
					coloursOpposed = (enemyColour == RED) ? true : false;
					break;

			}
			
			return coloursOpposed;
		}
		
		/*Increases difficulty of next level*/
		function AdvanceLevel()
		{
			level++;
			enemyCount = 0;
			enemySpeed += 0.2;
			enemyTotal += 2;
			totalOnScreenEnemies++;
		}
		
		function ResetGame()
		{
			gameOver = false;
			level = 1;
			enemyCount = 0;
			enemySpeed = 2;
			enemyTotal = 20;
			totalOnScreenEnemies = 4;
			currentOnScreenEnemies = 0;
			bulletCount = 0;
			superBulletCount = 0;
			vx = 0;
			vy = 0;
			defaultSpeed = 5;
			speedMod = 0;
			bulletTimer = 0;
			playerDead = false;
			playerInvincible = false;
			playerLives = 3;
			
			//remove onscreen elements
			for each(var b:ShipBullet in bulletList)
			{
				removeChild(b);
			}
			bulletList = new Vector.<ShipBullet>();
			

			for each(var e:Enemy in enemyList)
			{
				removeChild(e);
			}
			enemyList = new Vector.<Enemy>();
			
			
			for each(var sb:SuperBullet in superBulletList)
			{
				removeChild(sb);
			}
			superBulletList = new Vector.<SuperBullet>();
			
			if (this.contains(power1))
				removeChild(power1);
			if (this.contains(power2))
				removeChild(power2);
			if (this.contains(speedLabel))
				removeChild(speedLabel);
			if (this.contains(gaugeUpLabel))
				removeChild(gaugeUpLabel);
			if (this.contains(absorbLabel))
				removeChild(absorbLabel);
						
			
			showSpeedLabel = false;
			showGaugeUpLabel = false;
			powerup1OnScreen = false;
			powerup2OnScreen = false;
			showAbsorbLabel = false;
			
			removeChild(gameOverImg);
			
			//restore player
			player.x = 500;
			player.y = 600;
			player.gotoAndStop(WHITE);
			currentColour = WHITE;
			addChild(player);
			
			
		}
		
		/*Genereate an random enemy and add him to enemyList */
		function GenerateEnemy(list:Vector.<Enemy>)
		{
			//generate an enemy
			var chance:Number = Math.floor(Math.random() * 4);
			var randX:Number = -1124 * Math.random() + stageRef.stageWidth;	//number between -100 and 1024
			//trace (randX);
			var offscreenPos:Number;
			
			var destinationX:Number;
			var destinationY:Number;
			
			if (randX < 0)
			{
				//set off-screen position to something greater than 0 and have the enemy appear from the left side
				offscreenPos = Math.random() * (stageRef.stageHeight - 60);
				destinationX = Number(stageRef.stageWidth);
				destinationY = 0;
			}
			else if (randX >= stageRef.stageWidth * 0.75)
			{
				//enemy appears from the right and moves left.
				randX = stageRef.stageWidth;
				offscreenPos = Math.random() * (stageRef.stageHeight - 60);
				destinationX = -30;
				destinationY = 0;
			}
			else
			{
				//enemy appears from above
				offscreenPos = -10 + (Math.random() * -80);
				destinationX = 0;
				destinationY = Number(stageRef.stageHeight);
			}
			
			switch (chance)
			{
				case 0:
					red = new Enemy(new Point(randX, offscreenPos), new Point(destinationX, destinationY), 40, RED);
					list.push(red);
					addChild(red);
					break;
				case 1:
					blue = new EnemyBlue(new Point(randX, offscreenPos), new Point(destinationX, destinationY), 40, BLUE);
					list.push(blue);
					addChild(blue);
					break;
				case 2:
					white = new EnemyWhite(new Point(randX, offscreenPos), new Point(destinationX, destinationY), 40, WHITE);
					list.push(white);
					addChild(white);
					break;
				case 3:
					black = new EnemyBlack(new Point(randX, offscreenPos), new Point(destinationX, destinationY), 40, BLACK);
					list.push(black);
					addChild(black);
					break;
			
			}
			
		}
		
		function Update(event:Event):void
		{
			if (!gameOver)
			{
				var i:int = 0;	//used to iterate through for loops
				var j:int = 0;		
				/*****update each star's movement.  If they go off-screen, they are repositioned at the top of the screen.****/
				for each (var star:Star in starList)
				{
					star.y++;
					if (star.y > stageRef.stageHeight)
					{
						star.y = -10;
					}
				}
				
				/***********Generate Enemies**************/
				/*periodically, enemies will appear either from the top of the screen or from the sides, but never the bottom.
				They will move from one point to another, sometimes shooting at the player. The speed and frequency of the enemies
				increase as the player advances a level. */
				while (currentOnScreenEnemies < totalOnScreenEnemies)
				{
					GenerateEnemy(enemyList);
					currentOnScreenEnemies++;
					//trace ("added enemy");
				}
				
				/************Enemy movement************/
				i = 0;
				for each(var e:Enemy in enemyList)
				{
					if (e.EndPoint().y >= stageRef.stageHeight)	//moving down
						e.y += enemySpeed;
					else if (e.EndPoint().x < 0)	//moving left
						e.x -= enemySpeed;
					else if (e.EndPoint().x >= stageRef.stageWidth)	//moving right
						e.x += enemySpeed;
						
					if (e.y > stageRef.stageHeight || e.x > stageRef.stageWidth || e.x < -60)	//60 is the enemy width
					{
						try
						{
							enemyList.splice(i, 1);
							removeChild(e);
							currentOnScreenEnemies--;
						}
						catch(err:Error)
						{
							//trace ("Unable to remove Enemy!");
						}
					}
					i++;
				}
				
				/*********UI Update***************/
				//update rainbow meter.
				if (dmgMeter.width > rainbowMeter.width)
				{
					dmgMeter.width -= 0.5;
				}
				
				if (superBulletEnabled)
				{
					rainbowMeter.width -= 0.5;
					dmgMeter.width -= 0.5;
				
					if (invulTimer <= 0)
					{
						//super state is over.
						i = 0;
						for each(var sb:SuperBullet in superBulletList)
						{
							removeChild(sb);
							i++;
						}
						//empty the list
						superBulletList = new Vector.<SuperBullet>();
						
						superBulletCount = 0;
						superBulletEnabled = false;
						
					}
				}
				
				//update lives & kill count
				livesText.text = "x " + playerLives;
				enemyCountText.text = "Level " + level + "\nKill Count: " + enemyCount + "/" + enemyTotal;
				
				//check for invincibility
				if (invulTimer > 0)
					invulTimer--;
				Invincibility(invulTimer);
				
				if (showAbsorbLabel)
				{
					absorbLabel.alpha -= 0.01;
					////trace (absorbLabel.alpha);
					absorbLabel.y--;
					if (absorbLabel.alpha <= 0)
					{
						removeChild(absorbLabel);
						showAbsorbLabel = false;
						//trace ("label removed");
					}
				}
				
				if (showSpeedLabel)
				{
					speedLabel.alpha -= 0.01;
					speedLabel.y--;
					if (speedLabel.alpha <= 0)
					{
						removeChild(speedLabel);
						showSpeedLabel = false;
						//trace ("label removed");
					}
				}
				
				if (showGaugeUpLabel)
				{
					gaugeUpLabel.alpha -= 0.01;
					gaugeUpLabel.y--;
					if (gaugeUpLabel.alpha <= 0)
					{
						removeChild(gaugeUpLabel);
						showGaugeUpLabel = false;
						//trace ("gauge label removed");
					}
				}
					
				/************update player movement & colour**********/
				player.x += vx;
				player.y += vy;
				
				//boundary check
				if (player.x < 0)
					player.x = 0;
				if (player.x > stageRef.stageWidth - player.width)
					player.x = stageRef.stageWidth - player.width;
				if (player.y > stageRef.stageHeight - player.height)
					player.y = stageRef.stageHeight - player.height;
				if (player.y < 0)
					player.y = 0;
				
				player.gotoAndStop(currentColour);
				playerIcon.gotoAndStop(currentColour);
				
				//if player is dead, reposition back to starting point.
				if (playerDead)
				{
					player.x = 500;
					player.y = 600;
					addChild(player);
					invulTimer = 180;
					playerDead = false;
				}
				
				/******Powerup movement & collision******/
				if (powerup1OnScreen)
				{
					power1.y += 3;
					if (power1.y > stageRef.stageHeight - power1.height)
					{
						//kill the powerup
						removeChild(power1);
						powerup1OnScreen = false;
					}
					else if (player.hitTestObject(power1))	//speed boost
					{
						removeChild(power1);
						powerup1OnScreen = false;
						speedMod += 0.5;
						
						//show speed label
						showSpeedLabel = true;
						speedLabel.x = player.x;
						speedLabel.y = player.y;
						speedLabel.alpha = 1;
						addChild(speedLabel);
						
						//trace ("Received powerup!");
					}
				}
				
				if (powerup2OnScreen)
				{
					power2.y += 3;
					if (power2.y > stageRef.stageHeight - power2.height)
					{
						//kill the powerup
						removeChild(power2);
						powerup2OnScreen = false;
					}
					else if (player.hitTestObject(power2))	//fill rainbow meter
					{
						removeChild(power2);
						powerup2OnScreen = false;
						if (rainbowMeter.width + 20 >= FULL_METER)
						{
							rainbowMeter.width = FULL_METER;
							dmgMeter.width = FULL_METER;
							superBulletEnabled = true;
							playerInvincible = true;
							invulTimer = 400;	//rainbowMeter width * 2;
							
							//clear out bullet list
							i = 0;
							for each(var c:ShipBullet in bulletList)
							{
								removeChild(c);
								bulletList.splice(i, 1);
								i++;
							}
							bulletCount = 0;
						}
						else
						{
							rainbowMeter.width += 20;
							dmgMeter.width += 20;
						}
						
						//show gauge up label
						showGaugeUpLabel = true;
						gaugeUpLabel.x = player.x;
						gaugeUpLabel.y = player.y;
						gaugeUpLabel.alpha = 1;
						addChild(gaugeUpLabel);
						
						//trace ("Received gauge powerup!");
					}
				}
				
				/**********update bullets***********/
				
				if (bulletTimer > 0)
					bulletTimer--;
				
				if (!superBulletEnabled)
				{
					for each(var b:ShipBullet in bulletList)
					{
						b.y -= (defaultSpeed * 1.5) + speedMod;
						if (b.y < 0)
						{
							//destroy bullet
							try
							{
								bulletList.splice(bulletList.indexOf(b), 1);
								removeChild(b);
								bulletCount--;
								//trace ("bullet destroyed");
							}
							catch (err:Error)
							{
								//trace ("Unable to remove child!");
							}				
						}
						else	//check for collision between a bullet and enemy
						{
							for each(var e:Enemy in enemyList)
							{
								if (b.hitTestObject(e) && currentColour != e.Colour())
								{
									//call explosion
									var explode:Death = new Death();
									explode.x = e.x;
									explode.y = e.y;
									addChild(explode);	//this should be going out of scope once out of the loop.
									
									//if enemy was an opposing colour, generate a power up
									if (PlayerIsOpposingColour(e.Colour()))
									{
										var chance:Number = Math.random() * 100;
										if (chance <= 20)	//drop power2, which fills rainbow meter
										{
											power2.x = e.x;
											power2.y = e.y;
											addChild(power2);
											powerup2OnScreen = true;
										}
										else
										{
											power1.x = e.x;
											power1.y = e.y;
											addChild(power1);
											powerup1OnScreen = true;
										}
									}
									
									bulletList.splice(bulletList.indexOf(b), 1);
									enemyList.splice(enemyList.indexOf(e), 1);
									removeChild(b);
									removeChild(e);
									bulletCount--;
									enemyCount++;
									currentOnScreenEnemies--;
									//check if player advances a level
									if (enemyCount >= enemyTotal)
									{
										AdvanceLevel();
									}
								}
								else if (b.hitTestObject(e) && currentColour == e.Colour())
								{
									//only the bullet dies
									bulletList.splice(bulletList.indexOf(b), 1);
									removeChild(b);
									bulletCount--;
								}
							}
						}
					}
				}
				else	//fire a super bullet, which has different rules.
				{
					for each(var sb:SuperBullet in superBulletList)
					{
						sb.y -= (defaultSpeed * 3) + speedMod;
						if (sb.y < 0)
						{
							//destroy bullet
							try
							{
								superBulletList.splice(superBulletList.indexOf(sb), 1);
								removeChild(sb);
								superBulletCount--;
								//trace ("Super bullet destroyed");
							}
							catch (err:Error)
							{
								//trace ("Unable to remove child!");
							}				
						}
						else	//check for collision between a super bullet and enemy. super bullets cut through everything.
						{
							for each(var e:Enemy in enemyList)
							{
								if (sb.hitTestObject(e))
								{
									//call explosion
									var explode:Death = new Death();
									explode.x = e.x;
									explode.y = e.y;
									addChild(explode);	//this should be going out of scope once out of the loop.
									
									enemyList.splice(enemyList.indexOf(e), 1);
									removeChild(e);
									enemyCount++;
									currentOnScreenEnemies--;
									//check if player advances a level
									if (enemyCount >= enemyTotal)
									{
										AdvanceLevel();
									}
								}
							}
						}
					}
				}
				
				
				/************Collision Check***********/
				//Collision between player & enemy//
				for each(var e:Enemy in enemyList)
				{
					
					if (!playerInvincible && player.hitTestObject(e) && currentColour == e.Colour())
					{
						
						if (rainbowMeter.width + e.MeterAmount() >= FULL_METER)
						{
							rainbowMeter.width = FULL_METER;
							dmgMeter.width = FULL_METER;
							superBulletEnabled = true;
							playerInvincible = true;
							invulTimer = 400;	//rainbowMeter width * 2;
							
							//clear out bullet list
							for each(var d:ShipBullet in bulletList)
							{
								removeChild(d);
							}
							bulletList = new Vector.<ShipBullet>();
							bulletCount = 0;
						}
						else
						{
							rainbowMeter.width += e.MeterAmount();
							dmgMeter.width += e.MeterAmount();
						}
						
						//show absorb label
						showAbsorbLabel = true;
						absorbLabel.x = player.x;
						absorbLabel.y = player.y;
						absorbLabel.alpha = 1;
						addChild(absorbLabel);
						
						
						//destroy enemy
						enemyList.splice(enemyList.indexOf(e), 1);
						removeChild(e);
						enemyCount++;
						currentOnScreenEnemies--;
						
						//check if player advances a level
						if (enemyCount >= enemyTotal)
						{
							AdvanceLevel();
						}
						//trace("absorbed enemy");
					}
					else if (!playerInvincible && player.hitTestObject(e) && currentColour != e.Colour())
					{
						if (rainbowMeter.width <= 0 && !playerDead)
						{
							//player dies
							var explode:Death = new Death();
							explode.x = player.x;
							explode.y = player.y;
							removeChild(player);	//must remove player in order for explosion to play properly.
							playerDead = true;
							playerLives--;
							
							if (playerLives < 0)
								gameOver = true;
								
							speedMod = 0;
							addChild(explode);	//this should be going out of scope once out of the loop.
							//trace ("player dead");
						}
						else
						{
							var mod:Number = 1;	//used to adjust damage depending on colours
							var damage:Number;
							
							if (PlayerIsOpposingColour(e.Colour()))
								//deal more damage to player
								mod = 1.5;
							
								
							damage = Math.ceil(e.MeterAmount() * mod);
							
							//lose rainbow meter
							if (rainbowMeter.width < damage)
								rainbowMeter.width = 0;
							else
								rainbowMeter.width -= damage;
							
							//kill enemy & call explosion
							var explode:Death = new Death();
							explode.x = e.x;
							explode.y = e.y;
							addChild(explode);
							enemyList.splice(enemyList.indexOf(e), 1);
							removeChild(e);
							enemyCount++;
							currentOnScreenEnemies--;
							
							//check if player advances a level
							if (enemyCount >= enemyTotal)
							{
								AdvanceLevel();
							}
							
							//player is temporarily invincible.
							playerInvincible = true;
							invulTimer = 60;
							//trace ("crashed into enemy");
						}
					}
					
				}
				
		
				
				/**********debug**********/
				//UpdateDebug();
			}	//end Update()
			else	//game pauses and a game over messge appears
			{
				addChild(gameOverImg);
				//trace ("game is over");
			}
		}

	}
	
}
