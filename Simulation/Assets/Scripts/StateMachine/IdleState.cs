using UnityEngine;

public class IdleState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    float idleTime;
    float idleDuration;
    float minWait = 30f;
    float maxWait = 60f;


    public IdleState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        SetDurations();
    }


    public void UpdateState() {
        Look();
        Idle();
    }


    public void OnTriggerEnter (Collider other) {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();
            Debug.Log( " A predator hit us!: " + predator.ID.ToString() );

        } else if ( other.gameObject.CompareTag( "Herbivore" ) ) {

            StatePatternCritter herbivore = other.gameObject.GetComponent<StatePatternCritter>();
            Debug.Log( " A Herbivore hit us!: " + herbivore.ID.ToString() );

        } else if ( other.gameObject.CompareTag( "Resource" ) ) {
            Debug.Log( " We're stuck in a bush!: "+ other.gameObject.name );
        }
    }

    private void Idle()
    {
        idleTime += Time.deltaTime;

        if ( idleTime >= idleDuration)
        {
            ToWanderState();
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
                Debug.Log( " We've spotted a predator: " + predator.ID.ToString() );

            } else if ( sighted.CompareTag("Herbivore") ) {

                StatePatternCritter herbivore = sighted.GetComponent<StatePatternCritter>();
                Debug.Log( " We've spotted a herbivore: " + herbivore.ID.ToString() );

            } else if ( sighted.CompareTag("Resource") ) {

                Debug.Log( " We've spotted a Bush: "+ sighted.name );

            }

        }
    }

    public void ToWanderState() {
        Debug.Log(" Its time to wander! " + critter.ID.ToString() );

        idleTime = 0f;
        critter.navMeshAgent.Resume();
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState() {

    }

    public void HandlePredator( StatePatternCritter predator ) { }

    public void HandleHerbivore( StatePatternCritter herbivore ) { }

    public void HandleResource( GameObject plant ) { }

    private void SetDurations()
    {
        idleTime = 0;
        idleDuration = Random.Range(minWait, maxWait);
    }

}