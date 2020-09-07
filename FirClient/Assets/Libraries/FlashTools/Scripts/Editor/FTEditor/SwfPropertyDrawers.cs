using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

using FTRuntime;
using FTRuntime.Internal;

namespace FTEditor {

	//
	// SwfIntRange
	//

	[CustomPropertyDrawer(typeof(SwfIntRangeAttribute))]
	class SwfIntRangeDrawer : PropertyDrawer {
		static void ValidateProperty(SerializedProperty property, int min, int max) {
			if ( !property.hasMultipleDifferentValues ) {
				if ( property.propertyType == SerializedPropertyType.Integer ) {
					var clamp = Mathf.Clamp(property.intValue, min, max);
					if ( clamp != property.intValue ) {
						property.intValue = clamp;
						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}
		public override void OnGUI(
			Rect position, SerializedProperty property, GUIContent label)
		{
			var attr = attribute as SwfIntRangeAttribute;
			ValidateProperty(property, attr.Min, attr.Max);
			EditorGUI.PropertyField(position, property, label, true);
		}
	}

	//
	// SwfFloatRange
	//

	[CustomPropertyDrawer(typeof(SwfFloatRangeAttribute))]
	class SwfFloatRangeDrawer : PropertyDrawer {
		static void ValidateProperty(SerializedProperty property, float min, float max) {
			if ( !property.hasMultipleDifferentValues ) {
				if ( property.propertyType == SerializedPropertyType.Float ) {
					var clamp = Mathf.Clamp(property.floatValue, min, max);
					if ( clamp != property.floatValue ) {
						property.floatValue = clamp;
						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}
		public override void OnGUI(
			Rect position, SerializedProperty property, GUIContent label)
		{
			var attr = attribute as SwfFloatRangeAttribute;
			ValidateProperty(property, attr.Min, attr.Max);
			EditorGUI.PropertyField(position, property, label, true);
		}
	}

	//
	// SwfSortingLayerDrawer
	//

	[CustomPropertyDrawer(typeof(SwfSortingLayerAttribute))]
	class SwfSortingLayerDrawer : PropertyDrawer {

		const string DefaultLayerName = "Default";

		static List<string> GetAllSortingLayers(bool include_empty) {
			var result = new List<string>();
			var tag_assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
			if ( tag_assets.Length > 0 ) {
				var layers = SwfEditorUtils.GetPropertyByName(
					new SerializedObject(tag_assets[0]),
					"m_SortingLayers");
				if ( layers.isArray ) {
					for ( var i = 0; i < layers.arraySize; ++i ) {
						var layer_prop = layers.GetArrayElementAtIndex(i);
						var layer_prop_name = layer_prop != null
							? layer_prop.FindPropertyRelative("name")
							: null;
						var layer_name = layer_prop_name != null && layer_prop_name.propertyType == SerializedPropertyType.String
							? layer_prop_name.stringValue
							: string.Empty;
						if ( !string.IsNullOrEmpty(layer_name) ) {
							result.Add(layer_name);
						}
					}
				}
			}
			if ( !result.Contains(DefaultLayerName) ) {
				result.Add(DefaultLayerName);
			}
			if ( include_empty ) {
				result.Add(string.Empty);
			}
			return result;
		}

		static void ValidateProperty(SerializedProperty property) {
			if ( !property.hasMultipleDifferentValues ) {
				if ( property.propertyType == SerializedPropertyType.String ) {
					var all_sorting_layers = GetAllSortingLayers(false);
					if ( !all_sorting_layers.Contains(property.stringValue) ) {
						property.stringValue = string.Empty;
						property.stringValue = DefaultLayerName;
						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}

		public override void OnGUI(
			Rect position, SerializedProperty property, GUIContent label)
		{
			if ( property.propertyType == SerializedPropertyType.String ) {
				ValidateProperty(property);
				SwfEditorUtils.DoWithMixedValue(
					property.hasMultipleDifferentValues, () => {
						label = EditorGUI.BeginProperty(position, label, property);
						EditorGUI.BeginChangeCheck();
						var all_sorting_layers  = GetAllSortingLayers(true);
						var sorting_layer_index = EditorGUI.Popup(
							position,
							label,
							property.hasMultipleDifferentValues
								? all_sorting_layers.FindIndex(p => string.IsNullOrEmpty(p))
								: all_sorting_layers.FindIndex(p => p == property.stringValue),
							all_sorting_layers.Select(p => new GUIContent(p)).ToArray());
						if ( EditorGUI.EndChangeCheck() ) {
							if ( sorting_layer_index >= 0 && sorting_layer_index < all_sorting_layers.Count ) {
								var new_value = all_sorting_layers[sorting_layer_index];
								if ( !string.IsNullOrEmpty(new_value) ) {
									if ( property.hasMultipleDifferentValues ) {
										property.stringValue = string.Empty;
									}
									property.stringValue = new_value;
									property.serializedObject.ApplyModifiedProperties();
								}
							}
						}
						EditorGUI.EndProperty();
					});
			} else {
				EditorGUI.LabelField(position, label.text, "Use SwfSortingLayer with string attribute.");
			}
		}
	}

	//
	// SwfPowerOfTwoIfDrawer
	//

	[CustomPropertyDrawer(typeof(SwfPowerOfTwoIfAttribute))]
	class SwfPowerOfTwoIfDrawer : PropertyDrawer {

		static SerializedProperty FindNextBoolProperty(SerializedProperty property, string next_prop) {
			var prop = property.Copy();
			while ( prop.Next(false) ) {
				if ( prop.name == next_prop && prop.propertyType == SerializedPropertyType.Boolean ) {
					return prop;
				}
			}
			return null;
		}

		static int GetPowerOfTwo(int value) {
			return Mathf.RoundToInt(Mathf.Pow(2, value));
		}

		static int[] GenPowerOfTwoValues(int min_pow2, int max_pow2) {
			var values = new List<int>();
			while ( min_pow2 <= max_pow2 ) {
				values.Add(GetPowerOfTwo(min_pow2));
				++min_pow2;
			}
			return values.ToArray();
		}

		static void ValidateProperty(SerializedProperty property, bool need_pow2, int min_pow2, int max_pow2) {
			if ( !property.hasMultipleDifferentValues ) {
				if ( property.propertyType == SerializedPropertyType.Integer ) {
					var last_value = property.intValue;
					if ( need_pow2 && !Mathf.IsPowerOfTwo(property.intValue) ) {
						property.intValue = Mathf.ClosestPowerOfTwo(property.intValue);
					}
					property.intValue = Mathf.Clamp(
						property.intValue,
						GetPowerOfTwo(min_pow2),
						GetPowerOfTwo(max_pow2));
					if ( last_value != property.intValue ) {
						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}

		public override void OnGUI(
			Rect position, SerializedProperty property, GUIContent label)
		{
			if ( property.propertyType == SerializedPropertyType.Integer ) {
				var attr      = attribute as SwfPowerOfTwoIfAttribute;
				var bool_prop = FindNextBoolProperty(property, attr.BoolProp);
				var need_pow2 = (bool_prop != null && (bool_prop.boolValue || bool_prop.hasMultipleDifferentValues));
				ValidateProperty(property, need_pow2, attr.MinPow2, attr.MaxPow2);
				SwfEditorUtils.DoWithMixedValue(
					property.hasMultipleDifferentValues, () => {
						if ( need_pow2 ) {
							var values = GenPowerOfTwoValues(attr.MinPow2, attr.MaxPow2);
							var vnames = values.Select(p => new GUIContent(p.ToString())).ToArray();
							EditorGUI.IntPopup(position, property, vnames, values, label);
						} else {
							EditorGUI.PropertyField(position, property, label, true);
						}
					});
			} else {
				EditorGUI.LabelField(position, label.text, "Use SwfPowerOfTwoIf with integer attribute.");
			}
		}
	}

	//
	// SwfReadOnlyDrawer
	//

	[CustomPropertyDrawer(typeof(SwfReadOnlyAttribute))]
	class SwfReadOnlyDrawer : PropertyDrawer {
		public override void OnGUI(
			Rect position, SerializedProperty property, GUIContent label)
		{
			SwfEditorUtils.DoWithEnabledGUI(false, () => {
				EditorGUI.PropertyField(position, property, label, true);
			});
		}
	}

	//
	// SwfDisplayNameDrawer
	//

	[CustomPropertyDrawer(typeof(SwfDisplayNameAttribute))]
	class SwfDisplayNameDrawer : PropertyDrawer {
		public override void OnGUI(
			Rect position, SerializedProperty property, GUIContent label)
		{
			var new_label  = new GUIContent(label);
			new_label.text = (attribute as SwfDisplayNameAttribute).DisplayName;
			if ( EditorGUI.PropertyField(position, property, new_label) ) {
				foreach ( SerializedProperty child in property ) {
					EditorGUILayout.PropertyField(child);
				}
			}
		}
	}
}