using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move the rotors depending on the input using add force
        

        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(transform.up*2);
        }
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            if(hit.distance< 1)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 1.5f);
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
        }

    }
}
