using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour {

    public bool isReversed = false;
    public Transform[] waypoints;

    private NavMeshAgent entityAgent;
    private int destPoint = 0;
    private int side = 0;


    void OnInspectorGUI() {
        //UI of the inspector for the checkbox for Reversion Path
        GUILayout.BeginHorizontal();
        GUILayout.Label("Reversion Path", GUILayout.Width(70));
        isReversed = EditorGUILayout.Toggle(isReversed);
        GUILayout.EndHorizontal();
 
    }


    void Start() {
        entityAgent = GetComponent < NavMeshAgent>();

        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;

        ToNextWaypoint();
    }


    void ToNextWaypoint() {
        //In case of no attribuated waypoint : return
        if (waypoints.Length == 0) {
            return;
        }

        //Say to the agent to go to this currently selected waypoint
        entityAgent.destination = waypoints[destPoint].position;

        //Define the destination point of the agent
        //Cycling to start if necessary
        if (isReversed == false) {
            destPoint = (destPoint + 1) % waypoints.Length;
        }

        if (isReversed == true) {
            if (destPoint == waypoints.Length - 1) {
                side = 0;
            }

            if (destPoint == 0) {
                side = 1;
            }

            if (side == 0) {
                destPoint = destPoint - 1;
            }

            if (side == 1) {
                destPoint = destPoint + 1;
            }
            
        }
        

    }


    void Update() {
       
        //define the next destination point
        if (!entityAgent.pathPending && entityAgent.remainingDistance < 0.5f) {
            ToNextWaypoint();
        }

    }



}


/*
//Allow for a list of Waypoints
[System.Serializable]
public class Waypoints {
    //solution de scours: public Transform WaypointPosition;
    public GameObject Waypoint;
}
*/   
      