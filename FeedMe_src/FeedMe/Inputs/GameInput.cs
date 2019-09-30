using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace FeedMe.Inputs
{
    class GameInput
    {
        Dictionary<string, Input> inputs = new Dictionary<string, Input>();

        public GameInput()
        {
        }

        public Input MyInput(string theAction)
        {
            // Add the Action if it doesn't already exist
            if (inputs.ContainsKey(theAction) == false)
            {
                inputs.Add(theAction, new Input());
            }

            return inputs[theAction];
        }

        public void BeginUpdate()
        {
            Input.BeginUpdate();
        }

        public void EndUpdate()
        {
            Input.EndUpdate();
        }

        public bool IsConnected(PlayerIndex thePlayer)
        {
            // If there never WAS a gamepad connected, then just
            // say that the gamepad is still connected...
            if (Input.GamepadConnectionState[thePlayer] == false)
            {
                return true;
            }

            return Input.IsConnected(thePlayer);
        }

        public bool IsPressed(string theAction,
                              Rectangle theCurrentObjectLocation)
        {
            if (!inputs.ContainsKey(theAction))
            {
                return false;
            }

            return inputs[theAction].IsPressed(PlayerIndex.One,
                                               theCurrentObjectLocation);
        }

        public bool IsPressed(string theAction)
        {
            if (!inputs.ContainsKey(theAction))
            {
                return false;
            }
            return inputs[theAction].IsPressed(PlayerIndex.One);
        }

        public bool IsPressed(string theAction, PlayerIndex thePlayer)
        {
            if (inputs.ContainsKey(theAction) == false)
            {
                return false;
            }

            return inputs[theAction].IsPressed(thePlayer);
        }

        public bool IsPressed(string theAction, PlayerIndex? thePlayer)
        {
            if (thePlayer == null)
            {
                PlayerIndex theReturnedControllingPlayer;
                return IsPressed(theAction,
                                 thePlayer,
                                 out theReturnedControllingPlayer);
            }

            return IsPressed(theAction, (PlayerIndex)thePlayer);
        }

        public bool IsPressed(string theAction,
                              PlayerIndex? thePlayer,
                              out PlayerIndex theControllingPlayer)
        {
            if (!inputs.ContainsKey(theAction))
            {
                theControllingPlayer = PlayerIndex.One;
                return false;
            }

            if (thePlayer == null)
            {
                if (IsPressed(theAction, PlayerIndex.One))
                {
                    theControllingPlayer = PlayerIndex.One;
                    return true;
                }

                if (IsPressed(theAction, PlayerIndex.Two))
                {
                    theControllingPlayer = PlayerIndex.Two;
                    return true;
                }

                if (IsPressed(theAction, PlayerIndex.Three))
                {
                    theControllingPlayer = PlayerIndex.Three;
                    return true;
                }

                if (IsPressed(theAction, PlayerIndex.Four))
                {
                    theControllingPlayer = PlayerIndex.Four;
                    return true;
                }

                theControllingPlayer = PlayerIndex.One;
                return false;
            }

            theControllingPlayer = (PlayerIndex)thePlayer;
            return IsPressed(theAction, (PlayerIndex)thePlayer);
        }

        public void AddGamePadInput(string theAction,
                                    Buttons theButton,
                                    bool isReleasedPreviously)
        {
            MyInput(theAction).AddGamepadInput(theButton,
                                               isReleasedPreviously);
        }

        public void AddKeyboardInput(string theAction,
                                     Keys theKey,
                                     bool isReleasedPreviously)
        {
            MyInput(theAction).AddKeyboardInput(theKey,
                                                isReleasedPreviously);
        }

        public void AddTouchTapInput(string theAction,
                                     Rectangle theTouchArea,
                                     bool isReleasedPreviously)
        {
            MyInput(theAction).AddTouchTapInput(theTouchArea,
                                                isReleasedPreviously);
        }

        public void AddTouchSlideInput(string theAction,
                                       Input.Direction theDirection,
                                       float slideDistance)
        {
            MyInput(theAction).AddTouchSlideInput(theDirection,
                                                  slideDistance);
        }

        public void AddTouchGestureInput(string theAction,
                                         GestureType theGesture,
                                         Rectangle theRectangle)
        {
            MyInput(theAction).AddTouchGesture(theGesture, theRectangle);
        }

        //public void AddAccelerometerInput(string theAction,
        //                                  Input.Direction theDirection,
        //                                  float tiltThreshold)
        //{
        //    MyInput(theAction).AddAccelerometerInput(theDirection,
        //                                             tiltThreshold);
        //}

        public Vector2 CurrentGesturePosition(string theAction)
        {
            return MyInput(theAction).CurrentGesturePosition();
        }

        public Vector2 CurrentGestureDelta(string theAction)
        {
            return MyInput(theAction).CurrentGestureDelta();
        }

        public Vector2 CurrentGesturePosition2(string theAction)
        {
            return MyInput(theAction).CurrentGesturePosition2();
        }

        public Vector2 CurrentGestureDelta2(string theAction)
        {
            return MyInput(theAction).CurrentGestureDelta2();
        }

        public Point CurrentTouchPoint(string theAction)
        {
            Vector2? currentPosition =
                     MyInput(theAction).CurrentTouchPosition();
            if (currentPosition == null)
            {
                return new Point(-1, -1);
            }

            return new Point((int)currentPosition.Value.X,
                             (int)currentPosition.Value.Y);
        }

        public Vector2 CurrentTouchPosition(string theAction)
        {
            Vector2? currentTouchPosition =
                     MyInput(theAction).CurrentTouchPosition();
            if (currentTouchPosition == null)
            {
                return new Vector2(-1, -1);
            }

            return (Vector2)currentTouchPosition;
        }

        public float CurrentGestureScaleChange(string theAction)
        {
            // Scaling is dependent on the Pinch gesture. 
            // If no input has been setup for Pinch then just 
            // return 0 indicating no scale change has occurred.
            if (!MyInput(theAction).PinchGestureAvailable)
            {
                return 0;
            }

            // Get the current and previous locations of the two fingers
            Vector2 currentPositionFingerOne =
                    CurrentGesturePosition(theAction);

            Vector2 previousPositionFingerOne =
                    CurrentGesturePosition(theAction)
                    - CurrentGestureDelta(theAction);

            Vector2 currentPositionFingerTwo =
                    CurrentGesturePosition2(theAction);

            Vector2 previousPositionFingerTwo =
                    CurrentGesturePosition2(theAction)
                    - CurrentGestureDelta2(theAction);

            // Figure out the distance between current & previous locations
            float currentDistance =
                  Vector2.Distance(currentPositionFingerOne,
                                   currentPositionFingerTwo);

            float previousDistance =
                  Vector2.Distance(previousPositionFingerOne,
                                   previousPositionFingerTwo);

            // Calculate the diff between the two & use it to alter the scale
            float scaleChange = (currentDistance - previousDistance) * .01f;
            return scaleChange;
        }

        public Vector3 CurrentAccelerometerReading(string theAction)
        {
            return MyInput(theAction).CurrentAccelerometerReading;
        }
    }
}
