
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

}
