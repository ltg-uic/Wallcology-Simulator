using UnityEngine;
using System.Collections;

public class objDestination : MonoBehaviour {

	// Use this for initialization
	public Transform goal;
	
	void Start () {
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.destination = goal.position; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

