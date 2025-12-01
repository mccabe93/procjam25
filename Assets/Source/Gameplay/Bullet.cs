using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (
            collision.gameObject.tag == "LowerWallTile"
            || collision.gameObject.tag == "UpperWallTile"
        )
        {
            Destroy(gameObject);
        }
    }
}
