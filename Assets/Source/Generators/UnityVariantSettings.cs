using UnityEngine;

public class UnityVariantSettings : MonoBehaviour
{
    /// <summary>
    /// If true, a fillable tile area can be filled with this variant.
    /// </summary>
    public bool CanFill;

    public bool IsDefault = false;

    [Range(0.0f, 1.0f)]
    public float Frequency;
}
