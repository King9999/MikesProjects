﻿/* Each defeated enemy gives a score; bonus points can be awarded depending on how an enemy is 
defeated.
normal kill (destroy an enemy who doesn't oppose the player's colour) - 1x bonus
critical (destroy an enemy that opposes player's colour) - 2x bonus
absorb (player touches an enemy of same colour) - 3x bonus 

Enemies have different flight paths.  They have a starting point and an end point, but how they reach their destination can vary.*/

package  
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
	import flash.display.Shape;
	import flash.display.Sprite;
	import flash.media.Sound;
	import flash.media.SoundChannel;
	import flash.net.URLRequest;
	import flash.media.SoundTransform;
	import flash.geom.Point;
	import flash.display.BitmapData;
	import flash.display.Bitmap;
	
	public class Enemy extends MovieClip
	{
		protected var startPoint:Point;		//the point the enemy starts at, usually offscreen.
		protected var endPoint:Point;			//the point the enemy travels to.
		protected var colour:uint;			//designated colour. 1 = white, 2 = red, 3 = blue, 4 = black
		protected var meterGain:Number;		//the amount added to rainbow gauge if absorbed
		
		

		public function Enemy(startPt:Point, endPt:Point, meterAmt:Number, colour:uint) 
		{
			
			startPoint = startPt;
			endPoint = endPt;
			meterGain = meterAmt;
			this.x = startPt.x;
			this.y = startPt.y;
			this.colour = colour;
		}
		
		public function StartPoint():Point
		{
			return startPoint;
		}
		
		public function EndPoint():Point
		{
			return endPoint;
		}
		
		public function MeterAmount():Number
		{
			return meterGain;
		}
		
		/*public function Image():DisplayObject
		{
			return graphic;
		}*/
		
		public function Colour():uint
		{
			/*var colourStr:String;
			
			switch(colour)
			{
				case 1:
					colourStr = "White";
					break;
				case 2:
					colourStr = "Red";
					break;
				case 3:
					colourStr = "Blue";
					break;
				case 4:
					colourStr = "Black";
					break;
			}*/
			
			return colour;
		}
		
		/* This function is used by enemies to follow a path specified in bitmaps containing lines. A starting position
		is supplied depending on the path bitmap. 
		path: the image containing the data
		startPoint: where the object begins on the path
		startAngle: the direction (in degrees) the object moves. If no point is found, the angle changes until one is found. */
		public function FollowPath(path:BitmapData, startAngle:Number)
		{
			var xPos:Number = 0;
			var yPos:Number = 0;
			var angle:Number;
			var pixelFound:Boolean;
			
			//first check if the starting point contains a pixel of the path.
			if (path.getPixel(startPoint.x, startPoint.y) != 0)
			{
				try
				{
					angle = 0;
					while (angle < 360 && !pixelFound)
					//for (angle = startAngle; angle < 360; angle++)
					{
						//check for pixels at current angle. Must convert angle to radians and then get the sine add result to 
						//object's If there is no pixel, then move to next angle.
						var angleRad:Number = angle * Math.PI / 180;
						xPos = Math.sin(angleRad) * 15 + startPoint.x;
						yPos = Math.cos(angleRad) * 15 + startPoint.y;
						
						if (path.getPixel(xPos, yPos) != 0)
						{
							//move to this spot
							startPoint.x = xPos;
							startPoint.y = yPos;
							trace ("X: " + xPos + " Y: " + yPos);
							pixelFound = true;
						}
						else
							angle++;
					}
				}
				catch(error:Error)
				{
					trace ("Object not on path")
				}
			}
		}

	}
	
}
