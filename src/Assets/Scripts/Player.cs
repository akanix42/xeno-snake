using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject {

  public Sprite headSprite;
  public Sprite bodySprite;
  public Sprite tailSprite;
  public float restartLevelDelay = 1f;
  public GameObject bodySegmentPrefab;


  private int bodyLength = 2;
  private bool isStarted = true;
  private bool isParalyzed = false;
  private Vector2 inputVector = new Vector2(0, 1f);
  private List<GameObject> body = new List<GameObject>();

  // Use this for initialization
  void Start() {
    InitBody();
    base.Start();
  }

  void InitBody() {
    body.Add(this.gameObject);
    for (var i = 0; i < bodyLength - 1; i++) {
      var bodySegment = Instantiate(bodySegmentPrefab, new Vector3(transform.position.x, transform.position.y - 1, 0f), Quaternion.identity);
      bodySegment.GetComponent<SpriteRenderer>().sprite = bodySprite;
      body.Add(bodySegment);
    }
    //var tailSegment = Instantiate(bodySegmentPrefab, transform.position - new Vector3(inputVector.x, inputVector.y, 0f), Quaternion.identity);
    //tailSegment.GetComponent<SpriteRenderer>().sprite = tailSprite;
    //body.Add(tailSegment);

  }

  protected override void AttemptMove<T>(int xDir, int yDir) {
    if (isParalyzed) {
      return;
    }

    base.AttemptMove<T>(xDir, yDir);
    RaycastHit2D hit;

    CheckIfGameOver();

  }

  void Update() {
    var newInputVector = new Vector2(System.Math.Sign(Input.GetAxis("Horizontal")), System.Math.Sign(Input.GetAxis("Vertical")));
    if (newInputVector != Vector2.zero && newInputVector * -1 != inputVector) {
      inputVector = newInputVector;
    }

    if (Input.GetKeyDown(KeyCode.Space)) {
      Debug.Log("isStarted");
      isStarted = true;
    }
    if (isStarted && !isMoving) {
      Debug.Log("move " + inputVector.x + " " + inputVector.y);
      AttemptMove<BlockingObject>((int)inputVector.x, (int)inputVector.y);

    }
  }


  private void CheckIfGameOver() {
    if (bodyLength == 0) {
      Debug.Log("Game over");

    }
  }

  protected override void OnMove(Vector3 previousPosition) {
    GetComponent<PreviousPosition>().position = previousPosition;
    if (body.Count == 1) {
      return;
    }

    for (var i = 1; i < body.Count; i++) {
      var segment = body[i];
      var previousSegment = body[i - 1];
      segment.GetComponent<BodySegment>().SlaveMove(previousSegment);
      
    }
  }

  protected override void OnCantMove<T>(T component) {
    if (component.tag == "wall") {
      isParalyzed = true;
      // TODO remove body segment
      // TODO disable movement
    }
  }

}
