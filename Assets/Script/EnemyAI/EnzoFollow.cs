using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnzoFollow : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;

    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;

    //Patrol
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float timeWalk;
    [SerializeField] private float timeTaunt;

    private float timerTaunt;
    private float timerWalking;
    private bool walkPointSet;

    [SerializeField] private float sightRange;
    [SerializeField] private bool playerInRange;

    //Debug
    [SerializeField] private TextMeshProUGUI debugWalking;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (playerInRange)
        {
            walkPointSet = false;
            FindObjectOfType<AudioManager>().Play("SeeYou");
            Chase();
        }
        else
        {
            RandomPatrol();
        }

        debugWalking.SetText("Enzo: " + walkPointSet.ToString());
    }

    void RandomPatrol()
    {
        if (!walkPointSet)
        {
            RandomWalkPoint();
        }

        if (walkPointSet)
        {
            enemy.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (timerWalking > 0f)
        {
            timerWalking -= Time.deltaTime;
        }

        if (timerTaunt > 0f)
        {
            timerTaunt -= Time.deltaTime;
        }

        if (distanceToWalkPoint.magnitude < 1f || timerWalking <= 0f)
        {
            walkPointSet = false;
            timerWalking = timeWalk;
        }

        if (timerTaunt <= 0f)
        {
            timerTaunt = timeTaunt;
            FindObjectOfType<AudioManager>().Play("WhereAreYou");
        }
    }

    private void RandomWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    void Chase()
    {
        enemy.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
    }
}
