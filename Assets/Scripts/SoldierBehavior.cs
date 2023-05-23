using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehavior : MonoBehaviour
{
    private GameObject Character;
    public Transform player;
    private ArmyManager army_Script;
    public float followDistance = 3.0f;
    public float smoothTime = 0.3f;
    public GameObject Totem;
    public DrawSpline DSpline_Script;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Rigidbody _rigidbody;

    public GenericClass.E_Action _actionState;

    //Enemy Things
    private GameObject[] targets;        // Tableau contenant tous les PNJ cibles
    public GameObject currentTarget;    // Référence au PNJ cible actuel
    public Statistique currentTargetStatistique;    // Référence au PNJ cible actuel
    public string targetTag = "Enemy"; // Tag du PNJ cible

    private bool isAttacking = false;    // Indique si l'ennemi est en train d'attaquer

    public bool SelfIdle;
    public bool FlagSelfIdle;

    public GenericClass.E_Zone _zoneAttribute = GenericClass.E_Zone.Back;

    private Statistique Stat_Script;
    private AnimationManager Anim_Script;


    private void Start()
    {
        DSpline_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DrawSpline>();

        Anim_Script = GetComponent<AnimationManager>();
        Stat_Script = GetComponent<Statistique>();

        Character = GameObject.FindGameObjectWithTag("LocalPlayer");
        player = Character.transform;
        army_Script = Character.GetComponent<ArmyManager>();
        //_actionState = GenericClass.E_Action.Follow;
        SelfIdle = false;
    }

    private void FixedUpdate()
    {
        if (SelfIdle && SelfIdle != FlagSelfIdle)
        {
            FlagSelfIdle = SelfIdle;
            Anim_Script._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);
        }

        else if (!SelfIdle && SelfIdle != FlagSelfIdle)
        {
            FlagSelfIdle = SelfIdle;
            Anim_Script._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Run);

        }
    }



    private void Update()
    {
        float distance;
        switch (_actionState)
        {
            case GenericClass.E_Action.Wait:
                #region Wait
                // Si on n'a pas de cible actuelle on trouve la plus proche et on vérrifi qu'elle est à porté de detection
                if (currentTarget == null)
                {
                    #region Trouve le PNJ le plus proche et le met dans currentTarget
                    
                    targets = GameObject.FindGameObjectsWithTag(targetTag);
                    float minDistance = Mathf.Infinity;
                    foreach (GameObject target in targets)
                    {
                        distance = Vector3.Distance(target.transform.position, transform.position);
                        if (distance < minDistance)
                        {
                            if (distance < Stat_Script.detectDistance)
                            {
                                currentTarget = target;
                                currentTargetStatistique = currentTarget.GetComponent<Statistique>();
                                minDistance = distance;
                            }

                        }
                    }
                    #endregion
                }

                // Si on a une cible actuelle
                else if (currentTarget != null)
                {
                    #region Si on a un target on lui fonce dessus, et à distance d'attaque on attaque
                    // Calcule la distance entre l'ennemi et la cible
                    distance = Vector3.Distance(currentTarget.transform.position, transform.position);
                    // Si la cible n'est pas à portée d'attaque
                    if (distance >= Stat_Script.attackDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, Stat_Script.speed * Time.deltaTime);
                        Vector3 direction = currentTarget.transform.position - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    //Sinon
                    else
                    {
                        Vector3 direction = currentTarget.transform.position - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction);
                        if (currentTarget.transform.tag == "MyMonster")
                        {
                            currentTarget = null;
                            SetAllIdle(false);
                        }
                        else if (!isAttacking)
                        {

                            if (currentTarget.transform.tag == "Enemy")
                            {
                                SelfIdle = true;
                                isAttacking = true;
                                StartCoroutine(ResetBoolAfterDelay());
                                currentTargetStatistique.TakeDamage(Mathf.RoundToInt(10 + Stat_Script.damage * GetComponent<Statistique>()._healthbar.fillAmount), _actionState);
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                break;
            case GenericClass.E_Action.Follow:
                #region Follow

                switch (_zoneAttribute)
                {
                    case GenericClass.E_Zone.Back:
                        player = army_Script.ZonesBackMiddle.transform;
                        break;
                    case GenericClass.E_Zone.Left:
                        player = army_Script.ZonesLeft.transform;
                        break;
                    case GenericClass.E_Zone.Right:
                        player = army_Script.ZonesRight.transform;
                        break;
                    case GenericClass.E_Zone.FrontLeft:
                        player = army_Script.ZonesFrtLeft.transform;
                        break;
                    case GenericClass.E_Zone.FrontRight:
                        player = army_Script.ZonesFrtRight.transform;
                        break;
                    case GenericClass.E_Zone.Totem:
                        player = Totem.transform;
                        break;

                }
                // Calcule la distance entre le PNJ et le joueur
                distance = Vector3.Distance(player.position, transform.position);
                // Si la distance est supérieure à la distance de suivi souhaitée
                if (distance > followDistance && !SelfIdle)
                {
                    // Calcule la direction vers le joueur
                    Vector3 direction = player.position - transform.position;
                    direction.y = 0f;
                    direction.Normalize();
                    // Calcule la nouvelle position du PNJ
                    Vector3 targetPosition = player.position - direction * followDistance;
                    // Déplace le PNJ vers la nouvelle position de manière fluide
                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, Stat_Script.speed);
                    // Oriente le PNJ dans la direction de déplacement
                    if (direction != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                }
                else
                {
                    SelfIdle = true;
                }
                #endregion
                break;
            case GenericClass.E_Action.Attack:
                #region Attack

                // Si on n'a pas de cible actuelle et qu'on n'est pas déjà en train d'attaquer
                if (currentTarget == null)
                {
                    #region Follow
                    switch (_zoneAttribute)
                    {
                        case GenericClass.E_Zone.Back:
                            player = army_Script.ZonesBackMiddle.transform;
                            break;
                        case GenericClass.E_Zone.Left:
                            player = army_Script.ZonesLeft.transform;
                            break;
                        case GenericClass.E_Zone.Right:
                            player = army_Script.ZonesRight.transform;
                            break;
                        case GenericClass.E_Zone.FrontLeft:
                            player = army_Script.ZonesFrtLeft.transform;
                            break;
                        case GenericClass.E_Zone.FrontRight:
                            player = army_Script.ZonesFrtRight.transform;
                            break;
                        case GenericClass.E_Zone.Line:
                            player = Character.GetComponent<PlayerController>().LineFollower.transform;
                            break;
                        case GenericClass.E_Zone.Totem:
                            player = Totem.transform;
                            break;
                    }

                    // Calcule la distance entre le PNJ et le joueur
                    distance = Vector3.Distance(player.position, transform.position);
                    // Si la distance est supérieure à la distance de suivi souhaitée
                    if (distance > followDistance && !SelfIdle)
                    {
                        // Calcule la direction vers le joueur
                        Vector3 direction = player.position - transform.position;
                        direction.y = 0f;
                        direction.Normalize();
                        // Calcule la nouvelle position du PNJ
                        Vector3 targetPosition = player.position - direction * followDistance;
                        // Déplace le PNJ vers la nouvelle position de manière fluide
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, Stat_Script.speed);
                        // Oriente le PNJ dans la direction de déplacement
                        if (direction != Vector3.zero)
                        {
                            transform.rotation = Quaternion.LookRotation(direction);
                        }
                    }
                    else
                    {
                        if (_zoneAttribute == GenericClass.E_Zone.Line)
                        {

                        }
                        else
                        {
                            SelfIdle = true;
                        }
                    }
                    #endregion

                    // Trouve le PNJ le plus proche


                    targets = GameObject.FindGameObjectsWithTag(targetTag);
                    float minDistance = Mathf.Infinity;
                    foreach (GameObject target in targets)
                    {
                        distance = Vector3.Distance(target.transform.position, transform.position);
                        if (distance < minDistance)
                        {
                            if (distance < Stat_Script.detectDistance)
                            {
                                currentTarget = target;
                                currentTargetStatistique = currentTarget.GetComponent<Statistique>();
                                minDistance = distance;
                            }

                        }
                    }
                }

                // Si on a une cible actuelle
                else if (currentTarget != null)
                {
                    #region Si on a un target on lui fonce dessus, et à distance d'attaque on attaque
                    // Calcule la distance entre l'ennemi et la cible
                    distance = Vector3.Distance(currentTarget.transform.position, transform.position);
                    // Si la cible n'est pas à portée d'attaque
                    if (distance >= Stat_Script.attackDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, Stat_Script.speed * Time.deltaTime);
                        Vector3 direction = currentTarget.transform.position - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    //Sinon
                    else
                    {
                        Vector3 direction = currentTarget.transform.position - transform.position;
                        transform.rotation = Quaternion.LookRotation(direction);
                        if (currentTarget.transform.tag == "MyMonster")
                        {
                            currentTarget = null;
                            SetAllIdle(false);
                        }
                        else if (!isAttacking)
                        {

                            if (currentTarget.transform.tag == "Enemy")
                            {
                                SelfIdle = true;
                                isAttacking = true;
                                StartCoroutine(ResetBoolAfterDelay());
                                currentTargetStatistique.TakeDamage(Mathf.RoundToInt(10 + Stat_Script.damage * GetComponent<Statistique>()._healthbar.fillAmount), _actionState);
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                break;
        }


    }

    IEnumerator ResetBoolAfterDelay()
    {
        #region Coroutine d'attaque
        Anim_Script._animator.SetBool("Attack", true);
        yield return new WaitForSeconds(Stat_Script.attackSpeed); // wait for 1 second
        isAttacking = false; // set your bool to false
        Anim_Script._animator.SetBool("Attack", false);
        #endregion
    }
    

    private void OnCollisionStay(Collision collision)
    {
        #region Quand le monstre est en contact d'un allié Idle il devient Idle
        if (collision.transform.tag == "MyMonster")
        {
            if (collision.transform.gameObject.GetComponent<SoldierBehavior>().SelfIdle == true)
            {
                if(collision.transform.gameObject.GetComponent<SoldierBehavior>()._zoneAttribute == _zoneAttribute)
                {
                    if(currentTarget == null)
                        SelfIdle = true;
                }
            }
        }
        #endregion
    }
    private void OnTriggerStay(Collider other)
    {
        #region Quand le monstre est en contact de sa zone il devient Idle

        if (other.transform.tag == "zone")
        {
            if (other.transform.gameObject.GetComponent<ZoneInfos>().zone == _zoneAttribute)
            {
                if (currentTarget == null)
                    SelfIdle = true;
            }
        }
        #endregion
    }



    private void SetAllIdle(bool isIdle)
    {
        targets = GameObject.FindGameObjectsWithTag("MyMonster");
        foreach (GameObject target in targets)
        {
            SoldierBehavior soldier = target.GetComponent<SoldierBehavior>();
            if (soldier.currentTarget == null && soldier._actionState == GenericClass.E_Action.Attack)
            {
                soldier.SelfIdle = isIdle;
            }
        }
    }


    /*private void OnMouseDown()
    {
        if(tag == "MyMonster")
        {
            DSpline_Script.points.Clear();
            DSpline_Script.readyToDraw = true;
            DSpline_Script.ZoneChoosed = GetComponent<SoldierBehavior>()._zoneAttribute;
            Character.GetComponent<PlayerController>().LineFollower.transform.position = transform.position;
            //if (!Character.GetComponent<PlayerController>().ManagerActivate)
            //{
            //    DSpline_Script.UiPannel.SetActive(true);
            //}
        }

    }

    private void OnMouseUp()
    {
        if (tag == "MyMonster")
        {
            DSpline_Script.readyToDraw = false;
            //DSpline_Script.ZoneChoosed = GenericClass.E_Zone.Back;
        }
    }*/
}
