using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/*
 * 
 * Attach this script to button prefab
 * 
 * 
 */
public class TitleButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,IPointerUpHandler
{
    public MakeButton controller;

    int id; //
    float animtimer = 0;
    ButtonState state;
    Color bgsource;
    Color textsource;
    Color bgtarget;
    Color texttarget;
    Color bgcolor;
    Color textcolor;

    public GameObject panel; //background to modify
    public GameObject textmesh; //reference to text mesh
    Image image; //reference to the image used to generate background panel
    TMPro.TMP_SubMeshUI sm; //reference to the submesh used to generate text sprites

    public enum ButtonState
    {
        normal, hover, clicked, post
    }


    void CheckStateChanged(ButtonState newstate, Color tc, Color bgc)
    {
        if (state != newstate)
        {
            animtimer = 0;
            bgtarget = bgc; //target background color
            texttarget = tc; //target text color
            bgsource = bgcolor; //update source color
            textsource = textcolor; //update text color
            state = newstate;
            animtimer = 0;
        }
     }

    //linearly interpolates colors based on timer
    void UpdateColor()
    {
        if (bgcolor.Equals(bgtarget) && textcolor.Equals(texttarget))
        {
            bgsource = bgcolor;
            textsource = textcolor;
            return; //nothing to do if colors match
        }

        //update timer
        if (animtimer < controller.animationtime)
        {
            animtimer += Time.deltaTime;
            if (animtimer > controller.animationtime)
            {
                animtimer = controller.animationtime;
            }
            float prop = animtimer / controller.animationtime;
            bgcolor = Color.Lerp(bgsource, bgtarget, prop);
            textcolor = Color.Lerp(textsource, texttarget, prop);
            image.color = bgcolor;
            sm.material.color = textcolor;
        }
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{

        //throw new System.NotImplementedException();
    //}
    public void OnPointerDown(PointerEventData eventData)
    {
        CheckStateChanged(ButtonState.clicked, controller.fontTintClicked, controller.bgClicked);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state != ButtonState.clicked && state != ButtonState.post)
        {
            CheckStateChanged(ButtonState.hover, controller.fontTintHover, controller.bgHover);
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (state == ButtonState.hover)
        {
            CheckStateChanged(ButtonState.normal, controller.fontTintStd, controller.bgStd);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == ButtonState.clicked)
        {
            CheckStateChanged(ButtonState.post, controller.fontTintPostClick, controller.bgPostClick);
            controller.NotifyClicked(id);
        }
    }

    //assigns observer and corresponding id to this script
    public void SetController(MakeButton mb, int num)
    {
        controller = mb;
        id = num;
    }

    //called by button manager when a different button is pressed
    public void SetUnclicked()
    {
        Debug.Log("Unclicking #" + id);
        CheckStateChanged(ButtonState.normal, controller.fontTintStd, controller.bgStd);
    }
    public void SetId(int i)
    {
        id = i;
    }
    public int GetId()
    {
        return id;
    }
    // Start is called before the first frame update
    void Awake()
    {
        image = panel.GetComponent<Image>();
        sm = textmesh.GetComponent<TMPro.TMP_SubMeshUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor();
    }
}
