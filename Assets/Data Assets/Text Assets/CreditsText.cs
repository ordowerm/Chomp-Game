using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsText : MonoBehaviour
{
    string text = "" +
        "This game was made by Michael Ordower.\n" +
        "Game engine used: Unity 4.11\n" +
        "Audio software used: Ableton Live 9\n" +
        "Sprite editing software used: Piskel, available at www.piskelapp.com\n" +
        "" +
        "All sprites, sounds, custom scripts, and text were created by Michael.\n" +
        "" +
        "Thanks for playing! Consider hiring Michael, please.";

    private void Awake()
    {
        GetComponent<TMP_Squiggle>().SetSource(text);
    }

}
