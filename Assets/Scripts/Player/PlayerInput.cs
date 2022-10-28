using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameEvents;

public class PlayerInput : MonoBehaviour
{
    [Header("All Weapons")]
    [SerializeField] Cannon Frontcannon;
    [SerializeField] Cannon[] Sidewaycannon;
    [SerializeField] ExplosiveMines explosiveMines;

    [Header("Fire Rate of Each Wapon")]
    [SerializeField] float frontCannonFireRate;
    [SerializeField] float SidewayCannonFireRate;
    [SerializeField] float BackMineFireRate;

    DefaultPlayerInput inputs;

    private void Awake()
    {
        inputs = new DefaultPlayerInput();
    }

    private void Start()
    {
        inputs.InGame.Pause.started += _ => PauseInput();

        Frontcannon.fireRate = frontCannonFireRate;

        foreach (var sidewayCannon in Sidewaycannon)
        {
            sidewayCannon.fireRate = SidewayCannonFireRate;
        }

        explosiveMines.fireRate = BackMineFireRate;
    }

    private void OnEnable()
    {
        inputs.InGame.Enable();
    }

    private void OnDisable()
    {
        inputs.InGame.Disable();
    }

    void Update()
    {
        if (GameplayManager.Instance.currentGameState == GameStates.PLAYING)
        {
            HandleShooting();
        }

        if (GameplayManager.Instance.currentGameState == GameStates.PLAYING
            || GameplayManager.Instance.currentGameState == GameStates.PAUSED)
        {
            PauseInput();
        }
    }

    private void HandleShooting()
    {
        float tryShootForward = inputs.InGame.ShootForward.ReadValue<float>();
        float tryShootBack = inputs.InGame.ShootBack.ReadValue<float>();
        float tryShootSideways = inputs.InGame.ShootSideways.ReadValue<float>();

        if (tryShootForward > 0.1f)
        {
            Frontcannon.Shoot();
        }
        else
        {
            Frontcannon.StopShoot();
        }

        if (tryShootBack > 0.1f)
        {
            explosiveMines.Shoot();
        }
        else
        {
            explosiveMines.StopShoot();
        }

        if (tryShootSideways > 0.1f)
        {
            foreach (var cannon in Sidewaycannon)
            {
                cannon.Shoot();
            }
        }
        else
        {
            foreach (var cannon in Sidewaycannon)
            {
                cannon.StopShoot();
            }
        }
    }

    private void PauseInput()
    {
        bool pressESC = inputs.InGame.Pause.triggered;

        if (pressESC)
        {
            UtilityEvents.OnGamePauseToggle();
        }
    }
}
