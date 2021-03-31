using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidAIMovement : DinoMovement
{
    /*
     * 
     * Script for moving the least intelligent dinos.
     * Movement decisions are randomized. 
     * The probability of moving left vs. right should be uniformly distributed.
     * When deciding what direction to walk, the dino should choose left or right with the same likelihood.
     * The neutral state should be used sparingly. Use low probabilities and short durations for standing still.
     * 
     */

    float dir=0; //direction of movement. Use -1 for left, 0 for neutral, 1 for right
    float timer=0; //time until next walking decision
    float jumptimer=0; //time until next attempted jump
    public float mintime_neutral=0; //minimum duration of neutral state
    public float maxtime_neutral=1; //maximum time before exiting neutral state
    public float mintime_walk=0;
    public float maxtime_walk=2; //maximum time before recalculating action
    public float neutral_prob=0.1f; //probability of staying put vs. walking
    public float changedir_prob=0.5f; //probability of changing direction
    public float min_jump=0.01f; //minimum time between pressing random jump presses
    public float max_jump=0.5f;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        ChooseMovement();
        ChooseJump();
    }

    //makes decision as to next movement
    void ChooseMovement()
    {
        float rval = Random.value; //gets random number between 0 and 1
        
        //if currently not moving, move.
        if (dir == 0)
        {
            if (rval < 0.5f)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }
            timer = Random.Range(mintime_walk, maxtime_walk);
        }
        else
        {
            if (Random.value < neutral_prob)
            {
                dir = 0;
                timer = Random.Range(mintime_neutral, maxtime_neutral);
            }
            else
            {
                if (Random.value < changedir_prob)
                {
                    dir *= -1;
                }
                timer = Random.Range(mintime_walk, maxtime_walk);
            }

        }
    }
    //sets jump timer
    void ChooseJump()
    {
        //Debug.Log(jumptimer);
        bool beforecheck = grounded;
        CheckGrounded();

        //check to see if 
        if (beforecheck != grounded && grounded)
        {
            jumptimer = Random.Range(min_jump, max_jump);
        }
    }

    // Update is called once per frame
    new void Update()
    {
        timer -= Time.deltaTime;
        if (grounded)
        {
            jumptimer -= Time.deltaTime;
        }
        else
        {
            jumptimer = 1;
        }

        if (timer <= 0)
        {
            ChooseMovement();
        }
        if (jumptimer < 0)
        {
            Jump(true);
            jumptimer = 1;
        }
        Move(dir);
        ChooseJump();
        UpdateAnimation();

    }
}
