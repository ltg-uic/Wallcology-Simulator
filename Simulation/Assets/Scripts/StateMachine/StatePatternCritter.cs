using UnityEngine;
using System.Collections;

public class StatePatternCritter : MonoBehaviour
{

	//public GameObject enemyLabel;                // The enemy to be aware of.

	// Predators should have a smaller Vigilance, but are faster
    public int ID;    // 0-Flappy, 1-Bally, etc
    public int habitat; // 0-Pipe, 1-Gen, 2-Brick
    public float maxWalkDistance = 200f;
	public float searchingTurnSpeed = 120f;
	public float idleDuration = 4f;
	public float sightRange = 20f;
	public Transform eyes;
    public int [] predatorList;
    public int [] preyList;

    public Vector3 offset = new Vector3 (0,.5f,0);

	[HideInInspector] public ICritterState currentState;
	[HideInInspector] public WanderState wanderState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public ForageState forageState;
    // [HideInInspector] public FlightState flightState;
	[HideInInspector] public NavMeshAgent navMeshAgent;



	private void Awake()
	{
        wanderState = new WanderState (this); // huntStart = new HuntState (this); May replace with this
        idleState = new IdleState (this); // huntStart = new HuntState (this); May replace with this
        forageState = new ForageState (this);
        // flightState = new FlightState (this);
		navMeshAgent = GetComponent<NavMeshAgent>();

	}

	// Use this for initialization
	void Start ()
	{
		currentState = wanderState;
	}


	// Update is called once per frame
	void Update ()
	{
        Debug.Log(currentState);
		currentState.UpdateState();
	}


	private void OnTriggerEnter(Collider other)
	{
		currentState.OnTriggerEnter (other);
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