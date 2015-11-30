using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Physics : MonoBehaviour
{
	public Text Mkmax;
	public Text Ms;
	public Text Mk;
	public Text M;
	public Text E;
	public Text w;
	public Text v;
	public Text vec_v;
	public Text gaz;
	public Text rpm;
	public List<GameObject> wheels;
	public GameObject emptos;

	float zoomUp = 20;
	float zoomRight = 40;
	float zoomForward = 20;
	private float sensitivity = -5f;
	private float minFov = 1f;
	private float maxFov = 15f;
	private Vector3 previousCarPos;

	private float speed;
	private float angular_velocity;
	private float angular_acceleration;
	private float total_torque;
	private float drive_torque;
	private float breaking_torque = 0;
	private float traction_torque;
	private float engine_torque;
	private float max_torque;
	private float gear_ratio;
	private float differential_ratio = 3.42f;
	private float wheel_mass = 100;
	private float wheel_radius = 0.62f;
	private float efficiency = 0.7f;
	private float tractive_force;
	private float engine_force = 360;
	private float throttle = 0;
	private int gear = 1;
	private Vector3 velocity;
	private float wheels_rpm;
	private float wheel_rotation;
	private Vector3 car_position;
	private float engine_rpm;

	Vector3 offset;

	void Start()
	{
		car_position = transform.position;
		gear = 1;
		Camera.main.transform.position = transform.position - 5 * transform.right + 20 * transform.up;
		offset = emptos.transform.position - transform.position;
	}

	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if (throttle < 1f)
				throttle += 0.01f;
			else
				throttle = 1f;
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if (throttle > -0.3f)
				throttle -= 0.01f;
			else
				throttle = -0.3f;
		}
		else
		{
			if (throttle > 0.01f)
			{
				throttle -= 0.01f;
			}
			else if (throttle < -0.01f)
			{
				throttle += 0.01f;
			}
			else
				throttle = 0.0f;
		}

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			transform.Rotate(-Vector3.up);
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.up);
		}


		//gaz.text = "gaz = " + throttle.ToString();

		GetComponent<Rigidbody>().angularVelocity = Vector2.zero;

		Calc();
		RotateWheels();

		previousCarPos = transform.position;
	}	

	float CalculateTractiveForce()
	{
		return (engine_torque * efficiency * GetGearRatio(gear) * differential_ratio) / wheel_radius;
	}

	float CalculateTractionTorque()
	{
		return tractive_force * wheel_radius;
	}

	float CalculateEngineTorque()
	{
		return throttle * max_torque;
	}

	float CalculateDriveTorque()
	{
		return engine_torque * GetGearRatio(gear) * 3.42f * efficiency;
	}

	float CalculateTotalTorque()
	{
		return engine_torque + 8 * drive_torque + breaking_torque;
	}

	float CalculateAngularAcceleration()
	{
		return (2 * total_torque) / (wheel_mass * wheel_radius * wheel_radius);
	}

	float CalculateAngularVelocity()
	{
		float temp = 0;
		//if (transform.eulerAngles.z > 300 && transform.eulerAngles.z < 359)
		//	temp = angular_acceleration * Time.fixedDeltaTime * 1/(360 - transform.eulerAngles.z);
		//else if (transform.eulerAngles.z < 100 && transform.eulerAngles.z > 1)
		//	temp = angular_acceleration * Time.fixedDeltaTime * (1/-transform.eulerAngles.z);
		//else
			temp = angular_acceleration * Time.fixedDeltaTime;
        return temp;
	}

	float CalculateSpeed()
	{
		return angular_velocity * wheel_radius;
	}

	Vector3 CalculateVelocity()
	{
		return transform.right * speed;
	}

	float CalculateWheelRotation()
	{
		return angular_velocity * 30 / Mathf.PI;
	}

	Vector3 CalculateCarPosition()
	{
		return car_position + Time.fixedDeltaTime * velocity;
	}

	void MoveCar()
	{
		float mass = GetComponent<Rigidbody>().mass;
		GetComponent<Rigidbody>().velocity = velocity;
		GetComponent<Rigidbody>().AddForce(-transform.up * mass * 90 * 9.81f);

		//emptos.transform.position = transform.position - transform.localPosition;// + offset;
		//emptos.transform.eulerAngles = transform.eulerAngles;
		//GetComponent<Rigidbody>().AddForce(new Vector3(0, -9.81f * mass * 90, 0));
		//if (transform.eulerAngles.z > 300)
		//	GetComponent<Rigidbody>().AddForce(transform.right * (360 - transform.eulerAngles.z));
		//else if(transform.eulerAngles.z < 100)
		//	GetComponent<Rigidbody>().AddForce(transform.right * -transform.eulerAngles.z * mass * 100);
	}

	void Calc()
	{
		max_torque = GetMaxWheelTorque(gear);
		engine_torque = CalculateEngineTorque();
		tractive_force = CalculateTractiveForce();
		drive_torque = CalculateDriveTorque();
		total_torque = CalculateTotalTorque();
		angular_acceleration = CalculateAngularAcceleration();
		angular_velocity = CalculateAngularVelocity();
		speed = CalculateSpeed();
		velocity = CalculateVelocity();
		wheel_rotation = CalculateWheelRotation();

		MoveCar();

		/*Mkmax.text = "Mkmax = " + max_torque.ToString();
		Ms.text = "Ms = " + engine_torque.ToString();
		Mk.text = "Mk = " + drive_torque.ToString();
		M.text = "M = " + total_torque.ToString();
		E.text = "E = " + angular_acceleration.ToString();
		w.text = "w = " + angular_velocity.ToString();
		v.text = "v = " + Mathf.Abs(speed).ToString();
		vec_v.text = "vec v = " + GetComponent<Rigidbody>().velocity;

		if (angular_velocity < 0.01f && angular_velocity > -0.01f)
		{
			w.text = "w = 0";
			v.text = "v = 0";
		}*/
	}

	void RotateWheels()
	{
		foreach (GameObject wheel in wheels)
		{
			wheel.transform.Rotate(new Vector3(0, 0, -wheel_rotation * Time.fixedDeltaTime));
		}
	}

	float GetGearRatio(int gear)
	{
		switch (gear)
		{
			case 1:
				return 2.97f;
			case 2:
				return 2.07f;
			case 3:
				return 1.43f;
				break;
			case 4:
				return 1.0f;
				break;
			case 5:
				return 0.84f;
				break;
			case 6:
				return 0.56f;
			case -1:
				return -3.38f;
				break;
			default:
				return 0;
		}
	}

	float GetMaxWheelTorque(int gear)
	{
		switch (gear)
		{
			case 1:
				if (wheels_rpm == 0)
					return 3500;
				else if (wheels_rpm == 500)
					return 4300;
				else if (wheels_rpm == 700)
					return 3500;
				else if (wheels_rpm > 0 && wheels_rpm < 500)
					return 3500 + 2 * wheels_rpm;
				else if (wheels_rpm > 500 && wheels_rpm < 700)
					return 4300 - 4 * (wheels_rpm - 500);
				else
					return 0;
				break;

			default:
				return 0;
		}
	}
}
