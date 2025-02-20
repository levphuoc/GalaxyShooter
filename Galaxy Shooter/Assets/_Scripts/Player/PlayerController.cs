using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;


}
public enum ItemEffect
{
    shield, levelUp, rocket, star
}
public class PlayerController : MonoBehaviour
{
    public int level;
    public int maxLevel;
    public int upgradeCost;
    public int fireLevel = 1;
    public float fireRate;
    public float nextFire;
    public float speed;
    public int rocketAmount;
    private Animator anim;
    private Rigidbody2D rig;
    public GameObject playerBullet;
    public GameObject playerRocket;
    public GameObject shild;
    public Transform[] firePoints;
    public int lives = 1;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    public float spawnTime;
    public float invincibilityTime;


    public Boundary boundary;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.SetLivesText(lives);

        rig.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        rig.position = new Vector2(Mathf.Clamp(rig.position.x, boundary.xMin, boundary.xMax),
                                    Mathf.Clamp(rig.position.y, boundary.yMin, boundary.yMax));
        if (!isDead && gameObject.layer != 8)
        {
            if ((Input.GetButton("Fire1") || Input.GetButton("Fire3")) && Time.time > nextFire)
            {
                AudioManager.instance.PlaySFX(13);
                nextFire = Time.time + fireRate;
                if (fireLevel >= 1)
                {
                    Instantiate(playerBullet, firePoints[0].position, firePoints[0].rotation);

                }
                if (fireLevel >= 2)
                {
                    Instantiate(playerBullet, firePoints[0].position, firePoints[0].rotation);
                    Instantiate(playerBullet, firePoints[1].position, firePoints[1].rotation);
                    Instantiate(playerBullet, firePoints[2].position, firePoints[2].rotation);
                }

            }
            if (Input.GetMouseButtonDown(1) && Time.time > nextFire && rocketAmount > 0)
            {
                Instantiate(playerRocket, firePoints[0].position, firePoints[0].rotation);
                rocketAmount--;
                AudioManager.instance.PlaySFX(12);
            }

        }

    }
    public void Respawn()
    {
        lives--;
        if (lives > 0)
        {
            // Lưu lại vị trí hiện tại của người chơi khi chết
            startPosition = transform.position;
            StartCoroutine(Spawning());
        }
        else
        {
            lives = 0;
            isDead = true;
            spriteRenderer.enabled = false;
        }
    }

    IEnumerator Spawning()
    {
        isDead = true;
        spriteRenderer.enabled = true;
        fireLevel = 0;
        gameObject.layer = 8;
        yield return new WaitForSeconds(spawnTime);
        isDead = false;

        // Đặt lại vị trí của người chơi ngay tại vị trí họ đã chết
        transform.position = startPosition;

        for (float i = 0; i < invincibilityTime; i += 0.1f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.layer = 6;
        spriteRenderer.enabled = true;
        fireLevel = 1;
    }

    public void SetItemEffect(ItemEffect itemEffect)
    {
        if (itemEffect == ItemEffect.levelUp)
        {
            AudioManager.instance.PlaySFX(7);
            fireLevel++;
            if (fireLevel >= 2)
            {
                fireLevel = 2;
            }
        }
        else if (itemEffect == ItemEffect.rocket)
        {
            rocketAmount++;
            AudioManager.instance.PlaySFX(7);

        }
        else if (itemEffect == ItemEffect.shield)
        {
            Instantiate(shild, transform);
            AudioManager.instance.PlaySFX(7);

        }else if (itemEffect == ItemEffect.star)
        {
            GameManager.instance.AddStar();
            AudioManager.instance.PlaySFX(7);
            
        }


    }
   

}
