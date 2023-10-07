using System.Collections.Generic;
using System;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // カーソル位置を取得
        Vector3 mousePosition = Input.mousePosition;
        // カーソル位置のz座標を10に
        mousePosition.z = 10;
        // ボタンを押した瞬間
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.enabled = true;
            // カーソル位置をワールド座標に変換
            startPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        // ボタンを押している間
        if (Input.GetMouseButton(0))
        {
            // カーソル位置をワールド座標に変換
            endPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        // ボタンを離した瞬間
        if (Input.GetMouseButtonUp(0))
        {
        }
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
}
