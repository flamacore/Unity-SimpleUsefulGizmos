using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Overlay : MonoBehaviour {
	public Color MeshColor = Color.green;
	public Color WireColor = Color.green;
	public Color DirectionalGuideColor = Color.red;
	public float Bias = 1.06f;
	public float DirectionalGuideLength = 1000f;
	public bool ShowDirectionalGuides = true;
	public bool ShowMesh = true;
	public bool ShowWire = true;
	public bool ShowInfo = true;
	public GUIStyle labelStyle;
	private Vector3 centerOfSelection;
	private int totalVertices;
	GameObject[] ms;
	void OnEnable() {
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnDisable() {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}

	void OnSceneGUI(SceneView sceneView) {
		if (Selection.gameObjects.Length > 0) {
			bool hasMesh = false;
			GameObject[] activeObj = Selection.gameObjects;
			for (int i = 0; i < activeObj.Length; i++) {
				if (activeObj [i].GetComponent<MeshFilter> ()) {
					hasMesh = true;
				}
			}
			if (hasMesh) {
				int vertexes = 0;
				Vector3 medianPoint = Vector3.zero;
				Vector3 pos = activeObj [0].transform.position;
				Vector3 siz = activeObj [0].GetComponent<MeshFilter> ().sharedMesh.bounds.size;
				Vector3 sca = activeObj [0].transform.localScale; 
				Mesh gizmoMesh = activeObj [0].GetComponent<MeshFilter> ().sharedMesh;
				Mesh spareMesh = activeObj [0].GetComponent<MeshFilter> ().sharedMesh;

				for (int i = 0; i < activeObj.Length; i++) {
					if (activeObj [i].GetComponent<MeshFilter> ()) {
						vertexes += activeObj [i].GetComponent<MeshFilter> ().sharedMesh.vertexCount;
						medianPoint += activeObj [i].transform.position / activeObj.Length;
					}
				}
				if (ShowInfo) {
					Handles.BeginGUI ();
					if (Selection.gameObjects.Length > 1) {
						GUI.Label (new Rect (10, 10, 500, 20), 
							"Total Vertex Count: " + vertexes.ToString () + "\n" +
							"Median Point: " + medianPoint
						, labelStyle);
					}

					if (Selection.gameObjects.Length == 1) {
						GUI.Label (new Rect (10, 10, 500, 350),	
							"Selected Object Vertex Count:" + gizmoMesh.vertexCount + "\n" +
							"Sub-Mesh Count:" + gizmoMesh.subMeshCount + "\n" +
							"On Layer:" + LayerMask.LayerToName (activeObj [0].layer) + "\n" +
							"World Scale:" + activeObj [0].transform.lossyScale + "\n" +
							"Local Scale:" + activeObj [0].transform.localScale + "\n" +
							"World Position:" + activeObj [0].transform.position + "\n" +
							"Local Position:" + activeObj [0].transform.localPosition + "\n" +
							"Euler Angles (World Rotation):" + activeObj [0].transform.eulerAngles + "\n" +
							"World Mesh Size : " + activeObj [0].GetComponent<MeshRenderer>().bounds.size
						, labelStyle);
					}
					Handles.EndGUI (); 
				}
			}
		}
	}
	void OnDrawGizmos()
	{
		if (Selection.gameObjects.Length > 0) {
			GameObject[] activeObj = Selection.gameObjects;
			for (int i = 0; i < activeObj.Length; i++) {
				if (activeObj[i].GetComponent<MeshFilter> ()) {
					Vector3 pos = activeObj [i].transform.position;
					Vector3 siz = activeObj [i].GetComponent<MeshFilter> ().sharedMesh.bounds.size;
					Vector3 sca = activeObj [i].transform.localScale; 
					Mesh gizmoMesh = activeObj[i].GetComponent<MeshFilter> ().sharedMesh;
					Mesh spareMesh = activeObj [i].GetComponent<MeshFilter> ().sharedMesh;
					Gizmos.color = MeshColor;
					if (ShowMesh)
						Gizmos.DrawMesh (gizmoMesh, pos, activeObj[i].transform.rotation, activeObj[i].transform.lossyScale * Bias);
					Gizmos.color = WireColor;
					if (ShowWire)
						Gizmos.DrawWireMesh (gizmoMesh, pos, activeObj[i].transform.rotation, activeObj[i].transform.lossyScale * Bias);
					Gizmos.color = DirectionalGuideColor;
					if (ShowDirectionalGuides) {
						Gizmos.DrawLine (pos, pos + activeObj [i].transform.up * DirectionalGuideLength);
						Gizmos.DrawLine (pos, pos + -activeObj [i].transform.up * DirectionalGuideLength);
						Gizmos.DrawLine (pos, pos + activeObj [i].transform.right * DirectionalGuideLength);
						Gizmos.DrawLine (pos, pos + -activeObj [i].transform.right * DirectionalGuideLength);
						Gizmos.DrawLine (pos, pos + activeObj [i].transform.forward * DirectionalGuideLength);
						Gizmos.DrawLine (pos, pos + -activeObj [i].transform.forward * DirectionalGuideLength);
					}
				}
			}
		}
	}
}
