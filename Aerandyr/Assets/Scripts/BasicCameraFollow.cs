using UnityEngine;
using System.Collections;

public class BasicCameraFollow : MonoBehaviour 
{
	private Vector3 startingPosition;
	public Transform followTarget;
	private Vector3 targetPos;
	public float moveSpeed;
	
	void Start()
	{
		startingPosition = transform.position;
		var camera = GetComponent<Camera>();
		if(camera != null)
		{
			camera.transparencySortMode = TransparencySortMode.CustomAxis;
			camera.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
		}
	}

	void Update () 
	{
		if(followTarget != null)
		{
			targetPos = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
			Vector3 velocity = (targetPos - transform.position) * moveSpeed;
			transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);
		}
	}
}

