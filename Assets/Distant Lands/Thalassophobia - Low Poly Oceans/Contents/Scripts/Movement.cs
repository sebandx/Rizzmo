using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Transform t;

    [Header("Player Rotation")]
    public float sensitivity = 1;

    public float rotationMin;
    public float rotationMax;

    // mouse input variables
    float rotationX;
    float rotationY;

    [Header("Player Movement")]
    public float speed = 1;
    float moveX;
    float moveY;
    float moveZ;

    [Header("Idle Bobbing")]
    public float bobbingAmount = 0.05f;
    public float bobbingSpeed = 3f;
    private bool isBobbing = false;
    private Coroutine bobbingCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        LookAround();

        if (Input.GetKey(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void LookAround() {
        // get the mouse input
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;

        // clamp the y rotation
        rotationY = Mathf.Clamp(rotationY, rotationMin, rotationMax);
        
        // setting the rotation value every update
        t.localRotation = Quaternion.Euler(-rotationY, rotationX, 0);
    }

    void Move() {
        // get the movement input

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveZ = Input.GetAxis("Forward");
 
        // move the character (swimming version)

        t.Translate(new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed);
        t.Translate(new Vector3(0, moveY, 0) * Time.deltaTime * speed, Space.World);
    }

    private void FixedUpdate() {
        Move();

        // Check if the player is idle and start/stop bobbing
        if (IsPlayerIdle() && !isBobbing) {
            isBobbing = true;
            bobbingCoroutine = StartCoroutine(Bobbing());
        } else if (!IsPlayerIdle() && isBobbing) {
            isBobbing = false;
            StopCoroutine(bobbingCoroutine);
        }
    }

    private bool IsPlayerIdle() {
        return moveX == 0 && moveY == 0 && moveZ == 0;
    }

    IEnumerator Bobbing() {
        float startY = t.position.y;
        float timer = 0;

        while (isBobbing) {
            timer += Time.deltaTime * bobbingSpeed;
            float newY = startY + Mathf.Sin(timer) * bobbingAmount;
            t.position = new Vector3(t.position.x, newY, t.position.z);
            yield return null;
        }

        // Reset position when bobbing stops
        t.position = new Vector3(t.position.x, startY, t.position.z);
    }

}
