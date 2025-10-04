using UnityEngine;

public class GunFunctionality : MonoBehaviour
{
    public WeaponItem gunItem;
    public string gunAnimName;
    public float gunRange = 200f;
    public Animator gunAnim;
    public GameObject gunMuzzleFlash;
    public GameObject shootSign;
    public LayerMask enemyMask;
    public void InjectGunAnim()
    {
        if(gunAnim == null)
        {
            gunAnim = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        InjectGunAnim();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, gunRange, enemyMask))
        {
            shootSign.transform.position = hit.transform.position + new Vector3(0f, 6.5f, 0f);
            shootSign.SetActive(true);
        }
        else
        {
            shootSign.SetActive(false);
        }

        PlayGunAnimation();
    }

    public void PlayGunAnimation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gunAnim.SetTrigger(gunAnimName);
        }
    }

    public void CheckForInputs()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, gunRange, enemyMask))
        {
            GameObject muzzleFlash = Instantiate(gunMuzzleFlash, hit.transform.position, Quaternion.identity);
            Destroy(muzzleFlash, 1.5f);

            hit.transform.GetComponent<HealthManager>().TakeDamage(gunItem.weaponDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * gunRange);
    }
}