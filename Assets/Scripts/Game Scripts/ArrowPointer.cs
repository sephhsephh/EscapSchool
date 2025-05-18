using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public static ArrowPointer Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float hideDistance = 3f;
    [SerializeField] private float rotationSmoothness = 5f;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject arrowHead;

    private Transform target;
    private bool isActive = false;
    private float currentDistance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (!isActive || target == null || playerTransform == null)
            return;

        // Calculate distance to target
        currentDistance = Vector3.Distance(
            new Vector3(playerTransform.position.x, 0, playerTransform.position.z),
            new Vector3(target.position.x, 0, target.position.z)
        );

        // Auto-hide when close to target
        if (currentDistance <= hideDistance)
        {
            arrowHead.SetActive(false);
            return;
        }
       

        // Smooth rotation toward target (horizontal only)
        Vector3 direction = target.position - playerTransform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmoothness * Time.deltaTime
            );
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isActive = true;
        arrowHead.SetActive(true);
    }

    public void ClearTarget()
    {
        target = null;
        isActive = false;
        arrowHead.SetActive(false);
    }

    public float GetCurrentDistance() => currentDistance;


/// <summary>Show or hide the arrow mesh manually.</summary>
public void SetVisible(bool visible)
    {
        Debug.Log("ArrowPointer: SetVisible(" + visible + ")");
        arrowHead.SetActive(visible);
    }
}
