using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject m_marker;
	public GameObject markersObj;
	public Text m_Index;
	public GameObject targetImage;
	public GameObject dynamicMesh;

	private TangoPointCloud m_pointCloud;
	private bool isMarkingFinished = false;
	private TangoDynamicMesh m_dynamicMesh;
	private int markerAmount = 6;
	private int k = 0;
	private List<GameObject> markers;
	private string[] tags = {"r_knee", "r_femur", "r_tibia", "l_knee", "l_femur", "l_tibia"};
	private bool modelState = true;

	GameObject[] uiElemets;

	void Start()
	{
		uiElemets = GameObject.FindGameObjectsWithTag("dynamic_UI");
		setMindMapReferenceColor (0);
		markers = new List<GameObject> ();
		m_dynamicMesh = FindObjectOfType<TangoDynamicMesh>();
		m_pointCloud = FindObjectOfType<TangoPointCloud>();

	}

	void Update ()
	{
		Vector2 screenPoint =  GetScreenCoordinates (targetImage.GetComponent<RectTransform> ()).center;
		//print (screenPoint);
		if (Input.touchCount == 1)
		{
			if (k < markerAmount) {
				// Trigger place kitten function when single touch ended.
				Touch t = Input.GetTouch (0);

				if (t.phase == TouchPhase.Ended) {
					

					switch (k) {
					case 0:
						placeObject (screenPoint, k);
						break;
					case 1:
						placeObject (screenPoint, k);
						break;
					case 2:
						placeObject (screenPoint, k);
						break;
					case 3:
						placeObject (screenPoint, k);
						break;
					case 4:
						placeObject (screenPoint, k);
						break;
					case 5:
						placeObject (screenPoint, k);
						break;
					default:
						break;
					}


					k++;
					if (k == markerAmount) 
					{
						GameObject.Find ("mind_map").gameObject.SetActive (false);
						targetImage.SetActive (false);
						//dynamicMesh.SetActive (true);

						if (!isMarkingFinished) 
						{
							markersObj.GetComponent<AngleMeasurer> ().calculateAngles (markers);
							isMarkingFinished = true;
						}
					}
				}
			}
		}
	}

	void placeObject(Vector2 touchPosition, int k)
	{
		// Find the plane.
		Camera cam = Camera.main;
		Vector3 planeCenter;
		Plane plane;
		int index = m_pointCloud.FindClosestPoint (cam, touchPosition, 30);

//		if (!m_pointCloud.FindPlane(cam, touchPosition, out planeCenter, out plane))
//		{
//			Debug.Log("cannot find plane.");
//			return;
//		}
//
		if (index == -1) 
		{
			print ("cannot find surface");
			return;
		}

		//m_Index.text = "Index at " + m_pointCloud.m_points [index];
		Vector3 pointIndex = m_pointCloud.m_points [index];


		GameObject marker = Instantiate(m_marker, pointIndex, Quaternion.identity);
		m_Index.text = "d: " + marker.transform.position;
		marker.transform.parent = markersObj.transform; 
		marker.tag = tags[k];
		markers.Add (marker);
		if(k < 5)
			setMindMapReferenceColor (k+1);
	}

	public Rect GetScreenCoordinates(RectTransform uiElement)
	{
		var worldCorners = new Vector3[4];
		uiElement.GetWorldCorners(worldCorners);
		var result = new Rect(
			worldCorners[0].x,
			worldCorners[0].y,
			worldCorners[2].x - worldCorners[0].x,
			worldCorners[2].y - worldCorners[0].y);
		return result;
	}

	public void setMindMapReferenceColor(int k)
	{
		GameObject mindMapReference = GameObject.Find ("ui_" + tags[k]);
		mindMapReference.GetComponent<Image> ().color = Color.red;
		if (k>0) {
			mindMapReference = GameObject.Find ("ui_" + tags[k-1]);
			mindMapReference.GetComponent<Image> ().color = Color.white;
		}
	}

	public void toggle3DModel(bool state)
	{
		if (modelState) {
			dynamicMesh.transform.localScale = Vector3.one;
			foreach (GameObject el in uiElemets) {
				el.SetActive (false);
			}
			modelState = false;
		} else 
		{
			dynamicMesh.transform.localScale = Vector3.zero;
			foreach (GameObject el in uiElemets) {
				el.SetActive (true);
			}
			modelState = true;
		}
	}
}
