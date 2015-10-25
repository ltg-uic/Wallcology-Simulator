using UnityEngine;
using System.Collections;

public class ExitState: ICritterState
{
    private readonly StatePatternCritter critter;

    public ExitState(StatePatternCritter activeCritter)
    {
        critter = activeCritter;
    }

    public void UpdateState()
    {
        HeadOut();
    }

    public void OnTriggerEnter (Collider other) {}

    public void OnTriggerStay(Collider other) {}

    public void Look() {}

    public void ToWanderState() {}

    public void ToIdleState() {}

    public void ToForageState() {}

    public void ToPursuitState(StatePatternCritter prey) {}

    public void ToFlightState(StatePatternCritter predator) {}

    public void ToEnterState() {}

    public void ToExitState() {}

    private void HeadOut()
    {
        // Wait until weve reached boundary
        // critter.meshRendererFlag.material.color = Color.magenta;
        if  (critter.navMeshAgent.remainingDistance <= critter.navMeshAgent.stoppingDistance)
        {
            // Stop moving
            critter.navMeshAgent.Stop();
            critter.prey = null;
            Object.Destroy(critter.gameObject);
        }
    }
}