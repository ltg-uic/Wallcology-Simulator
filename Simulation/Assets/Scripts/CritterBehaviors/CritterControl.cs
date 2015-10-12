using UnityEngine;
using System.Collections;

public class CritterControl : MonoBehaviour {

    public int ID;
    public bool timeToDie = false;
    public bool inPursuit = false;
    public bool isEating = false;
    public bool isSeeking = false;
    private int destPoint = 0;
    private Transform[] points;
    private NavMeshAgent agent;


	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe"));
        Debug.Log(agent);

        switch( ID )
        {
            case 0:
                points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIBrick"));
                break;
            case 1:
                points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe"));
                break;
            case 2:
                points = _join(
                            SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe")),
                            SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIBrick"))
                        );
                break;
            case 3:
                points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIBrick"));
                break;
            case 6:
                points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe"));
                break;
            case 7:
                points = _join(
                            SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe")),
                            SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIBrick"))
                        );
                break;
            case 8:
                points = _join(
                            SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe")),
                            SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIBrick"))
                        );
                break;
            default:
                break;
        }

        // Dont think we want to disable auto breaking...
        GotoNextPoint();
	}


	// Update is called once per frame
	void Update () {
        if (agent.remainingDistance < 0.05f)

            if (timeToDie) {
                Debug.Log("Goodbye Cruel world!");
                Object.Destroy(this.gameObject);
            } else {
                GotoNextPoint();
            }
	}


    void OnTriggerEnter( Collider col )
    {
        Debug.Log(tag + "Collided with " + col.gameObject.tag);
        if( col.gameObject.tag == "1" )
            Destroy(col.gameObject);
    }


    void OnTriggerStay( Collider col )
    {
        Debug.Log(tag + "Collided with " + col.gameObject.tag);
        if( col.gameObject.tag == "1" )
            Destroy(col.gameObject);
    }


    void OnTriggerExit( Collider col )
    {
        Debug.Log(tag + "Collided with " + col.gameObject.tag);
        if( col.gameObject.tag == "1" )
            Destroy(col.gameObject);
    }



    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination
        agent.SetDestination( points[destPoint].position );

        int nextPoint = Random.Range(0, points.Length);

        while (nextPoint == destPoint) {
            nextPoint = Random.Range(0, points.Length);
        }

        destPoint = nextPoint;
    }



    // Assumes point Array is already instantiated and just needs to be populated
    private Transform[] SetUpPointArrays ( GameObject[] objects)
    {

        Transform[] pointArray = new Transform[ objects.Length ];

        int i = 0;
        foreach (GameObject t in objects) {


            pointArray[i++] = t.transform;
        }


        return pointArray;
    }


    private Transform[] _join( Transform[] x, Transform[] y )
    {
        var z = new Transform[x.Length + y.Length];
        x.CopyTo(z, 0);
        y.CopyTo(z, x.Length);


        return z;
    }

}
