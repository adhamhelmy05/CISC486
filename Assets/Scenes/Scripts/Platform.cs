
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Registry of every active platform.
    public static readonly List<Platform> All = new List<Platform>();

    private Renderer[] platformRenderers;
    private bool hasTurnedBlack = false;

    // Number of platforms that have changed color.
    public static int ChangedCount { get; private set; } = 0;

    void OnEnable()
    {
        if (!All.Contains(this))
            All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    void Awake()
    {
        platformRenderers = GetComponentsInChildren<Renderer>();
    }
    

    // Center of the top face, in world space.
    public Vector3 TopCenter
    {
        get
        {
            Collider[] cols = GetComponentsInChildren<Collider>();

            if (cols.Length == 0)
                return transform.position;

            Bounds b = cols[0].bounds;

            for (int i = 1; i < cols.Length; i++)
                b.Encapsulate(cols[i].bounds);

            return new Vector3(b.center.x, b.max.y, b.center.z);
        }
    }

    // Change this platform to black only once.
    public void TurnBlack()
    {
        if (hasTurnedBlack)
            return;

        hasTurnedBlack = true;
        ChangedCount++;

        if (platformRenderers == null)
            platformRenderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in platformRenderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", Color.black);
                else if (mat.HasProperty("_Color"))
                    mat.color = Color.black;
            }
        }

        Debug.Log("Tiles changed: " + ChangedCount);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCounters()
    {
      ChangedCount = 0;
      All.Clear();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(TopCenter, 0.1f);
    }
#endif
}
