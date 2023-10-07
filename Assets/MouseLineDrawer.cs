using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private Vector4 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // カーソル位置を取得
        Vector3 mousePosition = Input.mousePosition;
        // カーソル位置のz座標を10に
        mousePosition.z = 10;
        if (Input.GetMouseButtonDown(0))
        {
            // カーソル位置をワールド座標に変換
            startPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            // カーソル位置をワールド座標に変換
            endPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
}
