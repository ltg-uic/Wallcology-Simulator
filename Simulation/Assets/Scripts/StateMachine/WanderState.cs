using UnityEngine;

public class WanderState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    NavMeshHit hit;
    int MeshArea;
    float wanderDuration;
    float wanderTime;
    float minWait = 30f;
    float maxWait = 60f;


    public WanderState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;

        SetDurations();
        MeshArea = 1 << NavMesh.GetNavMeshLayerFromName("Brick");

        Debug.Log("" + critter.ID.ToString() + " I can walk on " + MeshArea.ToString() );
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
            HandlePredator(predator);

        } else if ( other.gameObject.CompareTag( "Herbivore" ) ) {

            StatePatternCritter herbivore = other.gameObject.GetComponent<StatePatternCritter>();
            // Debug.Log( ""+critter.ID.ToString() + " We've run into a Herbivore: " + herbivore.ID.ToString() );
            HandleHerbivore(herbivore);

        } else if ( other.gameObject.CompareTag( "Resource" ) ) {

            Debug.Log( ""+critter.ID.ToString() + " We've run into a Bush: "+ other.gameObject.name );
            HandleResource( other );

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
                // Debug.Log( ""+critter.ID.ToString() + " We've spotted a predator: " + predator.ID.ToString() );
                HandlePredator(predator);

            } else if ( sighted.CompareTag("Herbivore") ) {

                StatePatternCritter herbivore = sighted.GetComponent<StatePatternCritter>();
                // Debug.Log( ""+critter.ID.ToString() + " We've spotted a herbivore: " + herbivore.ID.ToString() );
                HandleHerbivore(herbivore);

            } else if ( sighted.CompareTag("Resource") ) {

                Debug.Log( " We've spotted a Bush: "+ sighted.name );
                HandleResource( hit.collider );

            }
        }
    }


    private void Wander()
    {
        wanderTime += Time.deltaTime;

        if ( critter.navMeshAgent.remainingDistance <= critter.navMeshAgent.stoppingDistance && !critter.navMeshAgent.pathPending)
        {
            direction = Random.insideUnitSphere * critter.maxWalkDistance;
            direction += critter.transform.position;

            // Sample the provided Mesh Area and get the nearest point
            NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

            Vector3 position = hit.position;

            critter.navMeshAgent.SetDestination( hit.position );
            // Debug.Log (" Pick a point! " + critter.ID.ToString() + position);

        }

        if ( wanderTime >= wanderDuration )
        {
            ToIdleState();
        }
    }


    public void HandlePredator( StatePatternCritter predator ) {
        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            foreach ( int predatorID in critter.predatorList )
            {
                if ( predator.ID == predatorID )
                {
                    Debug.Log( "" + critter.ID.ToString() + " RUUUUUNN!!! " + predator.ID.ToString() );
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
                    Debug.Log( "" + critter.ID.ToString() + " DINNER!!! " + herbivore.ID.ToString() );
                    break;
                }
            }
        }

    }

    public void HandleResource( Collider plantColl ) {
        GameObject plant = plantColl.gameObject;
        if ( critter.gameObject.CompareTag("Herbivore") )  // Are we a Herbivore?
        {
            Debug.Log("Weve hit a Plant!" + critter.ID.ToString());
            foreach ( int preyID in critter.preyList )
            {
                Debug.Log("Looking for " + preyID.ToString() + " in " + plant.name);
                if ( plant.name.Contains( preyID.ToString() ) )
                {
                    if ( Random.value < .80f )
                    {
                        Debug.Log("" + critter.ID.ToString() + " Nomm nomm " + plant.name);
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


    public void ToIdleState()
    {
        SetDurations();
        // Debug.Log( " Stopping!! " + critter.ID.ToString());
        critter.navMeshAgent.Stop();
        critter.currentState = critter.idleState;
    }

    public void ToForageState()
    {
        Debug.Log( " Omm Nomm Nomm! " + critter.ID.ToString());
        SetDurations();
        critter.currentState = critter.forageState;
    }

    // public void ToPursueState() {}
    public void ToWanderState() {}
    public void ToFlightState() {}

    private void SetDurations()
    {
        wanderTime = 0;
        wanderDuration = Random.Range(minWait, maxWait);
    }

}