using UnityEngine;
using System.Collections;

public class FlightState: ICritterState
{
    private readonly StatePatternCritter critter;
    NavMeshHit hit;
    Vector3 direction;
    float fleeingDistance = 5f;
    int MeshArea;


    public FlightState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        MeshArea = 1 << NavMesh.GetNavMeshLayerFromName("Brick");
    }

    public void UpdateState()
    {
        Look();
        Flee();
    }

    public void OnTriggerEnter (Collider other)
    {
        // if  ( other.gameObject.CompareTag( "Predator" ) ) {

        //     StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
        //     // Debug.Log( ""+critter.ID.ToString() + " We've run into a predator: " + predator.ID.ToString() );
        //     HandlePredator(predator);

        // }
    }


    public void Look() {}


    public void HandlePredator( StatePatternCritter predator ) {
        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            foreach ( int predatorID in critter.predatorList )
            {
                if ( predator.ID == predatorID )
                {
                    Debug.Log( "FlightState " + critter.ID.ToString() + " RUUUUUNN!!! " + predator.ID.ToString() );
                    direction = (predator.transform.position - critter.transform.position).normalized;

                    direction += critter.transform.position * fleeingDistance;

                    // Sample the provided Mesh Area and get the nearest point
                    NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

                    critter.navMeshAgent.SetDestination( hit.position );
                }
            }
        }
    }

    public void HandleHerbivore( StatePatternCritter herbivore ) {}

    public void HandleResource( Collider plant ) {}


    public void ToWanderState()
    {
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        critter.currentState = critter.idleState;
    }

    public void ToForageState()
    {
        critter.currentState = critter.forageState;
    }

    public void ToFlightState(StatePatternCritter predator) {}


    public void ToPursuitState(StatePatternCritter prey) { }


    private void SetToAvoid( )
    {
        Debug.Log(""+ critter.ID.ToString() + "RUUUUUNN!!!! " + critter.enemy.ID.ToString());
        direction = (critter.enemy.transform.position - critter.transform.position).normalized;

        direction += critter.transform.position * fleeingDistance;

        // Sample the provided Mesh Area and get the nearest point
        NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

        critter.navMeshAgent.SetDestination( hit.position );
    }


    private void Flee()
    {
        critter.meshRendererFlag.material.color = Color.red;
        direction = (critter.enemy.transform.position - critter.transform.position).normalized;
        direction += critter.transform.position * fleeingDistance;

        // Sample the provided Mesh Area and get the nearest point
        // NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );
        // critter.navMeshAgent.SetDestination( hit.position );
        critter.navMeshAgent.SetDestination( direction );

        if ( Vector3.Distance(critter.transform.position, critter.enemy.transform.position) > fleeingDistance )
        {
            Debug.Log("PHEW!! " + critter.ID.ToString());
            ToWanderState();

        }

        // if ( critter.navMeshAgent.remainingDistance >= critter.navMeshAgent.stoppingDistance && !critter.navMeshAgent.pathPending)
        // {
        //     if  (Vector3.Distance(critter.transform.position, critter.enemy.transform.position) < fleeingDistance)
        //     {
        //         SetToAvoid();
        //     }
        // }
        // else
        // {
        // }

    }



}