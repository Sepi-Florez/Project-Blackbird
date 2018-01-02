using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {
    public Transform player;
    Vector3 move;
    public float speed;

    void Update() {
        Movement();
        Control();
    }
    void Control() {

        move = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
    }
    void Movement() {
        player.Translate(move * speed * Time.deltaTime, Space.World) ;
    }

}
