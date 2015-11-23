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

    float zoomUp = 20;
    float zoomRight = 40;
    float zoomForward = 20;

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

    //public float throttle;
    //private float max_wheel_torque;
    //private float wheels_rpm;
    //private float engine_rpm;
    //private int gear;

    //private float engine_force = 360; //moc silnika - 360 [kW]
    //private float drag_coefficient = 0.82f; //współczynnik oporu powietrza dla prostokąta
    //private float rolling_resistance_coefficient = 0.9f; //współczynnik oporu toczenia
    //private float mass = 20000; //masa pojazdu - 20 000 [kg]
    //private float engine_efficiency = 0.7f;
    //private float wheel_mass = 100;
    //private float r = 0.62f;

    //private float speed = 0;
    //private float wheel_angular_acceleration;
    //private float wheel_angular_velocity;
    //private float engine_torque;
    //private float wheel_torque;
    //private float breaking_torque = 0;
    //private float total_torque;
    //private float rotation_speed;
    //private Vector3 velocity = Vector3.zero;
    //private Vector3 tractive_force;
    //private Vector3 drag_force;
    //private Vector3 rolling_resistance;
    //private Vector3 longitudinal_force;
    //private Vector3 acceleration;
    private Vector3 car_position;

    void Start ()
    {
        car_position = transform.position;
        gear = 1;
	}
	
	void FixedUpdate ()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Calculations();
            if (throttle < 1f)
                throttle += 0.01f;
            else
                throttle = 1f;            
        }
        else if(Input.GetKey(KeyCode.DownArrow))
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

            //wheel_angular_velocity = 0;
        }
        gaz.text = "gaz = " + throttle.ToString();

        Calc();
        RotateWheels();

        UpdateCamera();
	}

    void UpdateCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            zoomRight--;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            zoomRight++;
        }

        if (Input.GetKey(KeyCode.A))
        {
            zoomForward--;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            zoomForward++;
        }

        Camera.main.transform.position = transform.position - transform.right * zoomRight + transform.up * zoomUp - transform.forward * zoomForward;
        Camera.main.transform.LookAt(transform);
    }

    //void Calculations()
    //{
    //    tractive_force = CalculateTractiveForce();
    //    drag_force = CalculateDragForce();
    //    rolling_resistance = CalculateRollingResistance();
    //    longitudinal_force = CalculateLongitudinalForce();
    //    acceleration = CalculateAcceleration();
    //    velocity = CalculateVelocity();
    //    car_position = CalculateCarPosition();
    //    transform.position = car_position;
    //    Debug.Log(transform.right);
    //    Debug.Log("tractive: " + tractive_force);
    //    Debug.Log("force: " + longitudinal_force);
    //    Debug.Log("acceleration: " + acceleration);
    //    Debug.Log("velocity: " + velocity);
    //    Debug.Log("position: " + car_position);
    //}

    float CalculateTractiveForce()
    {
        return (engine_torque * efficiency * GetGearRatio(gear) * differential_ratio) / wheel_radius;
        //return transform.right * engine_force;
    }

    float CalculateTractionTorque()
    {
        return tractive_force * wheel_radius;
    }

    //Vector3 CalculateDragForce()
    //{
    //    speed = Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2) + Mathf.Pow(velocity.z, 2));
    //    Vector3 _dragForce = new Vector3();
    //    _dragForce.x = -drag_coefficient * velocity.x * speed;
    //    _dragForce.y = -drag_coefficient * velocity.y * speed;
    //    _dragForce.z = -drag_coefficient * velocity.z * speed;
    //    return _dragForce;
    //}

    //Vector3 CalculateRollingResistance()
    //{
    //    return -rolling_resistance_coefficient * velocity;
    //}

    //Vector3 CalculateLongitudinalForce()
    //{
    //    return tractive_force + drag_force + rolling_resistance;
    //}

    //Vector3 CalculateAcceleration()
    //{
    //    return longitudinal_force / mass;
    //}

    //Vector3 CalculateVelocity()
    //{
    //    return velocity + Time.fixedDeltaTime * acceleration;
    //}

    //Vector3 CalculateCarPosition()
    //{
    //    return car_position + Time.fixedDeltaTime * velocity;
    //}

    ///////// NOWE OBLICZENIA

    //float CalculateTractiveForce()
    //{
    //    return transform.right * engine_force;
    //}

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
        return angular_acceleration * Time.fixedDeltaTime;
        //return speed / r;
    }

    float CalculateSpeed()
    {
        return angular_velocity * wheel_radius;
        //return 2 * Mathf.PI * r * wheel_angular_velocity;
    }

    Vector3 CalculateVelocity()
    {
        return transform.right * speed;
    }

    float CalculateWheelRotation()
    {
        return angular_velocity * 30 / Mathf.PI;
    }

    //float CalculateRotationSpeed()
    //{
    //    //return wheel_angular_velocity / (Mathf.PI * 2);
    //    //return speed / r;
    //}

    //float CalculateEngineRPMs()
    //{
    //    //return rotation_speed * GetGearRatio(gear) * 3.42f * GetGearRatio(gear) * 60 / (Mathf.PI * 2);
    //}

    //float CalculateWheelsRPMs()
    //{
    //    //return speed / r;
    //}

    Vector3 CalculateCarPosition()
    {
        return car_position + Time.fixedDeltaTime * velocity;
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

        //max_wheel_torque = GetMaxWheelTorque(gear);
        //engine_torque = CalculateEngineTorque();
        //wheel_torque = CalculateWheelTorque();
        //total_torque = CalculateTotalTorque();
        //wheel_angular_acceleration = CalculateAngularAcceleration();
        //wheel_angular_velocity = CalculateAngularVelocity();
        //speed = CalculateSpeed();
        //velocity = CalculateVelocity();
        car_position = CalculateCarPosition();
        transform.position = car_position;
        //rotation_speed = CalculateRotationSpeed();
        //engine_rpm = CalculateEngineRPMs();
        //if (engine_rpm < 1000)
        //    engine_rpm = 1000;

        //wheels_rpm = CalculateWheelsRPMs();
        //if (wheels_rpm > 700)
        //    wheels_rpm = 700;


        Mkmax.text = "Mkmax = " + max_torque.ToString();
        Ms.text = "Ms = " + engine_torque.ToString();
        Mk.text = "Mk = " + drive_torque.ToString();
        M.text = "M = " + total_torque.ToString();
        E.text = "E = " + angular_acceleration.ToString();
        w.text = "w = " + angular_velocity.ToString();
        v.text = "v = " + speed.ToString();

        //Mkmax.text = "Mkmax = " + max_wheel_torque.ToString();
        //Ms.text = "Ms = " + engine_torque.ToString();
        //Mk.text = "Mk = " + wheel_torque.ToString();
        //M.text = "M = " + total_torque.ToString();
        //E.text = "E = " + wheel_angular_acceleration.ToString();
        //w.text = "w = " + wheel_angular_velocity.ToString();
        //v.text = "v = " + speed.ToString();
        //vec_v.text = "vec v = " + velocity.ToString();
        //rpm.text = "engine rpm = " + engine_rpm.ToString();
    }

    void RotateWheels()
    {
        foreach(GameObject wheel in wheels)
        {
            wheel.transform.Rotate(new Vector3(0, 0, -wheel_rotation*Time.fixedDeltaTime));
        }
    }

    float GetGearRatio(int gear)
    {
        switch(gear)
        {
            case 1:
                return 2.97f;
                break;
            case 2:
                return 2.07f;
                break;
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
