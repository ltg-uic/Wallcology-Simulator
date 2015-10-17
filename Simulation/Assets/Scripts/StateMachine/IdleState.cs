using UnityEngine;

public class IdleState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    float idleTime = 0f;


    public IdleState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;

    }


    public void UpdateState() {
        Look();
        Idle();
    }


    public void OnTriggerEnter (Collider other) {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            Debug.Log( " We've struck a predator: " + predator.ID.ToString() );

        } else if ( other.gameObject.CompareTag( "Herbivore" ) ) {

            StatePatternCritter herbivore = other.gameObject.GetComponent<StatePatternCritter>();
            Debug.Log( " We've struck a Herbivore: " + herbivore.ID.ToString() );

        } else if ( other.gameObject.CompareTag( "Resource" ) ) {
            Debug.Log( " We've struck a Bush: "+ other.gameObject.name );
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
                Debug.Log( " We've struck a predator: " + predator.ID.ToString() );

            } else if ( sighted.CompareTag("Herbivore") ) {

                StatePatternCritter herbivore = sighted.GetComponent<StatePatternCritter>();
                Debug.Log( " We've struck a herbivore: " + herbivore.ID.ToString() );

            } else if ( sighted.CompareTag("Resource") ) {

                Debug.Log( " We've struck a Bush: "+ sighted.name );

            }

        }
    }


    public void ToWanderState() {
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState() {}

}