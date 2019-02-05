using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;


public class EntityAI : MonoBehaviour {


    private GameObject target;
    private Animator anim;
    private Ray rayToTarget;
    private RaycastHit hitTarget;
    private Vector3 checkDirection;

    private Transform[] targets;
    private int priorityTarget;
    
    //Patrol behaviour
    public NavMeshAgent entityAgent;
    public bool isReversed = false;
    public Transform[] waypoints;

    //Investigate behaviour
    private IEnumerator coroutine;

    //See EditorGUI 
    public static bool typePatrol = true;

    
    //TEST
    private EntityClass entityT;
    [SerializeField] EntitySO eSO;
    

    //See how to implement scrolling menu to choose the type of entity, check these instructions: https://docs.unity3d.com/ScriptReference/EditorGUILayout.Toggle.html
    void OnInspectorGUI() {
        //UI of the inspector for the checkbox for typePatrol
        GUILayout.BeginHorizontal();
        GUILayout.Label("Patrolling Type", GUILayout.Width(70));
        typePatrol = EditorGUILayout.Toggle(typePatrol);
        GUILayout.EndHorizontal();
          
    
        //UI of the inspector for the checkbox for Reversion Path
        GUILayout.BeginHorizontal();
        GUILayout.Label("Reversion Path", GUILayout.Width(70));
        isReversed = EditorGUILayout.Toggle(isReversed);
        GUILayout.EndHorizontal();
        
    }


    void Start() {
        target = GameObject.FindWithTag("Player");
        anim = gameObject.GetComponent<Animator>();

        coroutine = _Investigate(2.0f, anim);
        
        if (typePatrol) { 
            anim.SetBool("typePatrol", true);
        } else if (!typePatrol) {
            anim.SetBool("typePatrol", false);
        }

        //TEST
        //entityT = new EntityClass(eSO.typeName, eSO.isPatrolling, eSO.isBig);
    }


    void Update() {
                
    }

    public void Investigate() {
        StartCoroutine(_Investigate(1.0f, anim));
    }

    IEnumerator _Investigate(float InvestigateTime, Animator animator) {


        yield return new WaitForSeconds(InvestigateTime);

        Debug.Log("Investigate enter. Does it detect target :" + gameObject.GetComponent<FieldOfView>().visible);
        if (gameObject.GetComponent<FieldOfView>().visible == true) {
            animator.SetBool("isChasing", true);
            animator.SetBool("targetVisible", true);
        } else {
            animator.SetBool("isChasing", false);
            animator.SetBool("targetVisible", false);
        }

    }


}
 
      