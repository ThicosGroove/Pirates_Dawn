using UnityEngine;
using UnityEngine.InputSystem;
using GameEvents;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private float rotationSpeed = 720f;

    private void OnEnable()
    {
        GameplayEvents.RestartGame += RespawnPlayer;
    }

    private void OnDisable()
    {
        GameplayEvents.RestartGame -= RespawnPlayer;

    }

    void RespawnPlayer()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PLAYING) return;

        Move();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 MovementDirection = new Vector3(horizontalInput, verticalInput, 0);
        float inputMagnetude = Mathf.Clamp01(MovementDirection.magnitude);
        MovementDirection.Normalize();

        //transform.Translate(MovementDirection * movespeed * inputMagnetude * Time.deltaTime, Space.World);
        transform.position += MovementDirection * moveSpeed * Time.deltaTime;

        if (MovementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, -MovementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
