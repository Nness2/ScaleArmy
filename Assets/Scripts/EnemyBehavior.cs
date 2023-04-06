using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 5.0f;          // Vitesse de déplacement de l'ennemi
    public float attackDistance = 0.5f; // Distance d'attaque de l'ennemi
    public float DetectDistance = 5.0f; // Distance d'attaque de l'ennemi
    public string targetTag = "MyMonster"; // Tag du PNJ cible

    public GameObject[] targets;        // Tableau contenant tous les PNJ cibles
    public GameObject currentTarget;    // Référence au PNJ cible actuel
    private Statistique currentTargetStatistique;    // Référence au PNJ cible actuel
    private bool isAttacking = false;    // Indique si l'ennemi est en train d'attaquer
    public int _damage = 21;
    public float attackSpeed = 1;

    private void Start()
    {
        // Trouve tous les PNJ avec le tag "monster"
    }

    private void Update()
    {

        // Si on n'a pas de cible actuelle et qu'on n'est pas déjà en train d'attaquer
        if (currentTarget == null)
        {
            // Trouve le PNJ le plus proche
            targets = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance < DetectDistance)
                {
                    currentTarget = target;
                    currentTargetStatistique = currentTarget.GetComponent<Statistique>();
                }
            }
        }

        // Si on a une cible actuelle
        if (currentTarget != null)
        {
            // Calcule la distance entre l'ennemi et la cible
            float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
            // Si la cible est à portée d'attaque
            if (distance < DetectDistance && distance >= attackDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);


                if (transform.position != currentTarget.transform.position) // Vérifie que le PNJ n'a pas atteint sa cible
                {
                    Vector3 direction = currentTarget.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                }
            }
            else if(distance < attackDistance && !isAttacking)
            {
                isAttacking = true; // set your bool to true

                StartCoroutine(ResetBoolAfterDelay());

                currentTargetStatistique.TakeDamage(_damage);
            }
        }
    }

    IEnumerator ResetBoolAfterDelay()
    {
        yield return new WaitForSeconds(attackSpeed); // wait for 1 second
        isAttacking = false; // set your bool to false
    }
}