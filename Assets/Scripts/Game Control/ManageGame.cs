using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    public DinoParams[] dinos;
    public DinoData[] defaultdinos;
    public StageData stage; 
    public string scenePath; //path to first scene to load during debugging

    GameOptions options;
    public InputScript input;
    public PhysicsSettings physics;


    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //dumb way to initalize this
        dinos = new DinoParams[4];
        dinos[0] = new DinoParams(0, "Jerry", DinoParams.AIType.player, defaultdinos[0], this);
        dinos[1] = new DinoParams(1, "Paul", DinoParams.AIType.stupid, defaultdinos[1], this);
        dinos[2] = new DinoParams(2, "Adelaide", DinoParams.AIType.stupid, defaultdinos[2], this);
        dinos[3] = new DinoParams(3, "Pinecone", DinoParams.AIType.stupid, defaultdinos[3], this);

        //references to other scripts
        options = new GameOptions();
        physics = GetComponent<PhysicsSettings>();
        input = GetComponent<InputScript>();
        SceneManager.LoadSceneAsync(scenePath,LoadSceneMode.Single);
    }

    public PhysicsSettings GetPhysics()
    {
        return physics;
    }
    public InputScript GetInput()
    {
        return input;
    }

}
