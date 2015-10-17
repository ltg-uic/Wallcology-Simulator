using UnityEngine;
using System.Collections;

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
        if ( Random.value < 0.25f )
        {
            ToWanderState();
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

        } else if ( other.gameObject.CompareTag( "Herbivore" ) ) {

            StatePatternCritter herbivore = other.gameObject.GetComponent<StatePatternCritter>();
            Debug.Log( " We've run into a Herbivore: " + herbivore.ID.ToString() );
            HandleHerbivore(herbivore);

        } else if ( other.gameObject.CompareTag( "Resource" ) ) {
            Debug.Log( " We've run into a Bush: "+ other.gameObject.name );
            HandleResource( other.gameObject );
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

            } else if ( sighted.CompareTag("Herbivore") ) {

                StatePatternCritter herbivore = sighted.GetComponent<StatePatternCritter>();
                // Debug.Log( " We've spotted a herbivore: " + herbivore.ID.ToString() );
                HandleHerbivore(herbivore);

            } else if ( sighted.CompareTag("Resource") ) {

                // Debug.Log( " We've spotted a Bush: "+ sighted.name );
                HandleResource( sighted );

            }
        }
    }


    public void HandlePredator( StatePatternCritter predator )
    {

    }

    public void HandleHerbivore( StatePatternCritter herbivore )
    {

    }

    public void HandleResource( GameObject plant )
    {

    }

    private void SetDurations()
    {
        forageTime = 0;
        forageDuration = Random.Range(minWait, maxWait);
    }


    public void ToWanderState()
    {
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        critter.currentState = critter.idleState;
    }


}