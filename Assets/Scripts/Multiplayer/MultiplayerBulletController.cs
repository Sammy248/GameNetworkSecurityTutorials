using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class MultiplayerBulletController : MonoBehaviourPunCallbacks
{

    Rigidbody rigidBody;
    public int damage = 10;
    public float bulletSpeed = 15f;

    public Photon.Realtime.Player player;

    public GameObject bulletImpactEffect;

    public AudioClip BulletHitAudio;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody != null)
            print("Rigidbody is found!");
        else
            print("Rigidbody isn't found!");
    }

    public void InitializeBullet(Vector3 originalDirection) 
    {
        print(originalDirection);
        transform.forward = originalDirection;
        rigidBody.linearVelocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);

        VFXManager.Instance.PlayVFX(bulletImpactEffect,transform.position);

        Destroy(gameObject);
    }
}
