using UnityEngine;
using System.Collections;

public class CritterAI_Patrol : MonoBehaviour {

    public Transform[] Waypoints;
    public float Speed;
    public int curWaypoint;
    public bool onPatrol = true;
    public Vector3 Target;
    public Vector3 MoveDirection;
    public Vector3 Velocity;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        if (curWaypoint < Waypoints.Length) {
            Target = Waypoints[curWaypoint].position;
            MoveDirection = Target - transform.position;
            Velocity = GetComponent<Rigidbody>().velocity;

            if  (MoveDirection.magnitude < 1) {
                curWaypoint++;
            }
            else {
                Velocity = MoveDirection.normalized * Speed;
            }
        }
        else {
            if ( onPatrol ) {
                curWaypoint = 0;
            }
            else {
                Velocity = Vector3.zero;
            }
        }
        GetComponent<Rigidbody>().velocity = Velocity;
        transform.LookAt( Target );
	}
}
