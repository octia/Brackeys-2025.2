using UnityEngine;

public class EnvrionmentRandomizer : MonoBehaviour
{
    public Vector2 scaleRange;
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        transform.localScale = Vector3.one * scale;
    }
}
