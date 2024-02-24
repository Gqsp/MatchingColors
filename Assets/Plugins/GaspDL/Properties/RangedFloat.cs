using System;

namespace GaspDL
{
	[Serializable]
	public struct RangedFloat
	{
		public float minValue;
		public float maxValue;

		public float Value
		{
			get { return UnityEngine.Random.Range(minValue, maxValue); }
		}

		public RangedFloat(float min, float max)
		{
			minValue = min;
			maxValue = max;
		}
	}
}
