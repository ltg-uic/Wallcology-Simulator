using UnityEngine;
using System.Collections;

public class CritterManager : MonoBehaviour {
    // Our Prefab Critters
    public GameObject Bally;
    public GameObject Slimy;
    public GameObject Dino;
    public GameObject FlappyStripe;
    public GameObject Piston;
    public GameObject FlappyRing;
    public GameObject Jumpy;


    private Transform[] spawnPoints;
    private Transform[] exitPoints;

    private Transform[] spawnPointsPipe;
    private Transform[] exitPointsPipe;

    private Transform[] spawnPointsBrick;
    private Transform[] exitPointsBrick;

    private Transform[] spawnPointsGen;
    private Transform[] exitPointsGen;


	// Use this for initialization
	void Start () {
        DebugCritters();
	}


    private void DebugCritters() {
        Debug.Log("SpawnPipe tags! " + GameObject.FindGameObjectsWithTag("SpawnPipe").Length);
        // Populate them
        spawnPointsPipe = SetUpPointArrays(0, GameObject.FindGameObjectsWithTag("SpawnPipe"));
        spawnPointsBrick = SetUpPointArrays(0, GameObject.FindGameObjectsWithTag("SpawnBrick"));
        Debug.Log("spawnPointsPipe " + spawnPointsPipe.Length);
        Debug.Log(spawnPointsBrick);

        exitPointsPipe = SetUpPointArrays(0, GameObject.FindGameObjectsWithTag("ExitPipe"));
        exitPointsBrick = SetUpPointArrays(0, GameObject.FindGameObjectsWithTag("ExitBrick"));
        Debug.Log(exitPointsPipe.Length);
        Debug.Log(exitPointsBrick.Length);

        // You have got to be kidding me....
        spawnPoints = _join( spawnPointsPipe, spawnPointsBrick );
        exitPoints = _join( exitPointsPipe,exitPointsBrick );

        //
        for (int i = 0; i < 9; i++) {
            SpawnCritter( i );
        };

    }

    //
    private Transform[] _join( Transform[] x, Transform[] y )
    {
        var z = new Transform[x.Length + y.Length];
        x.CopyTo(z, 0);
        y.CopyTo(z, x.Length);
        Debug.Log(z.Length);

        return z;
    }

    // Spawns a requested Critter in a radomized location based on the available
    // waypoints established
    void SpawnCritter( int id )
    {
        switch( id )
        {
            case 0:
                InstantiateCritter(spawnPoints, FlappyStripe);
                break;
            case 1:
                InstantiateCritter(spawnPointsPipe, Bally);
                break;
            case 2:
                InstantiateCritter(spawnPoints, Slimy);
                break;
            case 3:
                InstantiateCritter(spawnPointsBrick, Dino);
                break;
            case 6:
                InstantiateCritter(spawnPointsPipe, Piston);
                break;
            case 7:
                InstantiateCritter(spawnPoints, FlappyRing);
                break;
            case 8:
                InstantiateCritter(spawnPoints, Jumpy);
                break;
            default:
                break;
        }
    }


    // Instatiates a given Critter positioned at a random location provided
    private void InstantiateCritter(Transform[] waypoints, GameObject critter)
    {
        int index = Random.Range(0, waypoints.Length);
        Instantiate(critter, waypoints[index].position, waypoints[index].rotation);
    }


    // Assumes point Array is already instantiated and just needs to be populated
    private Transform[] SetUpPointArrays ( int i, GameObject[] objects)
    {
        Debug.Log("SetUpPointArrays");

        Transform[] pointArray = new Transform[ objects.Length ];

        foreach (GameObject t in objects) {

            Debug.Log(t.tag + " " + t.transform.position + " " + " " + objects.Length);
            pointArray[i++] = t.transform;
        }

        Debug.Log(pointArray.Length);
        return pointArray;
    }

}
