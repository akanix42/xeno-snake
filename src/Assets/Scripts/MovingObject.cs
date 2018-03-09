using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {
  public GameObject smoothMovementSpritePrefab;

  public float moveTime = .1f;
  private float oldMoveTime;
  public LayerMask blockingLayer;

  private BoxCollider2D boxCollider;
  private Rigidbody2D rb2d;
  private float inverseMoveTime;
  protected bool isMoving = false;
  public bool showSpriteWhileMoving;
  //public bool showSpriteWhileMoving {
  //  get { return _showSpriteWhileMoving; }
  //  set {
  //    _showSpriteWhileMoving = value;
  //    smoothMovementSprite.GetComponent<SpriteRenderer>().enabled = value;
  //  }
  //}
  private GameObject smoothMovementSprite;
  private SpriteRenderer smoothMovementSpriteRenderer;
  // Use this for initialization
  protected virtual void Awake() {
    boxCollider = GetComponent<BoxCollider2D>();
    rb2d = GetComponent<Rigidbody2D>();
    inverseMoveTime = 1f / moveTime;
    oldMoveTime = moveTime;

    smoothMovementSprite = Instantiate(smoothMovementSpritePrefab, transform.position, transform.rotation);
    smoothMovementSpriteRenderer = smoothMovementSprite.GetComponent<SpriteRenderer>();
    smoothMovementSpriteRenderer.sortingOrder = -1;
    smoothMovementSpriteRenderer.enabled = showSpriteWhileMoving;
  }

  private void Update() {
    if (oldMoveTime != moveTime) {
      inverseMoveTime = 1f / moveTime;
    }
    if (smoothMovementSpriteRenderer.enabled != showSpriteWhileMoving) {
      smoothMovementSpriteRenderer.enabled = showSpriteWhileMoving;
    }
  }

  protected virtual void AttemptMove<T>(int xDir, int yDir)
    where T : Component {
    if (isMoving) {
      return;
    }

    RaycastHit2D hit;
    bool canMove = Move(xDir, yDir, out hit);

    if (hit.transform == null) {
      return;
    }

    T hitComponent = hit.transform.GetComponent<T>();

    if (!canMove && hitComponent != null) {
      OnCantMove(hitComponent);
    }
  }

  protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
    Vector2 start = transform.position;
    Vector3 end = start + new Vector2(xDir, yDir);

    boxCollider.enabled = false;
    hit = Physics2D.Linecast(start, end, blockingLayer);
    boxCollider.enabled = true;

    var outOfBoundsComponent = GetComponent<StartsOutOfBoundsComponent>();
    if (outOfBoundsComponent || hit.transform == null) {
      if (outOfBoundsComponent) {
        Destroy(outOfBoundsComponent);
      }

      StartCoroutine(SmoothMovement(end));

      OnMove(start);
      return true;
    }
    return false;
  }

  protected IEnumerator SmoothMovement(Vector3 end) {
    isMoving = true;
    smoothMovementSprite.transform.position = transform.position;
    smoothMovementSprite.transform.rotation = transform.rotation;

    //smoothMovementSprite.GetComponent<SpriteRenderer>().enabled = showSpriteWhileMoving;
    float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

    while (sqrRemainingDistance > float.Epsilon) {
      Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
      rb2d.MovePosition(newPosition);
      sqrRemainingDistance = (transform.position - end).sqrMagnitude;
      yield return null;
    }

    //if (showSpriteWhileMoving) {
    //  smoothMovementSprite.GetComponent<SpriteRenderer>().enabled = false;
    //}
    isMoving = false;
  }

  protected abstract void OnMove(Vector3 previousPosition);

  protected abstract void OnCantMove<T>(T component)
    where T : Component;
}
