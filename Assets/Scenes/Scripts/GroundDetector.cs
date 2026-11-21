
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.25f;

    private readonly Collider[] results = new Collider[32];

    void Update()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRadius,
            results
        );

        for (int i = 0; i < count; i++)
        {
            if (results[i] == null)
                continue;

            Platform platform =
                results[i].GetComponentInParent<Platform>();

            if (platform != null)
                platform.TurnBlack();
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}
