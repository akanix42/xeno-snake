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
  public int foodRemaining;
  public Count foodCount = new Count(0, 10);
  public GameObject camera;
  public GameObject exitPrefab;
  public GameObject player;
  public GameObject[] enemyTiles;
  public GameObject[] floorTiles;
  public GameObject[] foodTiles;
  public GameObject[] outerWallTiles;
  public Map map;
  private GameObject exit;
  private Transform boardHolder;
  private List<Vector3> gridPositions = new List<Vector3>();
  public class Map {
    private GameObject[] map;
    private int columns;
    private int rows;
    private int borderOffset;
    private int negativeBorderOffset;

    public Map(int columns, int rows) {
      this.columns = columns;
      this.rows = rows;
      int border = (columns + 2) * 2 + (rows * 2);

      borderOffset = columns * rows;
      negativeBorderOffset = borderOffset + (border / 2);

      map = new GameObject[columns * rows + border];
    }

    public GameObject GetPosition(int x, int y) {
      return map[GetIndex(x, y)];
    }

    public void SetPosition(int x, int y, GameObject gameObject) {
      map[GetIndex(x, y)] = gameObject;
    }

    private int GetIndex(int x, int y) {
      return ((x + 1) * (rows + 2)) + (y + 1);
    }

    public void Clear() {
      foreach (var gameObject in map) {
        Destroy(gameObject);
      }
    }
  }

  private bool isGameRunning = false;

  void InitializeGridPositions() {
    gridPositions.Clear();
    for (int x = 1; x < columns - 1; x++) {
      for (int y = 1; y < rows - 1; y++) {
        gridPositions.Add(new Vector3(x, y, 0f));
      }
    }
  }

  void BoardSetup() {
    boardHolder = new GameObject("Board").transform;
    map = new Map(columns, rows);

    for (int x = -1; x < columns + 1; x++) {
      for (int y = -1; y < rows + 1; y++) {
        GameObject toInstantiate = floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)];
        if (x == -1 || x == columns || y == -1 || y == rows) {
          toInstantiate = outerWallTiles[UnityEngine.Random.Range(0, floorTiles.Length)];
        }
        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
        instance.transform.SetParent(boardHolder);
        map.SetPosition(x, y, instance);
      }
    }
  }

  Vector3? PickRandomGridPosition() {
    if (gridPositions.Count == 0) {
      return null;
    }

    int randomIndex = UnityEngine.Random.Range(0, gridPositions.Count);
    Vector3 randomPosition = gridPositions[randomIndex];
    gridPositions.RemoveAt(randomIndex);
    return randomPosition;
  }

  public int LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
    int objectcount = UnityEngine.Random.Range(minimum, maximum + 1);
    int spawnedObjectCount = 0;
    for (int i = 0; i < objectcount; i++) {
      Vector3? randomPosition = PickRandomGridPosition();
      if (randomPosition == null) {
        break;
      }
      GameObject tileChoice = tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
      Instantiate(tileChoice, (Vector3)randomPosition, Quaternion.identity);
      spawnedObjectCount++;
    }

    return spawnedObjectCount;
  }

  private Vector3Int PickRandomWallPosition() {
    var randomX = UnityEngine.Random.Range(-1, columns + 1);
    int randomY;
    if (randomX >= 0 && randomX < columns) {
      var availablePositions = new int[] { -1, rows };
      randomY = availablePositions[UnityEngine.Random.Range(0, availablePositions.Length)];
    }
    else {
      randomY = UnityEngine.Random.Range(0, rows);
    }

    return new Vector3Int(randomX, randomY, 0);
  }

  public void SetupScene(int level) {
    CleanUp();
    BoardSetup();
    InitializeGridPositions();

    //PlaceEnemies(level);
    PlaceFood(level);
    PlaceCamera();
    PlacePlayer();
    PlaceExit();
    isGameRunning = true;
  }

  private void CleanUp() {
    if (map != null) {
      map.Clear();
    }

  }

  public void PlaceCamera() {
    camera.transform.position = new Vector3(columns / 2f, rows / 2f, -10f);
  }

  public void PlaceEnemies(int level) {
    int enemyCount = (int)Mathf.Log(level, 2f);
    LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
  }

  public void PlaceExit() {
    var position = PickRandomWallPosition();
    exit = Instantiate(exitPrefab, position, Quaternion.identity);
    Debug.Log("Exit position " + position);
    Debug.Log("Wall position " + map.GetPosition(position.x, position.y).transform.position);
    exit.GetComponent<ExitComponent>().exitWall = map.GetPosition(position.x, position.y);
  }

  public void PlaceFood(int level) {
    int maxFoodCount = 20 * (1 / level);
    foodRemaining = LayoutObjectAtRandom(foodTiles, 1, maxFoodCount);
  }

  public void PlacePlayer() {
    Instantiate(player, new Vector3(columns / 2f, -1, 0f), Quaternion.identity);
  }
  public void Update() {
    //if (foodRemaining == 0) {
    //  exit.GetComponent<SpriteRenderer>().enabled = true;
    //  var exitPosition = exit.transform.position;
    //  Destroy(map[(int)exitPosition.x][(int)exitPosition.y]);
    //}
  }
}
