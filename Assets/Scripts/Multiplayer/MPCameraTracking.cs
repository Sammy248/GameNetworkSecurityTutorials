using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MPCameraTracking : MonoBehaviour
{
    public Transform playerCharacter;
    public GameObject[] players;
    public PhotonView photonView;

    public Vector3 cameraOffset = new Vector3(0, 13, -9.87f);

    // Start is called before the first frame update
    void Start()
    {
        //cameraOffset = transform.position - playerCharacter.position;
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
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
