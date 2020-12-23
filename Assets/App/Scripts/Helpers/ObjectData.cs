using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicBox.Helpers
{
	[Serializable]
	public class ObjectData : MonoBehaviour
	{
		[Header ("Parameters")] 
		[SerializeField] private double instanceID;

		public void SetInstanceID (double _id)
		{
			instanceID = _id;
		}
	}
}