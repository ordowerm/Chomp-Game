using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions
{
    bool audioOn=false; //default setting is fault because nobody wants to hear music if it's on a webpage
    bool squiggleTextOn = true; //squiggly text is hard to read for some. toggle this.
    bool particleEffectsOn = true;
    bool turboOn = true;

    //some settings are based on preset values (e.g. 30 seconds is option 1, 60 seconds is option 2) 
    //these variables contain the array ids corresponding to the current preset used
    int gameLengthId=2; //length of each round
    int roundsPerGameId = 0;
    
    //fixed round lengths
    float[] roundTimes = {30, 45, 60, 90, 120, 150, 180};
    int[] roundsPerGameOptions = { 1, 3, 5, 7 }; //best of 1, best of 3, etc.
    
    //haven't looked into Unity audio. Implement this correctly later
    int musicVolume = 5;
    int sfxVolume = 5;
}
