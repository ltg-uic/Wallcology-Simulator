using UnityEngine;
using System.Collections;


// Forage state is almost exclusively for Herbivores

public class ForageState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    float forageTime;
    float forageDuration;
    float minWait = 15f;
    float maxWait = 30f;


    public ForageState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        SetDurations();
    }

    public void UpdateState()
    {
        Look();
        Forage();
    }

    // Keep aware of predators
    public void OnTriggerEnter (Collider other)
    {
        if  ( other.gameObject.CompareTag( "Resource" ) ) {
            ;
        }

        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log("" + critter.ID.ToString() + " We've run into a predator: " + predator.ID.ToString() );
            HandlePredator(predator);

        }
    }

    public void OnTriggerStay(Collider other) {}

    // Watch for predators
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
                    // Debug.Log( "ForageState " + critter.ID.ToString() + " RUUUUUNN!!! " + predator.ID.ToString() );
                    ToFlightState(predator);
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
        SetDurations();
        critter.navMeshAgent.Resume();
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        SetDurations();
        critter.currentState = critter.idleState;
    }

    public void ToFlightState(StatePatternCritter predator)
    {
        SetDurations();
        critter.predator = predator;
        critter.currentState = critter.flightState;

    }

    public void ToPursuitState(StatePatternCritter prey) { }

    public void ToForageState() {}
    // public void ToPursueState() {}


    private void Forage()
    {
        // Wait until weve reached boundary
        critter.meshRendererFlag.material.color = Color.magenta;
        if  (critter.navMeshAgent.remainingDistance <= critter.navMeshAgent.stoppingDistance)
        {
            // Keep track of time at feeding site
            forageTime += Time.deltaTime;
            if (forageTime >= forageDuration) {
                float rand = Random.value;
                // Debug.Log("" + critter.ID.ToString() + " We be eating!! " + rand.ToString());

                // Stop moving
                critter.navMeshAgent.Stop();

                // Go idle for a bit
                if ( rand < 0.2f )
                {
                    ToIdleState();
                }

            }

        }
    }


}