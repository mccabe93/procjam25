using System.Collections;
using UnityEngine;

public class FlashVFX : MonoBehaviour
{
    public Color Color = Color.red;
    public float Duration = 1.0f;
    public bool IsFlashing = false;

    public void BeginFX()
    {
        if (!IsFlashing)
        {
            IsFlashing = true;
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        float elapsedTime = 0.0f;
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        float rDiff = originalColor.r - Color.r;
        float gDiff = originalColor.g - Color.g;
        float bDiff = originalColor.b - Color.b;
        while (elapsedTime < Duration)
        {
            elapsedTime += 0.1f;
            float delta = Mathf.Lerp(1.0f, 0.0f, elapsedTime / Duration);
            renderer.material.color = new Color(
                originalColor.r - (rDiff * delta),
                originalColor.g - (gDiff * delta),
                originalColor.b - (bDiff * delta)
            );
            yield return new WaitForSeconds(0.1f);
        }
        IsFlashing = false;
    }
}
