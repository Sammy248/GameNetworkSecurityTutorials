using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class MPCameraTracking : MonoBehaviour
{
    public Transform playerCharacter;
    public GameObject[] players;
    public PhotonView photonView;

    public Vector3 cameraOffset = new Vector3(0, 13, -9.87f);
    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    void Start()
    {
        //cameraOffset = transform.position - playerCharacter.position;

        Debug.Log ("Player length: " + players.Length);
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("minefound");
                playerCharacter = player.transform;
            }
        }
        transform.position = playerCharacter.position + cameraOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCharacter.position + cameraOffset;
    }
}
