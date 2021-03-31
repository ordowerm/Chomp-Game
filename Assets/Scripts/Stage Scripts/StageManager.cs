using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 
 * 
 * This script creates the stage scene.
 * 
 * 
 * 
 * 
 */
public class StageManager : MonoBehaviour
{
    const string TAG = "Stage mgmt: ";
    public bool debug;

    //prefabs
    public GameObject dinofab; //prefab for dinos
    public GameObject platformfab; //prefab for platforms



    /*
     *
     * Game uses an FSM to determine what to do, and when, during a scene.
     * Current state is stored in gamestate.
     * Calling AdvanceState moves to the next state in the chain and updates any variables.
     * RunState is called every frame.
     * There aren't really branching paths, other than switching between pause and ingame.
     * 
     */
    public enum GameState
    {
        loading, //default state
        pregame, //display on your mark, get set, chomp
        ingame, //regular game
        postgame, //display who won, unless there was a tie
        waitrestart, //await rematch or return to main menu
        pause
    }
    GameState gamestate=GameState.loading;
    bool paused = false; //putting this here in case I decide to implement pause. Might not do it.
    public void AdvanceState()
    {
        switch (gamestate)
        {
            //when transitioning from loading to pregame, intialize message display 
            case GameState.loading:
                gamestate = GameState.pregame;
                break;
            case GameState.pregame: //during this transition, enable all movables and make message display invisible
                gamestate = GameState.ingame;
                break;
            case GameState.ingame:
                //make message box visible and set its message mode
                //if paused, enable quit game and continue game buttons
                //disable all movables
                if (!paused)
                {
                    gamestate = GameState.postgame;
                }
                else
                {
                    gamestate = GameState.pause;
                }
                break;
            case GameState.postgame:
                //update message box
                //add rematch and quit buttons
                gamestate = GameState.waitrestart;
                break;
            case GameState.pause:
                paused = false;
                gamestate = GameState.ingame;
                //make message box invisible
                break;
        }
    }
    void RunState()
    {
        switch (gamestate)
        {
            case GameState.loading:
                break;
            case GameState.pregame:
                break;
            case GameState.ingame:
                break;
            case GameState.postgame:
                break;
        }
    }

    //data parameters
    StageData stageData;
    DinoParams[] pars;
    
    //references to components and game objects
    GameObject[] dinos; //list of dinos
    List<GameObject> platforms; //list of platform game objects
    GameObject manager;
    ManageGame mgmt;
    
    //params related to camera and background scaling
    Camera cam;
    GameObject[] walls;
    GameObject[] bg; //array of background images
    float screenwidth=0;

    //params related to diplaying score and timer
    public Canvas canvas; //reference to canvas.
    GameObject uiPanel; //reference to programmatically-genreated UI panel of which the score and time panels will be children
    public GameObject scorePanel; //reference to score panel UI prefab
    public GameObject timerPanel; //reference to timer prefab
    public GameObject messagePane; //reference to message prefab
    public MessageDisplay message; //reference to message pane
    
    float gameTimer;
    int[] scores;


    //TODO: POPULATE THIS FOR TESTING.
    void LoadMessages()
    {
        message.AddMessage("Elvis is cool", null);
        message.AddMessage("I am cool.", null);
        message.AddMessage("No way, hose b.", null);
        message.AddMessage("Bebebebebebe", null);
        message.AddMessage("Bad news, beagle", null);
        message.SetLoop(false);
        message.active = true;
    }

    //Uses dino data to spawn dinos
    void SpawnDinos()
    {
        if (pars.Length > stageData.spawnLocations.Length)
        {
            Debug.LogError(TAG + "Fewer spawn locations than dinos");
        }
        for (int i = 0; i < pars.Length; i++)
        {
            GameObject newdino = Instantiate(dinofab); //spawn dino
            newdino.transform.position = stageData.spawnLocations[i]; //set dino to spawn location
            DinoMovement dm = pars[i].AttachAI(this, newdino); //attaches dino movement script to newly-spawned dude
            dm.playerid = i; //assign player id
            dm.name = "Dino " + i.ToString(); //for debugging
            dm.SetOverride(pars[i].data.animator); //assign animator override
            dm.SetManager(mgmt);
        }
    }

    //manages background images
    void ScaleBg()
    {
        if (bg == null)
        {
            GameObject[] b = { new GameObject(), new GameObject() };
            bg = b;

            //create objects to control sprite background
            bg[0].transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
            SpriteRenderer spr = bg[0].AddComponent<SpriteRenderer>();
            bg[0].name = "BGImage";
            bg[1].name = "BGImageLayer2";
            bg[1].AddComponent<SpriteRenderer>();
            //set up main background
            spr.drawMode = SpriteDrawMode.Sliced;
            spr.sortingOrder = -3; //setting priority to -3 should make everything with a higher sorting order draw over it
            spr.sprite = stageData.bg1; //set background image
        }


            
            float ysize = (2.0f * cam.orthographicSize); //2*size gives the total y-size of the camera
            float xsize = cam.aspect * (2.0f * cam.orthographicSize); //aspect ratio * total y-size = x-size of camera
            bg[0].GetComponent<SpriteRenderer>().size = new Vector2(xsize, ysize); //scale background image appropriately
            bg[0].transform.position = this.transform.position-new Vector3(0,0,this.transform.position.z);

       
    }
       

    //call when building stage or altering window size.
    void UpdateCamera()
    {
        if (Screen.width == screenwidth) { return; } //return if there's no change in window size. Since the script initializes the stored "window size" to 0, the script treats the actual screen size as a change in window size.
        
        if (cam == null)
        {
        //create camera if it doesn't exist yet
        cam = gameObject.AddComponent<Camera>();
        cam.orthographic = true; //make it ortho
        }


        //adjusts camera to fit bounds
        float width = stageData.maxx - stageData.minx;
        float aspect = cam.aspect;
        float size = width / (2.0f * aspect);
        cam.orthographicSize = size;
        cam.transform.position = new Vector3(0, size, -10);
        ScaleBg();
        
        //rescale UI if it's already been scaled
        if (uiPanel != null)
        {
            ResizeUIPanel();
        }

        screenwidth = Screen.width;
    }
    
    //builds stage platforms. Repurposed from BuildStage script
    void BuildStage()
    {
        platforms = new List<GameObject>();
        BuildFloor(0.5f); //since all platforms are defaulted to be 1 unit x 1 unit, with center pivot point, we hardcode this value
        for (int i = 0; i < stageData.platforms.Length; i++)
        {
            BuildPlatform(stageData.platforms[i],i);
        }
        UpdateCamera();
    }

    //helper function for building floor and walls. Call in BuildStage
    public void BuildFloor(float yvalue)
    {
        //builds the floor
        GameObject floor = Instantiate(platformfab);
        floor.name = "Floor";
        BoxCollider2D col = floor.GetComponent<BoxCollider2D>();
        SpriteRenderer spr = floor.GetComponent<SpriteRenderer>();
        spr.sprite = stageData.tile;
        float width = stageData.maxx - stageData.minx;
        col.size = new Vector2(width + 1, 1);
        spr.drawMode = SpriteDrawMode.Tiled;
        spr.size = new Vector2(width + 1, 1);
        floor.transform.position = new Vector2(0, yvalue);
        platforms.Add(floor); //treats floor as a platform

        //build walls
        float height = stageData.height;
        GameObject[] walls = { new GameObject(), new GameObject() };
        for (int i = 0; i < 2; i++)
        {
            walls[i].name = "Wall";
            walls[i].AddComponent<BoxCollider2D>();
            walls[i].GetComponent<BoxCollider2D>().size = new Vector2(1, height + 1);
        }
        walls[0].transform.position = floor.transform.position + new Vector3(-(width / 2.0f + 0.5f), height / 2.0f, 0);
        walls[1].transform.position = floor.transform.position + new Vector3(width / 2.0f + 0.5f, height / 2.0f, 0);

    }

    //helper function for spawning platforms. Call in BuildStage
    public void BuildPlatform(Vector3 pdata,int pnum)
    {
        GameObject plat = Instantiate(platformfab);
        plat.name = "Platform" + pnum.ToString(); //Included for debugging in Unity Editor.
        BoxCollider2D col = plat.GetComponent<BoxCollider2D>();
        SpriteRenderer spr = plat.GetComponent<SpriteRenderer>();
        spr.sprite = stageData.tile;
        col.size = new Vector2(pdata.z, 1);
        spr.drawMode = SpriteDrawMode.Tiled;
        spr.size = new Vector2(pdata.z, 1);
        plat.transform.position = new Vector2(pdata.x, pdata.y);
        platforms.Add(plat);
    }

    //returns blank space between heighest jumping point and camera upper bound, in world space. Call when setting up and scaling score panels
    float GetBlankSpace()
    {
        float camMaxY = cam.transform.position.y + cam.orthographicSize / 2.0f;
        float blankSpaceHeight = camMaxY - CalculateApex();
        return blankSpaceHeight;
    }
    
    //builds score/timer UI pane
    void BuildUI()
    {
        uiPanel = BuildBasePanel(GetBlankSpace());
        AttachUIPanels(uiPanel);

        GameObject mess = Instantiate(messagePane);
        message = mess.GetComponent<MessageDisplay>();
        message.SetLoop(true);
        RectTransform rect = message.GetComponent<RectTransform>();
        rect.parent = canvas.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0,0);

    }

    //helper function that creates the base panel used for displaying UI info
    //I originally did this manually in the editor, but I decided that creating this programmatically would be better.
    //This also allows for dynamic resizing of the window -- we can rescale this single object, which will, in turn, rescale its child objects
    //Call in BuildUI
    GameObject BuildBasePanel(float space)
    {
        GameObject scorepane = new GameObject();
        //scorepane.transform.position = canvas.transform.position;
        scorepane.name = "Base Panel";
        RectTransform rect = scorepane.AddComponent<RectTransform>();
        rect.parent = canvas.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 1);
        rect.localScale = new Vector3(1, 1, 1);
        rect.sizeDelta=new Vector2(0,space/cam.orthographicSize*cam.pixelHeight);
        HorizontalLayoutGroup group = scorepane.AddComponent<HorizontalLayoutGroup>();
        group.childControlHeight = true;
        group.childControlWidth = true;
        group.childForceExpandHeight = true;
        group.childForceExpandWidth = true;
        return scorepane;
    }

    //resizes UI panel. Call when building UI panel or resizing camera
    //TODO: get this to actually work
    void ResizeUIPanel()
    {
        float space = GetBlankSpace();
        RectTransform rect = canvas.GetComponent<RectTransform>();

        uiPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0,space / cam.orthographicSize * rect.sizeDelta.y);

    }


    //helper function attaching the score and timer panels to the base panel, while also passing in dino parameters
    void AttachUIPanels(GameObject basepanel)
    {
          
        //PLACEHOLDER LOOP. Content is used for sizing. 
        //TODO: pass in dinoparams, switch loop to 4 score panels instead of 5, add 1 timer panel, then switch timer panel's sibling index to 2, so that the score panels sandwich it in the layout
        for (int i = 0; i< 5; i++)
        {

        GameObject newguy = Instantiate(scorePanel, basepanel.transform);
        RectTransform parentrect = basepanel.GetComponent<RectTransform>();
        RectTransform childrect = newguy.GetComponent<RectTransform>();
        childrect.parent = parentrect;
        parentrect.ForceUpdateRectTransforms();
        }
        
    }

    //uses physics information to calculate the maximum jumpable y-value in the stage
    //assume platforms are 1 unit tall, with player also 1 unit tall
    //use to size/resize base panel, and also to place initial position of meat spawner
    public float CalculateApex()
    {
        float apex = 0;
        float h = mgmt.physics.GetJumpHeight();
        for (int i = 0; i < platforms.Count; i++)
        {
            apex = Mathf.Max(apex, platforms[i].transform.position.y + h+1.0f);
        }
        return apex;
    }
    
   // Update is called once per frame
    void Update()
    {
        UpdateCamera(); //detect changes in aspect ratio/window size
    }

    
    private void Awake()
    {
        //before there are too many objects active, get reference to game manager
        manager = GameObject.FindGameObjectsWithTag("GameController")[0];
        mgmt = manager.GetComponent<ManageGame>();
        pars = mgmt.dinos;
        stageData = mgmt.stage;

        //build the stage
        BuildStage();
        SpawnDinos();
        BuildUI();
        LoadMessages();
    }
}
