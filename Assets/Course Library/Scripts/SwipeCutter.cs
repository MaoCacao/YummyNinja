using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class SwipeCutter : MonoBehaviour
{
    [Header("References")]
    private GameManager gameManager;
    private Camera swipeCamera;
    private TrailRenderer trail;

    [Header("Swipe Settings")]
    [SerializeField] private float zDistanceFromCamera = 10f;
    [SerializeField] private float minCuttingSpeed = 0.1f;
    [SerializeField] private float colliderThickness = 0.6f;
    [SerializeField] private float smoothing = 0.35f;

    private bool isSwiping;
    private Vector3 currentWorldPos;
    private Vector3 previousWorldPos;
    private Vector3 smoothedWorldPos;

    void Awake()
    {
        swipeCamera = Camera.main;
        trail = GetComponent<TrailRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        trail.enabled = false;
    }

    void Update()
    {
        if (!gameManager.isGameActive)
        {
            StopSwipe();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartSwipe();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSwipe();
        }

        if (isSwiping)
        {
            ContinueSwipe();
        }
    }

    void StartSwipe()
    {
        isSwiping = true;

        currentWorldPos = GetMouseWorldPosition();
        previousWorldPos = currentWorldPos;
        smoothedWorldPos = currentWorldPos;

        transform.position = currentWorldPos;

        trail.Clear();
        trail.enabled = true;
    }

    void StopSwipe()
    {
        isSwiping = false;
        trail.enabled = false;
    }

    void ContinueSwipe()
    {
        currentWorldPos = GetMouseWorldPosition();
        smoothedWorldPos = Vector3.Lerp(smoothedWorldPos, currentWorldPos, smoothing);

        Vector3 delta = smoothedWorldPos - previousWorldPos;
        float distance = delta.magnitude;

        if (distance > minCuttingSpeed)
        {
            Vector3 center = previousWorldPos + delta * 0.5f;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, delta.normalized);

            Vector3 halfExtents = new Vector3(
                distance * 0.5f,
                colliderThickness,
                colliderThickness
            );

            Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation);

            foreach (Collider hit in hits)
            {
                Target target = hit.GetComponent<Target>();
                if (target != null)
                {
                    target.DestroyTarget();
                }
            }

            transform.position = center;
        }

        previousWorldPos = smoothedWorldPos;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = zDistanceFromCamera;
        return swipeCamera.ScreenToWorldPoint(screenPos);
    }

    // For debugging purposes
    /*void OnDrawGizmos()
    {
        if (!Application.isPlaying || !isSwiping) return;

        Gizmos.color = Color.red;

        Vector3 delta = smoothedWorldPos - previousWorldPos;
        Vector3 center = previousWorldPos + delta * 0.5f;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, delta.normalized);

        Vector3 halfExtents = new Vector3(
            delta.magnitude * 0.5f,
            colliderThickness,
            colliderThickness
        );

        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);
    }*/
}
