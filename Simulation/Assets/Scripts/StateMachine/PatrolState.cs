using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState

{
	private readonly StatePatternEnemy enemy;
	private int nextWayPoint;


	public PatrolState (StatePatternEnemy statePatternEnemy)
	{
		enemy = statePatternEnemy;
	}

	public void UpdateState()
	{
		Look ();
		Patrol ();
	}

	public void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Predator1"))
			ToAlertState ();
	}

	public void ToPatrolState()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToAlertState()
	{
		enemy.currentState = enemy.alertState;
	}

	public void ToChaseState()
	{
		enemy.currentState = enemy.chaseState;
	}

	private void Look()
	{
		RaycastHit hit;
		if (Physics.Raycast (enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag ("Predator1")) {
			enemy.chaseTarget = hit.transform;
			ToChaseState();
		}
	}

	void Patrol ()
	{
		enemy.meshRendererFlag.material.color = Color.green;
		enemy.navMeshAgent.destination = enemy.wayPoints[nextWayPoint-1].position;
		enemy.navMeshAgent.Resume ();

		if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending) {
			nextWayPoint =(nextWayPoint + 1) % enemy.wayPoints.Length;

		}


	}
}