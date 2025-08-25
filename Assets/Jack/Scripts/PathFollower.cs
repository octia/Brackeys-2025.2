using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public TDPath path;
    public float speed = 3f;
    [Range(0f, 1f)] public float t; 
    public bool destroyAtEnd = true;

    private void Reset()
    {
        speed = 3f;
        t = 0f;
    }

    private void Update()
    {
        if (!path || path.Count < 2) return;

        t += (speed / 10f) * Time.deltaTime;
        if (t >= 1f)
        {
            if (destroyAtEnd) Destroy(gameObject);
            else t = 1f;
        }

        transform.position = path.Sample(t);
        float lookAhead = Mathf.Min(t + 0.01f, 1f);
        Vector3 fwd = (path.Sample(lookAhead) - transform.position);
        if (fwd.sqrMagnitude > 0.0001f)
            transform.forward = Vector3.Lerp(transform.forward, fwd.normalized, 0.5f);
    }
}
