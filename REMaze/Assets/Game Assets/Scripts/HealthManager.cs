using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    public Vector3 damageAttack = new Vector3(-5f, 50f, -35f);
    public void TakeDamage(float damage)
    {
        
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Die();
        }

        this.GetComponent<Rigidbody>().AddForce(damageAttack, ForceMode.Impulse);
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}