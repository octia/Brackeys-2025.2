
using UnityEngine;
using UnityEngine.UI;

public class MoveBackgroundImage : MonoBehaviour
{
    public Image image;
    public Vector2 speed;
    private Material mat;

    void Start()
    {
        mat = Instantiate(image.material);
        image.material = mat;
    }

    void Update()
    {
        mat.mainTextureOffset += speed * Time.deltaTime;
    }

    //public RawImage rawImage;
    //public Vector2 speed;

    //void Update()
    //{
    //    Rect r = rawImage.uvRect;
    //    r.position += speed * Time.deltaTime;
    //    r.x = Mathf.Repeat(r.x, 1f);
    //    r.y = Mathf.Repeat(r.y, 1f);
    //    rawImage.uvRect = r;
    //}
}
