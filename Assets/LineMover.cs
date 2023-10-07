using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class LineMover : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private float speed = 1f;

    private int currentIndex;
    public EventHandler OnEndReached;
    private Vector3 previous_position;

    private void Start()
    {
        int[] values = {1, 4, 3, 5, 2, 6};
        lineRenderer = generateLineRenderer(values);
        Initialize(0,speed,lineRenderer);
        previous_position = transform.position;
    }
    public void Initialize(int index, float speed, LineRenderer lineRenderer)
    {
        enabled = true;
        currentIndex = index;
        this.speed = speed;
        this.lineRenderer = lineRenderer;
        transform.position = this.lineRenderer.GetPosition(currentIndex);
    }

    public static (Vector3 targetPosition, bool isEnd) GetTargetPosition(ref int index, float moveSpeed, Vector3 currentPosition, LineRenderer lineRenderer)
    {
        int nextIndex = index + 1;

        if (lineRenderer.positionCount <= nextIndex)
        {
            return (lineRenderer.GetPosition(index)+ lineRenderer.transform.position, true);
        }
        var nextPosition = lineRenderer.GetPosition(nextIndex) + lineRenderer.transform.position;

        float distance = Vector3.Distance(currentPosition, nextPosition);

        if (distance < moveSpeed)
        {
            index += 1;
            return GetTargetPosition(ref index, moveSpeed - distance, nextPosition, lineRenderer);
        }
        else
        {
            Vector3 direction = (nextPosition - currentPosition).normalized;
            return (currentPosition + direction * moveSpeed, false);
        }
    }
    private void Update()
    {
        var result = GetTargetPosition(
            ref currentIndex,
            speed * Time.deltaTime,
            previous_position,
            lineRenderer);
            
        Vector3 shift = new Vector3(0.0f, 1.0f, 0.0f); 
        previous_position = result.targetPosition;
        transform.position = result.targetPosition + shift;
        if (result.isEnd)
        {
            OnEndReached?.Invoke(this, EventArgs.Empty);
            enabled = false;
        }
    }

    private LineRenderer generateLineRenderer(int[] values){

        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = gameObject.AddComponent<LineRenderer>();

        //線の幅を設定
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        List<int> indexes = Enumerable.Range(0, values.Length).ToList();
        Vector3[] positions = indexes.Select(i => new Vector3(i, values[i], 0.0f)).ToArray();

        lineRenderer.positionCount = positions.Length;

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);

        return lineRenderer;

    }
}