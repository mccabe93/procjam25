using UnityEngine;

public class SuicidalWall : MonoBehaviour
{
    private BoxCollider _collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (
            collision.gameObject != gameObject
            && gameObject.tag == collision.gameObject.tag
            && collision.gameObject.GetComponent<SuicidalWall>() != null
        )
        {
            Destroy(gameObject);
        }
    }
}
