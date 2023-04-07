using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joyStick;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed;

    public List<GenericClass.E_Zone> ActivesZone;

    public Image buttonCenterR;
    public Image buttonLeftR;
    public Image buttonRightR;

    public Color green;
    public Color white;

    void Start()
    {
        ActivesZone.Add(GenericClass.E_Zone.BackLeft);
        ActivesZone.Add(GenericClass.E_Zone.BackMiddle);
        ActivesZone.Add(GenericClass.E_Zone.BackRight);
        ActivesZone.Add(GenericClass.E_Zone.FrontLeft);
        ActivesZone.Add(GenericClass.E_Zone.FrontRight);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joyStick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joyStick.Vertical * _moveSpeed);
        if(_joyStick.Horizontal != 0 || _joyStick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            _animator.SetBool("isRunning", true);
            GameObject[] monsterList = GameObject.FindGameObjectsWithTag("MyMonster");
            foreach(GameObject soldier in monsterList)
            {
                soldier.GetComponent<SoldierBehavior>().SelfIdle = false;
            }
        }

        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    public void ChangeActionToAttack()
    {
        GameObject[] Army = GameObject.FindGameObjectsWithTag("MyMonster");

        foreach (GameObject obj in Army)
        {
            if (ActivesZone.Contains(obj.GetComponent<SoldierBehavior>()._zoneAttribute))
            {
                obj.GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Attack;
            }
        }
    }

    public void ChangeActionToFollow()
    {
        GameObject[] Army = GameObject.FindGameObjectsWithTag("MyMonster");

        foreach (GameObject obj in Army)
        {
            if (ActivesZone.Contains(obj.GetComponent<SoldierBehavior>()._zoneAttribute))
            {
                obj.GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Follow;
            }
        }
    }

    public void ChangeActionToWait()
    {
        GameObject[] Army = GameObject.FindGameObjectsWithTag("MyMonster");

        foreach (GameObject obj in Army)
        {
            if (ActivesZone.Contains(obj.GetComponent<SoldierBehavior>()._zoneAttribute))
            {
                obj.GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Wait;
            }
        }
    }


    public void SelectZoneCenterR()
    {
        if(ActivesZone.Contains(GenericClass.E_Zone.BackMiddle))
        {
            ActivesZone.Remove(GenericClass.E_Zone.BackMiddle);
            buttonCenterR.color = white;
        }
        else
        {
            ActivesZone.Add(GenericClass.E_Zone.BackMiddle);
            buttonCenterR.color = green;

        }
    }

    public void SelectZoneLeftR()
    {
        if (ActivesZone.Contains(GenericClass.E_Zone.BackLeft))

        {
            ActivesZone.Remove(GenericClass.E_Zone.BackLeft);

            buttonLeftR.color = white;
        }
        else
        {
            ActivesZone.Add(GenericClass.E_Zone.BackLeft);
            buttonLeftR.color = green;

        }
    }

    public void SelectZoneRightR()
    {
        if (ActivesZone.Contains(GenericClass.E_Zone.BackRight))

        {
            ActivesZone.Remove(GenericClass.E_Zone.BackRight);
            buttonRightR.color = white;
        }
        else
        {
            ActivesZone.Add(GenericClass.E_Zone.BackRight);
            buttonRightR.color = green;

        }
    }

    public void SelectZoneFront()
    {
        if (ActivesZone.Contains(GenericClass.E_Zone.BackRight))

        {
            ActivesZone.Remove(GenericClass.E_Zone.BackRight);
            buttonRightR.color = white;
        }
        else
        {
            ActivesZone.Add(GenericClass.E_Zone.BackRight);
            buttonRightR.color = green;

        }
    }

}
