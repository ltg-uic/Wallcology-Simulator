using UnityEngine;
using System.Collections;


// N States
// Hunt
// Pursue
// Idle
//
public class Predator : Critter {
    protected float Vigilance = 1f;
    protected float Speed = 1.5f;
    protected float Acceleration = 2f;
    protected bool isIdle = false;
    protected GameObject Prey;  // The targeted prey
    protected Vector3 LastPosition;
    protected bool TargetAcquired = false;


	// Use this for initialization
	void Start ()
    {
        // agent.destination = Vector3();
        points = SetUpPointArrays(GameObject.FindGameObjectsWithTag("POIPipe"));

    }

    // Update is called once per frame
    void Update ()
    {
        if  ( isIdle ) {
            // Idle();
        }
        // else if (isHunting) {}
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


    void Pursuing()
    {
        if ( Prey.transform.position == transform.position ){}
    }

}


// public class EnemyAI : MonoBehaviour
// {
//     public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
//     public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
//     public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
//     public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
//     public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.


//     private EnemySight enemySight;                          // Reference to the EnemySight script.
//     private NavMeshAgent nav;                               // Reference to the nav mesh agent.
//     private Transform player;                               // Reference to the player's transform.
//     private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
//     private LastPlayerSighting lastPlayerSighting;          // Reference to the last global sighting of the player.
//     private float chaseTimer;                               // A timer for the chaseWaitTime.
//     private float patrolTimer;                              // A timer for the patrolWaitTime.
//     private int wayPointIndex;                              // A counter for the way point array.


//     void Awake ()
//     {
//         // Setting up the references.
//         enemySight = GetComponent<EnemySight>();
//         nav = GetComponent<NavMeshAgent>();
//         player = GameObject.FindGameObjectWithTag(Tags.player).transform;
//         playerHealth = player.GetComponent<PlayerHealth>();
//         lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
//     }


//     void Update ()
//     {
//         // If the player is in sight and is alive...
//         if(enemySight.playerInSight && playerHealth.health > 0f)
//             // ... shoot.
//             Shooting();

//         // If the player has been sighted and isn't dead...
//         else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
//             // ... chase.
//             Chasing();

//         // Otherwise...
//         else
//             // ... patrol.
//             Patrolling();
//     }


//     void Shooting ()
//     {
//         // Stop the enemy where it is.
//         nav.Stop();
//     }


//     void Chasing ()
//     {
//         // Create a vector from the enemy to the last sighting of the player.
//         Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

//         // If the the last personal sighting of the player is not close...
//         if(sightingDeltaPos.sqrMagnitude > 4f)
//             // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
//             nav.destination = enemySight.personalLastSighting;

//         // Set the appropriate speed for the NavMeshAgent.
//         nav.speed = chaseSpeed;

//         // If near the last personal sighting...
//         if(nav.remainingDistance < nav.stoppingDistance)
//         {
//             // ... increment the timer.
//             chaseTimer += Time.deltaTime;

//             // If the timer exceeds the wait time...
//             if(chaseTimer >= chaseWaitTime)
//             {
//                 // ... reset last global sighting, the last personal sighting and the timer.
//                 lastPlayerSighting.position = lastPlayerSighting.resetPosition;
//                 enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
//                 chaseTimer = 0f;
//             }
//         }
//         else
//             // If not near the last sighting personal sighting of the player, reset the timer.
//             chaseTimer = 0f;
//     }


//     void Patrolling ()
//     {
//         // Set an appropriate speed for the NavMeshAgent.
//         nav.speed = patrolSpeed;

//         // If near the next waypoint or there is no destination...
//         if(nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
//         {
//             // ... increment the timer.
//             patrolTimer += Time.deltaTime;

//             // If the timer exceeds the wait time...
//             if(patrolTimer >= patrolWaitTime)
//             {
//                 // ... increment the wayPointIndex.
//                 if(wayPointIndex == patrolWayPoints.Length - 1)
//                     wayPointIndex = 0;
//                 else
//                     wayPointIndex++;

//                 // Reset the timer.
//                 patrolTimer = 0;
//             }
//         }
//         else
//             // If not near a destination, reset the timer.
//             patrolTimer = 0;

//         // Set the destination to the patrolWayPoint.
//         nav.destination = patrolWayPoints[wayPointIndex].position;
//     }
// }