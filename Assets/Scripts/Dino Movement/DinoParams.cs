using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class DinoParams
{
    public int playerid;
    public string playername;
    public enum AIType
    {
        player, stupid, medium, smart
    }
    public AIType type; 
    public DinoData data;
    public ManageGame mgmt;
    
    public DinoParams(int id, string name, AIType atype, DinoData ddata, ManageGame m)
    {
        playerid = id;
        playername = name;
        type = atype;
        data = ddata;
        mgmt = m;
    }
   
    //Call this while building the stage.
    //Pass in the instance of the Dinosaur prefab that we're building + reference to the stage manager used to build it.
    //This script will assign relevant values
    public DinoMovement AttachAI(StageManager s, GameObject target)
    {
        DinoMovement script;
        switch (type)
        {
            case AIType.player:
                script = target.AddComponent<PlayerMovement>();
                break;
            case AIType.stupid:
            default:
                script = target.AddComponent<StupidAIMovement>();
                break;
        }
        script.mgmt = mgmt;
        script.stage = s;
        return script;
    }

}
