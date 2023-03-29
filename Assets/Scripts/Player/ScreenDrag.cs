using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDrag : MonoBehaviour
{
    [SerializeField]
    public FoxSoundController foxSoundController;

    [SerializeField]
    public GameStateController gameStateController;
    // Player riogidbody.
    private new Rigidbody rigidbody;
    // Camera.
    private Camera cam;

    private Vector3 moveDirection;
    // view rotate sensity.
    private float rotateIntensity = 0.3f;
    // default is zero to forbid vertical movement.
    private int verticalMovement = 0; 

    private float horizontal;

    private float vertical;
    // player move speed.
    private float moveSpeed = 30f;
    // drag force of friction.
    private float rigidbodyDrag = 4f;
 
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
            if (1 == Input.touchCount)// When one finger touch the screen.
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;
                transform.Rotate(Vector3.down * deltaPos.x * rotateIntensity);//rotate around y axis.
            }
            if (2 == Input.touchCount)// When two fingers touch the screen.
            {
                foxSoundController.setIsActivate(false);
                Touch touch = Input.GetTouch(0);
                moveDirection = transform.forward * 1;
                rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);// move towards to the camera's orientation.
            }
        }
    }
}
