using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    private DrawSpline DSpline_Script;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 1f;
    public bool CoroutineRun;
    public float initY;
    private AnimationManager Anim_Script;
    private PlayerController _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
        Anim_Script = GetComponent<AnimationManager>();
        CoroutineRun = false;
        initY = transform.position.y;
        DSpline_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DrawSpline>();
    }

    private void Update()
    {
        // Calcule la distance entre le PNJ et le joueur

        if(GetComponent<SoldierBehavior>()._actionState == GenericClass.E_Action.LineMove)
        {
            if (DSpline_Script.points.Count > 1)
            {
                Anim_Script._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Run);

                Vector3 pointsDirection = new Vector3(DSpline_Script.points[0].x, initY, DSpline_Script.points[0].z);
                Vector3 pointsDirectionNext = new Vector3(DSpline_Script.points[1].x, initY, DSpline_Script.points[1].z);


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
            else
            {
                Anim_Script._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);
                GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Wait;
                DSpline_Script.UiPannel.SetActive(false);
            }
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

    private void OnMouseDown()
    {
        DSpline_Script.points.Clear();
        DSpline_Script.readyToDraw = true;
        DSpline_Script.ZoneChoosed = GetComponent<SoldierBehavior>()._zoneAttribute;
        if (!_player.GetComponent<PlayerController>().ManagerActivate)
        {
            DSpline_Script.UiPannel.SetActive(true);
        }
    }

    private void OnMouseUp()
    {
        DSpline_Script.readyToDraw = false;
        //DSpline_Script.ZoneChoosed = GenericClass.E_Zone.Back;
    }


}
