using UnityEngine;
using System.Collections;

public interface ICritterState
{

	void UpdateState();

	void OnTriggerEnter (Collider other);

    void Look();

	void ToWanderState();

    void ToIdleState();

}