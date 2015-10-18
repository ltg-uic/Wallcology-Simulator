using UnityEngine;
using System.Collections;

public class PursuitState: ICritterState
{
    private readonly StatePatternCritter critter;
    NavMeshHit hit;
    Vector3 direction;
    float fleeingDistance = 5f;
    int MeshArea;


    public PursuitState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
        // MeshArea = 1 << NavMesh.GetNavMeshLayerFromName("Brick");
    }

    public void UpdateState()
    {
        Look();
        Pursue();
    }

    public void OnTriggerEnter (Collider other) { }


    public void Look()
    {
        RaycastHit hit;
        Vector3 preyToTarget = (critter.prey.transform.position + critter.offset) - critter.eyes.transform.position;
        if ( Physics.Raycast (critter.eyes.transform.position, preyToTarget, out hit, critter.sightRange) )
        {
            if ( hit.collider.CompareTag("Herbivore") )
            {
                GameObject prey = hit.collider.gameObject;
                critter.prey = prey;
            }
            else
            {
                ToIdleState();
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
        Debug.Log(""+ critter.ID.ToString() + "RUUUUUNN!!!! " + critter.enemy.ID.ToString());
        direction = (critter.enemy.transform.position - critter.transform.position).normalized;

        direction += critter.transform.position * fleeingDistance;

        // Sample the provided Mesh Area and get the nearest point
        NavMesh.SamplePosition( direction, out hit, Random.Range( 0f, critter.maxWalkDistance), MeshArea  );

        critter.navMeshAgent.SetDestination( hit.position );
    }


    private void Pursue()
    {
        critter.meshRendererFlag.material.color = Color.red;
        critter.navMeshAgent.SetDestination(critter.prey.transform.position);
        critter.navMeshAgent.Resume();

        if ( Vector3.Distance(critter.transform.position, critter.prey.transform.position) < 1f )
        {
            GameObject prey = critter.prey;
            critter.prey = null;

            prey.GetComponent<NavMeshAgent>().Stop();

            Object.Destroy(prey);

            ToIdleState();
        }
    }



}