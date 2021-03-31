using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DinoData", menuName = "Dino Info")]
public class DinoData : ScriptableObject
{
    //colors for generating UI
    public Color primarycolor;
    public Color secondarycolor;
    public Color tertiarycolor;
    public Color textcolor;

    //other data
    public string colorname; //for when the player selects their dino
    public Sprite portrait; //portrait to be shown in UI
    public AnimatorOverrideController animator; //animation set to be used for dino
}
