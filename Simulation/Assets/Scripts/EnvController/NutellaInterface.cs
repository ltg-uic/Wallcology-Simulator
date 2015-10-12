using UnityEngine;
using System.Collections;

public class NutellaInterface : MonoBehaviour {

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



	// Use this for initialization
	void Start ()
    {
        Debug.Log("Calling external application!!");
	    Application.ExternalCall("initWallScopeStartState", true);
        Application.ExternalCall("ProgressUpdate", "Start", true);
        InitializeSpawnPoints();
        // DebugCritters();
	}


    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            KillCritter( Random.Range(0, 10) );
        }

    }

    // Send the specified critter ID to the Client-side
    void GetPopulationCount( string id ) {
        Debug.Log("GetPopulationCount " + id);
        int total = GameObject.FindGameObjectsWithTag( id ).Length;
        Application.ExternalCall("ReceivePopulationCount", id, total);

    }




    // Spawns a requested Critter in a radomized location based on the available
    // waypoints established
    void SpawnCritter( int id )
    {
        Debug.Log("SpawnCritter " + id);
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
                Debug.Log("SpawnCritter DEFAULT" + id);
                break;
        }
    }


    // Spawns a requested Critter in a radomized location based on the available
    // waypoints established
    void KillCritter( int id )
    {
        Debug.Log("KillCritter " + id);
        switch( id )
        {
            case 0:
                DestroyCritter( exitPoints, "0");
                break;
            case 1:
                DestroyCritter( exitPointsPipe, "1");
                break;
            case 2:
                DestroyCritter( exitPoints, "2");
                break;
            case 3:
                DestroyCritter( exitPointsBrick, "3");
                break;
            case 6:
                DestroyCritter( exitPointsPipe, "6");
                break;
            case 7:
                DestroyCritter( exitPoints, "7");
                break;
            case 8:
                DestroyCritter( exitPoints, "8");
                break;
            default:
                break;
        }
    }

    private void InitializeSpawnPoints() {
        Debug.Log("InitializeSpawnPoints");
        // Populate them
        spawnPointsPipe = _SetUpPointArrays( GameObject.FindGameObjectsWithTag("SpawnPipe") );
        spawnPointsBrick = _SetUpPointArrays( GameObject.FindGameObjectsWithTag("SpawnBrick") );

        exitPointsPipe = _SetUpPointArrays( GameObject.FindGameObjectsWithTag("ExitPipe") );
        exitPointsBrick = _SetUpPointArrays( GameObject.FindGameObjectsWithTag("ExitBrick") );

        // You have got to be kidding me....
        spawnPoints = _join( spawnPointsPipe, spawnPointsBrick );
        spawnPoints = _join( spawnPointsPipe, spawnPointsBrick );
        exitPoints = _join( exitPointsPipe,exitPointsBrick );

        Debug.Log("InitializeSpawnPoints---DONE");
        Application.ExternalCall("ProgressUpdate", "InitializeSpawnPoints", true);

    }


    // Instatiates a given Critter positioned at a random location provided
    private void InstantiateCritter(Transform[] waypoints, GameObject critter)
    {
        Debug.Log("InstantiateCritter");
        int index = Random.Range(0, waypoints.Length);
        Instantiate(critter, waypoints[index].position, waypoints[index].rotation);
        Application.ExternalCall("ProgressUpdate", "InstantiateCritter", true);
    }

    // Passes message to bug informing it is time to Die. Sets NavMesh Agents position to nearest exit point
    private void DestroyCritter(Transform[] waypoints, string tagName)
    {
        Debug.Log("DestroyCritter!! " + tagName);
        GameObject Critter = GameObject.FindWithTag(tagName);
        if ( Critter ) {
            NavMeshAgent agent = Critter.GetComponent<NavMeshAgent>();
            // agent.SetDestination(waypoints[index].position);
            Transform cur = waypoints[0];
            float min = Vector3.Distance(cur.position, agent.destination);

            foreach(Transform pt in waypoints) {
                float distance =  Vector3.Distance(pt.position, agent.destination);
                if( distance < min ) {  // distance between the center object and the enemy car
                    cur = pt;
                    min = distance;
                }
            }
            agent.SetDestination(cur.position);
            CritterControl cc = Critter.GetComponent<CritterControl>();
            cc.timeToDie = true;

        }

    }


    // Assumes point Array is already instantiated and just needs to be populated
    private Transform[] _SetUpPointArrays ( GameObject[] objects)
    {
        Debug.Log("_SetUpPointArrays");
        int i = 0;
        Transform[] pointArray = new Transform[ objects.Length ];

        foreach (GameObject t in objects) {

            // Debug.Log(t.tag + " " + t.transform.position + " " + " " + objects.Length);
            pointArray[i++] = t.transform;
        }

        return pointArray;
    }

    //
    private Transform[] _join( Transform[] x, Transform[] y )
    {
        // This code was found online from the Unity help site...I forgot the link...
        // I didnt come up with this!! -KRA
        var z = new Transform[x.Length + y.Length];
        x.CopyTo(z, 0);
        y.CopyTo(z, x.Length);
        Debug.Log(z.Length);

        return z;
    }


    private void DebugCritters() {
        for (int j = 0; j < Random.Range(1, 4); j++) {
            for (int i = 0; i < 9; i++) {
                SpawnCritter( i );
            };
        };
    }
}
