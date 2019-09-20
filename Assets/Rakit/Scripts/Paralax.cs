using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{

  SpriteRenderer render;
  Material spriteMaterial;

  [Range(0, 1)]
  public float speed;
  private void Awake()
  {
    render = GetComponent<SpriteRenderer>();
    spriteMaterial = render.material;
  }

  Vector2 offset = Vector2.zero;
  void Update()
  {
    if (!spriteMaterial)
      return;

    offset.x = transform.position.x * speed;
    spriteMaterial.SetTextureOffset("_MainTex", offset);
  }
}
