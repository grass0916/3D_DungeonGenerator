using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GenerateDungeon : MonoBehaviour {

	private List<SpaceGrammarMapping> sgm = new List<SpaceGrammarMapping> {
		new SpaceGrammarMapping("Entrance",        "EN"),
		new SpaceGrammarMapping("Room",            "EX"),
		new SpaceGrammarMapping("Room",            "EX"),
		new SpaceGrammarMapping("Corner-R",        "EX"),
		new SpaceGrammarMapping("Corner-L",        "EX"),
		new SpaceGrammarMapping("DownStair",       "FL"),
		new SpaceGrammarMapping("T-Path-FR",       "CEX"),
		new SpaceGrammarMapping("T-Path-FL",       "CEX"),
		new SpaceGrammarMapping("BossRoom",        "BO"),
		new SpaceGrammarMapping("EndRoom-Monster", "ER"),
		new SpaceGrammarMapping("EndRoom-Item",    "ER"),
		new SpaceGrammarMapping("TreasureRoom",    "GO"),
	};

	Queue<string> missionNodes = new Queue<string>( new[] { "EN", "EX", "CEX", "FL", "CEX", "EX", "EX", "FL", "EX", "FL", "CEX", "BO", "GO" });


	// Use this for initialization
	void Start() {
		ExtendMap(1, 1, GameObject.Find("Connection"));
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	void ExtendMap(int mapIndex, int connectionIndex, GameObject firstConnection) {
		GameObject mapContainer;
		string mapContainerName = "Map - " + mapIndex + " iteration";
		string prefabName       = string.Empty;
		int    number           = Random.Range(1, 20);

		// Map container is created or loaded now.
		if (GameObject.Find(mapContainerName) == null) {
			mapContainer = new GameObject(mapContainerName);
		} else {
			mapContainer = GameObject.Find(mapContainerName);
		}

		if (missionNodes.Count > 0) {
			string currentRoom = missionNodes.Dequeue();
			string room = sgm
				.Where(pair => pair.spaceRuleName == currentRoom)
				.OrderBy(e => Random.value)
				.FirstOrDefault().mapName;
			prefabName = room;
		} else {
			if (number <= 4) {
				prefabName = sgm
					.Where(pair => pair.spaceRuleName == "FL")
					.OrderBy(e => Random.value)
					.FirstOrDefault().mapName;
			} else if (number <= 5) {
				prefabName = sgm
					.Where(pair => pair.spaceRuleName == "CEX")
					.OrderBy(e => Random.value)
					.FirstOrDefault().mapName;
			} else if (number <= 18) {
				prefabName = sgm
					.Where(pair => pair.spaceRuleName == "EX")
					.OrderBy(e => Random.value)
					.FirstOrDefault().mapName;
			} else if (number <= 19) {
				prefabName = sgm
					.Where(pair => pair.spaceRuleName == "ER")
					.OrderBy(e => Random.value)
					.FirstOrDefault().mapName;
			} else {
				prefabName = "EndRoom";
			}
		}
		// if (mapIndex >= 10) {
		// 	prefabName = "EndRoom";
		// } else if (number <= 7) {
		// 	prefabName = "Room";
		// } else if (number <= 9) {
		// 	prefabName = "Corner-R";
		// } else if (number <= 11) {
		// 	prefabName = "Corner-L";
		// } else if (number <= 13) {
		// 	prefabName = "DownStair";
		// } else if (number <= 15) {
		// 	prefabName = "T-Path-FR";
		// } else if (number <= 18) {
		// 	prefabName = "T-Path-FL";
		// } else {
		// 	prefabName = "BossRoom";
		// }

		GameObject newMap = Instantiate(Resources.Load("SpaceGrammar/Prefabs/Maps/" + prefabName)) as GameObject;
		newMap.name = "Map-" + mapIndex + "-" + connectionIndex;
		newMap.transform.parent = mapContainer.transform;

		GameObject startingPoint = newMap.transform.Find("Identities/StartingPoint").gameObject;
		GameObject connections   = newMap.transform.Find("Identities/Connections").gameObject;
		GameObject monsters      = newMap.transform.Find("Identities/Monsters").gameObject;
		// Rotate by parent's connection.
		newMap.transform.rotation = firstConnection.transform.rotation;
		// Change the position from parent's connection.
		newMap.transform.position += firstConnection.transform.position - startingPoint.transform.position;

		// Set monsters.
		foreach (Transform symbol in monsters.transform) {
			GameObject monster = Instantiate(Resources.Load(Random.Range(0, 2) < 1 ? "Snake-Temple/door_blocker_snake" : "Snake-Temple/door_blocker_snake_lock")) as GameObject;
			monster.transform.parent   =  newMap.transform;
			monster.transform.rotation =  symbol.transform.rotation;
			monster.transform.position += symbol.transform.position;
			// Add rigidbody.
			monster.AddComponent<Rigidbody>();
			monster.GetComponent<Rigidbody>().useGravity  = false;
			monster.GetComponent<Rigidbody>().isKinematic = true;
			// Add box collider.
			monster.AddComponent<BoxCollider>();
		}

		// Recursive.
		for (int i = 0; i < connections.transform.childCount; i++) {
			ExtendMap(mapIndex + 1, i + 1, connections.transform.GetChild(i).gameObject);
		}
	}

	public struct SpaceGrammarMapping {
		public string mapName;
		public string spaceRuleName;

		public SpaceGrammarMapping(string mapName, string spaceRuleName) {
			this.mapName       = mapName;
			this.spaceRuleName = spaceRuleName;
		}
	}
}