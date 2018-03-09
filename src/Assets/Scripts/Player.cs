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
  private bool isStarted = false;
  private bool isAttemptingMove = false;
  private bool isParalyzed = false;
  private Vector2 inputVector = new Vector2(0, 1f);
  private Vector2 currentVector = new Vector2(0, 1f);
  private List<GameObject> body = new List<GameObject>();
  
  // Used for debugging 
  int counter = 0;

  protected override void Awake() {
    InitBody();
    base.Awake();
  }

  void InitBody() {
    body.Add(this.gameObject);
    for (var i = 0; i < bodyLength - 2; i++) {
      var bodySegment = Instantiate(bodySegmentPrefab, new Vector3(transform.position.x, transform.position.y - (i + 1), 0f), Quaternion.identity);
      bodySegment.GetComponent<SpriteRenderer>().sprite = bodySprite;
      body.Add(bodySegment);
    }
    AddTail();
  }

  void Update() {
    if (!isStarted && Input.GetKeyDown(KeyCode.Space)) {
      isStarted = true;
    }

    if (!isStarted) {
      return;
    }

    var newInputVector = new Vector2Int(System.Math.Sign(Input.GetAxis("Horizontal")), System.Math.Sign(Input.GetAxis("Vertical")));
    var nextInputVector = newInputVector;

    if (newInputVector.y != 0 && nextInputVector.x != 0) {
      newInputVector.x = 0;
      nextInputVector.y = 0;
    }

    //if (isMoving && newInputVector != Vector2.zero && newInputVector != inputVector && newInputVector != currentVector) {
      //Debug.Log(counter + "queue " + newInputVector);
    //}

    if (newInputVector != Vector2.zero && newInputVector * -1 != currentVector) {
      inputVector = newInputVector;
    }
    if (isStarted && !isMoving && !isAttemptingMove) {
      AttemptMove<BlockingObject>((int)inputVector.x, (int)inputVector.y);

      if (nextInputVector != Vector2.zero) {
        inputVector = nextInputVector;
      }
    }
  }

  protected override void AttemptMove<T>(int xDir, int yDir) {
    if (isParalyzed) {
      return;
    }
    isAttemptingMove = true;
    GetComponent<CurrentDirection>().SetDirection(xDir, yDir);
    base.AttemptMove<T>(xDir, yDir);
    RaycastHit2D hit;

    CheckIfGameOver();
    isAttemptingMove = false;
  }

  private void CheckIfGameOver() {
    if (bodyLength == 0) {
      Debug.Log("Game over");

    }
  }

  protected override void OnMove(Vector3 previousPosition) {
    //Debug.Log(counter++ + "move " + inputVector.x + " " + inputVector.y);
    
    currentVector = inputVector;
    //if (previousPosition == transform.position) {
    //  return;
    //}

    GetComponent<PreviousPosition>().position = previousPosition;
    if (body.Count == 1) {
      return;
    }

    if (GetComponent<AteFoodComponent>()) {
      Grow();
    }

    for (var i = 1; i < body.Count; i++) {
      var segment = body[i];
      var previousSegment = body[i - 1];
      var previousSegmentPosition = previousSegment.GetComponent<PreviousPosition>().position;
      segment.GetComponent<BodySegment>().SlaveMove((Vector3)previousSegmentPosition, moveTime);
    }
  }

  private void Grow() {
    if (body.Count == 1) {
      AddTail();
    } else {
      var tailSegment = body[body.Count - 1];
      GrowBodySegment(tailSegment.transform);
      Debug.Log("insert at " + tailSegment.transform.position);
    }
    bodyLength = body.Count;
    Destroy(GetComponent<AteFoodComponent>());
  }

  private void GrowBodySegment(Transform transform) {
    var bodySegment = Instantiate(bodySegmentPrefab, transform.position, transform.rotation);
    bodySegment.GetComponent<SpriteRenderer>().sprite = bodySprite;
    bodySegment.GetComponent<PreviousPosition>().position = transform.position;
    bodySegment.GetComponent<BodySegment>().showSpriteWhileMoving = true;

    body.Insert(body.Count - 1, bodySegment);
  }

  private void AddTail() {
    var tailSegment = Instantiate(bodySegmentPrefab, new Vector3(transform.position.x, transform.position.y - (bodyLength - 1), 0f), Quaternion.identity);
    tailSegment.GetComponent<SpriteRenderer>().sprite = tailSprite;
    tailSegment.GetComponent<SpriteRenderer>().sortingOrder++;
    tailSegment.GetComponent<BodySegment>().showSpriteWhileMoving = false;
    body.Add(tailSegment);
  }

  protected override void OnCantMove<T>(T component) {
    if (component.tag == "wall") {
      isParalyzed = true;
      // TODO remove body segment
      // TODO disable movement
    }
  }

}
