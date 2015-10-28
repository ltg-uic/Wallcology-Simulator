using UnityEngine;
using System.Collections;

public class FlightState: ICritterState
{
    private readonly StatePatternCritter critter;
    NavMeshHit hit;
    // Vector3 direction;
    Vector3 destination;
    float fleeingDistance = 5f;
    int MeshArea;


    public FlightState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        MeshArea = NavMesh.GetAreaFromName("Brick") * 4;// 1 << NavMesh.GetNavMeshLayerFromName("Brick");

    }

    public void UpdateState()
    {
        Look();
        Flee();
    }

    public void OnTriggerEnter (Collider other)
    {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log( ""+critter.ID.ToString() + " We've run into a predator: " + predator.ID.ToString() );
            HandlePredator(predator);

        }
    }

    public void OnTriggerStay (Collider other)
    {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log( ""+critter.ID.ToString() + " We've run into a predator: " + predator.ID.ToString() );

            if ( predator == critter.predator ) {
                HandlePredator(predator);
            }


        }
    }


    public void Look() {}


    public void HandlePredator( StatePatternCritter predator ) {
        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            foreach ( int predatorID in critter.predatorList )
            {
                if ( predator.ID == predatorID )
                {
                    critter.predator = predator;

                    // Debug.Log( "FlightState " + critter.ID.ToString() + " RUUUUUNN!!! " + predator.ID.ToString() );
                    Vector3 direction = (critter.transform.position - critter.predator.transform.position).normalized;

                    Vector3 destination = critter.transform.position + direction * fleeingDistance;

                    // Sample the provided Mesh Area and get the nearest point
                    NavMesh.SamplePosition( destination, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

                    critter.navMeshAgent.SetDestination( hit.position );
                }
            }
        }
    }


    public void ToWanderState()
    {
        critter.navMeshAgent.avoidancePriority = Random.Range(50, 100);
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        critter.navMeshAgent.avoidancePriority = 90;
        critter.currentState = critter.idleState;
    }

    public void ToForageState()
    {
        critter.navMeshAgent.avoidancePriority = 70;
        critter.currentState = critter.forageState;
    }

    public void ToExitState()
    {
        critter.prey = null;
        critter.navMeshAgent.avoidancePriority = 0;
        critter.currentState = critter.exitState;
    }

    public void ToEnterState() {}

    public void ToFlightState(StatePatternCritter predator) {}

    public void ToPursuitState(StatePatternCritter prey) { }


    private void SetToAvoid( )
    {
        // Debug.Log(""+ critter.ID.ToString() + "RUUUUUNN!!!! " + critter.predator.ID.ToString());
        Vector3 direction = (critter.transform.position - critter.predator.transform.position).normalized;

        Vector3 destination = critter.transform.position + direction * fleeingDistance;

        // Sample the provided Mesh Area and get the nearest point
        NavMesh.SamplePosition( destination, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

        critter.navMeshAgent.SetDestination( hit.position );
    }


    private void Flee()
    {
        // critter.meshRendererFlag.material.color = Color.red;

        // Make sure the Predator being tracked is not set to Die
        if (critter.predator.currentState == critter.predator.exitState)
        {
            critter.predator = null;
            ToWanderState();
        }


        SetToAvoid();

        if ( Vector3.Distance(critter.transform.position, critter.predator.transform.position) > fleeingDistance )
        {
            // Debug.Log("PHEW!! " + critter.ID.ToString());
            ToWanderState();

        }

    }



}