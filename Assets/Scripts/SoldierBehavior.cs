using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehavior : MonoBehaviour
{
    private GameObject Character;
    private Transform player;
    private ArmyManager army_Script;
    public float speed = 5.0f;
    public float followDistance = 3.0f;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Rigidbody _rigidbody;

    public GenericClass.E_Action _actionState;

    //Enemy Things
    private GameObject[] targets;        // Tableau contenant tous les PNJ cibles
    private GameObject currentTarget;    // Référence au PNJ cible actuel
    private Statistique currentTargetStatistique;    // Référence au PNJ cible actuel
    public string targetTag = "Enemy"; // Tag du PNJ cible

    public float DetectDistance = 5.0f; // Distance d'attaque de l'ennemi
    public float attackDistance = 0.5f; // Distance d'attaque de l'ennemi
    public int _damage = 15;
    private bool isAttacking = false;    // Indique si l'ennemi est en train d'attaquer

    public bool SelfIdle;

    public GenericClass.E_Zone _zoneAttribute = GenericClass.E_Zone.BackMiddle;

    public float attackSpeed = 1;

    private void Start()
    {
        Character = GameObject.FindGameObjectWithTag("LocalPlayer");
        player = Character.transform;
        army_Script = Character.GetComponent<ArmyManager>();
        //_actionState = GenericClass.E_Action.Follow;
        SelfIdle = false;
    }
    private void Update()
    {
        switch (_actionState)
        {
            case GenericClass.E_Action.Wait:

                break;
            case GenericClass.E_Action.Follow:
                #region Follow

                switch (_zoneAttribute)
                {
                    case GenericClass.E_Zone.BackMiddle:
                        player = army_Script.ZonesBackMiddle.transform;
                        break;
                    case GenericClass.E_Zone.BackLeft:
                        player = army_Script.ZonesBackLeft.transform;
                        break;
                    case GenericClass.E_Zone.BackRight:
                        player = army_Script.ZonesBackRight.transform;
                        break;
                }
                // Calcule la distance entre le PNJ et le joueur
                float distance = Vector3.Distance(player.position, transform.position);
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
                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
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
                        case GenericClass.E_Zone.BackMiddle:
                            player = army_Script.ZonesBackMiddle.transform;
                            break;
                        case GenericClass.E_Zone.BackLeft:
                            player = army_Script.ZonesBackLeft.transform;
                            break;
                        case GenericClass.E_Zone.BackRight:
                            player = army_Script.ZonesBackRight.transform;
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
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
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

                    // Trouve le PNJ le plus proche


                    targets = GameObject.FindGameObjectsWithTag(targetTag);
                    float minDistance = Mathf.Infinity;
                    foreach (GameObject target in targets)
                    {
                        distance = Vector3.Distance(target.transform.position, transform.position);
                        if (distance < minDistance)
                        {
                            if (distance < DetectDistance)
                            {
                                currentTarget = target;
                                currentTargetStatistique = currentTarget.GetComponent<Statistique>();
                                minDistance = distance;
                            }

                        }
                    }
                }

                // Si on a une cible actuelle
                if (currentTarget != null)
                {
                    // Calcule la distance entre l'ennemi et la cible
                    distance = Vector3.Distance(currentTarget.transform.position, transform.position);
                    // Si la cible est à portée d'attaque
                    if (distance < DetectDistance && distance >= attackDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);
                    }
                    else if (distance < attackDistance && !isAttacking)
                    {
                        if (currentTarget.transform.tag == "MyMonster")
                        {
                            currentTarget = null;
                        }
                        else
                        {
                            isAttacking = true; // set your bool to true

                            StartCoroutine(ResetBoolAfterDelay());

                            currentTargetStatistique.TakeDamage(_damage);
                        }
                    }
                }
                #endregion
                break;
        }
    }

    IEnumerator ResetBoolAfterDelay()
    {
        yield return new WaitForSeconds(attackSpeed); // wait for 1 second
        isAttacking = false; // set your bool to false
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "MyMonster")
        {
            if (collision.transform.gameObject.GetComponent<SoldierBehavior>().SelfIdle == true)
            {
                if(collision.transform.gameObject.GetComponent<SoldierBehavior>()._zoneAttribute == _zoneAttribute)
                {
                    SelfIdle = true;
                }
            }
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "zone")
        {
            if (other.transform.gameObject.GetComponent<ZoneInfos>().zone == _zoneAttribute)
            {
                SelfIdle = true;
            }
        }
    }
}
