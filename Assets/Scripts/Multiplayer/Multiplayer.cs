using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Multiplayer : MonoBehaviour, IPunObservable
{
    public float movementSpeed = 10f;
    float[] bulletSpeed = { 1f, 2f };
    int[] bulletDamage = { 10, 20 };
    Rigidbody rb;

    public float[] fireRate = { 0.75f, 1.0f };
    public GameObject[] bulletPrefab;
    public Transform bulletPosition;
    float nextFire;

    public GameObject bulletFiringEffect;

    public GameObject audioPrefabScript;

    public AudioClip playerShootingAudio;

    PhotonView photonView;

    public int health = 100;
    public Slider healthBar;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Move();

        if (Input.GetKey(KeyCode.Space))
        {
            photonView.RPC("Fire", RpcTarget.AllViaServer, 0);

        }
        if (Input.GetKey(KeyCode.Q))
        {
            photonView.RPC("Fire", RpcTarget.AllViaServer, 1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            photonView.RPC("Fire", RpcTarget.AllViaServer, 2);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            MultiplayerBulletController bullet = collision.gameObject.GetComponent<MultiplayerBulletController>();
            TakeDamage(bullet);
        }
    }

    void TakeDamage(MultiplayerBulletController bullet)
    {
        health -= bullet.damage;
        healthBar.value = health;
        if (health <= 0)
        {
            bullet.owner.AddScore(1);
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        health = 100;
        healthBar.value = health;
    }

    void Move()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rb.MovePosition(rb.position + movementDir);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            healthBar.value = health;
        }
    }

    [PunRPC]
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
                        bulletPosition.position, Quaternion.identity);

                    ////change colour of bullet randomly
                    randomColourPick(type);

                    bullet.GetComponent<MultiplayerBulletController>()?.
                        InitializeBullet(transform.rotation * Vector3.forward, bulletDamage[type], photonView.Owner);
                    Debug.Log("ooee bullet go");

                    

                    //bullet.GetComponent<MultiplayerBulletController>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

                    randomSoundPitch(playerShootingAudio);
                    VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

                }
                break;
            case 1:
                if (Time.time > nextFire)
                {

                    nextFire = Time.time + fireRate[1];

                    GameObject bullet = Instantiate(bulletPrefab[type],
                        bulletPosition.position, Camera.main.transform.rotation);

                    randomColourPick(type);

                    bullet.GetComponent<MultiplayerBulletController>()?.
                        InitializeBullet(transform.rotation * Vector3.forward * bulletSpeed[1], bulletDamage[type], photonView.Owner);
                    //Debug.Log("ooee bullet go");

                    randomSoundPitch(playerShootingAudio);
                    VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

                }
                break;
            case 2: //insta kill
                if (Time.time > nextFire)
                {

                    nextFire = Time.time + fireRate[0];

                    GameObject bullet = Instantiate(bulletPrefab[1],
                        bulletPosition.position, Quaternion.identity);

                    randomColourPick(1);

                    bullet.GetComponent<MultiplayerBulletController>()?.
                        InitializeBullet(transform.rotation * Vector3.forward * bulletSpeed[1], 100, photonView.Owner);
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
