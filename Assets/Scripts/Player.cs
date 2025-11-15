using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;

    Rigidbody rigidbody;

    public float fireRate = 0.75f;
    public GameObject[] bulletPrefab;
    public Transform bulletPosition;
    float nextFire;

    public GameObject audioPrefabScript;


    public GameObject bulletFiringEffect;


    public AudioClip playerShootingAudio;


    public int health = 100;
    public Slider healthBar;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            TakeDamage(bullet.damage);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
        {
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        gameObject.SetActive(false);
    }

    void Move()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput,0,verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rigidbody.MovePosition(rigidbody.position + movementDir);
    }


    void Fire() 
    {
        if (Time.time > nextFire) 
        {
            
            nextFire = Time.time + fireRate;

            GameObject bullet;
            if (Mathf.Round(Time.time) % 2 == 0)
            {
                bullet = Instantiate(bulletPrefab[0], bulletPosition.position, Quaternion.identity);

            }
            else
            {
                bullet = Instantiate(bulletPrefab[1], bulletPosition.position, Quaternion.identity);

            }

            bullet.GetComponent<BulletController>()?.InitializeBullet(transform.rotation * Vector3.forward);

            randomSoundPitch(playerShootingAudio);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

        }
    }

    void randomSoundPitch(AudioClip sound)
    {
        int[] pentatonicSemitones = new[] { 0, 2, 4, 7, 9 };
        float pitch = 1;
        //int semitone = pentatonicSemitones[Random.Range(0, pentatonicSemitones.Length)];

        //for(int i=0; i<5; i++)        BREAKS CODE
        {
            pitch = Mathf.Pow(1.059463f, pentatonicSemitones[i]);
            if (i == 4)
            {
                i = 0;
            }
        }
        //float pitch = Mathf.Pow(1.059463f, semitone);

        audioPrefabScript.GetComponent<AudioSource>().pitch = pitch;
        Debug.Log(audioPrefabScript.GetComponent<AudioSource>().pitch);
        AudioManager.Instance.Play3D(sound, transform.position);

    }

}
