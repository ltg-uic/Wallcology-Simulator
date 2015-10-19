using UnityEngine;
using System.Collections;

public interface ICritterState
{

	void UpdateState();

	void OnTriggerEnter (Collider other);

    void OnTriggerStay(Collider other);

    void Look();

	void ToWanderState();

    void ToIdleState();

    void ToForageState();

    void ToPursuitState(StatePatternCritter prey);

    void ToFlightState(StatePatternCritter predator);

}