using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

using UnityEditorInternal;

using System.Reflection;

[CustomEditor(typeof(Puppet2D_SortingLayer))]
public class Puppet2D_SortingLayerEditor : Editor {


	string[] sortingLayerNames;//we load here our Layer names to be displayed at the popup GUI
	
	int popupMenuIndex;//The selected GUI popup Index
	int orderInLayer;
		
	void OnEnable()		
	{
		
		sortingLayerNames = GetSortingLayerNames(); //First we load the name of our layers
		var renderer = (target as Puppet2D_SortingLayer).gameObject.renderer;
		if (!renderer)
		{
			return;
		}
		popupMenuIndex = renderer.sortingLayerID;
		orderInLayer = renderer.sortingOrder;	

	}	
	public override void OnInspectorGUI()
		
	{
		var renderer = (target as Puppet2D_SortingLayer).gameObject.renderer;
		
		// If there is no renderer, we can't do anything
		if (!renderer)
		{
			return;
		}
		
		// Expose the sorting layer name
		int newPopupMenuIndex = popupMenuIndex;
		popupMenuIndex = EditorGUILayout.Popup("Sorting Layer", popupMenuIndex, sortingLayerNames);//The popup menu is displayed simple as that

		if (newPopupMenuIndex != renderer.sortingLayerID) {
			renderer.sortingLayerID = newPopupMenuIndex;
			EditorUtility.SetDirty(renderer);
			popupMenuIndex = newPopupMenuIndex;
		}
		int newSortingLayerOrder = orderInLayer;
		newSortingLayerOrder = EditorGUILayout.IntField("Sorting Layer Order", renderer.sortingOrder);
		if (newSortingLayerOrder != renderer.sortingOrder) {
			Undo.RecordObject(renderer, "Edit Sorting Order");
			renderer.sortingOrder = newSortingLayerOrder;
			EditorUtility.SetDirty(renderer);
		}
		//popupMenuIndex = EditorGUILayout.Popup("Sorting Layer", popupMenuIndex, sortingLayerNames);//The popup menu is displayed simple as that
		
			
	}
	
	
	
	// Get the sorting layer names
	
	public string[] GetSortingLayerNames()
		
	{
		
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		
	}
		

}
