using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FrameMovement : MonoBehaviour
{
    public float stability = 0.3f;
    [FormerlySerializedAs("speed")] public float speedStab = 1f;
    private GameObject[] _rotors;
    private float _maxSpeed = 400f;
    public bool targetAcquired = false;
    public int step;
    public Vector3 target;

    private GameObject tour;
    
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        tour = GameObject.Find("Tour");
        if (_rotors == null)
        {
            _rotors = GameObject.FindGameObjectsWithTag("Rotor");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        step = tour.GetComponent<Mission>().step;
        if (target != Vector3.zero)
        {
            float distance = Vector3.Distance(transform.position, target);
            if (distance > 5f)
            {
                Vector3 dif = target - transform.position;
                Vector3 direction = _rigidbody.velocity;
                dif.y = 0;
                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(dif);
                }
                _rigidbody.position = new Vector3(transform.position.x, 10, transform.position.z);
                _rigidbody.MovePosition(transform.position + dif.normalized * _maxSpeed * Time.deltaTime);
                targetAcquired = false;
            }
            else if (distance > 1f && distance < 5f)
            {
                Vector3 dif = target - transform.position;
                Vector3 direction = _rigidbody.velocity;
                dif.y = 0;
                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(dif);
                }
                _rigidbody.position = new Vector3(transform.position.x, 10, transform.position.z);
                _rigidbody.MovePosition(transform.position + dif.normalized * 1 * Time.deltaTime);
                targetAcquired = false;
            }
            else
            {
                targetAcquired = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (step == 2){
            String tagAttribute = other.tag;
            int priority=0;
            
            if (other.CompareTag("Low"))
                priority = 1;
            else if (other.CompareTag("Medium"))
                priority = 2;
            else if (other.CompareTag("High"))
                priority = 3;
            else if (other.CompareTag("VHigh"))
                priority = 4;

            if(priority != 0) 
            { 
                Tuple <Vector3, int> tuple = new Tuple<Vector3, int>(other.transform.position, priority);
                tour.SendMessage("addBuildings", tuple);
            }
        }
    }

    private void Update()
    {
        
    }
}
