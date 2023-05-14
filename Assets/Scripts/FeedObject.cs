using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedObject : MonoBehaviour
{
    public bool Owned = false;
    public GameObject Tofeed;

    void Start()
    {
        Tofeed = null;

    }

    void Update()
    {
        if (Owned)
        {
            if (Input.GetMouseButton(0))
            {
                #region permet de surelever le monster quand on clique dessus
                int layerMask = 1 << LayerMask.NameToLayer("Ground"); // ignore tous les layers sauf "Ground"

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 0.01f);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, layerMask))
                {
                    transform.position = new Vector3(hit.point.x, 2, hit.point.z);
                    //Debug.Log(hit.transform.name);
                }
                #endregion
            }

            #region permet de nourrir un monstre avec un autre quand on le lache dessus
            int layerMask3 = 1 << LayerMask.NameToLayer("Monster");  // ignore tous les layers sauf "Zone"
            Vector3 ray3 = transform.position;
            //Debug.DrawRay(ray2, -transform.up * 100, Color.green, 0.01f);
            RaycastHit hit3;
            if (Physics.Raycast(ray3, -transform.up * 100, out hit3, 100, layerMask3))
            {
                //transform.position = new Vector3(hit.point.x, 3, hit.point.z);
                Tofeed = hit3.transform.gameObject;
                Tofeed.GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Eat);
            }
            else
            {
                if (Tofeed != null)
                    Tofeed.GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);
                Tofeed = null;
            }
            #endregion
        }
    }
}
