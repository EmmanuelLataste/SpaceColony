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
    private bool inSuspect = false;

    //Patrol behaviour
    public NavMeshAgent entityAgent;
    public float beginAISpeed;
    public bool isReversed = false;
    public Transform[] waypoints;

    //Investigate behaviour
    private IEnumerator coroutine;
    [SerializeField] float investigateTime;
    [SerializeField] float approachTime;
    [SerializeField] float suspectTime;
    //See EditorGUI 
    public static bool typePatrol;

    public List<Transform> visibleTargets = new List<Transform>();
    

    //TEST
    private EntityClass entityT;
    [SerializeField] EntitySO eSO;
    [SerializeField] public GameObject linkedEntity;

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
        beginAISpeed = entityAgent.speed;
        

        //TEST
        //entityT = new EntityClass(eSO.typeName, eSO.isPatrolling, eSO.isBig);
    }


    void Update() {
        if (typePatrol)
        {
            anim.SetBool("typePatrol", true);
        }
        else if (!typePatrol)
        {
            anim.SetBool("typePatrol", false);
        }

       
    }

    public void Suspect() {
        if (inSuspect == false) {
            entityAgent.destination = visibleTargets[0].transform.position;
        }
        
        StartCoroutine(_Approach(approachTime, anim));       
    }

    public void Suspicious2()
    {
        StartCoroutine(_Suspect(suspectTime, anim, false));
    }

    public void Suspicious()
    {
        StartCoroutine(_Suspect(suspectTime, anim, true));
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
        Debug.Log("ApproachStart");

        Debug.Log(visibleTargets[0].transform.position);
        yield return new WaitForSeconds(ApproachTime);
        StartCoroutine(_Suspect(suspectTime, anim, true));       
    }

    IEnumerator _Suspect(float SuspectTime, Animator animator, bool investigate) {
        Debug.Log("SuspectStart");

        inSuspect = true;
        entityAgent.isStopped = true;
        yield return new WaitForSeconds(SuspectTime);
        entityAgent.isStopped = false;
        animator.SetBool("isInvestigating", investigate);
        animator.SetBool("targetVisible", false);
        animator.SetBool("targetAudible", false);
        animator.SetBool("isChecking", !investigate);

        Debug.Log("SuspectEnd");
        yield return null;
    }


}
 
      