using UnityEngine;

public class BackGround : MonoBehaviour
{
	[SerializeField]
	private Gradient gradient = default;
	private float currentTime = 0.0f;

	private void Start()
	{
	}

	private void Update()
	{
		currentTime += Time.deltaTime * 0.005f;
		if (currentTime > 1.0f) currentTime = 0.0f;
		Camera.main.backgroundColor = gradient.Evaluate(currentTime);
	}
}
