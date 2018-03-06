using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

  public Sprite headSprite;
  public Sprite bodySprite;
  public Sprite tailSprite;
  
  private float moveSpeed = 5f;
  private float gridSize = 1f;

  private bool isMoving = false;
  private bool isStarted = false;

  private Vector2 inputVector = new Vector2(0, 1f);

  private Rigidbody2D rBody; 
  // Use this for initialization
  void Start () {
    rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
    var newInputVector = new Vector2(System.Math.Sign(Input.GetAxis("Horizontal")), System.Math.Sign(Input.GetAxis("Vertical")));
    if (newInputVector != Vector2.zero) {
      inputVector = newInputVector;
    }

    if (Input.GetKeyDown(KeyCode.Space)) {
      Debug.Log("isStarted");
      isStarted = true;
    }
    if (isStarted && !isMoving) {
      StartCoroutine(Move(inputVector, transform));

    }
  }

  //private void FixedUpdate() {
  //  if (isStarted) {
  //    Move2(inputVector, transform);
  //  }
  //}

  IEnumerator Move(Vector2 inputVector, Transform transform) {
    isMoving = true;
    var startPosition = transform.position;
    //var speed = 10;
    //var movementIncrement = speed * Time.deltaTime;

    var endPosition = new Vector3(CalculateTargetCoordinateX(startPosition.x, inputVector.x), CalculateTargetCoordinateY(startPosition.y, inputVector.y), startPosition.z);

    var distanceMoved = 0f;
    while (distanceMoved < 1f) {
      distanceMoved += moveSpeed * Time.deltaTime;
      transform.position = Vector3.Lerp(startPosition, endPosition, distanceMoved);
      yield return null;
    }

    //while (transform.position.x != targetX || transform.position.y != targetY) {
    //  var movementVector = new Vector2();
    //  movementVector.x = CalculateMovement(transform.position.x, targetX, movementIncrement);
    //  movementVector.y = CalculateMovement(transform.position.y, targetY, movementIncrement);

    //  transform.Translate(movementVector);
    //}
    isMoving = false;
    yield return 0;
  }

  //void Move2(Vector2 inputVector, Transform transform) {
  //  //isMoving = true;
  //  var startPosition = transform.position;
  //  //var speed = 10;
  //  var movementIncrement = moveSpeed * Time.fixedDeltaTime;

  //  var endPosition = new Vector2(CalculateTargetCoordinate(startPosition.x, inputVector.x), CalculateTargetCoordinate(startPosition.y, inputVector.y));

  //  var destinationVector = new Vector2(CalculateMovement(startPosition.x, inputVector.x, movementIncrement), CalculateMovement(startPosition.y, inputVector.y, movementIncrement));
  //  var targetVector = new Vector2(moveSpeed * inputVector.x, moveSpeed * inputVector.y);
  //  rBody.MovePosition(rBody.position + targetVector);
  //  //GetComponent<Rigidbody2D>().MovePosition();
  //  //transform.Translate(destinationVector);
  //  //var distanceMoved = 0f;
  //  //while (distanceMoved < 1f) {
  //  //  distanceMoved += moveSpeed * Time.deltaTime;
  //  //  transform.position = Vector3.Lerp(startPosition, endPosition, distanceMoved);
  //  //  yield return null;
  //  //}

  //  //while (transform.position.x != targetX || transform.position.y != targetY) {
  //  //  var movementVector = new Vector2();
  //  //  movementVector.x = CalculateMovement(transform.position.x, targetX, movementIncrement);
  //  //  movementVector.y = CalculateMovement(transform.position.y, targetY, movementIncrement);

  //  //  transform.Translate(movementVector);
  //  //}
  //  //isMoving = false;
  //  //yield return 0;
  //}

  float CalculateTargetCoordinate(float currentCoordinate, float directionCoordinate, float minValue, float maxValue) {
    return Mathf.Clamp(currentCoordinate + directionCoordinate, minValue, maxValue);
  }

  float CalculateTargetCoordinateX(float currentCoordinate, float directionCoordinate) {
    return CalculateTargetCoordinate(currentCoordinate, directionCoordinate, 0, 12);
  }

  float CalculateTargetCoordinateY(float currentCoordinate, float directionCoordinate) {
    return CalculateTargetCoordinate(currentCoordinate, directionCoordinate, 0.25f, 9.5f);
  }

  float CalculateMovement(float currentCoordinate, float directionCoordinate, float movementIncrement) {
    return directionCoordinate == 0 ? 0 : movementIncrement * directionCoordinate;
  }

  int GetDirectionModifier(float currentUnit, int targetUnit) {
    return currentUnit < targetUnit ? 1 : -1;
  }
}
