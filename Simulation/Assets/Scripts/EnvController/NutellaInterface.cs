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


    private Transform[] GenPoints;
    private Transform[] PipePoints;
    private Transform[] BrickPoints;


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
            DebugCritters();
            // for ( int i = 0; i < Random.Range(7,10); i++) {
            //     KillCritter( Random.Range(0, 10) );
            // }
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
                Debug.Log("Lets make a FlappyStripe");
                InstantiateCritter(BrickPoints, FlappyStripe);
                break;
            case 1:
                Debug.Log("Lets make a Bally");
                InstantiateCritter(PipePoints, Bally);
                break;
            case 2:
                Debug.Log("Lets make a Slimy");
                InstantiateCritter(GenPoints, Slimy);
                break;
            case 3:
                Debug.Log("Lets make a Dino");
                InstantiateCritter(BrickPoints, Dino);
                break;
            case 6:
                Debug.Log("Lets make a Piston");
                InstantiateCritter(PipePoints, Piston);
                break;
            case 7:
                Debug.Log("Lets make a FlappyRing");
                InstantiateCritter(GenPoints, FlappyRing);
                break;
            case 8:
                Debug.Log("Lets make a Jumpy");
                InstantiateCritter(GenPoints, Jumpy);
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
                DestroyCritter( BrickPoints, "Herbivore", 0);
                break;
            case 1:
                DestroyCritter( PipePoints, "Predator", 1);
                break;
            case 2:
                DestroyCritter( GenPoints, "Herbivore", 2);
                break;
            case 3:
                DestroyCritter( BrickPoints, "Predator", 3);
                break;
            case 6:
                DestroyCritter( PipePoints, "Herbivore", 6);
                break;
            case 7:
                DestroyCritter( GenPoints, "Herbivore", 7);
                break;
            case 8:
                DestroyCritter( GenPoints, "Predator", 8);
                break;
            default:
                break;
        }
    }

    private void InitializeSpawnPoints() {
        Debug.Log("InitializeSpawnPoints");
        // Populate them
        PipePoints = _SetUpPointArrays( GameObject.FindGameObjectsWithTag("Pipe") );
        BrickPoints = _SetUpPointArrays( GameObject.FindGameObjectsWithTag("Brick") );

        // You have got to be kidding me....
        GenPoints = _join( PipePoints, BrickPoints );
        GenPoints = _join( PipePoints, BrickPoints );

        Debug.Log("InitializeSpawnPoints---DONE");
        Application.ExternalCall("ProgressUpdate", "InitializeSpawnPoints", true);

    }


    // Instatiates a given Critter positioned at a random location provided
    private void InstantiateCritter(Transform[] waypoints, GameObject critter)
    {
        int index = Random.Range(0, waypoints.Length-1);
        Vector3 position = waypoints[index].position;
        Debug.Log("InstantiateCritter ");
        Debug.Log(position);
        Instantiate(critter, position, waypoints[index].rotation);
        Application.ExternalCall("ProgressUpdate", "InstantiateCritter", true);
    }

    // Passes message to bug informing it is time to Die. Sets NavMesh Agents position to nearest exit point
    private void DestroyCritter(Transform[] waypoints, string tagName, int id )
    {
        Debug.Log("DestroyCritter!! " + tagName);
        GameObject [] Critters = GameObject.FindGameObjectsWithTag(tagName);

        StatePatternCritter critter;

        foreach ( GameObject cc in Critters)
        {
            critter = cc.GetComponent<StatePatternCritter>();
            if ( critter.ID == id ) {
                // agent.SetDestination(waypoints[index].position);
                Transform destination = waypoints[0];
                float min = Vector3.Distance(destination.position, critter.navMeshAgent.destination);

                foreach(Transform pt in waypoints) {
                    float distance =  Vector3.Distance(pt.position, critter.navMeshAgent.destination);
                    if( distance < min ) {  // distance between the point and the critter
                        destination = pt;
                        min = distance;
                    }
                }
                critter.navMeshAgent.SetDestination(destination.position);

            }
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
