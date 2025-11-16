using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Multiplayer : MonoBehaviour, IPunObservable
{
    public float movementSpeed = 10f;

    Rigidbody rb;

    public float fireRate = 0.75f;
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
            photonView.RPC("Fire", RpcTarget.AllViaServer);

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
    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab[Random.Range(0, bulletPrefab.Length)],
                            bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<MultiplayerBulletController>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

            randomSoundPitch(playerShootingAudio);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
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
}
