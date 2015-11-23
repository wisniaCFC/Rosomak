using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour
{
    float vehicle_speed = 0;

    private float gravityForce = 9.81f; //[m/s^2]
    private float engine_torque = 1.97f; //[kNm] Moment obrotowy silnika o mocy 360 kW przy 2100 obr./min. 
    private float engine_power = 360; //[kW]
    private float r = 0.62f; //[m]
    private float efficiency = 0.9f; //sprawność silnika
    private float rpm = 0;
    private float mass = 100; //[kg] masa koła 14.00R20
    private float c = 0; //obwód koła [m]
    private float adhesion_coefficient = 0.9f; //współczynnik przyczepności dla asfaltu
    private float vehicle_mass = 20000; //[kg] zakres masy pojazdu to 16t - 26t
    private float drag_coefficient = 0.82f; //współczynnik oporu powietrza dla prostokąta

    private float friction_coefficient; //współczynnik tarcia koła
    private float wheel_interia;
    private float angular_velocity;
    private float engine_angular_velocity;
    private float tractive_force;
    private float wind_drag_force; //siła oporu powietrza
    private float wheel_friction;


    void Start ()
    {
        c = 2 * Mathf.PI * r;
        wheel_interia = (mass * r * r) / 2;
    }
	
	void FixedUpdate ()
    {
        Steering();
        GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -gravityForce, 0.0f), ForceMode.Acceleration);
    }

    void Steering()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rpm += 10;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rpm -= 10;
        }
    }

    float CalculateEngineTorque()
    {
        return engine_power / (2 * Mathf.PI * rpm);
    }

    float CalculateAngularVelocity(float brake_torque, float tractive_force)
    {
        float delta = (Mathf.Pow(((wheel_interia * 8 * wind_drag_force) / (vehicle_mass * r)) - engine_torque + wheel_friction * r, 2) + 4 * wheel_interia * ((engine_torque * 8 * wind_drag_force + wheel_friction * 8 * wind_drag_force) / vehicle_mass * r));

        float temp = (-((wheel_interia * 8 * wind_drag_force)/vehicle_mass*r - engine_torque + wheel_friction*r) + Mathf.Sqrt(delta)) / (2 * wheel_interia);

        if(temp < 0)
            temp = (-((wheel_interia * 8 * wind_drag_force) / vehicle_mass * r - engine_torque + wheel_friction * r) - Mathf.Sqrt(delta)) / (2 * wheel_interia);

        return temp;
        //return (engine_torque - brake_torque - (r * tractive_force) - (r * wheel_friction)) / wheel_interia;
    }

    float CalculateWheelFriction()
    {
        return (friction_coefficient / r) * ((vehicle_mass / 8) * gravityForce); 
    }

    float CalculateFrictionCoefficient()
    {
        return (tractive_force * r) / ((vehicle_mass / 8) * gravityForce);
    }

    float CalculateTractiveForce()
    {
        float wheel_slip = 0;

        engine_angular_velocity = vehicle_speed / r;

        if (angular_velocity > engine_angular_velocity)
            wheel_slip = (angular_velocity - engine_angular_velocity) / angular_velocity;
        else
            wheel_slip = (angular_velocity - engine_angular_velocity) / engine_angular_velocity;

        return wheel_slip * adhesion_coefficient;
    }

    float CalculateVehicleSpeed()
    {
        return ((8 * tractive_force) - wind_drag_force) / vehicle_mass; //8 - liczba kół
    }

    float CalculateWindDragForce()
    {
        return -drag_coefficient * vehicle_speed;
    }

    void Drive()
    {
        engine_torque = CalculateEngineTorque();
        wind_drag_force = CalculateWindDragForce();
        friction_coefficient = CalculateFrictionCoefficient();
        wheel_friction = CalculateWheelFriction();
        angular_velocity = CalculateAngularVelocity(0, CalculateTractiveForce());



    }
}
