using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 60f;

    [SerializeField]
    private float rigidbodyDrag = 6f;

    [SerializeField]
    private float lookSensitivity = 100;

    [SerializeField]
    private float viewXLimit = 50f;

    [SerializeField]
    private float viewYLimit = 60f;

    enum LookState { Locked, Free };
    LookState currentState = LookState.Locked;

    private float verticalMovement;
    private Vector3 moveDirection;
    private new Rigidbody rigidbody;
    private Camera cam;

    private float xRotation, yRotation;

    //Base position and rotation describe the initial position and rotation of the player at startup
    private Vector3 basePosition;
    private Vector3 baseRotation;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = rigidbodyDrag;

        cam = GetComponentInChildren<Camera>();

        basePosition = transform.position;
        baseRotation = transform.rotation.eulerAngles;

        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        //player movement
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * verticalMovement;

        //player rotation
        float lookX = Input.GetAxisRaw("Look X");
        float lookY = 0;

#if UNITY_EDITOR
        //enable full mouse controls when in the editor, for ease of testing
        lookX += Input.GetAxisRaw("Look X Debug");
        lookY += Input.GetAxisRaw("Look Y Debug");
#endif


        yRotation += lookX * lookSensitivity * Time.deltaTime;
        xRotation -= lookY * lookSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -viewXLimit, viewXLimit);

        if (currentState == LookState.Locked)
            yRotation = Mathf.Clamp(yRotation, baseRotation.y - viewYLimit, baseRotation.y + viewYLimit);

        // up and down
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // left and right
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
    }

    /// <summary>
    /// Resets the player to the initial position and rotation
    /// </summary>
    public void ResetPlayer()
    {
        LockCamera();

        transform.position = basePosition;
        xRotation = baseRotation.x;
        yRotation = baseRotation.y;
    }

    /// <summary>
    /// Gets the forward vector of the player's initial rotation
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBaseForwardVector()
    {
        return Quaternion.Euler(baseRotation) * Vector3.forward;
    }

    public void LockCamera()
    {
        currentState = LookState.Locked;
    }

    public void UnlockCamera()
    {
        currentState = LookState.Free;
    }
}
