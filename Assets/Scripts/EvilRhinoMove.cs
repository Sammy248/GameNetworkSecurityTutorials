using UnityEngine;

public class EvilRhinoMove : MonoBehaviour
{
    public GameObject butt;
    private Rigidbody2D rb;
    public void clickRhino()
    {
        //transform.position = Vector3.Lerp(transform.position, Vector3.up, 500);
        transform.position += Vector3.left*5;
        butt.AddComponent(typeof(Rigidbody2D));
        rb = butt.GetComponent<Rigidbody2D>();
        rb.gravityScale = -1;
    }
}
