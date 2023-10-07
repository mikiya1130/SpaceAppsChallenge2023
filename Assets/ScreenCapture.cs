using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapture : MonoBehaviour
{
    private Texture2D texture;

    private bool isTapped = false;
    private Vector2 position;
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
    }

    // Update is called once per frame
    void Update()
    {
        // ボタンを離した瞬間
        if (Input.GetMouseButtonUp(0))
        {
            isTapped = true;
            position = Input.mousePosition;
        }
    }

    void OnPostRender()
    {
        if (isTapped)
        {
            texture.ReadPixels(new Rect(position.x, position.y, 1, 1), 0, 0);
            Color color = texture.GetPixel(0, 0);
            Debug.Log(color);
            isTapped = false;
        }
    }
}
