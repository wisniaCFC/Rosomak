using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public GameObject target;
	public float rotateSpeed = 5;
	public float zoomSpeed = 1;

	private Vector3 offset;
	private float horizontal = 0;
	private float vertical = 0;
	private float zoom = 1;

	//private float minHor = -180;
	//private float maxHor = 180;
	private float minVer = -30;
	private float maxVer = 0;
	private float minZoom = 1;
	private float maxZoom = 2;

	void Start ()
	{
		offset = target.transform.position - transform.position;
	}
	
	void LateUpdate ()
	{
		horizontal += Input.GetAxis("Mouse X") * rotateSpeed;
		vertical += Input.GetAxis("Mouse Y") * rotateSpeed;
		zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

		//horizontal = Mathf.Clamp(horizontal, minHor, maxHor);
		vertical = Mathf.Clamp(vertical, minVer, maxVer);
		zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

		float desiredAngle = target.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler(vertical, horizontal, 0);
		transform.position = target.transform.position - (rotation * offset * zoom);

		transform.LookAt(target.transform);
	}
}
