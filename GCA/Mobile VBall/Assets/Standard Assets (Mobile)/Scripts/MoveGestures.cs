using UnityEngine;
using System.Collections;

/* MoveGestures.cs by Haskell (Mike) Murray
 * This class deals with all of the gestures in the game. The gestures that are used
 * are slide, tap, and hold. 
 * Slide - used for moving the player and charging spikes and blocks
 * Tap - used for hitting the ball
 * Hold - used for jumping
 * 
 * For now, I will use Mouse input until I figure out the touch code.
 * 
 * UPDATE: We have decided to change the control scheme so that there are two onscreen buttons, each of them controlling one player.
 * pressing the edges of the button moves the player; tapping the button causes them to hit the ball, while double tapping causes them to jump.
 */
public class MoveGestures : MonoBehaviour 
{

    float speed = 0.5f;    //controls the player's movement speed
    Touch touch;
    int swipeLength;        //checks the length of the swipe.  The player will travel that amount.
    Vector3 move;
    Vector3 destination;    //the destination the player travels to
    Vector3 touchStart;     //beginning of movement line
    Vector3 touchEnd;       //end of movement line
    int touchCount;         //used to determine how many times screen was touched.
    Ray ray;
    RaycastHit hit;         //used for finding 3D coordinates
    const float GRAVITY = -6.8f;
    int frameCount;         //used for controlling input
    const float JUMPVELOCITY = 6.0f;
    bool isAirborne;        //disables the jump while player is in midair
    int chargeMeter;
    const int MAX_METER = 10;
    bool playerSelected;     //checks if player was selected before movement can occur
    bool positionSelected;  //checks if a mouse position was clicked.  Is true ONLY when playerSelected is true.


	// Use this for initialization
	void Start () 
    {
        swipeLength = 0;
        move = new Vector3(0.0f, 0, 0);
        touchCount = 0;
        destination = transform.position;
        touchStart = new Vector3(0.0f, 0, 0);
        hit = new RaycastHit();
        touchEnd = new Vector3(transform.position.x, 1, transform.position.z);
        frameCount = 0;
        isAirborne = false;
        chargeMeter = 0;
        touch = Input.GetTouch(0);
        playerSelected = false;
        positionSelected = false;
	}
	
    
	// Update is called once per frame
	void Update ()
    {
        //keep the player grounded
        transform.Translate(new Vector3(0, 1, 0) * GRAVITY * Time.deltaTime);
        if (transform.position.y < 1)
        {
            //player is on the ground
            Vector3 pos = new Vector3(transform.position.x, 1, transform.position.z);
            transform.position = pos;
            isAirborne = false;
        }
        else
        {
            //player is in the air
            isAirborne = true;
        }

        //Debug.Log("Touch Count: " + Input.touchCount);
        /* Drag in a direction */
        
        //if (Input.touchCount > 0 && touch.phase == TouchPhase.Began)
        if (Input.GetMouseButtonDown(0))
        {

            //check if player was selected previously. If true, then that means we can move the player
            if (playerSelected)
            {
                positionSelected = true;
            }
            else
                positionSelected = false;
            
            //convert screen coordinates to world space coordinates
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            //Draws a line to show where the player will move
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                touchEnd = new Vector3(hit.point.x, 1, hit.point.z);
            }

            //check if user clicked on the player
            if (hit.collider.tag == "Player")
            {
                Debug.Log("Player selected");
                playerSelected = true;
            }
            else
            {
                Debug.Log("Player not selected");
                playerSelected = false;
            }

            
        
        }

        //-----------------Jump code: press and hold to jump--------------
        if (Input.GetMouseButton(0) && playerSelected)
        {
            //User must press and hold for a few frames before jump is activated.  This will allow the player to move without jumping
            frameCount++;

            //player jumps in the air after 12 frames
            if (frameCount > 8)
            {
                if (!isAirborne)
                {
                    //translate the player's position until it reaches the max height.
                    
                    transform.Translate(new Vector3(0, 1, 0) * JUMPVELOCITY);
                    //rigidbody.AddRelativeForce(transform.up * JUMPVELOCITY, ForceMode.Impulse);
   
                    frameCount = 0;
                    isAirborne = true;
                }

                //if (transform.position.y > 3)
                //{
                //    Vector3 pos = new Vector3(transform.position.x, 3, transform.position.z);
                //    transform.position = pos;
                //}
            }
        }
        else
        {
            //any time the button is not held down, frame count should be reset.
            frameCount = 0;
        }

        //--------------Code for charging spikes/blocks----------------

        /*Charging can only occur while the player is in midair.  The user must slide back and forth to charge. */
        if (isAirborne)
        {
            //prevent player from moving in midair
            transform.Translate(new Vector3(0, 0, 0) * speed);
            if (Input.GetMouseButtonDown(0))
            {
                //check how many times the button is pressed while airborne.
                chargeMeter++;
                Debug.Log("Charging " + chargeMeter + " of " + MAX_METER);
                if (chargeMeter > MAX_METER)
                {
                    chargeMeter = MAX_METER;
                }
            }
        }
        else
        {
            //reset the charge
            chargeMeter = 0;
        }


        //player will move to the position tapped and remain there until another position is tapped
        if (positionSelected)
        {
            if (touchEnd.x < transform.position.x)
                transform.Translate(new Vector3(-1, 0, 0) * speed);
            if (touchEnd.x > transform.position.x)
                transform.Translate(new Vector3(1, 0, 0) * speed);
            if (touchEnd.z < transform.position.z)
                transform.Translate(new Vector3(0, 0, -1) * speed);
            if (touchEnd.z > transform.position.z)
                transform.Translate(new Vector3(0, 0, 1) * speed);

            //once the player stops moving, player is deselected.
            if (touchEnd == transform.position)
            {
                playerSelected = false;
            }
        }

        
	}
	
}
