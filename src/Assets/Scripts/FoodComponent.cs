using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodComponent : MonoBehaviour {
  private void OnTriggerEnter2D(Collider2D collision) {
    collision.gameObject.AddComponent<AteFoodComponent>();
    Destroy(gameObject);
    GameManager.Instance.boardManager.foodRemaining--;
  }
}
