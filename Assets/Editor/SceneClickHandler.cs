﻿using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Handles mouse click on scene objects in edit mode.
/// Set up event, Single Click, Optionally - Cancellable and LayerMask
/// Enable handler when needed (or call ToggleState)
/// </summary>
public class SceneClickHandler
{
	/// <param name="onClick">Event called onClick on the scene</param>
	/// <param name="singleClick">Single click handler will deactivate itself on click</param>
	/// <param name="cancellable">Cancellable handler will listen for Escape key to set Enabled to false</param>
	public SceneClickHandler(Action<Vector3> onClick, bool singleClick = false, bool cancellable = true)
	{
		_onClick = onClick;
		
		SingleClickHandler = singleClick;
		Cancellable = cancellable;
		
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	~SceneClickHandler()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	
	public bool Enabled
	{
		private get { return _enabled; }
		set
		{
			if (value) FocusSceneView();
			_enabled = value;

			// We need to focus on SceneView to handle cancellation
			// (OnSceneGUI not handling keyboard events otherwise)
			void FocusSceneView()
			{
				if (SceneView.sceneViews.Count == 0) return;
				((SceneView) SceneView.sceneViews[0]).Focus();
			}
		}
	}

	/// <summary>
	/// Cancellable handler will listen for Escape key to set Enabled to false
	/// </summary>
	public bool Cancellable;
	/// <summary>
	/// Single click handler will deactivate itself on click
	/// </summary>
	public bool SingleClickHandler;
	
	public void SetLayerMask(LayerMask mask)
	{
		_useMask = true;
		_mask = mask;
	}
	
	public void ToggleState() => Enabled = !Enabled;
	
	
	private readonly Action<Vector3> _onClick;

	private bool _enabled;
	private bool _useMask;
	private LayerMask _mask;


	private void OnSceneGUI(SceneView sceneview)
	{
		if (!Enabled) return;

		var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		RaycastHit hit;
		if (_useMask ? Physics.Raycast(ray, out hit, _mask.value) : Physics.Raycast(ray, out hit))
		{
			Handles.DrawWireCube(hit.point, Vector3.one * .2f);

			if (Handles.Button(Vector3.zero, SceneView.currentDrawingSceneView.rotation, 30, 5000, Handles.RectangleHandleCap))
				HandleClick(hit.point);
		}

		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) HandleEscape();

		void HandleClick(Vector3 point)
		{
			if (SingleClickHandler) Enabled = false;

			_onClick(point);

			Event.current.Use();
			HandleUtility.Repaint();
		}

		void HandleEscape()
		{
			if (!Cancellable) return;

			Debug.LogWarning("Cancelled");
			Enabled = false;
		}
	}
}