using UnityEngine;

public class RopeGizmo : MonoBehaviour
{
    [SerializeField]
    float radius = 0.5f;
    public Color gizmoColor;
    public Catenary thisCatenary;

    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
