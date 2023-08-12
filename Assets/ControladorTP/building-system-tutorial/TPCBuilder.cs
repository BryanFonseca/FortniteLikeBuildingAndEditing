using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCBuilder : ControladorTerceraPersona{
	BuildBehavior buildBehavior;
	bool canBuild;

	void Start () {
		buildBehavior = gameObject.GetComponent<BuildBehavior>();
	}
	
	new void Update () {
		base.Update();

		if (Input.GetKeyDown(KeyCode.Space)) {
			canBuild = !canBuild;
      GetAnimator().SetBool("Construyendo", canBuild);
			buildBehavior.build();
		}
	}
}
