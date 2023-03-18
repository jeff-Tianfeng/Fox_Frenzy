using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDrag : MonoBehaviour
{
    private float rotateIntensity = 0.3f;
    private int verticalMovement = 0; 
    private new Rigidbody rigidbody;
    private Camera cam;
    private Vector3 moveDirection;
    private float horizontal;
    private float vertical;
    private float moveSpeed = 40f;
    private float rigidbodyDrag = 4f;

    public FoxSoundController foxSoundController;
    public GameStateController gameStateController;
 

    // Start is called before the first frame update
    void Start()
    {
        foxSoundController.setIsActivate(true);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = rigidbodyDrag;
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateController.Instance.CurrentState == GameStateController.State.Game)
        {
            foxSoundController.setIsActivate(true);
            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;
                transform.Rotate(Vector3.down * deltaPos.x * rotateIntensity);//rotate around y axis.
            }
            if (2 == Input.touchCount)
            {
                foxSoundController.setIsActivate(false);
                Touch touch = Input.GetTouch(0);
                moveDirection = transform.forward * 1;
                rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
            }
        }
    }
}
