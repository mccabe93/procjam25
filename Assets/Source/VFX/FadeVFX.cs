using System.Collections;
using UnityEngine;

public class FadeVFX : MonoBehaviour
{
    public GameObject Parent;
    public float Duration = 3.0f;
    public bool IsFading = false;

    public void BeginFX()
    {
        if (!IsFading)
        {
            IsFading = true;
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        float elapsedTime = 0.0f;
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        while (elapsedTime < Duration)
        {
            elapsedTime += 0.1f;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / Duration);
            renderer.material.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(Parent);
        Destroy(this.gameObject);
    }
}
