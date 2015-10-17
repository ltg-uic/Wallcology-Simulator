using UnityEngine;
using System.Collections;


// Forage state is almost exclusively for Herbivores

public class ForageState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    float forageTime;
    float forageDuration;
    float minWait = 30f;
    float maxWait = 60f;


    public ForageState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        SetDurations();
    }

    private void Forage()
    {
        if  (critter.navMeshAgent.remainingDistance <= critter.navMeshAgent.stoppingDistance)
        {
            if ( Random.value < 0.20f )
            {
                ToWanderState();
            }
        }
    }

    public void UpdateState()
    {
        Look();
        Forage();
    }

    public void OnTriggerEnter (Collider other)
    {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            Debug.Log( " We've run into a predator: " + predator.ID.ToString() );
            HandlePredator(predator);

        }
    }


    public void Look()
    {
        RaycastHit hit;
        GameObject sighted;

        if ( Physics.Raycast( critter.eyes.transform.position, critter.eyes.transform.forward, out hit, critter.sightRange) ) {

            sighted = hit.collider.gameObject;

            if ( sighted.CompareTag("Predator") ) {

                StatePatternCritter predator = sighted.GetComponent<StatePatternCritter>();
                // Debug.Log( " We've spotted a predator: " + predator.ID.ToString() );
                HandlePredator(predator);

            }
        }
    }


    public void HandlePredator( StatePatternCritter predator ) {

        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            foreach ( int predatorID in critter.predatorList )
            {
                if ( predator.ID == predatorID )
                {
                    Debug.Log( " RUUUUUNN!!! " + critter.ID.ToString() );
                    break;
                }
            }
        }
    }

    public void HandleHerbivore( StatePatternCritter herbivore ) {
        if ( critter.gameObject.CompareTag("Predator") )  // Are we a Predator?
        {
            foreach ( int herbID in critter.preyList )
            {
                if ( herbivore.ID == herbID )
                {
                    Debug.Log( " DINNER!!! " + critter.ID.ToString() );
                    break;
                }
            }
        }

    }

    private void SetDurations()
    {
        forageTime = 0;
        forageDuration = Random.Range(minWait, maxWait);
    }

    public void ToWanderState()
    {
        critter.navMeshAgent.Resume();
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        critter.currentState = critter.idleState;
    }

    public void ToForageState() {}
    // public void ToPursueState() {}
    public void ToFlightState() {}

    public void HandleResource( Collider plant ) { }

}