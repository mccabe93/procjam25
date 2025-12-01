using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Animator Animator;
    public int Health = 10;

    public ZombieAI ZombieAi;
    private bool _isFading = false;

    void Awake()
    {
        ZombieAi.AwakeEvent += OnZombieAwake;
    }

    private void OnZombieAwake()
    {
        Animator.Play("zombie-walk");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet_Player")
        {
            Health -= 10;
            if (Health <= 0 && !_isFading)
            {
                Animator.Play("zombie-dead");
                StartCoroutine("Fade");
                Destroy(ZombieAi);
            }
        }
    }

    private IEnumerator Fade()
    {
        _isFading = true;
        float fadeDuration = 3.0f;
        float elapsedTime = 0.0f;
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += 0.1f;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            renderer.material.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);
    }
}
