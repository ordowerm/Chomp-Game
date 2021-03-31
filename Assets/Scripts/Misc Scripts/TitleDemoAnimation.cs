using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 
 * This script is used to move the panel left and right in the demo screen
 * 
 * 
 */
public class TitleDemoAnimation : MonoBehaviour
{
    RectTransform rt;
    int dir=1;
    public float speed;
    float height;
    float minx;
    float maxx;
    //references to child images
    public GameObject dino;
    public GameObject bone;
    public GameObject parent;
    void setImageSize(GameObject obj)
    {
        Image img = obj.GetComponent<Image>();
        //float height = parent.GetComponent<RectTransform>().rect.height / 2.0f;
        Debug.Log("Height: " + height);
        float aspect = img.sprite.rect.width / img.sprite.rect.height;
        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height/2.0f);
    }


    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        Debug.Log("In start, sizedelta=" + rt.sizeDelta);
        setImageSize(dino);
        setImageSize(bone);
        
    }

    void Move()
    {
        rt.localPosition += new Vector3(dir * speed * Time.deltaTime, 0,0);
    }


    void CheckSwitch()
    {
        if (minx ==0 && maxx == 0)
        {
            //determine min and max x values
            Vector3[] corners = new Vector3[4];
            rt.GetLocalCorners(corners);
            minx = 2 * corners[0].x;
            maxx = 2 * corners[2].x;
        }
       


        if (rt.localPosition.x > maxx && maxx!=0)
        {
            //Debug.Log("maxx = " + maxx);
            dir = -1;
            rt.localScale = new Vector3(-1, 1, 1);
        }

        if (rt.localPosition.x < minx && minx!=0)
        {
            dir = 1;
            rt.localScale = new Vector3(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckSwitch();
       
    }

    private void LateUpdate()
    {
        float h = parent.GetComponent<RectTransform>().rect.height;
        if (height != h)
        {
            height = h;
            setImageSize(dino);
            setImageSize(bone);
        }
        //Debug.Log(h);
    }
}
