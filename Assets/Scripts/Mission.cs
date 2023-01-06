using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mission : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] drones;
    public GameObject[] dronesRelais;
    public int step = 1;

    private Vector3 positionMission;
    private Vector3 mapSize;
    
    private bool allTargetAcquired;
    private bool waiting = false;
    
    private float targetx;
    private float targetz;
    private float indexPos;
    private float targetzInit;
    private float scanX;
    private int nbBackupDrone;
    private float distanceToMission;
    private Vector3 edgePos;

    public Dictionary<Vector3, int> buildings;

    private List<Vector3> startPosition;

    void Start()
    {
        buildings = new Dictionary<Vector3, int>();
        startPosition = new List<Vector3>();
        nbBackupDrone = 0;

        // Get all drones
        drones = GameObject.FindGameObjectsWithTag("Drone");
        dronesRelais = GameObject.FindGameObjectsWithTag("DroneRelais");

        // Stock start position of the drones
        foreach(GameObject drone in drones)
            startPosition.Add(drone.GetComponent<Transform>().position);
        
        // Generate position mission and set size
        positionMission = new Vector3(Random.Range(-1000f, 1000f), 5, Random.Range(-1000f, 1000f));
        mapSize = new Vector3(1200, 5, 1200);

        // Create position to edges of mission area
        targetx = positionMission.x + mapSize.x / 2;
        targetz = positionMission.z - mapSize.z / 2;
        indexPos = mapSize.z / drones.Length;
        targetzInit = targetz + (indexPos / 2);
        edgePos = new Vector3(targetx, 136, targetzInit+900);

        // Create position to scan the area
        scanX = targetx - mapSize.x;

        // Get the number of backup drone needed for mission
        distanceToMission = Vector3.Distance(transform.position, edgePos);

        for (int i = 0; i < distanceToMission; i += 300)
            nbBackupDrone++;
        print("nbback : " + nbBackupDrone);
    }

    public void addBuildings(Tuple<Vector3,int> building)
    {
        if(!buildings.ContainsKey(building.Item1))
            buildings.Add(building.Item1, building.Item2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!waiting)
        {
            switch (step)
            {
                case 1:
                    //Step 1 : position drone to edges of the area
                    for (int i = 0; i < drones.Length; i++)
                    {
                        drones[i].GetComponent<FrameMovement>().target = new Vector3(targetx, 136, targetzInit);
                        targetzInit += indexPos;
                    }

                    for (int i = 0; i < nbBackupDrone; i++)
                    {
                        Vector3 _direction = (edgePos - transform.position).normalized;
                        Vector3 direction = new Vector3(_direction.x, 0, _direction.z);

                        dronesRelais[i].GetComponent<Transform>().position += direction * 300 * i;
                    }

                    waiting = true;
                    break;

                case 2:
                    //Step 2 : Scan the area
                    for (int i = 0; i < drones.Length; i++)
                    {
                        float posZ = drones[i].GetComponent<Transform>().position.z;
                        drones[i].GetComponent<FrameMovement>().target = new Vector3(scanX, 136, posZ);
                    }
                    waiting = true;
                    break;

                case 3:

                    List<Vector3> targets = computeTarget();

                    for (int i = 0; i < drones.Length; i++)
                        drones[i].GetComponent<FrameMovement>().target = targets[i];

                    //DEBUG
                    foreach (KeyValuePair<Vector3, int> element in buildings)
                    {
                        Debug.Log("Count : " + buildings.Count + "\n"
                            + "dictionary Key   (position) : " + element.Key + "\n"
                            + "dictionary Value (priority) : " + element.Value);
                    }

                    waiting = true;
                    break;
                case 4:
                    Invoke("goBackInPlace", 10);
                    waiting = true;
                    break;
            }
        }
        else
        {
            // Verification for each drone
            allTargetAcquired = true;
            foreach(GameObject drone in drones)
            {
                if (drone.GetComponent<FrameMovement>().targetAcquired == false)
                    allTargetAcquired = false;
            }

            // If all target are up then go to step after
            if(allTargetAcquired)
            {
                waiting = false;
                step++;
            }
        }
    }

    private List<Vector3> computeTarget()
    {
        List<Vector3> targets = new List<Vector3>();
        float radius = drones[0].GetComponent<SphereCollider>().radius;
        print(radius);
        double sphereArea = Math.PI * radius * radius;
        double missionArea = mapSize.x * mapSize.z;

        print(" Sphere + drone : " + sphereArea * drones.Length);
        print(" mission area : " + missionArea);

        if (sphereArea * drones.Length >= missionArea)
        {
        }
        else
        {
            Debug.LogWarning("Not enough coverage for all mission area");
        }
        // Works only with 4 drone
        targets.Add(new Vector3(positionMission.x + mapSize.x/4, 136, positionMission.z + mapSize.z/4));
        targets.Add(new Vector3(positionMission.x - mapSize.x/4, 136, positionMission.z + mapSize.z/4));
        targets.Add(new Vector3(positionMission.x + mapSize.x/4, 136, positionMission.z - mapSize.z/4));
        targets.Add(new Vector3(positionMission.x - mapSize.x/4, 136, positionMission.z - mapSize.z/4));

        return targets;
    }

    private void goBackInPlace()
    {
        for(int i = 0; i < drones.Length; i++)
            drones[i].GetComponent<FrameMovement>().target = startPosition[i];
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(positionMission, mapSize);
    }
}
