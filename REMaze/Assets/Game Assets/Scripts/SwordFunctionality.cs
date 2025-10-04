using System.Collections;
using UnityEngine;

public class SwordFunctionality : MonoBehaviour
{
    public string gunAnimName;
    public Animator swordAnim;
    public WeaponItem swordItem;
    public LayerMask enemyMask;
    private float attackRange = 25;
    public GameObject slashFXObj;
    public bool hasPlayed = false;
    public void InjectSwordAnimator()
    {
        if(swordAnim == null)
        {
            swordAnim = GetComponent<Animator>();
        }

    }
    private void Start()
    {
        InjectSwordAnimator();
    }

    private void Update()
    {
        SetAttack();
    }

    public void SetAttack()
    {
        if (Input.GetMouseButtonDown(0) && !hasPlayed)
        {
            hasPlayed = true;
            Debug.Log("Sword Attack");
            swordAnim.SetTrigger(gunAnimName);
            StartCoroutine(ResetGunAnim());
        }
    }

    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(Camera.main.transform.position + Camera.main.transform.forward * attackRange, enemyMask);
        foreach (Collider c in hits)
        {
            HealthManager health = c.GetComponentInParent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(swordItem.weaponDamage);
                GameObject slashFX = Instantiate(slashFXObj, c.transform.position, Quaternion.identity);
                Destroy(slashFX, 1.5f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * attackRange);
    }

    private IEnumerator ResetGunAnim()
    {
        yield return new WaitForSeconds(0.5f);
        hasPlayed = false;
    }
}