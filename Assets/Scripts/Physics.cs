using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Physics : MonoBehaviour
{

	#region próba1

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

	void Start()
	{
		car_position = transform.position;
		gear = 1;
		Camera.main.transform.position = transform.position - 5 * transform.right + 20 * transform.up;
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


		gaz.text = "gaz = " + throttle.ToString();

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
		//foreach (GameObject wheel in wheels)
		//{
		//	float mass = wheel.GetComponent<Rigidbody>().mass;
		//	wheel.GetComponent<Rigidbody>().velocity = velocity;
		//	wheel.GetComponent<Rigidbody>().angularVelocity = new Vector3(angular_velocity, 0, 0);
		//	wheel.GetComponent<Rigidbody>().AddForce(-transform.up * mass * 90 * 9.81f);
		//}

		//transform.position = new Vector3(wheels[0].transform.position.x - 2.68f, wheels[0].transform.position.y + 2.67f, wheels[0].transform.position.z + 3);

		float mass = GetComponent<Rigidbody>().mass;
		GetComponent<Rigidbody>().velocity = velocity;
		GetComponent<Rigidbody>().AddForce(new Vector3(0, -9.81f * mass * 90, 0));
		if (transform.eulerAngles.z > 300)
			GetComponent<Rigidbody>().AddForce(transform.right * (360 - transform.eulerAngles.z));
		else if (transform.eulerAngles.z < 100)
			GetComponent<Rigidbody>().AddForce(transform.right * -transform.eulerAngles.z * mass * 100);
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

		Mkmax.text = "Mkmax = " + max_torque.ToString();
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
		}
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

	#endregion
	//////////////////////////////////////////////////////////////////////////////////////////
	#region próba 2

	float wheelbase; //rozstaw osi [m]
	float b; //odległość środka masy do przedniej osi [m]
	float c; //odległóść środka masy do tylnej osi [m]
	float h; //odległość (wysokość) środka masy od ziemi [m]
	float vehicle_mass; //masa pojazdu [kg]
	float interia; //moment bezwładności [kg*m^2]
	float length, width; //długość i szerokość pojazdu [m]
	float wheel_length, wheel_width; //długość i szerokość koła [m]

	Vector3 car_velocity;
	Vector3 world_velocity; //(?)
	Vector3 car_acceleration;
	Vector3 new_car_position;

	float angle; //kąt obrotu pojazdu [rad]
	float wheel_angular_velocity; //prędkość kołowa kół
	float wheel_angular_acceleration;

	float steer_angle; //kąt obrotu (input)
	float car_throttle; //gaz (input)
	float car_brake; //hamulec (input)

	float rotation_angle;
	float side_slip;
	float slip_angle_front;
	float slip_angle_rear;

	Vector3 force;
	bool rear_slip;
	bool front_slip;
	Vector3 resistance;
	float torque;
	float sinus, cosinus;
	float yaw_speed;
	float weight;
	Vector3 traction_force;
	Vector3 lateral_force_front, lateral_force_rear;

	float cornering_stiffness_front = -5.0f;
	float cornering_stiffness_rear = -5.2f;
	float rolling_resistance_coefficient = 30.0f;
	float drag_coefficient = 5.0f;
	float max_friction = 2.0f;

	float CalulateSinus()
	{
		return Mathf.Sin(angle);
	}

	float CalculateCosinus()
	{
		return Mathf.Cos(angle);
	}

	Vector3 CalculateWorldVelocity()
	{
		return new Vector3(cosinus * car_velocity.z, 0, sinus * car_velocity.x);
	}

	float CalculateYawSpeed()
	{
		return wheelbase * 0.5f * angular_velocity;
	}

	float CalculateRotationAngle()
	{
		if (world_velocity.x == 0)
			return 0;
		else
			return Mathf.Atan2(yaw_speed, world_velocity.x);
	}

	float CalculateSideSlip()
	{
		if (world_velocity.x == 0)
			return 0;
		else
			return Mathf.Atan2(world_velocity.z, world_velocity.x);
	}

	float CalculateFrontSlipAngle()
	{
		return side_slip + rotation_angle - steer_angle;
	}

	float CalculateRearSlipAngle()
	{
		return side_slip - rotation_angle;
	}

	float CalculateWeight()
	{
		return vehicle_mass * 9.81f * 0.25f; //ciężar na każdej osi
	}

	Vector3 CalculateLateralForceFront()
	{
		float tempZ = cornering_stiffness_front * slip_angle_front;
		tempZ = Mathf.Min(max_friction, tempZ);
		tempZ = Mathf.Max(-max_friction, tempZ);
		tempZ *= weight;
		if (front_slip)
			tempZ *= 0.5f;

		return new Vector3(0, 0, tempZ);
	}

	Vector3 CalculateLateralForceRear()
	{
		float tempZ = cornering_stiffness_rear * slip_angle_rear;
		tempZ = Mathf.Min(max_friction, tempZ);
		tempZ = Mathf.Max(-max_friction, tempZ);
		tempZ *= weight;
		if (rear_slip)
			tempZ *= 0.5f;

		return new Vector3(0, 0, tempZ);
	}

	Vector3 CalculateTractionForce()
	{
		if (rear_slip)
			return new Vector3(0.5f * 100 * (throttle - car_brake * Mathf.Sign(world_velocity.x)), 0, 0);
		else
			return new Vector3(100 * (throttle - car_brake * Mathf.Sign(world_velocity.x)), 0, 0);
	}

	Vector3 CalculateResistance()
	{
		return new Vector3(-(rolling_resistance_coefficient * world_velocity.x + drag_coefficient * world_velocity.x*Mathf.Abs(world_velocity.x)), 0, -(rolling_resistance_coefficient * world_velocity.z + drag_coefficient*world_velocity.z*Mathf.Abs(world_velocity.z)));
	}

	//Vector3 CalculateNetForce()
	//{
	//	float tempX, tempZ;

	//	tempX = traction_force.x + Mathf.Sin(steer_angle) * 
	//	return new Vector3(traction_force.x + )
	//}



	#endregion
}
