using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHandler : MonoBehaviour {

	public GameObject heightReferenceObject;
	public float heightReferenceFloat = 0;
	float heightCutOff;
	public float errorAdjustment=0;

	public GameObject markers;

	void Start() {


	}

	public void CutMesh()
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
//		Transform[] bounds = markers.GetComponentInChildren<Transform>();
//
//		float d = bounds [1].transform.position.y - bounds [2].transform.position.y;
//		float center = (bounds [0].transform.position.x + bounds [3].transform.position.x) / 2;
//		//bounds limits
//		float top = center + d/2;
//		float bottom = center - d / 2;



		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		List<Vector3> vertList = new List<Vector3>();
		List<Vector2> uvList = new List<Vector2>();
		List<Vector3> normalsList = new List<Vector3>();
		List<int> trianglesList = new List<int>();


		if (heightReferenceObject != null) {
			heightCutOff = heightReferenceObject.transform.position.y;
		}
		else{
			heightCutOff = heightReferenceFloat;
		}

		int i = 0;
		while (i < vertices.Length) {
			vertList.Add (vertices[i]); 
			uvList.Add (uv[i]);
			normalsList.Add (normals[i]);
			i++;
		}
		for (int triCount = 0; triCount < triangles.Length; triCount += 3) {
			if ((transform.TransformPoint(vertices[triangles[triCount  ]]).y < heightCutOff+errorAdjustment)  &&
				(transform.TransformPoint(vertices[triangles[triCount+1]]).y < heightCutOff+errorAdjustment)  &&
				(transform.TransformPoint(vertices[triangles[triCount+2]]).y < heightCutOff+errorAdjustment)) {

				trianglesList.Add (triangles[triCount]);
				trianglesList.Add (triangles[triCount+1]);
				trianglesList.Add (triangles[triCount+2]);
			}
		}


		triangles = trianglesList.ToArray ();
		vertices = vertList.ToArray ();
		uv = uvList.ToArray ();
		normals = normalsList.ToArray ();
		//mesh.Clear();
		mesh.triangles = triangles;
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.normals = normals;
	}
}
