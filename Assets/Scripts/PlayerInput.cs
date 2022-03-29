using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    //private InputAction.CallbackContext temp;
    public float jumpHeight = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public int shoot_rate = 5;
    public GameObject bulletPrefab;

    [SerializeField] private Camera mainCamera;

    private Vector2 prevMousePos;
    private bool shoot_held = false;
    private float shoot_elapsed = 0f;
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.ShootHold.started += Shoot_Start;
        playerInputActions.Player.ShootHold.canceled += Shoot_End;
        playerInputActions.Player.ShootSingle.performed += Shoot_Single;

        prevMousePos = new Vector2(0f, 0f);
        //playerInputActions.Player.Aim.performed += Mouse_Aim;
        //playerInputActions.Player.Movement.performed += Movement_Performed;
    }

    void FixedUpdate()
    {
        Vector2 movementVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        playerRigidbody.AddForce(new Vector3(movementVector.x, 0f, movementVector.y) * moveSpeed * 5);
        Vector2 rotateVector = playerInputActions.Player.Rotate.ReadValue<Vector2>();
        transform.Rotate(new Vector2(0, rotateVector.x) * rotateSpeed);

        Vector2 mousePos = playerInputActions.Player.Aim.ReadValue<Vector2>();
        Vector2 mouseDelta = mousePos - prevMousePos;
        //Debug.Log(mouseDelta);
        if (mouseDelta != Vector2.zero)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(ray, out rayLength))
            {
                Vector3 pointToLook = ray.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
        prevMousePos = mousePos;

        if (shoot_held) Shoot_Hold();
    }

    public void Shoot_Hold()
    {
        //Debug.Log("Fire!");
        shoot_elapsed += Time.deltaTime;
        if (shoot_elapsed >= 1f/shoot_rate)
        {
            shoot_elapsed = 0;
            Spawn_Bullet();
        }
    }

    public void Spawn_Bullet()
    {
        //Debug.Log("Fire~~");
        GameObject bullet = Instantiate(bulletPrefab);
        GameObject gun = GameObject.FindGameObjectWithTag("Gun");
        Vector3 spawnPoint = gun.transform.position + (gun.transform.rotation * new Vector3(0f, 1f, 0f));
        bullet.transform.position = spawnPoint;
        bullet.transform.rotation = transform.rotation * Quaternion.Euler(90,0,0);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -2, ForceMode.Impulse);

        //Vector3(-1.43173862, 1.2820003, -8.21416473)
    }

    public void Shoot_Single(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Spawn_Bullet();
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump" + context.phase);
        if (context.performed)
        {
            playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void Shoot_Start(InputAction.CallbackContext context)
    {
        //Debug.Log("Shoot" + context.phase);
        shoot_held = true;
    }

    public void Shoot_End(InputAction.CallbackContext context)
    {
        //Debug.Log("Shoot" + context.phase);
        shoot_held = false;
    }


}
