using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
   public Transform connection;
   private void OnTriggerEnter2D(Collider2D col)
   {
      Vector3 position = col.transform.position;
      position.x = this.connection.position.x;
      position.y = this.connection.position.y;

      col.transform.position = position;
   }
   
}
