using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortniteBuildingController : MonoBehaviour, BuildBehavior {
	public float baseSquareLength = 4.5f;
	private GameObject[] firstLevelWalls = new GameObject[4];
	public Material buildingGuidesMaterial;
	public Material invisibleMaterial;

	private GameObject baseSquare;
	private GameObject buildingGuides;

	void Start () {
		generateBuildingGuides();
	}
	
	void Update () {
		calculateBaseSquarePosition();
	}

	void FixedUpdate() {
		foreach (var wall in firstLevelWalls) {
			wall.GetComponent<MeshRenderer>().material = invisibleMaterial;
		}
		RaycastHit hitInfo;
		if(Physics.Raycast(transform.position, transform.forward, out hitInfo, 10, LayerMask.GetMask("BuildingReference"))) {
			Debug.DrawLine(transform.position, hitInfo.point);
			if (hitInfo.collider.tag == "BuildingReference") {
				hitInfo.collider.gameObject.GetComponent<MeshRenderer>().material = buildingGuidesMaterial;
				float distanceToHit = hitInfo.distance / baseSquareLength;
				float distanceComplement = (1 - distanceToHit);
				// the collider's center will only be between 0 and -1
				// so that the raycast will always be able to hit a building reference
				float centerYClamped = Mathf.Clamp(-(distanceComplement), -1, 0);
				hitInfo.collider.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, centerYClamped, 0);
			}
		}
	}

	void calculateBaseSquarePosition() {
		float xPos = baseSquareLength * (Mathf.Floor(transform.position.x / baseSquareLength));
		float zPos = baseSquareLength * (Mathf.Floor(transform.position.z / baseSquareLength));

		buildingGuides.transform.position = new Vector3(xPos + (baseSquareLength / 2), 0, zPos + (baseSquareLength / 2));
		buildingGuides.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public void build(){
		print("Building like in Fortnite");
	}

	void generateBuildingGuides() {
		buildingGuides = GameObject.Find("BuildingGuides");

		baseSquare = GameObject.CreatePrimitive(PrimitiveType.Cube);
		baseSquare.transform.localScale = new Vector3(baseSquareLength, .1f, baseSquareLength);
		baseSquare.transform.parent = buildingGuides.transform;
		baseSquare.GetComponent<MeshRenderer>().material = buildingGuidesMaterial;
		// baseSquare.GetComponent<BoxCollider>().isTrigger = true;
		baseSquare.name = "BaseFloor";

		// walls start counting from 0 clockwise
		for (int i = 0; i < firstLevelWalls.Length; i++) {
			firstLevelWalls[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			firstLevelWalls[i].name = "wall_" + i;
			firstLevelWalls[i].transform.parent = GameObject.Find("FirstLevelWalls").transform;
			firstLevelWalls[i].GetComponent<BoxCollider>().isTrigger = true;
			firstLevelWalls[i].GetComponent<BoxCollider>().size = new Vector3(1, 1.2f, 1);
			firstLevelWalls[i].tag = "BuildingReference";
			firstLevelWalls[i].layer = 12;
		}

		firstLevelWalls[0].transform.localScale = new Vector3(4.5f, 4, 0.1f);
		firstLevelWalls[0].transform.localPosition = new Vector3(0f, 2, 2.25f);

		firstLevelWalls[1].transform.localScale = new Vector3(0.1f, 4, 4.5f);
		firstLevelWalls[1].transform.localPosition = new Vector3(2.25f, 2, 0);

		firstLevelWalls[2].transform.localScale = new Vector3(0.1f, 4, 4.5f);
		firstLevelWalls[2].transform.localPosition = new Vector3(-2.25f, 2, 0);

		firstLevelWalls[3].transform.localScale = new Vector3(4.5f, 4, 0.1f);
		firstLevelWalls[3].transform.localPosition = new Vector3(0f, 2, -2.25f);
	}
}
