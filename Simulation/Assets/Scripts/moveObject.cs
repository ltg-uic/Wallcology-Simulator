using UnityEngine;
using System.Collections;

public class moveObject : MonoBehaviour {

	// Use this for initialization
	public float radius = 10f;
	public float speed = .5f;
	public bool offsetIsCenter = true;
	public Vector3 offset;
	
	void Start()
	{
		if(offsetIsCenter)
		{
			offset = transform.position;
		}
	}
	
	void Update()
	{
		transform.position = new Vector3(
			(radius * Mathf.Cos(Time.time*speed))+offset.x,0,
			(radius * Mathf.Sin(Time.time*speed))+offset.z);

//		transform.position = new Vector3(
//			(radius * Mathf.Cos(Time.time*speed))+offset.x,
//			(radius * Mathf.Sin(Time.time*speed))+offset.z);
	}
}

