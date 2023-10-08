using UnityEngine;
using UnityEngine.UI;

public class BackGroundMover : MonoBehaviour
{
	private const float k_maxLength = 1f;
	private const string k_propName = "_MainTex";

	[SerializeField]
	private Vector2 m_offsetSpeed;

	private Material m_material;
	
	private void Start()
	{
		if (GetComponent<Image>() is Image i)
		{
			m_material = i.material;
		}
	}

	private void Update()
	{
		if (m_material)
		{
			// xとyの値が0 ～ 1でリピートするようにする
			var x = Mathf.Repeat(Time.time * m_offsetSpeed.x, k_maxLength);
			var y = Mathf.Repeat(Time.time * m_offsetSpeed.y, k_maxLength);
			var offset = new Vector2(x, y);
			m_material.SetTextureOffset(k_propName, offset);
		}
	}

	private void OnDestroy()
	{
		// ゲームをやめた後にマテリアルのOffsetを戻しておく
		if (m_material)
		{
			m_material.SetTextureOffset(k_propName, Vector2.zero);
		}
	}
}
