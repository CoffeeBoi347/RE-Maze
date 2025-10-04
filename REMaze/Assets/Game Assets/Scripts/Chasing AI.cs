using EzySlice;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class ChasingAI : MonoBehaviour
{
    public GameObject capsuleObj;
    public GameObject playerObj;
    public NavMeshAgent agent;
    public Vector3 newPos;
    public Rigidbody rb;
    public float newPositionValue;
    public bool isNewPos = false;
    public ParticleSystem deathFX;
    public Material capsuleMaterial;
    [Header("Agent Values")]

    public float detectDistance;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        capsuleObj = this.gameObject;
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

    public void OnDead()
    {
        var hm = GetComponent<HealthManager>();

        if(hm != null)
        {
            SlicedHull hull = capsuleObj.Slice(transform.position, transform.up, capsuleMaterial); // slicing the upper half of the capsule
            GameObject deathFXAnim = Instantiate(deathFX.gameObject, transform.position, Quaternion.identity);
            Destroy(deathFXAnim, 1.5f);
            if (hull == null)
            {
                Debug.Log("Sliced hull is null.");
                return;
            }

            GameObject upperHall = hull.CreateUpperHull(capsuleObj, capsuleMaterial);
            GameObject lowerHall = hull.CreateLowerHull(capsuleObj, capsuleMaterial);

            StartCoroutine(HitStop());

            if (upperHall != null)
            {
                upperHall.AddComponent<Rigidbody>();
                upperHall.AddComponent<BoxCollider>();


                upperHall.GetComponent<Rigidbody>().AddExplosionForce(700f, transform.position, 1f);
                upperHall.GetComponent<Rigidbody>().AddTorque(new Vector3(155f, 145f, 155f), ForceMode.Impulse);
                upperHall.GetComponent<Rigidbody>().mass = 2f;
            }

            if (lowerHall != null)
            {
                lowerHall.AddComponent<Rigidbody>();
                lowerHall.AddComponent<BoxCollider>();

                lowerHall.GetComponent<Rigidbody>().AddExplosionForce(900f, transform.position, 1f);
                lowerHall.GetComponent<Rigidbody>().AddForce(Vector3.forward * 105f, ForceMode.Impulse);
                lowerHall.GetComponent<Rigidbody>().AddTorque(new Vector3(195, 155f, 180f), ForceMode.Impulse);
                lowerHall.GetComponent<Rigidbody>().mass = 2f;
            }

            //StartCoroutine(EffectsManager.instance.FlashScreen());
            Destroy(capsuleObj);

            Destroy(upperHall, 6f);
            Destroy(lowerHall, 6f);
        }
    }

    private IEnumerator HitStop()
    {
        Vector3 currentPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = currentPos + new Vector3(0f, 2f, 8f);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.position = currentPos;
    }
    private IEnumerator ResetPath()
    {
        yield return new WaitForSeconds(newPositionValue);
        isNewPos = !isNewPos;
    }

    private void ResetTime()
    {
        Time.timeScale = 1f;
    }
}