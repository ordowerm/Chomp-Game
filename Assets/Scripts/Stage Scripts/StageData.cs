using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage Info")]
public class StageData : ScriptableObject
{
    public Sprite tile; //Tile used for walkable surfaces. Assume 1 unit x 1 unit
    public Sprite bg1; //Image to scale for main background image
    public Sprite bg2; //Image to scale for secondary background layer
    public float height; //height of room
    public float minx; //minimum x value for player?
    public float maxx; //max x value for player?
    public MeatList meats;

    /*
     * Platforms are stored as Vector3s.
     * x and y coordinates for platforms are location
     * z coordinate is used to determine how wide the platform should be, as opposed to its actual depth (wanted to use 3-vectors in Unity editor w/o creating new type)
     */
    public Vector3[] platforms;
    public Vector2[] spawnLocations;
}
