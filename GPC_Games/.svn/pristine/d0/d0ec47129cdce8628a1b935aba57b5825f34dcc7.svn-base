using UnityEngine;
using System.Collections;

/* This class is the parent class of all weapons in the game.  It contains everything that all weapons have in common. */
public class Weapon : MonoBehaviour {

    public string name;        //name of weapon
    public string description;  //details
    //public int ammoCapacity;   //amount of rounds a weapon carries. An infinite amount is -1.
    //public int ammoCount;       //current ammo
    public int power;          //amount of damage dealt by weapon.
    public int level;          //level of the weapon.  Max level is 3.
    public static int totalMaxAmmo = 50;       //total amount of ammo across all weapons.  The lower this number is, the longer it will take to reload.
    public static int currentTotalAmmo = 50;    //current amount of ammo across all weapons.
    const int MAX_LEVEL = 3;
    public float reloadTimeRemaining = 0;
    float totalReloadTime;

    //reload meter
    float reloadBarWidth;
    Rect emptyBar; 
    Rect reloadBar;
    public Texture2D reloadBarTexture;
    public Texture2D emptyReloadBarTexture;


   
	// Use this for initialization
    void Start()
    {
        //The only thing that's initialized is the reload bar and reload time.  Everything else is specific to the subclasses.
       // name = "Herbivore";
        reloadBarWidth = 80;
        emptyBar = new Rect(Screen.width / 2, Screen.height / 2f, reloadBarWidth, 10);
        reloadBar = new Rect(Screen.width / 2, Screen.height / 2f, reloadBarWidth, 10);
        totalReloadTime = 0;
        reloadTimeRemaining = totalReloadTime;
    }
	
    // Update is called once per frame
    void Update()
    {
        float i;

        //when the player reloads, a bar is displayed.  When it's full, the reloading is complete.
        reloadTimeRemaining--;

        //the following line ensures that the bar has the same width, no matter how long the time is. The fill rate will increase
        //or decrease depending on the reload time.
        i = (1 - (reloadTimeRemaining / totalReloadTime)) * reloadBarWidth;
        reloadBar.width = i;

        if (reloadTimeRemaining < 0)
            reloadTimeRemaining = 0;
        if (reloadBar.width < 0)
            reloadBar.width = 0;
    }

    void OnGUI()
    {
        //show the amount of time to reload
        if (reloadTimeRemaining > 0)
        {
            GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) + 50, 100, 100), "Reloading" /*+ reloadTimeRemaining.ToString()*/);
            GUI.DrawTexture(emptyBar, emptyReloadBarTexture);
            GUI.DrawTexture(reloadBar, reloadBarTexture);
        }
    }

    //functions
    public virtual void FireWeapon()
    {
        /* This function has a different effect depending on what weapon is being fired. */
    }

    //public void SetReloadTime(float ammoPercentage)
    //{
    //    //reloads weapon and sets a time based on remaining ammo across all weapons.  This is not instantaneous, so the player must use with caution!
    //    //ammoPercentage = currentTotalAmmo / totalMaxAmmo;

    //    //check the percentage of ammo remaining. There are 60 frames in a second so the time to reload should be represented in frames.
    //    if (ammoPercentage <= 1)
    //        totalReloadTime = 0;
    //    else if (ammoPercentage <= 0.9f)
    //        totalReloadTime = 30;
    //    else if (ammoPercentage <= 0.8f)
    //        totalReloadTime = 80;
    //    else if (ammoPercentage <= 0.7f)
    //        totalReloadTime = 120;
    //    else if (ammoPercentage <= 0.6f)
    //        totalReloadTime = 150;
    //    else if (ammoPercentage <= 0.5f)
    //        totalReloadTime = 180;
    //    else if (ammoPercentage <= 0.4f)
    //        totalReloadTime = 200;
    //    else if (ammoPercentage <= 0.3f)
    //        totalReloadTime = 220;
    //    else if (ammoPercentage <= 0.2f)
    //        totalReloadTime = 255;
    //    else if (ammoPercentage <= 0.1f)
    //        totalReloadTime = 300;

    //    reloadTimeRemaining = totalReloadTime;

    //}

    public bool ReloadComplete()
    {
        return (reloadTimeRemaining == 0);
    }

    //public float ReloadCountdown()
    //{
    //    reloadTimeRemaining--;
    //    if (reloadTimeRemaining < 0)
    //        reloadTimeRemaining = 0;

    //    return reloadTimeRemaining;
    //}
}
