using UnityEngine;

public class IdleState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    float idleTime;
    float idleDuration;
    float minWait = 5f;
    float maxWait = 10f;


    public IdleState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        SetDurations();
    }


    public void UpdateState() {
        Look();
        Idle();
    }


    public void OnTriggerEnter (Collider other)
    {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log( " We've run into a predator: " + predator.ID.ToString() );
            HandlePredator(predator);

        } else if ( other.gameObject.CompareTag( "Herbivore" ) ) {

            StatePatternCritter herbivore = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log( " We've run into a Herbivore: " + herbivore.ID.ToString() );
            HandleHerbivore(herbivore);

        }
    }

    public void OnTriggerStay(Collider other) {}

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
                    // Debug.Log( "IdleState " + critter.ID.ToString() + " RUUUUUNN!!! " + predator.ID.ToString() );
                    ToFlightState(predator);
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
                    // Debug.Log( " DINNER!!! " + critter.ID.ToString() );
                    if (Random.value < 0.01) {
                        ToPursuitState(herbivore);
                        break;
                    }   // This is where we will initiate chase;
                }
            }
        }

    }

    public void ToWanderState()
    {
        SetDurations();
        critter.navMeshAgent.Resume();
        critter.navMeshAgent.avoidancePriority = Random.Range(50, 100);
        critter.currentState = critter.wanderState;
    }


    public void ToIdleState() {}

    public void ToForageState()
    {
        SetDurations();
        critter.navMeshAgent.avoidancePriority = 70;
        critter.currentState = critter.forageState;
    }


    public void ToFlightState(StatePatternCritter predator)
    {
        SetDurations();
        critter.predator = predator;
        critter.navMeshAgent.avoidancePriority = 20;
        critter.currentState = critter.flightState;
    }


    public void ToPursuitState(StatePatternCritter prey)
    {
        critter.prey = prey;
        critter.navMeshAgent.avoidancePriority = 20;
        critter.currentState = critter.pursuitState;
    }

    public void ToExitState()
    {
        critter.prey = null;
        critter.navMeshAgent.avoidancePriority = 0;
        critter.currentState = critter.exitState;
    }

    public void ToEnterState() {}

    public void HandleResource( Collider plant ) { }


    private void SetDurations()
    {
        idleTime = 0;
        idleDuration = Random.Range(minWait, maxWait);
    }

    private void Idle()
    {
        // critter.meshRendererFlag.material.color = Color.yellow;
        idleTime += Time.deltaTime;

        if ( idleTime >= idleDuration)
        {
            ToWanderState();
        }
    }

}