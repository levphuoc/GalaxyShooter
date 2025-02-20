using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int health;

    public GameObject explosion;
    public Color damageColor;
    public bool isDead = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public virtual void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                AudioManager.instance.PlaySFX(11);
                if (explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                }

                if (this.GetComponent<PlayerController>() != null)
                {
                    GetComponent<PlayerController>().Respawn();
                }
                else
                {
                    
                    isDead = true;
                 
                    Destroy(gameObject);
                }
            }
            else
            {
                StartCoroutine(TakingDamage());
            }
        }
    }

    IEnumerator TakingDamage()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
