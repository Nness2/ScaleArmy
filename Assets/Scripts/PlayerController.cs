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

    public ArmyManager armyManager_Script;
    public GameObject CircleLimite;
    public Image buttonCenterR;
    public Image buttonLeftR;
    public Image buttonRightR;

    public Color green;
    public Color white;

    public bool ManagerActivate;
    public bool FeedActivate;

    public CameraFollow CamFollow_Script;

    //CanvasInfo
    public GameObject CanvasInfos;
    public Image CanvasImage;
    public Text CanvasHealth;
    public Text CanvasDamage;
    public Text CanvasAtkSpeed;

    void Start()
    {

        ActivesZone.Add(GenericClass.E_Zone.Left);
        ActivesZone.Add(GenericClass.E_Zone.Back);
        ActivesZone.Add(GenericClass.E_Zone.Right);

        armyManager_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<ArmyManager>();

        //ActivesZone.Add(GenericClass.E_Zone.FrontLeft);
        //ActivesZone.Add(GenericClass.E_Zone.FrontRight);
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
            if (ManagerActivate)
            {
                ManagerActivate = false;
                armyManager_Script.ShowZones(false);
            }
            if (CamFollow_Script.CamMovable)
            {
                CamFollow_Script.SwitchCameraMode();
            }
            if (FeedActivate)
            {
                FeedActivate = false;
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
        if (this.enabled)
        {
            GameObject[] Army = GameObject.FindGameObjectsWithTag("MyMonster");

            foreach (GameObject obj in Army)
            {
                if (ActivesZone.Contains(obj.GetComponent<SoldierBehavior>()._zoneAttribute))
                {
                    obj.GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Follow;
                    obj.GetComponent<SoldierBehavior>().SelfIdle = false;
                    obj.GetComponent<SoldierBehavior>().currentTarget = null;
                }
            }
        }
    }

    public void ChangeActionToWait()
    {
        if (this.enabled)
        {
            GameObject[] Army = GameObject.FindGameObjectsWithTag("MyMonster");

            foreach (GameObject obj in Army)
            {
                if (!ActivesZone.Contains(obj.GetComponent<SoldierBehavior>()._zoneAttribute))
                {
                    obj.GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Wait;
                }
            }
        }

    }


    public void SelectZoneCenterR()
    {
        if (this.enabled)
        {
            if (ActivesZone.Contains(GenericClass.E_Zone.Back))
            {
                ActivesZone.Remove(GenericClass.E_Zone.Back);
                buttonCenterR.color = white;
                ChangeActionToWait();

            }
            else
            {
                ActivesZone.Add(GenericClass.E_Zone.Back);
                buttonCenterR.color = green;
                ChangeActionToAttack();
            }
        }
        //ChangeActivity();
    }

    public void SelectZoneLeftR()
    {
        if (this.enabled)
        {
            if (ActivesZone.Contains(GenericClass.E_Zone.Left))

            {
                ActivesZone.Remove(GenericClass.E_Zone.Left);

                buttonLeftR.color = white;
                ChangeActionToWait();

            }
            else
            {
                ActivesZone.Add(GenericClass.E_Zone.Left);
                buttonLeftR.color = green;
                ChangeActionToAttack();
            }
        }
        //ChangeActivity();
    }

    public void SelectZoneRightR()
    {
        if (this.enabled)
        {
            if (ActivesZone.Contains(GenericClass.E_Zone.Right))

            {
                ActivesZone.Remove(GenericClass.E_Zone.Right);
                buttonRightR.color = white;
                ChangeActionToWait();

            }
            else
            {
                ActivesZone.Add(GenericClass.E_Zone.Right);
                buttonRightR.color = green;
                ChangeActionToAttack();
            }
        }
        //ChangeActivity();
    }

    public void ChangeActivity()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("MyMonster");
        foreach (GameObject Soldier in soldiers)
        {
            if (ActivesZone.Contains(Soldier.GetComponent<SoldierBehavior>()._zoneAttribute))
            {
                Soldier.GetComponent<Statistique>()._healthbar.color = green;
            }
            else
            {
                Soldier.GetComponent<Statistique>()._healthbar.color = white;
            }
        }
    }

    public void SwitchFeedMode()
    {
        FeedActivate = !FeedActivate;
        ManagerActivate = false;
        armyManager_Script.ShowZones(false);
    }

    public void SwitchManagerMode()
    {
        ManagerActivate = !ManagerActivate;
        FeedActivate = false;
        armyManager_Script.ShowZones(ManagerActivate);
    }

}
