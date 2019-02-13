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
    [SerializeField] float investigateTime = 1.0f;
    [SerializeField] float approachTime = 3.0f;
    [SerializeField] float suspectTime = 5.0f;
    //See EditorGUI 
    public static bool typePatrol = true;

    public List<Transform> visibleTargets = new List<Transform>();

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
        visibleTargets = gameObject.GetComponent<FieldOfView>().visibleTargets;

        coroutine = _Investigate(investigateTime, anim);
        
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

    public void Suspect() {
        StartCoroutine(_Approach(approachTime, anim));
        StartCoroutine(_Suspect(suspectTime, anim));
    }

    public void Investigate() {
        StartCoroutine(_Investigate(investigateTime, anim));
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

    IEnumerator _Approach(float ApproachTime, Animator animator) {
        entityAgent.destination = visibleTargets[0].transform.position;
        yield return new WaitForSeconds(ApproachTime);
    }

    IEnumerator _Suspect(float SuspectTime, Animator animator) {
        entityAgent.isStopped = true;
        yield return new WaitForSeconds(SuspectTime);
        entityAgent.isStopped = false;
    }


}
 
      