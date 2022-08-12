using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour, IBuildingController {
	public bool isBuilding;

	void Start () {
		print("Building Controller loaded.");
	}
	
	void Update () {
	}

	public bool getIsBuilding() {
		return isBuilding;
	}

}
