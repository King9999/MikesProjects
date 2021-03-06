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
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.media.Sound;
	import flash.media.SoundChannel;
	import flash.net.URLRequest;
	import flash.media.SoundTransform;
	import flash.geom.Point;
	import flash.utils.ByteArray;		//used to deep copy arrays
	import flash.utils.getTimer;
	import flash.system.System;
	
	public class GameScreen extends MovieClip
	{
		var stageRef:Stage;
		
		//UI & game states
		var score:int;
		var level:uint;
		var levelIndex:uint;		//used to navigate levels in XML file.
		var enemyCount:uint;		//current # of uncaptured creatures remaining.
		var enemyTotal:uint;		//total number of creatures in the level.
		var previousTime:int;		//used for calculating FPS.
		var deltaTime:int;			//amount of time in milliseconds passed between frames.
		
		var levelText:TextField;
		var livesText:TextField;
		var enemyCountText:TextField;		//tracks how many enemies must be killed.
		var uiTextFormat:TextFormat;
		var gameOver:Boolean;
		var controlLocked:Boolean;				//prevents any player input during win screen.
		var waitTimer:int;				//used to delay screen changes in frames.
		var winLabel:WinLabel;
		
		//level variables
		var mapRowList:XMLList;				//gets all rows of map in XML file.
		var objRowList:XMLList;				//gets all rows of objects in XML file.
		var mapArray:Array;				//used to contain the map data from the file.
		var objectArray:Array;				//used to contain the object data from the file.
		var initMapArray:Array;			//used to restart current level
		var initObjArray:Array;			//used to restart current level
		const MAX_ROWS:int = 12;
		const MAX_COLS:int = 16;
		const TILE_SIZE:int = 64;
		const MOVE_SPEED:int = 4;
		const MAX_LEVEL:int = 10;
		
		//map tiles
		var waterTile:WaterTile;
		var landTile:LandTile;
		const WATER:String  = "0";
		const LAND:String  = "1";
		const LAND_BOTTOM:String = "2";
		const LAND_TOP:String = "3";
		const LAND_LEFT:String = "4";
		const LAND_RIGHT:String = "5";
		const LAND_UPLEFT:String = "6";
		const LAND_UPRIGHT:String = "7";
		const LAND_BTMLEFT:String = "8";
		const LAND_BTMRIGHT:String = "9";
		const LAND_TOPBTM:String = "A";
		const LAND_TOPBTMLEFT:String = "B";
		const LAND_TOPBTMRIGHT:String = "C";
		
		//objects
		const TREE:String  = "A";
		const TRAP:String  = "B";
		const CREATURE:String  = "C";
		const PLAYER:String  = "P";
		
		//player direction frames.
		const LEFT:int = 1;
		const RIGHT:int = 10;
		const UP:int = 30;
		const DOWN:int = 20;
		
		//creature direction frames.
		const CLEFT:int = 1;
		const CRIGHT:int = 2;
		const CUP:int = 3;
		const CDOWN:int = 4;
			
		
		//player.  
		var player:Player;
		var playerDead:Boolean;
		var playerLives:int;
		var playerIcon:Player;
		var playerDestination:Point;	//used to move player to new spot on map.
		var playerDirection:int;
		var frameAdvance:int;			//used to change animation frames.
		
		//creatures & objects
		var creatureList:Vector.<Creature>;
		var treeList:Vector.<PalmTree>;
		var trapList:Vector.<Trap>;
		var destinationList:Vector.<Point>;		//contains list of creature destinations on map.
		
		
		//debug
		var debugText:TextField;
		var debugFormat:TextFormat;
		
		//music & sound
		var gameMusic:Sound;	
		var musicChannel:SoundChannel;
		var _volume:SoundTransform;
		var fallSound:Sound;
		var winSound:Sound;
		

		public function GameScreen(stageRef:Stage) 
		{
			this.stageRef = stageRef;
			
			//******VARIABLE INITIALIZATION GOES HERE********//
			level = 1;
			levelIndex = 0;
			playerLives = 2;
			playerDead = false;
			controlLocked = false;
			waitTimer = 0;
			previousTime = 0;
			deltaTime = 0;
			playerDirection = LEFT;
			frameAdvance = 0;
			
			mapArray = new Array();
			objectArray = new Array();
			initMapArray = new Array();
			initObjArray = new Array();
			waterTile = new WaterTile();
		    landTile = new LandTile();
			
			//container setup
			creatureList = new Vector.<Creature>();
			treeList = new Vector.<PalmTree>();
			trapList = new Vector.<Trap>();
			destinationList = new Vector.<Point>();
			
			//player Icon
			playerIcon = new Player();
			playerIcon.x = 70;
			playerIcon.y = 695;
			playerIcon.scaleX = 0.55;
			playerIcon.scaleY = 0.55;
			playerIcon.gotoAndStop(DOWN);
			
			//win label
			winLabel = new WinLabel();
			winLabel.gotoAndStop(1);
			winLabel.x = stageRef.stageWidth + 96;	//starts offscreen.
			winLabel.y = (stageRef.stageHeight * 0.5) - 40;
			
			
			/**********TEXT SETUP. Change values as necessary*******************/
			uiTextFormat = new TextFormat();
			uiTextFormat.size = 20;
			uiTextFormat.bold = true;
			uiTextFormat.font = "Moire";
			
			levelText = new TextField();
			levelText.defaultTextFormat = uiTextFormat;
			levelText.textColor = 0xFFFF00;
			levelText.width = 120;
			levelText.height = 40;
			levelText.x = 80;
			levelText.y = 30;
			levelText.selectable = false;
			
			livesText = new TextField();
			livesText.defaultTextFormat = uiTextFormat;
			livesText.textColor = 0xFFFF00;
			livesText.width = 100;
			livesText.height = 40;
			livesText.x = 110;
			livesText.y = 710;
			livesText.text = "x " + playerLives;
			livesText.selectable = false;
			
			enemyCountText = new TextField();
			enemyCountText.defaultTextFormat = uiTextFormat;
			enemyCountText.textColor = 0xCCCCFF;
			enemyCountText.width = 220;
			enemyCountText.height = 80;
			enemyCountText.x = 50;
			enemyCountText.y = 30;
			enemyCountText.text = "";	//change this as necessary
			enemyCountText.selectable = false;
			
			/*******DEBUG*******/
			debugFormat = new TextFormat();
			debugFormat.size = 20;
			debugFormat.bold = true;
			debugFormat.font = "Fixedsys";
			
			debugText = new TextField();
			debugText.defaultTextFormat = debugFormat;
			debugText.textColor = 0xCCCCFF;
			debugText.width = 260;
			debugText.height = 200;
			debugText.x = 650;
			debugText.y = 50;
			debugText.selectable = false;
			
			/********SOUND SETUP****************/
			gameMusic = new Sound(new URLRequest("StarTropics - Cave.mp3"));	//StarTropics music
			musicChannel = new SoundChannel();
			_volume = new SoundTransform(0.5);	//50% volume
			fallSound = new FallSound();
			winSound = new WinSound();
			
			/*******READ FROM XML FILE*************/
			
			//level = Main.levels.lvl[levelIndex].attributes()[0];	//gets level number (index 0) from file.
			
			loadLevel(level);
			
			 
			/*****ADD ALL CHILD ELEMENTS HERE*****/
			
			updateUI();
	
			/****EVENT LISTENERS*****/
			addEventListener(Event.ENTER_FRAME, Update);
			stageRef.addEventListener(KeyboardEvent.KEY_UP, getInput);
			
			/*******play music***********/
			playMusic(musicChannel, gameMusic);
			
		}
		
		

		function playMusic(channel:SoundChannel, sound:Sound):void
		{
			channel = sound.play();
			channel.soundTransform = _volume;
			channel.addEventListener(Event.SOUND_COMPLETE, Replay);
		}
		
		function Replay(event:Event):void
		{
			//remove the event listener and start music again.
			playMusic(musicChannel, gameMusic);
			SoundChannel(event.target).removeEventListener(event.type, Replay);
		}

		function loadLevel(level:int):void
		{
			mapArray = new Array();
			objectArray = new Array();
			initObjArray = new Array();
			
			if (level <= 0)
				level = 1;
				
			
			/***load level. Two sets of arrays are used: one for the map terrain, and one for the objects. ***/
			mapRowList = Main.levels.lvl[level-1].mapData.row;
			objRowList = Main.levels.lvl[level-1].objects.row;
			
			for each(var row:XML in mapRowList)
			{
				var rowString:String = row.text();			//converts row values in file to string. This is done because there are commas in the rows.
				var rowArray:Array = rowString.split(",");	//comma is used to separate values.
				
				mapArray.push(rowArray);	//this creates a nested array.
			}
			
			for each(var row:XML in objRowList)
			{
				var rowString:String = row.text();	
				var rowArray:Array = rowString.split(",");
				
				objectArray.push(rowArray);
			}
			
			//clone the initObjArray so that it has its own references.
			initObjArray = clone(objectArray);
			
			buildMap(mapArray);
			buildObjects(objectArray);
		}
		
		//checks map array and draws terrain to the screen.
		function buildMap(sourceArray:Array):void
		{
			for (var row:int = 0; row < MAX_ROWS; row++)
			{
				for (var col:int = 0; col < MAX_COLS; col++)
				{
					switch(sourceArray[row][col])
					{
						case WATER:
							waterTile = new WaterTile();
							waterTile.x = col * waterTile.width;
							waterTile.y = row * waterTile.height;
							addChild(waterTile);
							break;
							
						case LAND:
							landTile = new LandTile();
							landTile.gotoAndStop(1);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_BOTTOM:
							landTile = new LandTile();
							landTile.gotoAndStop(2);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_TOP:
							landTile = new LandTile();
							landTile.gotoAndStop(3);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_LEFT:
							landTile = new LandTile();
							landTile.gotoAndStop(4);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_RIGHT:
							landTile = new LandTile();
							landTile.gotoAndStop(5);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_UPLEFT:
							landTile = new LandTile();
							landTile.gotoAndStop(6);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_UPRIGHT:
							landTile = new LandTile();
							landTile.gotoAndStop(7);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_BTMLEFT:
							landTile = new LandTile();
							landTile.gotoAndStop(8);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
						
						case LAND_BTMRIGHT:
							landTile = new LandTile();
							landTile.gotoAndStop(9);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_TOPBTM:
							landTile = new LandTile();
							landTile.gotoAndStop(10);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
							
						case LAND_TOPBTMLEFT:
							landTile = new LandTile();
							landTile.gotoAndStop(11);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
						
						case LAND_TOPBTMRIGHT:
							landTile = new LandTile();
							landTile.gotoAndStop(12);
							landTile.x = col * landTile.width;
							landTile.y = row * landTile.height;
							addChild(landTile);
							break;
					}
				}
			}
		}
		
		//checks object array and draws objects to the screen. Must always follow after buildMap.
		function buildObjects(sourceArray:Array):void
		{
			for (var row:int = 0; row < MAX_ROWS; row++)
			{
				for (var col:int = 0; col < MAX_COLS; col++)
				{
					switch(sourceArray[row][col])
					{
						case TREE:
							var tree:PalmTree = new PalmTree();
							tree.x = col * TILE_SIZE;
							tree.y = row * TILE_SIZE;
							treeList.push(tree);
							addChild(tree);
							break;
							
						case CREATURE:
							var creature:Creature = new Creature();
							creature.x = col * TILE_SIZE;
							creature.y = row * TILE_SIZE;
							creatureList.push(creature);
							destinationList.push(new Point(creature.x, creature.y));
							addChild(creature);
							creature.gotoAndStop(LEFT);
							
							break;
							
						case TRAP:
							var trap:Trap = new Trap();
							trap.x = col * TILE_SIZE;
							trap.y = row * TILE_SIZE;
							trapList.push(trap);
							addChild(trap);
							break;
							
						case PLAYER:
							player = new Player();
							player.x = col * TILE_SIZE;
							player.y = row * TILE_SIZE;
							addChild(player);
							playerDestination = new Point(player.x, player.y);
							player.gotoAndStop(UP);
							break;
					}
				}
			}
		}
		
		//updates object positions. If both objects move to the same space, the player
		//dies.
		function updateObjects():void
		{
						
			if (player.x < playerDestination.x)
			{
				player.x += MOVE_SPEED;
				player.gotoAndPlay(RIGHT + frameAdvance);	//animate right.
				if (++frameAdvance > 8)
					frameAdvance = 0;
			}
			else if (player.x > playerDestination.x)
			{
				player.x -= MOVE_SPEED;
				player.gotoAndPlay(LEFT + frameAdvance);	//animate left
				if (++frameAdvance > 8)
					frameAdvance = 0;
				
			}
			else if (player.y < playerDestination.y)
			{
				player.y += MOVE_SPEED;
				player.gotoAndPlay(DOWN + frameAdvance);	//animate down
				//frameAdvance++;
				if (++frameAdvance > 8)
					frameAdvance = 0;
				
			}
			else if (player.y > playerDestination.y)
			{
				player.y -= MOVE_SPEED;
				player.gotoAndPlay(UP + frameAdvance);		//animate up
				if (++frameAdvance > 8)
					frameAdvance = 0;
				
			}
			else	//let the player move again.
			{
				controlLocked = false;
				frameAdvance = 0;
				player.gotoAndStop(DOWN);
			}

			
			for (var i:int = 0; i < creatureList.length; i++)
			{
				if (creatureList[i].x < destinationList[i].x)
				{
					creatureList[i].x += MOVE_SPEED;
					creatureList[i].gotoAndStop(CRIGHT);
				}
				else if (creatureList[i].x > destinationList[i].x)
				{
					creatureList[i].x -= MOVE_SPEED;
					creatureList[i].gotoAndStop(CLEFT);
				}
				
				else if (creatureList[i].y < destinationList[i].y)
				{
					creatureList[i].y += MOVE_SPEED;
					creatureList[i].gotoAndStop(CDOWN);
				}
				else if (creatureList[i].y > destinationList[i].y)
				{
					creatureList[i].y -= MOVE_SPEED;
					creatureList[i].gotoAndStop(CUP);
				}
			}		
		}
		
		function removeTrap(sourceArray:Array, rowPos:int, colPos:int):void
		{
			var index:int = 0;
			var targetPoint:Point = new Point(colPos * TILE_SIZE, rowPos * TILE_SIZE);
			for each (var trap:Trap in trapList)
			{
				if(trap.x == targetPoint.x && trap.y == targetPoint.y)
				{
					sourceArray[rowPos][colPos] = "0";
					trapList.splice(index, 1);
					removeChild(trap);
				}
				else
					index++;
			}
			
		}
		
		function removeCreature(sourceArray:Array, rowPos:int, colPos:int):void
		{
			var index:int = 0;
			var targetPoint:Point = new Point(colPos * TILE_SIZE, rowPos * TILE_SIZE);
			for each (var c:Creature in creatureList)
			{
				//get the position of the current creature and compare it to the target position.
				//if there's a match, then we want to remove this creature.
				if(c.x == targetPoint.x && c.y == targetPoint.y)
				{
					fallSound.play();
					sourceArray[rowPos][colPos] = "0";
					creatureList.splice(index, 1);
					destinationList.splice(index, 1);
					removeChild(c);
				}
				else
					index++;
			}
		}
		
	
		
		function getInput(event:KeyboardEvent):void
		{
			//Anytime the player moves, all creatures move in the opposite direction if possible.
			//use the arrays to move objects around and to check for collision.
			if (!controlLocked)
			{
				var rowPos:int = player.y / TILE_SIZE;
				var colPos:int = player.x / TILE_SIZE;		//player position in array
				
				switch(event.keyCode)
				{
					case Keyboard.A:	//left
						if (colPos > 0 && objectArray[rowPos][colPos-1] != TREE && mapArray[rowPos][colPos-1] == LAND)
						{
							//if the destination is a creature or a trap, then player is dead
							if (objectArray[rowPos][colPos-1] == TRAP || objectArray[rowPos][colPos-1] == CREATURE)
								playerDead = true;
												
							//move player to new position
							controlLocked = true;
							objectArray[rowPos][colPos] = "0";
							colPos--;
							objectArray[rowPos][colPos] = PLAYER;
							playerDestination = new Point(colPos * TILE_SIZE, rowPos * TILE_SIZE);
							
							//move creatures in opposite direction.
							for each(var c:Creature in creatureList)
							{
								var cRowPos:int = c.y / TILE_SIZE;
								var cColPos:int = c.x / TILE_SIZE;								
								
								if (cColPos < MAX_COLS-1 && objectArray[cRowPos][cColPos+1] != TREE && objectArray[cRowPos][cColPos+1] != CREATURE
									&& mapArray[cRowPos][cColPos+1] == LAND)
									
								{
									if (objectArray[cRowPos][cColPos+1] == TRAP)
									{
										//creature is trapped; remove both the creature and the trap
										removeTrap(objectArray, cRowPos, cColPos+1);
										removeCreature(objectArray, cRowPos, cColPos);
									}
									else if (objectArray[cRowPos][cColPos+1] == PLAYER)
									{
										//player and creature are occupying the same space.
										playerDead = true;
									}
									else
									{
										//move creature to new position
										objectArray[cRowPos][cColPos] = "0";
										cColPos++;
										objectArray[cRowPos][cColPos] = CREATURE;
										
										//update destination list
										var index:int = creatureList.indexOf(c);
										var point:Point = new Point(cColPos * TILE_SIZE, cRowPos * TILE_SIZE);
										destinationList[index] = point;
									}
								}
							}
						}
						break;
					
					case Keyboard.D:	//right
						if (colPos < MAX_COLS-1 && objectArray[rowPos][colPos+1] != TREE && mapArray[rowPos][colPos+1] == LAND)
						{
							//if the destination is a creature or a trap, then player is dead
							if (objectArray[rowPos][colPos+1] == TRAP || objectArray[rowPos][colPos+1] == CREATURE)
								playerDead = true;
								
							//move player to new position
							controlLocked = true;
							objectArray[rowPos][colPos] = "0";
							colPos++;
							objectArray[rowPos][colPos] = PLAYER;
							playerDestination = new Point(colPos * TILE_SIZE, rowPos * TILE_SIZE);
							
							//move creatures in opposite direction.
							for each(var c:Creature in creatureList)
							{
								var cRowPos:int = c.y / TILE_SIZE;
								var cColPos:int = c.x / TILE_SIZE;								
								
								if (cColPos > 0 && objectArray[cRowPos][cColPos-1] != TREE && objectArray[cRowPos][cColPos-1] != CREATURE
									&& mapArray[cRowPos][cColPos-1] == LAND)
								{
									if (objectArray[cRowPos][cColPos-1] == TRAP)
									{
										//creature is trapped; remove both the creature and the trap
										removeTrap(objectArray, cRowPos, cColPos-1);
										removeCreature(objectArray, cRowPos, cColPos);
									}
									else if (objectArray[cRowPos][cColPos-1] == PLAYER)
									{
										playerDead = true;
									}
									else
									{
										//move creature to new position
										objectArray[cRowPos][cColPos] = "0";
										cColPos--;
										objectArray[cRowPos][cColPos] = CREATURE;
										
										//update destination list
										var index:int = creatureList.indexOf(c);
										var point:Point = new Point(cColPos * TILE_SIZE, cRowPos * TILE_SIZE);
										destinationList[index] = point;
									}
								}
							}
						}
						break;
						
					case Keyboard.W:	//up
						if (rowPos > 0 && objectArray[rowPos-1][colPos] != TREE && mapArray[rowPos-1][colPos] == LAND)
						{
							//if the destination is a creature or a trap, then player is dead
							if (objectArray[rowPos-1][colPos] == TRAP || objectArray[rowPos-1][colPos] == CREATURE)
								playerDead = true;
								
							//move player to new position
							controlLocked = true;
							objectArray[rowPos][colPos] = "0";
							rowPos--;
							objectArray[rowPos][colPos] = PLAYER;
							playerDestination = new Point(colPos * TILE_SIZE, rowPos * TILE_SIZE);
							
							//move creatures in opposite direction.
							for each(var c:Creature in creatureList)
							{
								var cRowPos:int = c.y / TILE_SIZE;
								var cColPos:int = c.x / TILE_SIZE;								
								
								if (cRowPos < MAX_ROWS-1 && objectArray[cRowPos+1][cColPos] != TREE && objectArray[cRowPos+1][cColPos] != CREATURE
									&& mapArray[cRowPos+1][cColPos] == LAND)
								{
									if (objectArray[cRowPos+1][cColPos] == TRAP)
									{
										//creature is trapped; remove both the creature and the trap
										removeTrap(objectArray, cRowPos+1, cColPos);
										removeCreature(objectArray, cRowPos, cColPos);
									}
									else if (objectArray[cRowPos+1][cColPos] == PLAYER)
									{
										playerDead = true;
									}
									else
									{
										//move creature to new position
										objectArray[cRowPos][cColPos] = "0";
										cRowPos++;
										objectArray[cRowPos][cColPos] = CREATURE;
										
										//update destination list
										var index:int = creatureList.indexOf(c);
										var point:Point = new Point(cColPos * TILE_SIZE, cRowPos * TILE_SIZE);
										destinationList[index] = point;
									}
								}
							}
						}
						break;
						
					case Keyboard.S:	//down
						if (rowPos < MAX_ROWS-1 && objectArray[rowPos+1][colPos] != TREE && mapArray[rowPos+1][colPos] == LAND)
						{
							//if the destination is a creature or a trap, then player is dead
							if (objectArray[rowPos+1][colPos] == TRAP || objectArray[rowPos+1][colPos] == CREATURE)
								playerDead = true;
								
							//move player to new position
							controlLocked = true;
							objectArray[rowPos][colPos] = "0";
							rowPos++;
							objectArray[rowPos][colPos] = PLAYER;
							playerDestination = new Point(colPos * TILE_SIZE, rowPos * TILE_SIZE);
							
							//move creatures in opposite direction.
							for each(var c:Creature in creatureList)
							{
								var cRowPos:int = c.y / TILE_SIZE;
								var cColPos:int = c.x / TILE_SIZE;
					
								
								if (cRowPos > 0 && objectArray[cRowPos-1][cColPos] != TREE && objectArray[cRowPos-1][cColPos] != CREATURE
									&& mapArray[cRowPos-1][cColPos] == LAND)
								{
									if (objectArray[cRowPos-1][cColPos] == TRAP)
									{
										//creature is trapped; remove both the creature and the trap
										removeTrap(objectArray, cRowPos-1, cColPos);
										removeCreature(objectArray, cRowPos, cColPos);
									}
									else if (objectArray[cRowPos-1][cColPos] == PLAYER)
									{
										playerDead = true;
									}
									else
									{
										//move creature to new position
										objectArray[cRowPos][cColPos] = "0";
										cRowPos--;
										objectArray[cRowPos][cColPos] = CREATURE;
										
										//update destination list
										var index:int = creatureList.indexOf(c);
										var point:Point = new Point(cColPos * TILE_SIZE, cRowPos * TILE_SIZE);
										destinationList[index] = point;
									}
								}
							}
						}
						break;
				}
			}
			
			/*var output:String = "";
			for (var i:int = 0; i < 12; i++)
			{
				for (var j:int = 0; j < 16; j++)
				{
					output += objectArray[i][j];
				}
				
				output += "\n";
			}
			trace (output);*/
		}
		
		
		function UpdateDebug()
		{
			/* Update debug data in here. This function must go in the update loop. */
			//get FPS
			
			var fps:int = 1000 / deltaTime;
			
			
			//get memory
			var mb:Number = System.totalMemory / 1024 / 1024;	//amount in Megabytes
			var memory:Number = Math.round(mb * 100) / 100;
			
			debugText.text = "Creature Count: " + creatureList.length +
								"\nTrap Count: " + trapList.length +
								"\nTree Count: " + treeList.length +
								"\nPlayer Pos: (" + player.x + ", " + player.y + ")" +
								"\nFPS: " + fps + 
								"\nMemory Used: " + memory + "MB";
								//trace ("Frame rate: " + fps);
								
			
		}
		
		//resets current level to original state
		function resetLevel()
		{
			for each (var c:Creature in creatureList)
			{
				removeChild(c);
			}
			creatureList = new Vector.<Creature>();
			destinationList = new Vector.<Point>();
			
			for each (var t:PalmTree in treeList)
			{
				removeChild(t);
			}
			treeList = new Vector.<PalmTree>();
			
			for each (var trap:Trap in trapList)
			{
				removeChild(trap);
			}
			trapList = new Vector.<Trap>();
			
			removeChild(player);
			
			
			//create a deep copy of the inital object array and store the contents into the object array.					
			objectArray = clone(initObjArray);
			buildObjects(objectArray);
		}
		
		function ResetGame()
		{
			/* All variables and containers are reset to default in here.*/
			
		}
		
		//creates a deep copy of an array so that changes made to one array doesn't affect another.
		//byte arrays must be used.
		function clone(sourceArray:Object):*
		{
			var tempArray:ByteArray = new ByteArray();
			tempArray.writeObject(sourceArray);
			tempArray.position = 0;
			return (tempArray.readObject());
		}
		
		//adds HUD to screen.
		function updateUI():void
		{
			//reset states & update text
			winLabel.x = stageRef.stageWidth + 96;
			winLabel.gotoAndStop(1);
			livesText.text = "x " + playerLives;
			levelText.text = "STAGE " + level;
			
			addChild(debugText);
			addChild(levelText);
			addChild(livesText);
			addChild(playerIcon);
			addChild(winLabel);
		}
		
		
		function Update(event:Event):void
		{
			//get delta time
			var time:int = getTimer();
			deltaTime = time - previousTime;
			previousTime = getTimer();
			//trace ("Delta Time: " + deltaTime);
			
			/* reduce fadescreen alpha. */
			if (Main.fadeScreen.alpha > 0)
				Main.fadeScreen.alpha -= 0.015;
			/* Game loop typically ends when a terminating condition is met. Until that happens, everything that isn't an onscreen image
			gets updated here.  Collision checking must be performed here. */
			if (!gameOver)
			{
				
				/*******Player movement*******/
				
				
				/************Recreate level if player dies***********/
				if (playerDead)
				{
					fallSound.play();
					//update lives
					if (playerLives == 0)
						//game over
						gameOver = true;
					else
					{
						Main.fadeScreen.alpha = 1;
						resetLevel();
						playerLives--;
						updateUI();
					}
						
					playerDead = false;
				}
				
				//win condition
				if (creatureList.length == 0)
				{
					controlLocked = true;
					if (winLabel.x > stageRef.stageWidth * 0.3)	//scroll win label till it reaches the centre
						winLabel.x -= 16;
					else
					{
						if (waitTimer == 0)	//ensures sound only plays once.
							winSound.play();
							
						waitTimer++;
						if (waitTimer > 60)
						{
							waitTimer = 0;
							Main.fadeScreen.alpha = 1;
							playerLives++;
							
							//there are 10 levels.  Game restarts if player beats all 10 levels.
							if (level >= MAX_LEVEL)
								level = 0;
							
							loadLevel(++level);
							resetLevel();
							controlLocked = false;
						
							//I have to add the UI again because they get covered up by
							//the new level's tiles.
							updateUI();
							
						}
					}
				}
				else
					updateObjects();
				
		
				
				/**********debug**********/
				//UpdateDebug();
			}	
			else	
			{
				/* If terminating condition is met, the result goes here. */
				winLabel.gotoAndStop(2);	//shows the "game over" image
				controlLocked = true;
				if (winLabel.x > stageRef.stageWidth * 0.3)	//scroll win label till it reaches the centre
					winLabel.x -= 16;
				else
				{
					//reset game.
					waitTimer++;
					if (waitTimer > 60)
					{
						waitTimer = 0;
						Main.fadeScreen.alpha = 1;
						playerLives = 2;
						level = 1;
						loadLevel(level);
						resetLevel();
						controlLocked = false;
						gameOver = false;
						updateUI();
					}
				}
			}
		}//end Update()

	}
	
}
