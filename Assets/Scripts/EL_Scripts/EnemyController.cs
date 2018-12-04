using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour {

    public NavMeshAgent entityAgent;
    private int destPoint = 0;
    private int side = 0;

    private GameObject target;
    private Animator anim;
    private Ray rayToTarget;
    private RaycastHit hitTarget;
    private Vector3 checkDirection;

    private Transform[] targets;
    private int priorityTarget;

    private bool targetOnSight = false;


    //See EditorGUI 
    public bool typePatrol = false;

    public bool isReversed = false;
    public Transform[] waypoints;

    

    //See how to implement scrolling menu to choose the type of entity, check these instructions: https://docs.unity3d.com/ScriptReference/EditorGUILayout.Toggle.html
    void OnInspectorGUI() {
        //UI of the inspector for the checkbox for typePatrol
        /*
        GUILayout.BeginHorizontal();
        GUILayout.Label("Patrolling Type", GUILayout.Width(70));
        typePatrol = EditorGUILayout.Toggle(typePatrol);
        GUILayout.EndHorizontal();
        */    
    
        //UI of the inspector for the checkbox for Reversion Path
        GUILayout.BeginHorizontal();
        GUILayout.Label("Reversion Path", GUILayout.Width(70));
        isReversed = EditorGUILayout.Toggle(isReversed);
        GUILayout.EndHorizontal();
        
    }


    void Start() {
        target = GameObject.FindWithTag("Player");
        anim = gameObject.GetComponent<Animator>();
        entityAgent = GetComponent<NavMeshAgent>();

        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;

        destPoint = 0;
    }


    void Update() {
        targetOnSight = FieldOfView.targetOnSight;

        if (!targetOnSight) {
            ToNextWaypoint();
            //define the next destination point
            if (!entityAgent.pathPending && entityAgent.remainingDistance < 0.5f) {
                ToNextWaypoint();
            }
        } else if (targetOnSight) {            
            ToTarget();
        }
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

    void ToTarget() {
        //entityAgent.destination = targets[priorityTarget].position;
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
      