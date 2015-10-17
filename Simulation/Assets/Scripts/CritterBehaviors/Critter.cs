using UnityEngine;
using System.Collections;

public class Critter : MonoBehaviour {

    public int ID;
    public bool timeToDie = false;
    public bool inPursuit = false;
    public bool isEating = false;
    public bool isSeeking = false;
    protected int destPoint = 0;
    protected Transform[] points;
    protected NavMeshAgent agent;


	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe"));
        Debug.Log(agent);

        switch( ID )
        {
            case 0:
            case 3:
                points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIBrick"));
                break;
            case 1:
            case 6:
                points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe"));
                break;
            case 2:
            case 7:
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
    protected Transform[] SetUpPointArrays ( GameObject[] objects)
    {

        Transform[] pointArray = new Transform[ objects.Length ];

        int i = 0;
        foreach (GameObject t in objects) {


            pointArray[i++] = t.transform;
        }


        return pointArray;
    }


    protected Transform[] _join( Transform[] x, Transform[] y )
    {
        var z = new Transform[x.Length + y.Length];
        x.CopyTo(z, 0);
        y.CopyTo(z, x.Length);


        return z;
    }

}
