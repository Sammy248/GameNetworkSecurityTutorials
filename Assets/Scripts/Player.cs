using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    float[] bulletSpeed = { 1f, 2f};
    Rigidbody rb;

    public float[] fireRate = { 0.75f, 1.0f };
    public GameObject[] bulletPrefab;
    public Transform bulletPosition;
    float nextFire;

    public GameObject levelManager;

    public GameObject audioPrefabScript;


    public GameObject bulletFiringEffect;


    public AudioClip playerShootingAudio;


    public int health = 100;
    public Slider healthBar;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(fireRate.Length);
    }

    void FixedUpdate()
    {
        Move();
        if (Input.GetKey(KeyCode.Space))
            Fire(0);
        if (Input.GetKey(KeyCode.Q))
        {
            Fire(1);
        }

        
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
        Debug.Log("Dead");
        gameObject.SetActive(false);
        levelManager.GetComponent<LevelManagerScript>().onPlayerKilledAction();
    }

    void Move()
    {
        
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            return;
        }
        
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput,0,verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rb.MovePosition(rb.position + movementDir);
    }


    void Fire(int type) 
    {
        switch (type)
        {
            case 0:
                if (Time.time > nextFire)
                {
                    Debug.Log("FireRate 1" + fireRate[type]);
                    nextFire = Time.time + fireRate[0];

                    GameObject bullet = Instantiate(bulletPrefab[type],
                        bulletPosition.position, Camera.main.transform.rotation);

                    ////change colour of bullet randomly
                    randomColourPick(type);

                    bullet.GetComponent<BulletController>()?.
                        InitializeBullet(transform.rotation * Vector3.forward);
                    Debug.Log("ooee bullet go");

                    randomSoundPitch(playerShootingAudio);
                    VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

                }
                break;
            case 1:
                if (Time.time > nextFire)
                {

                    nextFire = Time.time + fireRate[1];

                    GameObject bullet = Instantiate(bulletPrefab[type],
                        bulletPosition.position, Quaternion.identity);

                    randomColourPick(type);

                    bullet.GetComponent<BulletController>()?.
                        InitializeBullet(transform.rotation * Vector3.forward * bulletSpeed[1]);
                    //Debug.Log("ooee bullet go");

                    randomSoundPitch(playerShootingAudio);
                    VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

                }
                break;
        }
    }

    void randomSoundPitch(AudioClip sound)
    {
        int[] pentatonicSemitones = new[] { 0, 2, 4, 7, 9 };
        float pitch = 1;
        int semitone = pentatonicSemitones[Random.Range(0, pentatonicSemitones.Length)];

        pitch *= Mathf.Pow(1.059463f, semitone); //randomise pitch according to a pentatonic scale
        
        audioPrefabScript.GetComponent<AudioSource>().pitch = pitch;    //apply pitch
        audioPrefabScript.GetComponent<AudioSource>().volume = Random.Range(0.6f, 1); //randomise volume
        //Debug.Log(audioPrefabScript.GetComponent<AudioSource>().pitch);
        AudioManager.Instance.Play3D(sound, transform.position);//play sound
    }
    void randomColourPick(int type)
    {
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                bulletPrefab[type].GetComponent<Renderer>().
            sharedMaterial.SetColor("_Color", Color.red);
                break;
            case 1:
                bulletPrefab[type].GetComponent<Renderer>().
            sharedMaterial.SetColor("_Color", Color.yellow);
                break;
            case 2:
                bulletPrefab[type].GetComponent<Renderer>().
            sharedMaterial.SetColor("_Color", Color.magenta);
                break;
            case 3:
                bulletPrefab[type].GetComponent<Renderer>().
            sharedMaterial.SetColor("_Color", Color.green);
                break;
            case 4:
                bulletPrefab[type].GetComponent<Renderer>().
            sharedMaterial.SetColor("_Color", Color.cyan);
                break;

        }
    }
}
