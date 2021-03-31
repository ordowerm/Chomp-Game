using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{

    enum MessageState
    {
        inactive,
        fadein,
        showing,
        fadeout
    }

    public bool active = false;

    public StageManager sm;
    const float messageTime = 2f; //length between message updates
    const float fadeTime = 1f; //length it takes for message to fade
    float timer = 0;
    float fadetimer = 0;
    bool loop = false; //if the messages are looped, then
    bool fade = false; //flag denoting whether the fading process has begun
    int messageid=-1; //currently displayed message
    public TMP_Squiggle maintext; //reference to the squiggle scripts

    public Image border;//reference to teeth image
    public Image innermessagepanel; //reference to main panel
    public GameObject messageContainer; //get submesh component from this 

    //List of messages
    List<DinoParams> winners;
    List<MessageParams> messages;

    class MessageParams
    {
        internal Color panelcolor;
        internal Color textcolor;
        internal string text;
        List<MessageButton> buttons;

        internal MessageParams()
        {
            panelcolor = Color.black;
            textcolor = Color.white;
            text = "Text";
            buttons = new List<MessageButton>();
        }

        public void SetText(string t)
        {
            text = t;
        }

        public void SetColor(DinoData data)
        {
            if (data == null)
            {
                panelcolor = Color.black;
                textcolor = Color.white;
            }
            else
            {

                panelcolor = data.primarycolor;
                textcolor = data.textcolor;
            }
        }

        public string GetMessage()
        {
            return text;
        }
    }
    class MessageButton
    {
        string text;
        delegate void Execute(); //signature for callback function
        Execute callback; //reference to callback function

        //Constructor takes text to display + callback
        MessageButton(string t, Execute e)
        {
            text = t;
            callback = e;
        }
        
    }


    public void ClearMessages()
    {
        messages.Clear();
    }
    
    //add message to message queue
    public void AddMessage(string t, DinoData d)
    {
        MessageParams m = new MessageParams();
        m.SetText(t);

        if (d!= null)
        {
            m.SetColor(d);
        }
        messages.Add(m);
    }

    //cycle through messages
    public void ShowMessage()
    {
        if (!active)
        {
            return;
        }


        timer += Time.deltaTime;
        if (timer > messageTime)
        {
            messageid++;
            if (messageid >= messages.Count)
            {
                if (loop)
                {
                    messageid = 0;
                }
                else
                {
                    fadetimer = 0;
                    fade = true;
                }
            }
            else
            {
                maintext.SetSource(messages[messageid].GetMessage()); //updates timer text
            }

            timer = 0;
        }
    }

    //toggle loop
    public void SetLoop(bool v)
    {
        loop = v;
    }

    private void Update()
    {
        if (active&&!fade)
        {
            ShowMessage();
        }
        else if (fade)
        {
            fadetimer += Time.deltaTime;
            SetAlpha((1-fadetimer) / fadeTime);
        }
    }

    //alters alpha value of each component's material
    public void SetAlpha(float a)
    {
        border.color = new Color(border.color.r, border.color.g, border.color.b, a);
        innermessagepanel.color = new Color(innermessagepanel.color.r, innermessagepanel.color.g, innermessagepanel.color.b, a);
        TMPro.TMP_SubMeshUI submesh = messageContainer.GetComponentInChildren<TMPro.TMP_SubMeshUI>();
        submesh.material.color = new Color(submesh.material.color.r, submesh.material.color.g, submesh.material.color.b, a);
    }

    private void Awake()
    {
        messages = new List<MessageParams>();
        winners = new List<DinoParams>();
    }

}
