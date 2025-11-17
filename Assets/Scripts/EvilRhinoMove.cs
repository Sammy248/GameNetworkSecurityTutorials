using UnityEngine;

public class EvilRhinoMove : MonoBehaviour
{
    public void clickRhino()
    {
        //transform.position = Vector3.Lerp(transform.position, Vector3.up, 500);
        transform.position += Vector3.left*5;
    }
}
