using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    Rigidbody rigidBody;
    public int damage = 10;
    public float bulletSpeed = 15f;

    public GameObject bulletImpactEffect;

    public GameObject audioPrefabScript;

    public AudioClip BulletHitAudio;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        /*
        if (rigidBody != null)
            print("Rigidbody is found!");
        else
            print("Rigidbody isn't found!");
        */
    }

    public void InitializeBullet(Vector3 originalDirection) 
    {
        //print(originalDirection);
        transform.forward = originalDirection;
        rigidBody.linearVelocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioPrefabScript.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.5f);
        //Debug.Log(audioPrefabScript.GetComponent<AudioSource>().pitch);
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);

        VFXManager.Instance.PlayVFX(bulletImpactEffect,transform.position);

        Destroy(gameObject);
    }
}
