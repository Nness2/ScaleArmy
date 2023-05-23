using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    //[SerializeField] private FixedJoystick _joyStick;
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

    public bool IsAgressif;
    public bool IsRunning;
    public bool ManagerActivate;
    public bool FeedActivate;

    public CameraFollow CamFollow_Script;

    //CanvasInfo
    public GameObject CanvasInfos;
    public Image CanvasImage;
    public Text CanvasHealth;
    public Text CanvasDamage;
    public Text CanvasAtkSpeed;
    public Text CanvasLevel;

    private GameObject GM_Script;

    public GameObject LineFollower;
    public DrawSpline DSpline_Script;

    public GameObject AssaultButton;

    //public GameObject LastEnemyClicked;

    public Transform ArmyParent;

    public GameObject SelectUIAgressif;
    public GameObject SelectUIPassif;

    void Start()
    {
        //LastEnemyClicked = null;
        DSpline_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DrawSpline>();

        GM_Script = GameObject.FindGameObjectWithTag("GameManager");
        ActivesZone.Add(GenericClass.E_Zone.Back);
        ActivesZone.Add(GenericClass.E_Zone.Left);
        ActivesZone.Add(GenericClass.E_Zone.Right);
        ActivesZone.Add(GenericClass.E_Zone.FrontLeft);
        ActivesZone.Add(GenericClass.E_Zone.FrontRight);

        armyManager_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<ArmyManager>();
        IsAgressif = true;
        IsRunning = false;
        //ActivesZone.Add(GenericClass.E_Zone.FrontLeft);
        //ActivesZone.Add(GenericClass.E_Zone.FrontRight);
    }

    private void FixedUpdate()
    {
        //_rigidbody.velocity = new Vector3(_joyStick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joyStick.Vertical * _moveSpeed);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        _rigidbody.velocity = movement * _moveSpeed;

        //_rigidbody.velocity = new Vector3(horizontalInput * _moveSpeed, _rigidbody.velocity.y, verticalInput * _moveSpeed);

        //if(_joyStick.Horizontal != 0 || _joyStick.Vertical != 0)
        if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            _animator.SetBool("isRunning", true);
            IsRunning = true;
            GameObject[] monsterList = GameObject.FindGameObjectsWithTag("MyMonster");
            GM_Script.GetComponent<DrawSpline>().CleanLineRenderer();

            GM_Script.GetComponent<TaskManager>().CloseActifTaskCanvas();

            AssaultButton.SetActive(false);
            foreach (GameObject soldier in monsterList)
            {
                soldier.GetComponent<SoldierBehavior>().SelfIdle = false;
            }
            if (ManagerActivate)
            {
                ManagerActivate = false;
                armyManager_Script.ShowZones(false);
                CamFollow_Script.Zoom(false);
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
            IsRunning = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //DetectEnemyOnClick();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchManagerMode();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!IsAgressif)
            {
                ChangeActionToAttack();

            }
            else
            {
                ChangeActionToFollow();
            }
        }
    }

    public void ChangeActionToAttack()
    {
        IsAgressif = true;
        GameObject[] Army = GameObject.FindGameObjectsWithTag("MyMonster");

        foreach (GameObject obj in Army)
        {
            if (ActivesZone.Contains(obj.GetComponent<SoldierBehavior>()._zoneAttribute))
            {
                obj.GetComponent<SoldierBehavior>()._actionState = GenericClass.E_Action.Attack;
            }
        }
        SelectUIAgressif.SetActive(true);
        SelectUIPassif.SetActive(false);
    }

    public void ChangeActionToFollow()
    {
        IsAgressif = false;
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
        SelectUIAgressif.SetActive(false);
        SelectUIPassif.SetActive(true);
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
        if (!IsRunning)
        {
            ManagerActivate = !ManagerActivate;
            FeedActivate = false;
            armyManager_Script.ShowZones(ManagerActivate);
            GM_Script.GetComponent<DrawSpline>().CleanLineRenderer();

            if (ManagerActivate)
            {
                CamFollow_Script.Zoom(true);
            }
            else
            {
                CamFollow_Script.Zoom(false);
            }
        }
    }


    public void ButtonFollowLine()
    {
        SoldierBehavior[] soldiers = GameObject.FindObjectsOfType<SoldierBehavior>();

        DSpline_Script.OnNewPathCreated(DSpline_Script.points);
        DSpline_Script.isMooving = true;

        foreach (SoldierBehavior Soldier in soldiers)
        {
            if (GM_Script.GetComponent<DrawSpline>().ZoneChoosed == Soldier._zoneAttribute && Soldier.tag == "MyMonster")
            {
                Soldier._zoneAttribute = GenericClass.E_Zone.Line;
                Soldier.SelfIdle = false;

            }
        }
    }

    public void ButtonCancelLine()
    {
        GM_Script.GetComponent<DrawSpline>().CleanLineRenderer();
    }


    //Permet de garder le dernier enemy sur lequel le joueur à cliquer
    /*private void DetectEnemyOnClick()
    {
        #region Quand on clique sur un enemy on lui attribu un target 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask3 = 1 << LayerMask.NameToLayer("Monster");  // ignore tous les layers sauf "Monster"
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit, layerMask3))
        {
            if (hit.transform.tag == "Enemy")
            {
                LastEnemyClicked = hit.transform.gameObject;
                AssaultButton.SetActive(true);
            }
        }
        #endregion
    }*/

    //Permet d'envoyer à l'attaque nos monstres sur le dernier enemy sur lequel on a cliqué 
    public void LaunchAssault(GameObject monsterTarget)
    {
        SoldierBehavior[] soldiers = GameObject.FindObjectsOfType<SoldierBehavior>();
        foreach (SoldierBehavior Soldier in soldiers)
        {
            Soldier.currentTarget = monsterTarget;//LastEnemyClicked.transform.gameObject;
            Soldier.currentTargetStatistique = Soldier.currentTarget.GetComponent<Statistique>(); // on réccupére également ses information


            Soldier.SelfIdle = false;
        }
    }
}


