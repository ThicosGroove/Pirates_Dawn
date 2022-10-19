using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private GameObject player;

    [Header("Camera Settings")]
    [SerializeField] float smoothSpeed = 0.125f;
    [Tooltip("Enemies will only spawn outside of player screen")]
    [SerializeField] Vector3 offset;

    private Vector3 camInitialPosition;

    private void OnEnable()
    {
        GameplayEvents.RestartGame += ReturnToInitialPosition;
        UtilityEvents.SpawnPlayerEvent += GetPlayer;
    }

    private void OnDisable()
    {
        GameplayEvents.RestartGame -= ReturnToInitialPosition;
        UtilityEvents.SpawnPlayerEvent -= GetPlayer;
    }

    private void GetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        ReturnToInitialPosition();
    }

    void ReturnToInitialPosition() => this.transform.position = camInitialPosition;

    private void Start()
    {
        camInitialPosition = this.transform.position;
    }

    void Update()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PLAYING) return;

        offset = new Vector3(offset.x, offset.y, -15);
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smootthedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        this.transform.position = smootthedPosition;
    }

}
