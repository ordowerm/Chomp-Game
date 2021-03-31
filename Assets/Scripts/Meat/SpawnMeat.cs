using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeat : Movable
{
    /*
     * This script is used to spawn meat. Here's how it works:
     * This script randomly selects a "type" of meat to spawn.
     * It maintains an array of references to different "types" of meat, and a random integer is generated.
     * The random number is taken, modulo the array size, in order to select which meat is spawned.
     * The modified random number gives us the array index corresponding to whichever meat should be spawned.
     * 
     * We want higher-point-value meat to spawn less frequently. 
     * Since random number generation has a uniform distribution, we need to weight our meat-spawning array.
     * For simplicity's sake, we accomplish this by adding extra copies of a meat reference to our meat-spawning array.
     * For example, if we want meat #0 to appear more often than meat #1, we could construct an array of meat-references that looks like this:
     * spawnarray[] = {meat0,meat0,meat1};
     * We would then take our random number, modulo 3, to select the correct meat reference.
     * 
     * The frequency of the meat type in the array is provided through the "copies" field of the corresponding meat data.
     * The PopulateMeatArray function creates the corresponding array by repeatedly pushing the MeatData's index onto the list.
     * The number of repeated pushes is dictated by the "copies" field.
     * 
     * I'm not sure how Unity optimizes data.
     * In lieu of repeatedly pushing 32-bit references onto the array, I'm pushing byte-sized indices to save space, and then casting to unsigned when it's time to actually spawn the meat from the index.
     * This should add a little bit of overhead in terms of function calls, but it should save a LOT of space.
     * 
     */
    public bool debug;
    const string TAG = "Meat Spawner: ";

    public MeatList meatlist;
    List<byte> spawnarray;

    //parameters for actually spawning the meat
    public float spawny; //height at which to spawn meats
    public float minx; //minimum x value at which meat may spawn
    public float maxx; //maximum
    public float mintime; //minimum amount of time between meat spawns
    public float maxtime; //max
    float spawntimer;
    public uint maxnum; //maximum number of meat spawns
    uint numspawned;
    public GameObject meatfab; //prefab for meat

    public GameObject mgmt;

    //constructs the array of repeated meat indices
    public void PopulateMeatArray()
    {
        spawnarray = new List<byte>();
        for(int i = 0; i<meatlist.list.Length; i++)
        {
            for(int j = 0; j < meatlist.list[i].copies; j++)
            {
                spawnarray.Add((byte)i);
            }
        }


        if (debug)
        {
            string arraystring = "";
            for (int i = 0; i < spawnarray.Count; i++)
            {
                arraystring += meatlist.list[spawnarray[i]].meatname+"; ";
            }

            Debug.Log(TAG + "spawn array: " + arraystring);
        }
    }

    //spawns meat; call in update
    void Spawn()
    {
        //if this is not active, skip spawning
        if (!active)
        {
            return;
        }

        //update spawn timer
        spawntimer -= Time.deltaTime;

        //if timer <= 0 reset timer
        if (spawntimer<=0)
        {
            //if the max number of meats hasn't been reached, spawn meat
            if (numspawned<maxnum)
            {
                if (debug)
                {
                    Debug.Log(TAG + ": spawning meat");

                }

                GameObject spawnee = Instantiate(meatfab);
                spawnee.transform.position = new Vector2(Random.Range(minx, maxx), spawny);
                MeatMovement mm = spawnee.GetComponent<MeatMovement>();

                int index = Random.Range(0, spawnarray.Count- 1);
                if (debug)
                {
                    Debug.Log(TAG + ": spawning meat #" + index + ". Name = " + meatlist.list[spawnarray[index]]);
                }

                mm.SetMeatData(meatlist.list[spawnarray[index]]);
                mm.spawner = this; //not best practice to directly access this field, but whatever. This gives the MeatMovement script a reference to this script, so that the MeatMovement script will subtract 1 from this script's meat counter upon the meat's destruction.
                mm.SetScoreManager(mgmt.GetComponent<ScoreManager>());
                numspawned++;
            }
            spawntimer = Random.Range(mintime, maxtime);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        PopulateMeatArray();
    }

    public void NotifyDestroyed()
    {
        if (debug)
        {
            Debug.Log(TAG + "destruction of meat notified");
        }
        if (numspawned > 0)
        {
            numspawned -= 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }
}
