using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Transform t;
    public MeatData data;
    ScoreManager score;
    public SpawnMeat spawner;
    int pointvalue;
    SpriteRenderer sprite;

    //variables used for conditionals
    float timer=0;
    bool grounded;
    BoxCollider2D col;
     
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        t = GetComponent<Transform>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //checks to see if meat has landed
    void CheckGrounded()
    {
        if (grounded == true)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, 0);
            col.enabled = false;
            return;
        }

        Vector2 size = col.bounds.size;
        RaycastHit2D rc = Physics2D.BoxCast(rb.position, size, 0, Vector2.down, 0.1f, LayerMask.GetMask("Solid"));
        
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

    public bool GetGrounded()
    {
        return grounded;
    }

    //returns value of meat
    public int GetValue()
    {
        return pointvalue;
    }
   
    
    

    //call this in the meat spawner when spawning
    public void SetScoreManager(ScoreManager s)
    {
        score = s;
    }

    //call this to set the meat data
    public void SetMeatData(MeatData m)
    {
        data = m;
        sprite.sprite = data.pic;
        col.size = sprite.bounds.size;
        col.offset = new Vector2(0,0);
        pointvalue = m.pointvalue;
        rb.velocity = new Vector2(0, -m.fallspeed);
    }

    private void OnDestroy()
    {
        spawner.NotifyDestroyed();
        Debug.Log("I AM MORE DESTORYED");

    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        if (!grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, -data.fallspeed);
        }
        else
        {
            timer += Time.deltaTime;//rb.velocity = new Vector2(rb.velocity.x, 0);
            sprite.color = Color.Lerp(Color.white, Color.clear, timer / data.fadetime);
            if (timer > data.fadetime)
            {
                Destroy(gameObject);
                Debug.Log("I AM DESTORYED");
            }
        }
    }
}
