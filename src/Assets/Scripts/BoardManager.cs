using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

  [Serializable]
  public class Count {
    public int minimum;
    public int maximum;

    public Count(int min, int max) {
      minimum = min;
      maximum = max;
    }
  }

  public int columns = 8;
  public int rows = 8;
  public Count foodCount = new Count(0, 10);
  public GameObject camera;
  public GameObject exit;
  public GameObject player;
  public GameObject[] enemyTiles;
  public GameObject[] floorTiles;
  public GameObject[] foodTiles;
  public GameObject[] outerWallTiles;
  public GameObject[][] map;
  private Transform boardHolder;
  private List<Vector3> gridPositions = new List<Vector3>();

  void InitializeList() {
    gridPositions.Clear();
    map = new GameObject[columns - 1][];
    for (int x = 1; x < columns - 1; x++) {
      map[x] = new GameObject[rows - 2];

      for (int y = 1; y < rows - 1; y++) {
        gridPositions.Add(new Vector3(x, y, 0f));

      }
    }
  }

  void BoardSetup() {
    boardHolder = new GameObject("Board").transform;
    for (int x = -1; x < columns + 1; x++) {
      for (int y = -1; y < rows + 1; y++) {
        GameObject toInstantiate = floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)];
        if (x == -1 || x == columns || y == -1 || y == rows) {
          toInstantiate = outerWallTiles[UnityEngine.Random.Range(0, floorTiles.Length)];
        }
        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
        instance.transform.SetParent(boardHolder);
      }
    }
  }

  Vector3 RandomPosition() {
    int randomIndex = UnityEngine.Random.Range(0, gridPositions.Count);
    Vector3 randomPosition = gridPositions[randomIndex];
    gridPositions.RemoveAt(randomIndex);
    return randomPosition;
  }

  public void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
    int objectcount = UnityEngine.Random.Range(minimum, maximum + 1);
    for (int i = 0; i < objectcount; i++) {
      Vector3 randomPosition = RandomPosition();
      GameObject tileChoice = tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
      Instantiate(tileChoice, randomPosition, Quaternion.identity);
    }
  }

  public void SetupScene(int level) {
    BoardSetup();
    InitializeList();
    int enemyCount = (int)Mathf.Log(level, 2f);
    LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
    Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    Debug.Log("move camera to " + columns / 2f + " " + rows / 2f);
    camera.transform.position = new Vector3(columns / 2f, rows / 2f, -10f);

    Instantiate(player, new Vector3(columns / 2f, 6, 0f), Quaternion.identity);
  }

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
