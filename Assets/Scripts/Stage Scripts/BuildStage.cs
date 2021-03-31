using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * 
 * This was the original script used for building a stage from stage data. 
 * Migrating these methods to the stage manager script.
 * 
 * 
 */
public class BuildStage : MonoBehaviour
{
    public bool debug;
    const string TAG = "Stage builder: ";

    public StageData data; //data
    public GameObject platprefab; //platform prefab
    public Camera cam;
    List<GameObject> platforms; //list of platform game objects
    GameObject[] walls;
    GameObject[] bg; //array of background images

    // Start is called before the first frame update
    void Start()
    {
        platforms = new List<GameObject>();
        Build();
        InitializeCamera();
        ScaleBG();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * 
     * 
     * Build based on the following encodings: 
     * 
     * 
     */
    public void Build()
    {
        BuildFloor(0.5f);
        for (int i = 0; i < data.platforms.Length; i++)
        {
            BuildPlatform(data.platforms[i]);
        }
    }

    public void BuildPlatform(Vector3 pdata)
    {
        GameObject plat = Instantiate(platprefab);
        BoxCollider2D col = plat.GetComponent<BoxCollider2D>();
        SpriteRenderer spr = plat.GetComponent<SpriteRenderer>();
        spr.sprite = data.tile;
        col.size = new Vector2(pdata.z, 1);
        spr.drawMode = SpriteDrawMode.Tiled;
        spr.size = new Vector2(pdata.z, 1);
        plat.transform.position = new Vector2(pdata.x, pdata.y);
        platforms.Add(plat);
    }

    //Builds floor
    public void BuildFloor(float yvalue)
    {
        //builds the floor
        GameObject floor = Instantiate(platprefab);
        BoxCollider2D col = floor.GetComponent<BoxCollider2D>();
        SpriteRenderer spr = floor.GetComponent<SpriteRenderer>();
        spr.sprite = data.tile;
        float width = data.maxx - data.minx;
        col.size = new Vector2(width+1, 1);
        spr.drawMode = SpriteDrawMode.Tiled;
        spr.size = new Vector2(width+1, 1);
        floor.transform.position = new Vector2(0, yvalue);

        //build walls
        float height = data.height;
        GameObject[] walls = { new GameObject(), new GameObject() };
        for (int i = 0; i<2; i++)
        {
            walls[i].AddComponent<BoxCollider2D>();
            walls[i].GetComponent<BoxCollider2D>().size = new Vector2(1, height+1);
        }
        walls[0].transform.position = floor.transform.position + new Vector3(-(width / 2.0f+0.5f), height / 2.0f,0);
        walls[1].transform.position = floor.transform.position + new Vector3(width / 2.0f+0.5f, height / 2.0f, 0);

    }


    //places camera in correct position and resizes it to match appropriate width and height
    public void InitializeCamera()
    {
        float width = data.maxx - data.minx;
        float aspect = cam.aspect;
        float size = width / (2.0f * aspect);
        cam.orthographicSize = size;
        cam.transform.position = new Vector3(0, size, -10);

    }

    //Uses info from camera to scale backgrounds
    public void ScaleBG()
    {
        GameObject[] b = { new GameObject(), new GameObject()};
        bg = b;

        //create objects to control sprite background
        bg[0].transform.position = new Vector2(cam.transform.position.x,cam.transform.position.y);
        bg[0].AddComponent<SpriteRenderer>();
        bg[1].AddComponent<SpriteRenderer>();


        //set up main background
        bg[0].GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        bg[0].GetComponent<SpriteRenderer>().sortingOrder = -3; //setting priority to -3 should make everything with a higher sorting order draw over it
        bg[0].GetComponent<SpriteRenderer>().sprite = data.bg1; //set background image
        float ysize =  (2.0f * cam.orthographicSize); //2*size gives the total y-size of the camera
        float xsize = cam.aspect * (2.0f * cam.orthographicSize); //aspect ratio * total y-size = x-size of camera
        bg[0].GetComponent<SpriteRenderer>().size = new Vector2(xsize, ysize); //scale background image appropriately

    }
}
