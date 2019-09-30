// Joystick.js - Converted to C# - JoyStick.cs by Bryan Leister
// with some additional code for using Editor mouseinput based on Criistii
// Penelope iPhone Tutorial
//
// Joystick creates a movable joystick (via GUITexture) that 
// handles touch input, taps, and phases. Dead zones can control
// where the joystick input gets picked up and can be normalized.
//
// Optionally, you can enable the touchPad property from the editor
// to treat this Joystick as a TouchPad. A TouchPad allows the finger
// to touch down at any point and it tracks the movement relatively 
// without moving the graphic
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class ButtonBoundary
{
    public Vector2 min = Vector2.zero;
    public Vector2 max = Vector2.zero;
}

public class JoyStick : MonoBehaviour
{
    static private JoyStick[] joysticks;																	//a static collection of all joysticks
    static private bool enumeratedJoySticks = false;
    static private float tapTimeDelta = 0.3f;												//Time allowed between taps

    #region Public variables
    public bool touchPad;																	//is this a TouchPad?
    public Rect touchZone;
    public Vector2 deadZone = Vector2.zero;										//Control when position is output
    public bool normalize;																	//Normalize output after the deadzone?
    public Vector2 position;																	//[ -1, 1 ] in x,y
    public int tapCount;																	//current tap count					
    #endregion

    #region Private variables
    private int lastFingerId = -1;												//Finger last used for this joystick
    private float tapTimeWindow;																//How much time is there left for a tap to occur?
    private Vector2 fingerDownPos;
    //	private	float				fingerDownTime;
    //	private float				firstDeltaTime			= .5F;

    private GUITexture gui;																		//Joystick graphic
    private Rect defaultRect;																//Default position/ extents of the joystick graphic
    private ButtonBoundary guiButtonBoundary = new ButtonBoundary();							//Boundary for joystick graphic
    private Vector2 guiTouchOffset;																//Offset to apply to touch input
    private Vector2 guiCenter;																	//center of joystick
    #endregion

    public void Start()
    {
        gui = (GUITexture)GetComponent(typeof(GUITexture));												// Cache this component at startup instead of looking up every frame

        if (gui.pixelInset.x > Screen.width / 2)															//If this is a left button, it should be set in the inspector to a high number (800+),
        {																									// if so, we will check that and position it according to display resolution of the device. 
            //For convenience, I've positioned it offset by the same # I have used for the Y offset
            gui.pixelInset = new Rect((Screen.width - gui.pixelInset.width) - gui.pixelInset.y, 			//This may not work for all interfaces...
                                      gui.pixelInset.y, gui.pixelInset.width, gui.pixelInset.height);
        }

        defaultRect = gui.pixelInset;																		// Store the default rect for the gui, so we can snap back to it

        defaultRect.x += transform.position.x * Screen.width;												// + gui.pixelInset.x; // -  Screen.width * 0.5;
        defaultRect.y += transform.position.y * Screen.height;												// - Screen.height * 0.5;

        transform.position = new Vector3(0, 0, transform.position.z);

        if (touchPad)
        {
            if (gui.texture)																				// If a texture has been assigned, then use the rect ferom the gui as our touchZone
                touchZone = defaultRect;
        }
        else
        {
            guiTouchOffset.x = defaultRect.width * 0.5f;													//This is an offset for touch input to match with the top left corner of GUI
            guiTouchOffset.y = defaultRect.height * 0.5f;

            guiButtonBoundary.min.x = defaultRect.x - guiTouchOffset.x;											//Let's build the GUI boundary so we can clamp joystick movement
            guiButtonBoundary.max.x = defaultRect.x + guiTouchOffset.x;
            guiButtonBoundary.min.y = defaultRect.y - guiTouchOffset.y;
            guiButtonBoundary.max.y = defaultRect.y + guiTouchOffset.y;

            guiCenter.x = defaultRect.x + guiTouchOffset.x;													//Cache the center of the GUI, since it doesn't change
            guiCenter.y = defaultRect.y + guiTouchOffset.y;
        }
    }

    public void Disable()
    {
        gameObject.active = false;
        enumeratedJoySticks = false;
    }

    public void ResetJoystick()
    {
        gui.pixelInset = defaultRect;   																	//Release the finger control and set the joystick back to the default position
        lastFingerId = -1;
        position = Vector2.zero;
        fingerDownPos = Vector2.zero;

        if (touchPad)
            gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, .25F);								//make the touchpad semi-transparent on Reset

    }

    public bool IsFingerDown()																				//is this joystick being pressed? Accessible from other scripts...
    {
        return (lastFingerId != -1);
    }

    public void LatchedFinger(int fingerId)
    {
        if (lastFingerId == fingerId)																		//if another joystick has latched this finger, then we must release it
        {
            ResetJoystick();
        }
    }


    public void Update()
    {
        if (!enumeratedJoySticks)
        {
            joysticks = (JoyStick[])FindObjectsOfType(typeof(JoyStick));									// Collect all joysticks in the game, so we can relay finger latching messages
            enumeratedJoySticks = true;
        }

        int count = 0;
        //int count = Input.touchCount;

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER											//setting this up for Editor use

        if (Input.GetMouseButton(0))
        {
            count = 1;
            //Debug.Log("Mouse button: " + count);
        }
#else
			count = Input.touchCount;
#endif


        if (tapTimeWindow > 0)																				// Adjust the tap time window while it still available
        {
            tapTimeWindow -= Time.deltaTime;
        }
        else
        {
            tapCount = 0;
        }

        #region Begin Touch detecting
        if (count == 0)																					// no fingers are touching, so we reset the position
        {
            ResetJoystick();
        }
        else
        {
            int fingerID;

            for (int i = 0; i < count; i++)
            {
                Touch touch;


#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER						//Switch to use Editor for testing mouse input

                Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 guiTouchPos = mousePos - guiTouchOffset;
                fingerID = 1;
                Vector2 touchPosition = Input.mousePosition;
#else				
					touch = Input.GetTouch(i);
					Vector2 touchPosition = touch.position;					
					Vector2 guiTouchPos = touchPosition - guiTouchOffset;
					fingerID = touch.fingerId;				
#endif

                bool shouldLatchFinger = false;
                if (touchPad)
                {
                    if (touchZone.Contains(touchPosition))
                    {
                        shouldLatchFinger = true;
                    }
                }
                else if (gui.HitTest(touchPosition))
                {
                    shouldLatchFinger = true;
                }

                if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != fingerID)) 		// Latch the finger if this is a new touch
                {
                    if (touchPad)
                    {
                        gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, 1.0F);					//Set the touchpad to fully opaque
                        lastFingerId = fingerID;
                        fingerDownPos = touchPosition;														//Record the down touch position
                        //fingerDownTime = Time.time;															//Record the time when it was touched down
                    }

                    lastFingerId = fingerID;

                    if (tapTimeWindow > 0)																// Accumulate taps if it is within the time window
                        tapCount++;
                    else
                    {
                        tapCount = 1;
                        tapTimeWindow = tapTimeDelta;
                    }

                    foreach (JoyStick j in joysticks)														// Tell other joysticks we've latched this finger
                    {
                        if (j != this)
                            j.LatchedFinger(fingerID);
                    }
                }

                if (lastFingerId == fingerID)
                {

                    if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) //If we are are on the actual device
                    {
                        touch = Input.GetTouch(i);
                        if (touch.tapCount > tapCount)														// Override the tap count with what the iPhone SDK reports if it is greater
                        {																						// This is a workaround, since the iPhone SDK does not currently track taps
                            tapCount = touch.tapCount;														// for multiple touches
                        }
                    }
                    if (touchPad)																				// For a touchpad, let's just set the position directly based on distance from initial touchdown
                    {
                        position.x = Mathf.Clamp((touchPosition.x - fingerDownPos.x) / (touchZone.width / 2), -1, 1);
                        position.y = Mathf.Clamp((touchPosition.y - fingerDownPos.y) / (touchZone.height / 2), -1, 1);
                    }
                    else
                    {
                        Rect r = gui.pixelInset;
                        r.x = Mathf.Clamp(guiTouchPos.x, guiButtonBoundary.min.x, guiButtonBoundary.max.x);	// Change the location of the joystick graphic to match where the touch is
                        r.y = Mathf.Clamp(guiTouchPos.y, guiButtonBoundary.min.y, guiButtonBoundary.max.y);
                        gui.pixelInset = r;
                    }
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER								//Set up to use the Mouse while in the Editor

                    if (!Input.GetMouseButton(0))
                    {
                        ResetJoystick();
                    }
#else
					if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
					{
						ResetJoystick();										
					}
#endif
                }
            }
        }

        #endregion

        if (!touchPad)
        {
            position.x = (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x;		// Get a value between -1 and 1 based on the joystick graphic location
            position.y = (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y;
        }

        float absoluteX = Mathf.Abs(position.x);														// Adjust for dead zone	
        float absoluteY = Mathf.Abs(position.y);

        if (absoluteX < deadZone.x)																	// Report the joystick as being at the center if it is within the dead zone
        {
            position.x = 0;
        }
        else if (normalize)																			// Rescale the output after taking the dead zone into account
        {
            position.x = Mathf.Sign(position.x) * (absoluteX - deadZone.x) / (1 - deadZone.x);
        }

        if (absoluteY < deadZone.y)																	// Report the joystick as being at the center if it is within the dead zone
        {
            position.y = 0;
        }
        else if (normalize)																			// Rescale the output after taking the dead zone into account
        {
            position.y = Mathf.Sign(position.y) * (absoluteY - deadZone.y) / (1 - deadZone.y);
        }
    }
}
