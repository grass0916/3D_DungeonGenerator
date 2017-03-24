using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDungeon : MonoBehaviour {

	void Awake() {
	}


	// Use this for initialization
	void Start() {
		PlaceGround(16, 16);
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	void PlaceGround(int xMax, int zMax) {
		for (int z = 0; z < zMax; z++) {
			for (int x = 0; x < xMax; x++) {
				bool whichone = (x + z) % 2 == 0;
				GameObject cube = Instantiate(Resources.Load("Snake-Temple/ground_block_1x1x1_dark")) as GameObject;
				float resizeValue = 1 / cube.GetComponent<Renderer>().bounds.size.x - 1;
				// Resize it.
				cube.transform.parent =  gameObject.transform;
				Debug.Log(cube.transform.localScale);
				cube.transform.localScale =  new Vector3(x, 0, z);
				Debug.Log(cube.transform.localScale);
				//cube.transform.localScale += new Vector3(resizeValue, resizeValue, resizeValue);
				// Add rigidbody.
				cube.AddComponent<Rigidbody>();
				cube.GetComponent<Rigidbody>().useGravity  = false;
				cube.GetComponent<Rigidbody>().isKinematic = true;
				// Add box collider.
				cube.AddComponent<BoxCollider>();
				// Update name.
				cube.name = cube.transform.position.ToString();
			}
		}
	}
}
