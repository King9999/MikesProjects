using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace TileCrusher.Inputs
{
    class GestureDefinition
    {
        public GestureType Type;
        public Rectangle CollisionArea;
        public GestureSample Gesture;
        public Vector2 Delta;
        public Vector2 Delta2;
        public Vector2 Position;
        public Vector2 Position2;

        public GestureDefinition(GestureType theGestureType,
                                 Rectangle theGestureArea)
        {
            Gesture = new GestureSample(theGestureType,
                                        new TimeSpan(0),
                                        Vector2.Zero,
                                        Vector2.Zero,
                                        Vector2.Zero,
                                        Vector2.Zero);
            Type = theGestureType;
            CollisionArea = theGestureArea;
        }

        public GestureDefinition(GestureSample theGestureSample)
        {
            Gesture = theGestureSample;
            Type = theGestureSample.GestureType;
            CollisionArea = new Rectangle((int)theGestureSample.Position.X,
                                          (int)theGestureSample.Position.Y,
                                          5,
                                          5);

            Delta = theGestureSample.Delta;
            Delta2 = theGestureSample.Delta2;
            Position = theGestureSample.Position;
            Position2 = theGestureSample.Position2;
        }
    }
}
