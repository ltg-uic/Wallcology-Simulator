using UnityEngine;

public class IdleState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    float idleTime;
    float idleDuration;
    float minWait = 3f;
    float maxWait = 8f;


    public IdleState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        SetDurations();
    }

    private void Idle()
    {
        idleTime += Time.deltaTime;

        if ( idleTime >= idleDuration)
        {
            ToWanderState();
        }
    }

    public void UpdateState() {
        Look();
        Idle();
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

    public void ToWanderState()
    {
        // Debug.Log(" Its time to wander! " + critter.ID.ToString() );

        idleTime = 0f;
        critter.navMeshAgent.Resume();
        critter.currentState = critter.wanderState;
    }


    public void ToIdleState() {}
    public void ToForageState() {}
    // public void ToPursueState() {}
    public void ToFlightState() {}

    public void HandleResource( Collider plant ) { }


    private void SetDurations()
    {
        idleTime = 0;
        idleDuration = Random.Range(minWait, maxWait);
    }

}