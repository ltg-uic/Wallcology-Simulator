using UnityEngine;
using System.Collections;

public class PursuitState: ICritterState
{
    private readonly StatePatternCritter critter;
    NavMeshHit hit;
    Vector3 direction;
    float fleeingDistance = 5f;
    int MeshArea;
    float pursuitTime;
    float pursuitDuration;


    public PursuitState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        MeshArea = 1 << NavMesh.GetNavMeshLayerFromName("Brick");
        // NavMesh.GetAreaFromName("Brick")
    }

    public void UpdateState()
    {
        Look();
        Pursue();
    }

    public void OnTriggerEnter (Collider hit)
    {
       if ( hit.gameObject.CompareTag("Herbivore") )
        {
            Debug.Log("Triggered! " + critter.ID.ToString() + " Ive target acquired!");
            StatePatternCritter prey = hit.gameObject.GetComponent<StatePatternCritter>();
            critter.prey = prey;
        }
    }


    public void Look()
    {
        RaycastHit hit;
        // Vector3 preyToTarget = (critter.prey.transform.position + critter.offset) - critter.eyes.transform.position;
        if ( Physics.Raycast (critter.eyes.transform.position, critter.eyes.transform.forward, out hit, critter.sightRange -5f) )
        {
            if ( hit.collider.CompareTag("Herbivore") )
            {
                Debug.Log("Look! " + critter.ID.ToString() + " Target Acquired!");
                StatePatternCritter prey = hit.collider.gameObject.GetComponent<StatePatternCritter>();
                critter.prey = prey;
            }
            else
            {
                Debug.Log("" + critter.ID.ToString() + " Ive lost sight of him! " + hit.collider.tag);
                // ToIdleState();
            }
        }
    }


    public void HandleHerbivore( StatePatternCritter herbivore ) {}

    public void HandleResource( Collider plant ) {}


    public void ToWanderState()
    {
        critter.currentState = critter.wanderState;
    }

    public void ToIdleState()
    {
        critter.currentState = critter.idleState;
    }

    public void ToForageState()
    {
        critter.currentState = critter.forageState;
    }

    public void ToFlightState(StatePatternCritter predator) {}  // We are predators, we do not flee!

    public void ToPursuitState(StatePatternCritter prey) {}  // Already pursuing you fool!


    private void SetToAvoid( )
    {
        Debug.Log(""+ critter.ID.ToString() + "RUUUUUNN!!!! " + critter.predator.ID.ToString());
        // direction = (critter.predator.transform.position - critter.transform.position).normalized;
        direction = (critter.transform.position - critter.predator.transform.position).normalized;
        Vector3 destination = critter.transform.position + direction * fleeingDistance;

        // Sample the provided Mesh Area and get the nearest point
        NavMesh.SamplePosition( destination, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

        critter.navMeshAgent.SetDestination( hit.position );
    }


    private void Pursue()
    {
        if (critter.prey != null)
        {
            Debug.Log("In Pursuit of " + critter.prey.gameObject.GetComponent<StatePatternCritter>().ID.ToString() + " !!!");
            critter.meshRendererFlag.material.color = Color.red;
            NavMesh.SamplePosition( critter.prey.transform.position, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );
            critter.navMeshAgent.SetDestination(hit.position);
            critter.navMeshAgent.Resume();

            float distance = Vector3.Distance(critter.transform.position, critter.prey.transform.position);

            if ( distance < 1f )
            {
                GameObject prey = critter.prey.gameObject;
                critter.prey = null;

                // prey.GetComponent<NavMeshAgent>().Stop();
                Debug.Log("Kill IT!!!");
                Object.Destroy(prey);

                ToIdleState();
            } else if ( distance >= fleeingDistance )
            {
                Debug.Log("Target Out of Range");
                ToIdleState();
            }
        } else {
            ToIdleState();
        }
    }



}