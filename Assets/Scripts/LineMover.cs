using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    public DrawSpline DSpline_Script;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 1f;
    public bool CoroutineRun;
    private void Start()
    {
        CoroutineRun = false;

    }

    private void Update()
    {
        // Calcule la distance entre le PNJ et le joueur
        if(DSpline_Script.points.Count > 1)
        {
            Vector3 pointsDirection = DSpline_Script.points[0];
            Vector3 pointsDirectionNext = DSpline_Script.points[1];


            float distance = Vector3.Distance(pointsDirection, transform.position);
            // Si la cible est à portée d'attaque
            if (distance >= 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointsDirection, 6 * Time.deltaTime);
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
    }



    public IEnumerator MoveObjectOverTime(Quaternion startPosition, Quaternion endPosition, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float currentTime = startTime;
        Debug.Log("test");
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
