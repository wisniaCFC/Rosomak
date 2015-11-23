using UnityEngine;
using System.Collections;

public class Nadwozie : MonoBehaviour
{
    float zoomUp = 20;
    float zoomRight = 40;
    float zoomForward = 20;
    float velocity = 0.1f;
    float maxVelocity = 1;

    float Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            if (velocity < maxVelocity)
            {
                velocity = value;
            }
        }
    }

	void Start ()
    {
	
	}
	
	void FixedUpdate ()
    {
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector2.zero;
        
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y + 2, 0);
            Velocity += 0.01f;
            transform.position += transform.right * Velocity;
            transform.FindChild("koło").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (1)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (2)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (3)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (4)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (5)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (6)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (7)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y - 2, 0);
            Velocity += 0.01f;
            transform.position += transform.right * Velocity;
            transform.FindChild("koło").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (1)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (2)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (3)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (4)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (5)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (6)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (7)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y + 2, 0);
            transform.position -= transform.right;
            transform.FindChild("koło").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (1)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (2)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (3)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (4)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (5)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (6)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (7)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y - 2, 0);
            transform.position -= transform.right;
            transform.FindChild("koło").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (1)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (2)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (3)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (4)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (5)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (6)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (7)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Velocity += 0.01f;
            transform.position += transform.right* Velocity;
            transform.FindChild("koło").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (1)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (2)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (3)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (4)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (5)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (6)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
            transform.FindChild("koło (7)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z - 10);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.right;
            transform.FindChild("koło").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (1)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (2)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (3)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (4)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (5)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (6)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
            transform.FindChild("koło (7)").eulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.FindChild("koło (4)").eulerAngles.z + 10);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y - 2, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y + 2, 0);
        }

        if(Input.GetKey(KeyCode.W))
        {
            zoomRight--;
        }
        else if(Input.GetKey(KeyCode.S))
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

        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            velocity = 0.1f;
        }

        Camera.main.transform.position = transform.position - transform.right*zoomRight + transform.up*zoomUp - transform.forward * zoomForward;
        Camera.main.transform.LookAt(transform);

        
    }
}
