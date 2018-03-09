using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodComponent : MonoBehaviour {
  private void OnTriggerEnter2D(Collider2D collision) {
    Debug.Log("trigger");
    collision.gameObject.AddComponent<AteFoodComponent>();
    Destroy(gameObject);
  }
}
