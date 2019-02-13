using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour {

    //See reference : https://www.youtube.com/watch?v=rQG9aUWarwE
    //And : https://www.youtube.com/watch?v=73Dc5JTCmKI

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public float soundRadius = 360;
    [Range(0, 100)]
    public float maxSoundLength;
       
    public LayerMask targetViewMask;
    public LayerMask targetSoundMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector]
    public List<Transform> audibleTargets = new List<Transform>();

    public float dstToTarget = 0;
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    //Patrol State
    public Animator anim;
    public static bool targetOnSight;

    //VisibleState
    public bool visible;
    public bool audible;

    public NavMeshAgent nav;
    private float pathLength = 0f;
      


    void Start() {
        anim = GetComponent<Animator>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    void Update() {
        DrawFieldOfView();              
    }


    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            FindAudibleTargets();
        }
    }


    void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetViewMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    anim.SetFloat("targetDst", dstToTarget);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                        if (target.tag == "Player") {
                            visibleTargets.Insert(0, target);
                        } /* else if (target.tag == "SoundSource") {
                            //visibleTargets.RemoveAt(1);
                            visibleTargets.Add(target);
                        }
                        */
                        visible = true;
                        anim.SetFloat("targetDst", Vector3.Distance(target.transform.position, transform.position));

                    } else {
                        visible = false;
                    }
                } else {
                    visible = false;
                }
            }
        }

    }

    void FindAudibleTargets() {
        audibleTargets.Clear();
        Collider[] targetsInSoundRadius = Physics.OverlapSphere(transform.position, soundRadius, targetSoundMask);

        for (int i = 0; i < targetsInSoundRadius.Length; i++) {
            Transform target = targetsInSoundRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            CalculatePathLength(target);

            if (pathLength < maxSoundLength) {
                
                if (target.tag == "SoundSource") {
                    //visibleTargets.RemoveAt(1);
                    visibleTargets.Add(target);
                    audible = true;
                    /*
                    Vector2 agentPosition = gameObject.transform.position;
                    Vector2 targetPosition = target.position;

                    float angle = AngleBetweenAgentTarget(agentPosition, targetPosition);
                    transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                    */
                } else {
                    audible = false;
                }

            } else {
                audible = false;
            }
        }
    }


    void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }


        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount-1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2) { 
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }




    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);

    }


    ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else {
            return new ViewCastInfo(false, transform.position + dir*viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }

    }

    float CalculatePathLength(Transform target) {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled) {
            nav.CalculatePath(target.position, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length+2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = target.position;

        for (int i = 0; i <path.corners.Length; i++) {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;
        for(int i=0; i<allWayPoints.Length-1; i++) {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }
       
        return pathLength;
    }

    float AngleBetweenAgentTarget (Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}
