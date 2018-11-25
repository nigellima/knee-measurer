using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleMeasurer : MonoBehaviour {

	public Text debug;
	public GameObject angleInfoPrefab;
	public LineRenderer line;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void calculateAngles(List<GameObject> markers)
	{
		float rAngle = calculateTwoVectorsAngle (markers[0].transform, markers[1].transform, markers[2].transform);
		float lAngle = calculateTwoVectorsAngle (markers[3].transform, markers[4].transform, markers[5].transform);

		int rAngleDir = calculateAngleDirection(markers[0].transform, markers[1].transform, markers[2].transform);
		int lAngleDir = calculateAngleDirection(markers[3].transform, markers[4].transform, markers[5].transform);

//		debug.text = "r " + rAngleDir;
//		debug.text += "\nl " + lAngleDir;

		placeAngleInfo (true, rAngle, markers [0].transform, rAngleDir);
		placeAngleInfo (false, lAngle, markers [3].transform, lAngleDir);

		line.gameObject.SetActive (true);

		drawLine (markers[0].transform, markers[1].transform, markers[2].transform);
		drawLine (markers[3].transform, markers[4].transform, markers[5].transform);
	}

	public float calculateTwoVectorsAngle(Transform knee, Transform femur, Transform tibia)
	{
		Vector2 k = new Vector2 (knee.position.x,knee.position.y);
		Vector2 f = new Vector2 (femur.position.x,femur.position.y);
		Vector2 t = new Vector2 (tibia.position.x,tibia.position.y);


		Vector2 KF = f - k;
		Vector2 KT = t - k;

		float dotProduct = Vector2.Dot (KF, KT);

		float norms = getVectorDistance (KF, KT);

		float cosin = dotProduct / norms;

		return RadianToDegree (Mathf.Acos(cosin));
	}

	public float getVectorDistance(Vector2 v1, Vector2 v2)
	{
		float normV1 = Mathf.Sqrt( Mathf.Pow(v1.x, 2) +  Mathf.Pow(v1.y, 2));
		float normV2 = Mathf.Sqrt( Mathf.Pow(v2.x, 2) +  Mathf.Pow(v2.y, 2));

		return normV1 * normV2;
	}

	private float RadianToDegree(float angle)
	{
		return 180f - (angle * (180.0f / Mathf.PI));
	}

	void placeAngleInfo(bool isRightSide, float angle, Transform knee, int angleDirection)
	{
		Vector3 newPos = knee.position;
		if(isRightSide)
			newPos.x = newPos.x - 0.2f;
		else
			newPos.x = newPos.x + 0.2f;
		string devianType;

		if (isRightSide)
			devianType = (angleDirection == -1) ? "Valgo" : "Varo";
		else
			devianType = (angleDirection == 1) ? "Valgo" : "Varo";

		GameObject angleInfo = Instantiate(angleInfoPrefab, newPos, Quaternion.identity);
		angleInfo.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = angle.ToString("#.0") + "°\n" + devianType;
	}

	void drawLine(Transform knee, Transform femur, Transform tibia)
	{
		GameObject lineObj = Instantiate (line.gameObject);

		lineObj.GetComponent<LineRenderer>().SetPosition (0, femur.position);
		lineObj.GetComponent<LineRenderer>().SetPosition (1, knee.position);
		lineObj.GetComponent<LineRenderer>().SetPosition (2, tibia.position);
	}

	int calculateAngleDirection(Transform knee, Transform femur, Transform tibia)
	{
		if (femur.position.x > knee.position.x && tibia.position.x > knee.position.x)
			return 1; //Angle to left
		else
			return -1; //Angle to right
	}
}
