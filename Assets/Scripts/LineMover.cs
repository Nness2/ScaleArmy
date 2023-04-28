using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineMover : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    public DrawSpline DSpline_Script;

    private void Awake()
    {
        DSpline_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DrawSpline>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        FindObjectOfType<DrawSpline>().OnNewPathCreated += SetPoint;
    }

    private void SetPoint(IEnumerable<Vector3> points)
    {
        pathPoints = new Queue<Vector3>(points);
    }

    private void Update()
    {
        UpdatePathing();
    }

    private void UpdatePathing()
    {
        if (ShouldSetDestiontion())
        {
            navMeshAgent.SetDestination(pathPoints.Dequeue());
        }
    }

    private bool ShouldSetDestiontion()
    {
        if(pathPoints.Count == 0)
        {
            DSpline_Script.isMooving = false;
            return false;
        }
        if(navMeshAgent.hasPath == false || navMeshAgent.remainingDistance < 0.5f)
        {
            return true;
        }
        return false;
    }

}
    /*
    public class LineMover : MonoBehaviour
    {
        private DrawSpline DSpline_Script;
        private Vector3 velocity = Vector3.zero;
        public float smoothTime = 1f;
        public bool CoroutineRun;
        public float initY;
        private PlayerController _player;


        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
            CoroutineRun = false;
            initY = transform.position.y;
            DSpline_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DrawSpline>();
        }

        private void Update()
        {
            // Calcule la distance entre le PNJ et le joueur

            if (DSpline_Script.points.Count > 1)
            {

                Vector3 pointsDirection = new Vector3(DSpline_Script.points[0].x, initY, DSpline_Script.points[0].z);
                Vector3 pointsDirectionNext = new Vector3(DSpline_Script.points[1].x, initY, DSpline_Script.points[1].z);


                float distance = Vector3.Distance(pointsDirection, transform.position);
                // Si la cible est à portée d'attaque
                if (distance >= 0.1f)
                {
                    //transform.position = Vector3.MoveTowards(transform.position, pointsDirection, 6 * Time.deltaTime);

                    transform.position = Vector3.SmoothDamp(transform.position, pointsDirectionNext, ref velocity, 0.3f, 6);
                    if (!CoroutineRun)
                    {
                        CoroutineRun = true;
                        Vector3 direction = pointsDirectionNext - transform.position;
                        Quaternion angle = Quaternion.LookRotation(direction);
                        StartCoroutine(MoveObjectOverTime(transform.rotation, angle, 0.2f));
                    }

}
            else
            {

                DSpline_Script.points.RemoveAt(0);
                DSpline_Script.lineRenderer.positionCount = DSpline_Script.points.Count;
                DSpline_Script.lineRenderer.SetPositions(DSpline_Script.points.ToArray());
            }
        }
        else
        {
            //DSpline_Script.UiPannel.SetActive(false);
        }
    }



    public IEnumerator MoveObjectOverTime(Quaternion startPosition, Quaternion endPosition, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float currentTime = startTime;

        while (currentTime < endTime)
        {
            float t = (currentTime - startTime) / duration;
            transform.rotation = Quaternion.Lerp(startPosition, endPosition, t);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endPosition;
        CoroutineRun = false;
    }

}
*/