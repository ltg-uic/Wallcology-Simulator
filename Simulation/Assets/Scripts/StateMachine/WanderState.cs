using UnityEngine;

public class WanderState: ICritterState
{
    private readonly StatePatternCritter critter;
    Vector3 direction;
    NavMeshHit hit;
    int MeshArea;
    float wanderDuration;
    float wanderTime;
    float minWait = 4f;
    float maxWait = 10f;

    public WanderState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;

        wanderTime = 0f;
        wanderDuration = -1f;
        MeshArea = 1 << NavMesh.GetNavMeshLayerFromName("Brick");
        // switch (critter.habitat)
        // {
        //     case 0:
        //         MeshArea = NavMesh.GetAreaFromName("Pipe");
        //         break;
        //     case 1:
        //         MeshArea = NavMesh.GetAreaFromName("Brick");
        //         break;
        //     default:
        //         MeshArea = NavMesh.GetAreaFromName("Brick");
        //         break;
        // }
        Debug.Log("" + critter.ID.ToString() + " I can walk on " + MeshArea.ToString() );
    }

    public void UpdateState()
    {
        Look();
        Wander();
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

    private void Wander()
    {
        // wanderTime += Time.deltaTime;

        // if ( wanderTime >= wanderDuration )
        // {
        if ( critter.navMeshAgent.remainingDistance <= critter.navMeshAgent.stoppingDistance && !critter.navMeshAgent.pathPending)
        {
            direction = Random.insideUnitSphere * critter.maxWalkDistance;
            direction += critter.transform.position;

            // Sample the provided Mesh Area and get the nearest point
            NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

            Vector3 position = hit.position;

            critter.navMeshAgent.SetDestination( hit.position );
            Debug.Log (" Pick a point! " + critter.ID.ToString() + position);

        //     wanderTime = 0;
        //     wanderDuration = Random.Range( minWait, maxWait );
        }
    }

    private void Look()
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



    public void ToWanderState() {}

}