using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMovement : Movable
{
   
    /*
     * Parent class defining dinosaur movement, collisions, etc.
     * Individual player dinos and AI will inherit from this.
     */

    //Components from dino
    protected Transform t;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected Animator anim;

    //used to get array id in input controller and update score - might refactor later
    public int playerid; 
    
    //Physics flags
    protected bool grounded;
    protected bool mirrored;

    //Items from game manager - might refactor later
    public ManageGame mgmt;
    public PhysicsSettings phys;
    public ScoreManager score;
    public StageManager stage;
    public InputScript input; //putting this in the parent class b/c I couldn't figure out a bug where the game manager wasn't passing the InputScript to the PlayerMovement script.

    // Start is called before the first frame update
    protected void Awake()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        grounded = true;
    }

    //Checks to see if player is standing on solid surface
    protected void CheckGrounded()
    {
        Vector2 size = new Vector2(col.bounds.size.x, col.bounds.size.y / 2f);
        RaycastHit2D rc = Physics2D.BoxCast(rb.position - new Vector2(0f, 0.5f), size, 0, Vector2.down, 0.2f, LayerMask.GetMask("Solid"));
        //rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.2f,LayerMask.GetMask("Solid"));
        if (rc)
        {
            //Debug.Log("Grounded");
            grounded = true;
        }
        else
        {
            //Debug.Log("Not grounded.");
            grounded = false;
        }
    }

    //Takes the direction of movement. Use -1 for left, 0 for neutral, and 1 for right
    protected void Move(float dir)
    {
        if (!active)
        {
            return;
        }

       //sets mirrored state
        if (dir < 0)
        {
            mirrored = true;
            t.localScale = new Vector3(-1, 1, 1);
        }
        if (dir > 0)
        {
            mirrored = false;
            t.localScale = new Vector3(1, 1, 1);
        }

        Vector2 vect = new Vector2(phys.hspeed * dir, rb.velocity.y);
        rb.velocity = vect;
    }

    //Collision mgmt
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Meat"))
        {
            Debug.Log("DINO COLLISION!!!!");
            MeatMovement dm = collision.gameObject.GetComponent<MeatMovement>();
            int points = dm.GetValue(); //use this to update corresponding score in scoremanager
            score.AddPoints(playerid, points); //update score in score manager
            Destroy(collision.gameObject);
        }
    }

    //Takes whether the character should jump as a boolean.
    //For player characters, input whether the jump button is pressed.
    //For computer characters, set the jump flag based on AI functions
    protected void Jump(bool pressed)
    {
        if (!active)
        {
            return;
        }

        if (!grounded||!pressed)
        {
            return;
        }

        rb.velocity -= new Vector2(0, rb.velocity.y); //freeze vertical velocity right before jumping
        rb.AddForce(new Vector2(0, phys.jumpforce), ForceMode2D.Impulse);
        
    }

    //updates animation parameters
    protected void UpdateAnimation()
    {
        anim.SetBool("grounded", grounded);
        //anim.SetBool("mirror", mirrored);
        anim.SetFloat("hspeed", Mathf.Abs(rb.velocity.x));
    }

    public void SetManager(ManageGame m)
    {
        mgmt = m;
        phys = m.GetPhysics();
        input = m.input;
        
        rb.gravityScale = phys.dinogravityscale;
        rb.mass = phys.dinomass;

    }

    // Update is called once per frame
    protected void Update()
    {
        //Move((float)input.GetAxis(playerid));
        //Jump(input.GetJump(playerid));
        CheckGrounded();
        UpdateAnimation();
    }

    //Sets animator override controller. Call when creating the dino.
    public void SetOverride(AnimatorOverrideController cont)
    {
        anim.runtimeAnimatorController = cont;
    }
}
