using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : DinoMovement
{
    // Update is called once per frame
    new void Update()
    {
        Move((float)input.GetAxis(playerid));
        Jump(input.GetJump(playerid));
        base.Update();
    }

   
}
