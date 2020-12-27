using DynamicBox.Managers;
using TMPro;
using UnityEngine;

public class BoxGeneratorHelper : MonoBehaviour
{
	[SerializeField] private float maxDistance;

	[SerializeField] private float height;
	[SerializeField] private float width;

	[SerializeField] private TMP_InputField heightInputField;
	[SerializeField] private TMP_InputField widthInputField;

	[SerializeField] private Transform[] clickPoints;

	private bool isFirstTouchDetected;

	void Start ()
	{
		BoxGenerator.Instance.CreateBox (width, height, clickPoints);
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, maxDistance))
			{
				int index = isFirstTouchDetected ? 1 : 0;
				clickPoints[index].position = hit.point;

				isFirstTouchDetected = !isFirstTouchDetected;

				if (!isFirstTouchDetected)
				{
					height = float.Parse (heightInputField.text);
					width = float.Parse (widthInputField.text);

					BoxGenerator.Instance.CreateBox (width, height, clickPoints);
				}
			}
		}
	}
}