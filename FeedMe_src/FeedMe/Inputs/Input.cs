using System;
using System.Collections.Generic;


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
//using Microsoft.Devices.Sensors;

namespace FeedMe.Inputs
{
    class Input
    {
        Dictionary<Keys, bool> keyboardDefinedInputs = new Dictionary<Keys, bool>();
        Dictionary<Buttons, bool> gamepadDefinedInputs = new Dictionary<Buttons, bool>();
        Dictionary<Rectangle, bool> touchTapDefinedInputs = new Dictionary<Rectangle, bool>();
        Dictionary<Direction, float> touchSlideDefinedInputs = new Dictionary<Direction, float>();
        Dictionary<int, GestureDefinition> gestureDefinedInputs = new Dictionary<int, GestureDefinition>();
        Dictionary<Direction, float> accelerometerDefinedInputs = new Dictionary<Direction, float>();

        static public Dictionary<PlayerIndex, GamePadState> CurrentGamePadState = new Dictionary<PlayerIndex, GamePadState>();
        static public Dictionary<PlayerIndex, GamePadState> PreviousGamePadState = new Dictionary<PlayerIndex, GamePadState>();
        static public KeyboardState CurrentKeyboardState;
        static public KeyboardState PreviousKeyboardState;
        static public TouchCollection CurrentTouchLocationState;
        static public TouchCollection PreviousTouchLocationState;
        static public Dictionary<PlayerIndex, bool> GamepadConnectionState = new Dictionary<PlayerIndex, bool>();

        static private List<GestureDefinition> detectedGestures = new List<GestureDefinition>();
       // static private Accelerometer accelerometerSensor;
        static private Vector3 currentAccelerometerReading;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public Input()
        {
            if (CurrentGamePadState.Count == 0)
            {
                CurrentGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                CurrentGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                CurrentGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                CurrentGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

                PreviousGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                PreviousGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                PreviousGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                PreviousGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

                GamepadConnectionState.Add(PlayerIndex.One, CurrentGamePadState[PlayerIndex.One].IsConnected);
                GamepadConnectionState.Add(PlayerIndex.Two, CurrentGamePadState[PlayerIndex.Two].IsConnected);
                GamepadConnectionState.Add(PlayerIndex.Three, CurrentGamePadState[PlayerIndex.Three].IsConnected);
                GamepadConnectionState.Add(PlayerIndex.Four, CurrentGamePadState[PlayerIndex.Four].IsConnected);
            }

            //if (accelerometerSensor == null)
            //{
            //    accelerometerSensor = new Accelerometer();
            //    accelerometerSensor.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(AccelerometerReadingChanged);
            //}
        }

        static public void BeginUpdate()
        {
            CurrentGamePadState[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            CurrentGamePadState[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            CurrentGamePadState[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            CurrentGamePadState[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            CurrentKeyboardState = Keyboard.GetState(PlayerIndex.One);
            CurrentTouchLocationState = TouchPanel.GetState();

            detectedGestures.Clear();
            if (TouchPanel.EnabledGestures != GestureType.None)
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    detectedGestures.Add(new GestureDefinition(gesture));
                }
            }
        }

        static public void EndUpdate()
        {
            PreviousGamePadState[PlayerIndex.One] = CurrentGamePadState[PlayerIndex.One];
            PreviousGamePadState[PlayerIndex.Two] = CurrentGamePadState[PlayerIndex.Two];
            PreviousGamePadState[PlayerIndex.Three] = CurrentGamePadState[PlayerIndex.Three];
            PreviousGamePadState[PlayerIndex.Four] = CurrentGamePadState[PlayerIndex.Four];

            PreviousKeyboardState = CurrentKeyboardState;
            PreviousTouchLocationState = CurrentTouchLocationState;
        }

        //private void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        //{
        //    currentAccelerometerReading.X = (float)e.X;
        //    currentAccelerometerReading.Y = (float)e.Y;
        //    currentAccelerometerReading.Z = (float)e.Z;
        //}

        public void AddKeyboardInput(Keys theKey, bool isReleasedPreviously)
        {
            if (keyboardDefinedInputs.ContainsKey(theKey))
            {
                keyboardDefinedInputs[theKey] = isReleasedPreviously;
                return;
            }
            keyboardDefinedInputs.Add(theKey, isReleasedPreviously);
        }

        public void AddGamepadInput(Buttons theButton, bool isReleasedPreviously)
        {
            if (gamepadDefinedInputs.ContainsKey(theButton))
            {
                gamepadDefinedInputs[theButton] = isReleasedPreviously;
                return;
            }
            gamepadDefinedInputs.Add(theButton, isReleasedPreviously);
        }

        public void AddTouchTapInput(Rectangle theTouchArea, bool isReleasedPreviously)
        {
            if (touchTapDefinedInputs.ContainsKey(theTouchArea))
            {
                touchTapDefinedInputs[theTouchArea] = isReleasedPreviously;
                return;
            }
            touchTapDefinedInputs.Add(theTouchArea, isReleasedPreviously);
        }

        public void AddTouchSlideInput(Direction theDirection, float slideDistance)
        {
            if (touchSlideDefinedInputs.ContainsKey(theDirection))
            {
                touchSlideDefinedInputs[theDirection] = slideDistance;
                return;
            }
            touchSlideDefinedInputs.Add(theDirection, slideDistance);
        }

        public bool PinchGestureAvailable = false;
        public void AddTouchGesture(GestureType theGesture, Rectangle theTouchArea)
        {
            TouchPanel.EnabledGestures = theGesture | TouchPanel.EnabledGestures;
            gestureDefinedInputs.Add(gestureDefinedInputs.Count, new GestureDefinition(theGesture, theTouchArea));
            if (theGesture == GestureType.Pinch)
            {
                PinchGestureAvailable = true;
            }
        }

        //static private bool isAccelerometerStarted = false;
        //public void AddAccelerometerInput(Direction direction, float tiltThreshold)
        //{
        //    if (!isAccelerometerStarted)
        //    {
        //        try
        //        {
        //            accelerometerSensor.Start();
        //            isAccelerometerStarted = true;
        //        }
        //        catch (AccelerometerFailedException e)
        //        {
        //            isAccelerometerStarted = false;
        //        }
        //    }

        //    accelerometerDefinedInputs.Add(direction, tiltThreshold);
        //}

        //public void RemoveAccelerometerInputs()
        //{
        //    if (isAccelerometerStarted)
        //    {
        //        try
        //        {
        //            accelerometerSensor.Stop();
        //            isAccelerometerStarted = false;
        //        }
        //        catch (AccelerometerFailedException e)
        //        {
        //            //The sensor couldn't be stopped..
        //        }
        //    }

        //    accelerometerDefinedInputs.Clear();
        //}

        static public bool IsConnected(PlayerIndex thePlayerIndex)
        {
            return CurrentGamePadState[thePlayerIndex].IsConnected;
        }

        public bool IsPressed(PlayerIndex thePlayerIndex)
        {
            return IsPressed(thePlayerIndex, null);
        }

        public bool IsPressed(PlayerIndex thePlayerIndex, Rectangle? theCurrentObjectLocation)
        {
            if (IsKeyboardInputPressed())
            {
                return true;
            }

            if (IsGamepadInputPressed(thePlayerIndex))
            {
                return true;
            }

            if (IsTouchTapInputPressed())
            {
                return true;
            }

            if (IsTouchSlideInputPressed())
            {
                return true;
            }

            if (IsGestureInputPressed(theCurrentObjectLocation))
            {
                return true;
            }

            if (IsAccelerometerInputPressed())
            {
                return true;
            }

            return false;
        }

        private bool IsKeyboardInputPressed()
        {
            foreach (Keys aKey in keyboardDefinedInputs.Keys)
            {
                if (keyboardDefinedInputs[aKey] && CurrentKeyboardState.IsKeyDown(aKey) && !PreviousKeyboardState.IsKeyDown(aKey))
                {
                    return true;
                }
                else if (!keyboardDefinedInputs[aKey] && CurrentKeyboardState.IsKeyDown(aKey))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsGamepadInputPressed(PlayerIndex thePlayerIndex)
        {
            foreach (Buttons aButton in gamepadDefinedInputs.Keys)
            {
                if (gamepadDefinedInputs[aButton] && CurrentGamePadState[thePlayerIndex].IsButtonDown(aButton) && !PreviousGamePadState[thePlayerIndex].IsButtonDown(aButton))
                {
                    return true;
                }
                else if (!gamepadDefinedInputs[aButton] && CurrentGamePadState[thePlayerIndex].IsButtonDown(aButton))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsTouchTapInputPressed()
        {
            foreach (Rectangle touchArea in touchTapDefinedInputs.Keys)
            {
                if (touchTapDefinedInputs[touchArea] && touchArea.Intersects(CurrentTouchRectangle) && PreviousTouchPosition() == null)
                {
                    return true;
                }
                else if (!touchTapDefinedInputs[touchArea] && touchArea.Intersects(CurrentTouchRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsTouchSlideInputPressed()
        {
            foreach (Direction slideDirection in touchSlideDefinedInputs.Keys)
            {
                if (CurrentTouchPosition() != null && PreviousTouchPosition() != null)
                {
                    switch (slideDirection)
                    {
                        case Direction.Up:
                            {
                                if (CurrentTouchPosition().Value.Y + touchSlideDefinedInputs[slideDirection] < PreviousTouchPosition().Value.Y)
                                {
                                    return true;
                                }
                                break;
                            }

                        case Direction.Down:
                            {
                                if (CurrentTouchPosition().Value.Y - touchSlideDefinedInputs[slideDirection] > PreviousTouchPosition().Value.Y)
                                {
                                    return true;
                                }
                                break;
                            }

                        case Direction.Left:
                            {
                                if (CurrentTouchPosition().Value.X + touchSlideDefinedInputs[slideDirection] < PreviousTouchPosition().Value.X)
                                {
                                    return true;
                                }
                                break;
                            }

                        case Direction.Right:
                            {
                                if (CurrentTouchPosition().Value.X - touchSlideDefinedInputs[slideDirection] > PreviousTouchPosition().Value.X)
                                {
                                    return true;
                                }
                                break;
                            }
                    }
                }
            }

            return false;
        }

        private bool IsGestureInputPressed(Rectangle? theNewDetectionLocation)
        {
            //Clear out the current gesture definition each time through so the
            //information stored there is always the most recently detected
            currentGestureDefinition = null;

            //If no gestures have been detected immediately just exit out
            if (detectedGestures.Count == 0)
            {
                return false;
            }

            //Check to see if any of the Gestures have been fired
            foreach (GestureDefinition userDefinedGesture in gestureDefinedInputs.Values)
            {
                foreach (GestureDefinition detectedGesture in detectedGestures)
                {
                    if (detectedGesture.Type == userDefinedGesture.Type)
                    {
                        //If a Rectangle area to check against has been passed in, then
                        //we'll use that one otherwise we'll use the one the Input was
                        //originally setup with
                        Rectangle areaToCheck = userDefinedGesture.CollisionArea;
                        if (theNewDetectionLocation != null)
                        {
                            areaToCheck = (Rectangle)theNewDetectionLocation;
                        }

                        //If the gesture detected was made in the area where users were
                        //interested in Input (they interesect), then a gesture input is
                        //considered detected.
                        if (detectedGesture.CollisionArea.Intersects(areaToCheck))
                        {
                            if (currentGestureDefinition == null)
                            {
                                currentGestureDefinition = new GestureDefinition(detectedGesture.Gesture);
                            }
                            else
                            {
                                //For many gestures like FreeDrag and Flick, they are registered many,
                                //many times in a single Update frame. Since we only store on variable
                                //for the currentGestureInformation we add on any additionaly gesture
                                //values so we have a cumalition of all the gesture information in currentGesture
                                currentGestureDefinition.Delta += detectedGesture.Delta;
                                currentGestureDefinition.Delta2 += detectedGesture.Delta2;
                                currentGestureDefinition.Position += detectedGesture.Position;
                                currentGestureDefinition.Position2 += detectedGesture.Position2;
                            }
                        }
                    }
                }
            }


            if (currentGestureDefinition != null)
            {
                return true;
            }

            return false;
        }

        private bool IsAccelerometerInputPressed()
        {
            foreach (KeyValuePair<Direction, float> input in accelerometerDefinedInputs)
            {
                switch (input.Key)
                {
                    case Direction.Up:
                        {
                            if (Math.Abs(currentAccelerometerReading.Y) > input.Value && currentAccelerometerReading.Y < 0)
                            {
                                return true;
                            }
                            break;
                        }

                    case Direction.Down:
                        {
                            if (Math.Abs(currentAccelerometerReading.Y) > input.Value && currentAccelerometerReading.Y > 0)
                            {
                                return true;
                            }
                            break;
                        }

                    case Direction.Left:
                        {
                            if (Math.Abs(currentAccelerometerReading.X) > input.Value && currentAccelerometerReading.X < 0)
                            {
                                return true;
                            }
                            break;
                        }

                    case Direction.Right:
                        {
                            if (Math.Abs(currentAccelerometerReading.X) > input.Value && currentAccelerometerReading.X > 0)
                            {
                                return true;
                            }
                            break;
                        }
                }
            }

            return false;
        }

        //These properties return the Position and Delta information about the current gesture that was detected. 
        //If no gesture was detected, then the values returned are set to safe default values (hopefully...)
        GestureDefinition currentGestureDefinition;
        public Vector2 CurrentGesturePosition()
        {
            if (currentGestureDefinition == null)
            {
                return Vector2.Zero;
            }
            return currentGestureDefinition.Position;
        }

        public Vector2 CurrentGesturePosition2()
        {
            if (currentGestureDefinition == null)
            {
                return Vector2.Zero;
            }
            return currentGestureDefinition.Position2;
        }

        public Vector2 CurrentGestureDelta()
        {
            if (currentGestureDefinition == null)
            {
                return Vector2.Zero;
            }
            return currentGestureDefinition.Delta;
        }

        public Vector2 CurrentGestureDelta2()
        {
            if (currentGestureDefinition == null)
            {
                return Vector2.Zero;
            }
            return currentGestureDefinition.Delta2;
        }

        //Get the touch point for the current location. This doesn't use any of the 
        //Gesture information, but the actual touch point on the screen
        public Vector2? CurrentTouchPosition()
        {
            foreach (TouchLocation location in CurrentTouchLocationState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:
                        return location.Position;

                    case TouchLocationState.Moved:
                        return location.Position;
                }
            }

            return null;
        }

        private Vector2? PreviousTouchPosition()
        {
            foreach (TouchLocation location in PreviousTouchLocationState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:
                        return location.Position;

                    case TouchLocationState.Moved:
                        return location.Position;
                }
            }

            return null;
        }

        private Rectangle CurrentTouchRectangle
        {
            get
            {
                Vector2? touchPosition = CurrentTouchPosition();
                if (touchPosition == null)
                {
                    return Rectangle.Empty;
                }
                return new Rectangle((int)touchPosition.Value.X - 5, (int)touchPosition.Value.Y - 5, 10, 10);
            }
        }

        public Vector3 CurrentAccelerometerReading
        {
            get
            {
                return currentAccelerometerReading;
            }
        }
    }
}
