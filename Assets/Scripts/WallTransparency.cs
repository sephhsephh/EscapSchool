using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    [SerializeField] private Renderer wallRenderer;
    [SerializeField] private Material transparentMaterial;
    private Material originalMaterial;

    private void Start()
    {
        if (wallRenderer != null)
        {
            originalMaterial = wallRenderer.material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Entered: {other.name}");

        if (other.CompareTag("Player"))
        {
            if (wallRenderer != null)
            {
                wallRenderer.material = transparentMaterial;
                Debug.Log("Wall set to transparent.");
            }
            else
            {
                Debug.LogWarning("WallRenderer is not assigned!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (wallRenderer != null)
            {
                wallRenderer.material = originalMaterial;
                Debug.Log("Wall set to original material.");
            }
        }
    }


}
