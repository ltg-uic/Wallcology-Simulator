using UnityEngine;

public class WanderState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    NavMeshHit hit;
    int MeshArea;
    float wanderDuration;
    float wanderTime;
    float minWait = 5f;
    float maxWait = 10f;
    float fleeingDistance = 10f;

    public WanderState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;

        SetDurations();
        MeshArea = NavMesh.GetAreaFromName("Brick") * 4; //MeshArea = 1 << NavMesh.GetNavMeshLayerFromName("Brick");

        // Debug.Log("" + critter.ID.ToString() + " I can walk on " + MeshArea.ToString() );
    }


    public void UpdateState()
    {
        Look();
        Wander();
    }


    public void OnTriggerEnter (Collider other)
    {
        if  ( other.gameObject.CompareTag( "Predator" ) ) {

            StatePatternCritter predator = other.gameObject.GetComponent<StatePatternCritter>();

            // Debug.Log( ""+critter.ID.ToString() + " We've run into a predator: " + predator.ID.ToString() );
            HandlePredator("OnTriggerEnter", predator);

        } else if ( other.gameObject.CompareTag( "Herbivore" ) ) {

            StatePatternCritter herbivore = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log( ""+critter.ID.ToString() + " We've run into a Herbivore: " + herbivore.ID.ToString() );
            HandleHerbivore(herbivore);

        } else if ( other.gameObject.CompareTag( "Resource" ) ) {

            // Debug.Log( ""+critter.ID.ToString() + " We've run into a Bush: "+ other.gameObject.name );
            HandleResource( other );

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
                // Debug.Log( ""+critter.ID.ToString() + " We've spotted a predator: " + predator.ID.ToString() );
                HandlePredator("Look", predator);

            } else if ( sighted.CompareTag("Herbivore") ) {

                StatePatternCritter herbivore = sighted.GetComponent<StatePatternCritter>();
                // Debug.Log( ""+critter.ID.ToString() + " We've spotted a herbivore: " + herbivore.ID.ToString() );
                HandleHerbivore(herbivore);

            } else if ( sighted.CompareTag("Resource") ) {

                // Debug.Log( "" + critter.ID.ToString() + " We've spotted a Bush: "+ sighted.name );
                HandleResource( hit.collider );

            }
        }
    }


    public void HandlePredator( string source, StatePatternCritter predator ) {
        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            foreach ( int predatorID in critter.predatorList )
            {
                // Debug.Log( "\tMatch? " + predator.ID.ToString() );
                if ( predator.ID == predatorID )
                {
                    // Debug.Log( source + " WS " + critter.ID.ToString() + " RUUUUUNN!!! " + predator.ID.ToString() );
                    direction = (predator.transform.position - critter.transform.position).normalized;

                    direction += critter.transform.position * fleeingDistance;

                    // Sample the provided Mesh Area and get the nearest point
                    NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

                    critter.navMeshAgent.SetDestination( hit.position );

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
                    if (Random.value < 0.01) {
                        ToPursuitState(herbivore);
                        break;
                    }   // This is where we will initiate chase;
                }
            }
        }

    }

    public void HandleResource( Collider plantColl ) {
        GameObject plant = plantColl.gameObject;
        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            // Debug.Log("Weve hit a Plant!" + critter.ID.ToString());
            foreach ( int preyID in critter.preyList )
            {
                // Debug.Log("Looking for " + preyID.ToString() + " in " + plant.name);
                if ( plant.name.Contains( preyID.ToString() ) )
                {
                    if ( Random.value < .80f )
                    {
                        // Debug.Log("" + critter.ID.ToString() + " Nomm nomm " + plant.name);
                        critter.navMeshAgent.Stop();

                        Vector3 foragePoint = plantColl.ClosestPointOnBounds(critter.transform.position);
                        critter.navMeshAgent.SetDestination(foragePoint);
                        critter.navMeshAgent.Resume();
                        ToForageState();
                    }
                }
            }
        }
    }

    public void ToWanderState()
    {
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        SetDurations();
        critter.navMeshAgent.Stop();
        critter.currentState = critter.idleState;
    }

    public void ToForageState()
    {
        SetDurations();
        critter.currentState = critter.forageState;
    }

    public void ToFlightState(StatePatternCritter predator) {
        SetDurations();
        critter.predator = predator;
        // critter.predator.meshRendererFlag.material.color = Color.red;
        critter.currentState = critter.flightState;
    }

    public void ToPursuitState(StatePatternCritter prey)
    {
        SetDurations();
        critter.prey = prey;
        critter.currentState = critter.pursuitState;
    }

    public void ToExitState()
    {
        critter.prey = null;
        critter.currentState = critter.exitState;
    }

    public void ToEnterState() {}

    private void SetDurations()
    {
        wanderTime = 0;
        wanderDuration = Random.Range(minWait, maxWait);
    }


    private void Wander()
    {
        wanderTime += Time.deltaTime;
        // critter.meshRendererFlag.material.color = Color.cyan;

        if ( critter.navMeshAgent.remainingDistance <= critter.navMeshAgent.stoppingDistance && !critter.navMeshAgent.pathPending)
        {
            direction = Random.insideUnitSphere * critter.maxWalkDistance;
            direction += critter.transform.position;

            // Sample the provided Mesh Area and get the nearest point
            NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

            critter.navMeshAgent.SetDestination( hit.position );
            // Debug.Log (" Pick a point! " + critter.ID.ToString() + position);

        }

        if ( wanderTime >= wanderDuration )
        {
            ToIdleState();
        }
    }


}