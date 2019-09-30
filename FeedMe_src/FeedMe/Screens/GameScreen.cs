using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.IO; 
//using System.IO.IsolatedStorage;
//using System.Reflection;    //used for recovering embedded resources; in this case, the levels


using FeedMe.UI;
using FeedMe.Inputs;
using FeedMe.Texts;
using FeedMe.Objects;


namespace FeedMe.Screens
{
    class GameScreen : Screen
    {
        Texture2D playerImg;
        Texture2D groundImg;
        Texture2D platformImg;
        Texture2D foodImg;
        Texture2D backgroundImg;
        Texture2D debugImg;         //background for displaying debug info
        Texture2D capacityWarningImg;
       

        FoodMeter foodMeter;       //player's food bar
        Texture2D emptyBarTexture;
        Texture2D filledBarTexture;
        Texture2D capacityTexture;  //the size of this can change

        //fonts
        SpriteFont debugFont;
        SpriteFont numberFont;
        SpriteFont screenFont;
        SpriteFont itemFont;
        SpriteFont timerFont;

        //touch input
        GameInput input = new GameInput();
        const string screenTapped = "ScreenTap";
        const string jumpButtonPressed = "Jump";
        Rectangle screenArea;
        const int GROUND_HEIGHT = 330;  //player's Y pos cannot go past this

        //physics & movement
        const float GRAVITY = 1.5f;     //causes objects to fall to the ground.
        const float FRICTION = 0.8f;    //used to stop player gradually when approaching touch point.
        const float PLAYER_MOVE_SPEED = 4;
        const float PLAYER_TURBO_SPEED = 9;     //if the distance between creature and touch point is great, then it runs faster
        const float MAX_DISTANCE = 10000;
        const float MIN_DISTANCE = 10;
        const float FOOD_DROP_SPEED = 3;
        float distance;             //distance between player and touch point 
        float friction;             //used to slow down player movement gradually
        float vx;
        float vy;                   //player X and Y velocity
        float foodVy;               //food Y velocity
        const float JUMP_VEL = 20;         //player jump height
        bool playerIsMoving;    //if true, player is moving
        bool playerIsJumping;
        Vector2 tapLocation;            //the creatures moves to this point    

        Level level;

        //UI & game states
        short levelNum;         //current level.
        string lastItemPickup;  //name of last item picked up
        bool gameOver;          //if true, food meter is empty or past capacity
        Texture2D jumpButtonImg;
        Rectangle jumpButtonArea;
        Texture2D dangerImg;    //displayed when low on meter
        Texture2D itemWindowImg;
        Texture2D lastItemImg;  //last item picked up
        Timer deathTimer;       //countdown timer before player dies from overeating
        Timer gameOverTimer;    //when this is 0, go back to title screen
        bool timerStarted;
        Texture2D gameOverImg;
        const int DOOM_TIME = 10;   //time in seconds before death

        //game objects
        Player player;
        Food foodItem;
        FoodManager foodManager;
        List<Food> foodList = new List<Food>(); //tracks on-screen food items
        List<Food> trashCan = new List<Food>(); //removes destroyed food objects
        float dropTimer;                //controls how often food gets dropped.
        const float MAX_TIMER = 3;      //determines when food is placed on screen.
        const short DROP_LIMIT = 20;    //max amount of food that can be on screen at once.
        Random foodPos = new Random();
        Vector2 foodValuePos;           //food value pops up over the player's head when food is picked up
        bool foodValueDisplayed;
        int foodValue;
        string effectMsg;               //displays special effect text

        Vector2 levelUpMsgPos;      //alerts player when level has passed.
        string levelUpMsg;
        bool levelUpDisplayed;

        //sounds
        Song gameMusic;
        bool musicIsPlaying;

         public GameScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Gameplay)
        {
            dropTimer = 0;
            lastItemPickup = "";
            levelUpMsg = "";
            levelUpDisplayed = false;
            vx = 0;
            vy = 0;
            foodVy = 0;
            friction = 1;
            distance = 0;
            levelNum = 1;
            playerIsMoving = false;
            playerIsJumping = false;
            musicIsPlaying = false;
            foodValueDisplayed = false;
            foodManager = new FoodManager(content);
            deathTimer = new Timer();
            deathTimer.SetTimer(0, 0, DOOM_TIME);//5 seconds
            timerStarted = false;
            gameOver = false;
            gameOverTimer = new Timer();
            gameOverTimer.SetTimer(0, 0, 5);
           
            
        }

         protected override void SetupInputs()
         {
             input.AddTouchGestureInput(screenTapped, GestureType.Tap, screenArea);
             input.AddTouchGestureInput(jumpButtonPressed, GestureType.Tap, jumpButtonArea);
         }
         protected override void LoadScreenContent(ContentManager content)
         {
             //fonts
             debugFont = content.Load<SpriteFont>(@"Fonts/debugFont");
             numberFont = content.Load<SpriteFont>(@"Fonts/numberFont");
             screenFont = content.Load<SpriteFont>(@"Fonts/screenFont");
             itemFont = content.Load<SpriteFont>(@"Fonts/itemFont");
             timerFont = content.Load<SpriteFont>(@"Fonts/timerFont");

             //sprites
             playerImg = content.Load<Texture2D>(@"Images/player");
             jumpButtonImg = content.Load<Texture2D>(@"Images/jumpbutton");

             //food meter textures
             emptyBarTexture = content.Load<Texture2D>(@"Images/emptymeter");
             filledBarTexture = content.Load<Texture2D>(@"Images/foodmeter");
             capacityTexture = content.Load<Texture2D>(@"Images/capacitymarker");

             //background
             backgroundImg = content.Load<Texture2D>(@"Images/sky");
             debugImg = content.Load<Texture2D>(@"Images/debug_bg");

             //field
             groundImg = content.Load<Texture2D>(@"Images/ground");

             //touch gesture setup
             screenArea = new Rectangle(0, 0, Screen.ScreenWidth, Screen.ScreenHeight - 100); //bottom area can't be tapped
             jumpButtonArea = new Rectangle(650, 410, jumpButtonImg.Width, jumpButtonImg.Height);

             //misc UI
             dangerImg = content.Load<Texture2D>(@"Images/danger");
             itemWindowImg = content.Load<Texture2D>(@"Images/itemwindow");
             lastItemImg = content.Load<Texture2D>(@"Images/none");
             capacityWarningImg = content.Load<Texture2D>(@"Images/capacitywarning");
             gameOverImg = content.Load<Texture2D>(@"Images/gameover");

             //music
             gameMusic = content.Load<Song>(@"Sounds/music");
              
             //object setup
             level = new Level(playerImg, backgroundImg, groundImg, platformImg);
             foodMeter = new FoodMeter(emptyBarTexture, filledBarTexture, capacityTexture, 120, 420);
            // foodMeter.ChangeMarkerPosition(-200);
             player = new Player(playerImg, new Vector2(Screen.ScreenWidth / 2, GROUND_HEIGHT));
            
         }

         protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
         {
             level.Draw(batch);
             batch.Draw(backgroundImg, Vector2.Zero, Color.White);
             //DisplayDebug(batch);
             batch.Draw(playerImg, player.Position(), Color.White);

             //enable this line for hitbox checking
             //batch.Draw(debugImg, new Rectangle((int)player.Position().X, (int)player.Position().Y, player.Hitbox().Width, player.Hitbox().Height), Color.White);

             //draw timer
             if (timerStarted)
             {
                 batch.Draw(capacityWarningImg, new Vector2(Screen.ScreenWidth / 2 - 90, 20), Color.White);
                 batch.DrawString(timerFont, deathTimer.Seconds().ToString() + deathTimer.Milliseconds().ToString(),
                     new Vector2(Screen.ScreenWidth / 2 - 50, 40), Color.Red);
              }
             //draw food
             foreach (Food f in foodList)
             {
                 f.Draw(batch);
                 //enable for hitbox checking
                 // batch.Draw(debugImg, new Rectangle((int)f.Position().X, (int)f.Position().Y, f.Hitbox().Width, f.Hitbox().Height), Color.White);
                 
             }

             //item window - shows last item picked up.
             batch.Draw(itemWindowImg, new Vector2(650, 30), Color.White);
             batch.Draw(lastItemImg, new Vector2(665, 60), Color.White);
             batch.DrawString(itemFont, lastItemPickup, new Vector2(640, 110), Color.Salmon);

             if (levelUpDisplayed)
             {
                 batch.DrawString(numberFont, levelUpMsg, levelUpMsgPos, Color.Salmon);
             }

             if (foodValueDisplayed)
             {
                 if (foodValue >= 0)
                     batch.DrawString(numberFont, "+" + foodValue.ToString(), foodValuePos, Color.Blue);
                 else
                     //it's going to be displayed as a negative value, so display as is
                     batch.DrawString(numberFont, foodValue.ToString(), foodValuePos, Color.Red);

                 //display any special effects
                 batch.DrawString(numberFont, effectMsg, new Vector2(foodValuePos.X - 50, foodValuePos.Y + 20), Color.Orange);
             }

             batch.Draw(groundImg, new Vector2(0, 390), Color.White);
             batch.DrawString(screenFont, "Level: " + levelNum, new Vector2(20, 10), Color.Yellow);
             
            
             foodMeter.Draw(batch);
             if (foodMeter.Width() < foodMeter.MaxWidth() / 10) //Less than 10%
                batch.Draw(dangerImg, new Vector2(120, 395), Color.White);
             batch.Draw(jumpButtonImg, jumpButtonArea, Color.White);

             //game over
             if (gameOver)
             {
                 batch.Draw(gameOverImg, new Vector2(Screen.ScreenWidth / 2 - 50, Screen.ScreenHeight / 2 - 50), Color.White);
             }
         }

         protected override void UpdateScreen(GameTime gameTime, DisplayOrientation screenOrientation)
         {
             //the food meter is constantly decreasing, even when not moving. It decreases faster when moving.
             //level.Play();

             if (!gameOver)
             {
                 //death timer
                 if (timerStarted)
                     deathTimer.Tick(true);
                 else
                     deathTimer.SetTimer(0, 0, DOOM_TIME);

                 //start playing music
                 if (MediaPlayer.GameHasControl && !musicIsPlaying)
                 {
                     MediaPlayer.Play(gameMusic);
                     MediaPlayer.IsRepeating = true;
                     musicIsPlaying = true;
                 }


                 //check for player effects
                 player.ReduceTimer();
                 if (player.EffectEnded)
                 {
                     player.SetMod(0);
                 }

                 //display food value number
                 if (foodValueDisplayed)
                 {
                     foodValuePos.Y -= 1;
                     float distance = (foodValuePos.Y - player.Position().Y) * (foodValuePos.Y - player.Position().Y);
                     if (distance > 3000)
                         foodValueDisplayed = false;
                 }

                 if (levelUpDisplayed)
                 {
                     levelUpMsgPos.Y -= 1;
                     float dist = (levelUpMsgPos.Y - player.Position().Y) * (levelUpMsgPos.Y - player.Position().Y);
                     if (dist > 6000)
                         levelUpDisplayed = false;
                 }



                 if (foodList.Count != DROP_LIMIT)
                 {
                     dropTimer += 0.1f;
                     int foodX = foodPos.Next(Screen.ScreenWidth - 30);
                     if (dropTimer > MAX_TIMER)
                     {
                         dropTimer = 0;
                         foodItem = foodManager.GenerateFood(new Vector2(foodX, -50));
                         foodList.Add(foodItem);
                     }
                 }

                 if (BackButtonPressed())
                 {
                     //reset game
                     Reset();

                     //stop music
                     if (MediaPlayer.GameHasControl)
                         MediaPlayer.Stop();
                 }

                 //jump button action
                 if (input.IsPressed(jumpButtonPressed) && !playerIsJumping)
                 {
                     playerIsJumping = true;
                     vy -= JUMP_VEL;
                 }

                 //foodItem.ActivateSpecial(player);
                 if (input.IsPressed(screenTapped))
                 {
                     //set the tapped location. The tapped location uses a circle to indicate where the creature is moving.
                     tapLocation.X = input.CurrentGesturePosition(screenTapped).X;
                     tapLocation.Y = input.CurrentGesturePosition(screenTapped).Y;
                     friction = 1;
                     playerIsMoving = true;
                 }



                 //update player position
                 float playerX = player.Position().X;

                 if (playerIsMoving)
                 {
                     //increase food meter burn rate
                     foodMeter.SetCurrentRate(1.0f);
                     friction = 1;

                     float distance = (tapLocation.X - playerX) * (tapLocation.X - playerX);  //(x2 - x1)^2
                     //move player to touch location.
                     if (playerX <= tapLocation.X)
                     {
                         if (distance > MAX_DISTANCE)
                         {
                             vx = PLAYER_TURBO_SPEED + player.MoveMod();
                             foodMeter.SetCurrentRate(1.5f);
                         }
                         else
                             vx = PLAYER_MOVE_SPEED + player.MoveMod();
                     }
                     else if (playerX > tapLocation.X)
                     {
                         if (distance > MAX_DISTANCE)
                         {
                             vx = -PLAYER_TURBO_SPEED - player.MoveMod();
                             foodMeter.SetCurrentRate(1.5f);
                         }
                         else
                             vx = -PLAYER_MOVE_SPEED - player.MoveMod();
                     }

                     //TODO: Figure out how to prevent player from moving in opposite direction when tapping
                     //on same point as player's current position.

                     //slow down player when minimum distance is reached
                     playerIsMoving = (distance <= MIN_DISTANCE) ? false : true;

                 }
                 else
                 {
                     foodMeter.SetCurrentRate(0);
                     friction = FRICTION;
                 }


                 //apply gravity to objects
                 vx *= friction;
                 foodVy = FOOD_DROP_SPEED + GRAVITY;

                 //only apply gravity when airborne
                 if (playerIsJumping)
                     vy += GRAVITY;

                 //drop food
                 foreach (Food f in foodList)
                 {
                     float y = f.Position().Y + foodVy;
                     if (y > GROUND_HEIGHT + f.FoodImage().Height)
                     {
                         y = GROUND_HEIGHT + f.FoodImage().Height;
                         f.Countdown();  //decays only when on the ground
                     }



                     f.SetPosition(new Vector2(f.Position().X, y));

                     if (f.IsDecayed())  //remove item if decayed
                     {
                         trashCan.Add(f);
                     }

                     //collision checking
                     else if (player.Collides(f))
                     {
                         //play sound
                         soundEffects.PlaySound(@"Sounds/foodpickup");

                         //destroy food and add its value to the meter
                         foodMeter.IncreaseMeter(f.FoodValue());

                         foodValueDisplayed = true;
                         foodValuePos = new Vector2(player.Position().X, player.Position().Y - 25);
                         foodValue = f.FoodValue();
                         effectMsg = f.EffectMessage();
                         f.ActivateSpecial(player);  //use any special abilities on player.

                         lastItemPickup = f.FoodName() + "  " + f.FoodValue().ToString() + "pts.";
                         lastItemImg = f.FoodImage();

                         trashCan.Add(f);
                         //Debug.WriteLine("Collided with food " + foodList.IndexOf(f));
                     }
                 }
                 playerX += vx;

                 //boundary check
                 if (playerX + playerImg.Width > Screen.ScreenWidth)
                     playerX = Screen.ScreenWidth - playerImg.Width;
                 float playerY = player.Position().Y + vy;
                 //check Y
                 if (playerY > GROUND_HEIGHT)
                 {
                     playerY = GROUND_HEIGHT;
                     vy = 0;
                     playerIsJumping = false;
                 }

                 player.SetPosition(new Vector2(playerX, playerY));

                 foodMeter.Update();

                 //check lose conditions
                 if (foodMeter.CapacityExceeded() && !timerStarted)
                 {
                     //start timer
                     timerStarted = true;
                 }

                 if (!foodMeter.CapacityExceeded() && timerStarted)
                 {
                     //this should occur if at some point the player is not over capacity but didn't complete the level
                     //while the death timer was counting down.
                     timerStarted = false;
                     deathTimer.SetTimer(0, 0, DOOM_TIME);
                 }

                 if (foodMeter.IsEmpty() || deathTimer.TimeUp()) //don't let this happen!
                 {
                     deathTimer.SetMilliseconds(0); //this is here so that the player isn't fooled into thinking they had more time remaining.
                     gameOver = true;
                 }

                 //check win condition
                 else if (foodMeter.LevelWon())
                 {
                     levelUpMsg = "Capacity Clear! Level +1";
                     //raise level and change the capacity marker.
                     levelNum++;
                     timerStarted = false;
                     deathTimer.SetTimer(0, 0, DOOM_TIME);
                     foodMeter.ResetMeter();
                     if (foodMeter.MarkerAtMinCapacity())
                     {
                         //every time the level is beaten with the marker at min capacity, its position is reduced and
                         //the width is reset.  This increases the challenge over time, because the player is
                         //more likely to go over capacity and not be able to recover without eating the right food.
                         foodMeter.ResetCapacity();
                         levelUpMsg += "\nMarker Position Change";
                         foodMeter.ChangeMarkerPosition(-20);
                     }
                     else
                         foodMeter.DecreaseCapacity(10);

                     levelUpDisplayed = true;
                     levelUpMsgPos = new Vector2(200, player.Position().Y - 20);
                 }

                 //garbage cleanup
                 foreach (Food f in trashCan)
                 {
                     foodList.Remove(f);
                 }

                 base.UpdateScreen(gameTime, screenOrientation);
             }
             else
             {
                 Die();
                 //if we're here, then game is over.  Set a timer before being kicked back to title screen.
                 gameOverTimer.Tick(true);
                 if (gameOverTimer.TimeUp())
                 {
                     if (MediaPlayer.GameHasControl)
                         MediaPlayer.Stop();
                     Reset();
                     changeScreenDelegate(ScreenState.Title);
                 }
             }
         }


         void DisplayDebug(SpriteBatch batch)
         {
             batch.Draw(debugImg, new Rectangle(400, 100, 400, 220), Color.White);
             batch.DrawString(debugFont, "Touch Point: " + tapLocation, new Vector2(450, 100), Color.White);
             batch.DrawString(debugFont, "Player Pt: " + player.Position(), new Vector2(450, 120), Color.White);
             batch.DrawString(debugFont, "Player Moving?: " + playerIsMoving, new Vector2(450, 140), Color.White);
             batch.DrawString(debugFont, "VelX: " + vx, new Vector2(450, 160), Color.White);
             batch.DrawString(debugFont, "VelY: " + vy, new Vector2(450, 180), Color.White);
             batch.DrawString(debugFont, "Friction: " + friction, new Vector2(450, 200), Color.White);
             batch.DrawString(debugFont, "Distance: " + distance, new Vector2(450, 220), Color.White);
             batch.DrawString(debugFont, "Food Count: " + foodList.Count, new Vector2(450, 240), Color.White);
             batch.DrawString(debugFont, "Drop Timer: " + dropTimer + "/" + MAX_TIMER, new Vector2(450, 260), Color.White);
             batch.DrawString(debugFont, "Jump: " + playerIsJumping, new Vector2(450, 280), Color.White);
         }

        //resets game state to default
         private void Reset()
         {
             dropTimer = 0;
             lastItemPickup = "";
             levelUpMsg = "";
             lastItemImg = content.Load<Texture2D>(@"Images/none");
             vx = 0;
             vy = 0;
             foodVy = 0;
             friction = 1;
             distance = 0;
             levelNum = 1;
             playerIsMoving = false;
             playerIsJumping = false;
             foodValueDisplayed = false;
             levelUpDisplayed = false;
             timerStarted = false;
             musicIsPlaying = false;
             foodManager = new FoodManager(content);
             foodMeter = new FoodMeter(emptyBarTexture, filledBarTexture, capacityTexture, 120, 420);
             player = new Player(playerImg, new Vector2(Screen.ScreenWidth / 2, GROUND_HEIGHT));
             //foodList.Clear();
             //trashCan.Clear();
             foodList = new List<Food>();
            trashCan = new List<Food>();
            gameOver = false;
            gameOverTimer.SetTimer(0, 0, 5);
         }

         private void Die()
         {
             //creature falls through the floor when it dies from hunger. Otherwise, it explodes.
             //keep increasing y until off screen
             float y = player.Position().Y + 2;
             player.SetPosition(new Vector2(player.Position().X, y));
             //if (player.Position().Y > Screen.ScreenHeight)
             //    //game over
             //    gameOver = true;
             
         }
    }
}
