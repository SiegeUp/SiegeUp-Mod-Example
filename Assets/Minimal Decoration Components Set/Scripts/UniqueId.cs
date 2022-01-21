﻿// Code from https://answers.unity.com/questions/1249093/need-a-persistent-unique-id-for-gameobjects.html
// Script for generating a unique but persistent string identifier belonging to this 
// component
//
// We construct the identifier from two parts, the scene name and a guid.
// 
// The guid is guaranteed to be unique across all components loaded at 
// any given time. In practice this means the ID is unique within this scene. We 
// then append the name of the scene to it. This ensures that the identifier will be 
// unique accross all scenes. (as long as your scene names are unique)
// 
// The identifier is serialised ensuring it will remaing the same when the level is 
// reloaded
//
// This code copes with copying the game object we are part of, using prefabs and 
// additive level loading
//
// Final point - After adding this Component to a prefab, you need to open all the 
// scenes that contain instances of this prefab and resave them (to save the newly 
// generated identifier). I recommend manually saving it rather than just closing it
// and waiting for Unity to prompt you to save it, as this automatic mechanism 
// doesn't always seem to know exactly what needs saving and you end up being re-asked
// incessantly
//
// Written by Diarmid Campbell 2017 - feel free to use and ammend as you like
//
using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
public class UniqueId : MonoBehaviour {

	// global lookup of IDs to Components - we can esnure at edit time that no two 
	// components which are loaded at the same time have the same ID. 
	static Dictionary<string, UniqueId> allGuids = new Dictionary<string, UniqueId> ();

	public string uniqueId;

	[ContextMenu("Generate Id")]
	public void generateId()
    {
		uniqueId = Guid.NewGuid().ToString();
#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
	}

    // When we get destroyed (which happens when unloading a level)
    // we must remove ourselves from the global list otherwise the
    // entry still hangs around when we reload the same level again
    // but now the THIS pointer has changed and end up changing 
    // our ID
    void OnDestroy()
    {
        if (uniqueId != null)
            allGuids.Remove(uniqueId);
    }

    // Only compile the code in an editor build
#if UNITY_EDITOR

    // Whenever something changes in the editor (note the [ExecuteInEditMode])
    void Update(){
		// Don't do anything when running the game
		if (Application.isPlaying)
			return;

		bool anotherComponentAlreadyHasThisID = (uniqueId != null && 
			allGuids.ContainsKey (uniqueId) && 
			allGuids [uniqueId] != this);

		if (anotherComponentAlreadyHasThisID || uniqueId == null || uniqueId.Length == 0)
		{
			uniqueId = Guid.NewGuid ().ToString();
			EditorUtility.SetDirty (this);
			EditorSceneManager.MarkSceneDirty (gameObject.scene);
		}
		// We can be sure that the key is unique - now make sure we have 
		// it in our list
		if (!allGuids.ContainsKey (uniqueId)) {
			allGuids.Add(uniqueId, this);
		}
	}

#endif
}