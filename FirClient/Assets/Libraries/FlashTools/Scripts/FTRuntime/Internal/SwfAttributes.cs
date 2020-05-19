using UnityEngine;

namespace FTRuntime.Internal {
	public class SwfIntRangeAttribute : PropertyAttribute {
		public int Min;
		public int Max;
		public SwfIntRangeAttribute(int min, int max) {
			Min = min;
			Max = max;
		}
	}

	public class SwfFloatRangeAttribute : PropertyAttribute {
		public float Min;
		public float Max;
		public SwfFloatRangeAttribute(float min, float max) {
			Min = min;
			Max = max;
		}
	}

	public class SwfSortingLayerAttribute : PropertyAttribute {
	}

	public class SwfPowerOfTwoIfAttribute : PropertyAttribute {
		public int    MinPow2;
		public int    MaxPow2;
		public string BoolProp;
		public SwfPowerOfTwoIfAttribute(int min_pow2, int max_pow2, string bool_prop) {
			MinPow2  = min_pow2;
			MaxPow2  = max_pow2;
			BoolProp = bool_prop;
		}
	}

	public class SwfReadOnlyAttribute : PropertyAttribute {
	}

	public class SwfDisplayNameAttribute : PropertyAttribute {
		public string DisplayName;
		public SwfDisplayNameAttribute(string display_name) {
			DisplayName = display_name;
		}
	}
}