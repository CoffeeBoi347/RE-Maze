using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class ChasingAI : MonoBehaviour
{
    public GameObject playerObj;
    public NavMeshAgent agent;
    public Vector3 newPos;
    public Rigidbody rb;
    public float newPositionValue;
    public bool isNewPos = false;
    [Header("Agent Values")]

    public float detectDistance;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float time = Time.time;

        if (!isNewPos && !agent.hasPath)
        {
            float xPos = Random.Range(-10f, 10f);
            float zPos = Random.Range(-10f, 10f);

            newPos = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);
            agent.SetDestination(newPos);

            if(transform.position == agent.destination)
            {
                isNewPos = true;
                StartCoroutine(ResetPath());
            }
        }

        float distance = Vector3.Distance(playerObj.transform.position, transform.position);

        if(distance < detectDistance)
        {
            agent.SetDestination(playerObj.transform.position);
        }
    }

    private IEnumerator ResetPath()
    {
        yield return new WaitForSeconds(newPositionValue);
        isNewPos = !isNewPos;
    }
}