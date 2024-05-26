using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Brogue.Bullet;
using UnityEngine.Animations.Rigging;
using Brogue.UI;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.Assertions.Must;

namespace Brogue.Player{
public class PlayerMovement : MonoBehaviour, BattleProperties
{
    // out logic properties
    [SerializeField] private LayerMask mainFloor;
    [SerializeField] private Camera mainPlayerCamera;
    [SerializeField] private Transform aimingTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootRate;
    [SerializeField] private NormalBullet bullet1;
    [SerializeField] private RigBuilder gunHoldRig;
    [SerializeField] private Material material;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color getHitColor;
    
    // battle properties
    public int playerMaxHp;
    public int currentHp;
    [Range(0, 25)]
    public int defense;

    private bool isDead;
    [HideInInspector]public bool inPoison;
    [HideInInspector]public int poisonLevel;
    [HideInInspector]public float movementAlter;
    private Coroutine playerInPoisonCoro;
    
    
    
    // input
    private float inputWS;
    private float inputAD;
    private Vector2 inputMousePosition;
    private bool inputMouseLeftButton;
    
    // logic properties
    private float currentSpeedWS;
    private float currentSpeedAD;
    private Vector3 aniSpeed;
    private bool canShoot;
    private float shootInterval;
    
    
    // animator params index
    private int movementXString;
    private int movementZString;
    private int fireMainGunString;
    private int deadString;
    
    private void Awake()
    {
        inputWS = 0;
        inputAD = 0;
        inputMousePosition = new Vector2(0, 0);
        
        currentSpeedWS = 0;
        currentSpeedAD = 0;
        aniSpeed = Vector3.zero;

        canShoot = true;
        shootInterval = 1f / shootRate;
        poisonLevel = -1;
        inPoison = false;
        playerInPoisonCoro = null;
        movementAlter = 1;
    }

    private void Start()
    {
        // ani params index setup
        movementXString = Animator.StringToHash("MovementX");
        movementZString = Animator.StringToHash("MovementZ");
        fireMainGunString = Animator.StringToHash("Fire1");
        deadString = Animator.StringToHash("Dead");
        
        
        // ani params setup
        animator.SetFloat(movementXString, currentSpeedWS);
        animator.SetBool(fireMainGunString, false);
        
    }

    private void Update()
    {
        useInput();
    }

    private void FixedUpdate()
    {
        characterController.SimpleMove(aniSpeed);
    }

    private void OnAnimatorMove()
    {
        // // 为了矫正左右移动的动画错误所加的
        // if (inputAD > 0 && inputWS == 0)
        // {
        //     if (inputAD > 0)
        //     {
        //         characterController.SimpleMove(new Vector3(
        //             animator.velocity.magnitude,0, 0));
        //     }
        //     else
        //     {
        //         characterController.SimpleMove(new Vector3(
        //             -animator.velocity.magnitude,0, 0));
        //     }
        //    
        // }
        // else
        // {
        // characterController.SimpleMove(animator.velocity);
        // }

        aniSpeed = animator.velocity;
        // Debug.Log(animator.velocity);
    }


    // use input

    private void useInput()
    {
        if (!isDead)
        {
            RotatePlayerByInput();
            MovePlayerByInput();
            FirePlayerMainGun();
        }
        
    }
    private void RotatePlayerByInput()
    {
        Ray _ray = mainPlayerCamera.ScreenPointToRay(inputMousePosition);
        Physics.Raycast(_ray, out var raycastHit, float.MaxValue, mainFloor);
        aimingTarget.position = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
        
        // dead zone for mouse rotating
        if (inputMousePosition.x > 500 && inputMousePosition.x < 555)
        {
            if (inputMousePosition.y > 316 && inputMousePosition.y < 350)
            {
                return;
                // Debug.Log(inputMousePosition);
            }
        }
        
        transform.LookAt(aimingTarget, Vector3.up);
    }


    private void MovePlayerByInput()
    {
        if (inputWS * inputAD != 0)
        {
            if (inputWS > 0)
            {
                currentSpeedWS = Mathf.Lerp(currentSpeedWS, (float)(2 / 0.707106), 0.5f);
                // currentSpeedWS = Mathf.Lerp(currentSpeedWS, (float)(2), 0.5f);

            }
            else if (inputWS < 0)
            {
                currentSpeedWS = Mathf.Lerp(currentSpeedWS, (float)(-1.75 / 0.707106), 0.5f);
                // currentSpeedWS = Mathf.Lerp(currentSpeedWS, (float)(-1.75), 0.5f);

            }
            else if (inputWS == 0)
            {
                currentSpeedWS = Mathf.Lerp(currentSpeedWS, 0, 0.9f);
            }

            if (inputAD > 0)
            {
                currentSpeedAD = Mathf.Lerp(currentSpeedAD, (float)(2 / 0.707106), 0.5f);
                // currentSpeedAD = Mathf.Lerp(currentSpeedAD, (float)(2), 0.5f);

            }
            else if (inputAD < 0)
            {
                currentSpeedAD = Mathf.Lerp(currentSpeedAD, (float)(-2 / 0.707106), 0.5f);
                // currentSpeedAD = Mathf.Lerp(currentSpeedAD, (float)(-2), 0.5f);

            }
            else if (inputAD == 0)
            {
                currentSpeedAD = Mathf.Lerp(currentSpeedAD, 0, 0.9f);
            }

        }
        else
        {
            if (inputWS > 0)
            {
                currentSpeedWS = Mathf.Lerp(currentSpeedWS, 2, 0.5f);

            }
            else if (inputWS < 0)
            {
                currentSpeedWS = Mathf.Lerp(currentSpeedWS, -1.75f, 0.5f);

            }
            else if (inputWS == 0)
            {
                currentSpeedWS = Mathf.Lerp(currentSpeedWS, 0, 0.9f);
            }
            
            if (inputAD > 0)
            {
                currentSpeedAD = Mathf.Lerp(currentSpeedAD, 2, 0.5f);

            }
            else if (inputAD < 0)
            {
                currentSpeedAD = Mathf.Lerp(currentSpeedAD, -2, 0.5f);

            }
            else if (inputAD == 0)
            {
                currentSpeedAD = Mathf.Lerp(currentSpeedAD, 0, 0.9f);
            }
            
        }

        animator.SetFloat(movementXString, currentSpeedWS * movementAlter);
        animator.SetFloat(movementZString, currentSpeedAD * movementAlter);
    }

    private void FirePlayerMainGun()
    {
        animator.SetBool(fireMainGunString, inputMouseLeftButton);
        
        // normal shoot
        if (inputMouseLeftButton)
        {
            if (canShoot)
            {
                // NormalBullet _bullet = Instantiate(bullet1, shootPosition.position, Quaternion.identity).GetComponent<NormalBullet>();
                NormalBullet _bullet = BulletPool.Instance.GetNormalBullet();
                _bullet.transform.position = shootPosition.position;
                _bullet.transform.rotation = Quaternion.identity;
                _bullet.InitBullet(transform.forward);
                canShoot = false;
                StartCoroutine(CoroCounterForShoot());
            }
        }

        
    }
    

    // receive input
    public void ReadInputWSAD(InputAction.CallbackContext context)
    {
        inputWS = context.ReadValue<Vector2>().y;
        inputAD = context.ReadValue<Vector2>().x;
        // Debug.Log(inputAD);
    } 


    public void ReadInputMouse(InputAction.CallbackContext context)
    {
        inputMousePosition = context.ReadValue<Vector2>();
    }

    public void ReadInputMouseLeftButton(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
            {
                inputMouseLeftButton = true;
                break;
            }
            case InputActionPhase.Canceled:
            {
                inputMouseLeftButton = false;
                break;
            }
        }
    }

    private IEnumerator CoroCounterForShoot()
    {
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }

    public void GetHit(int damage)
    {
        if (isDead)
        {
            return;
        }

        GetHitEffect();
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Dead();
        }
    }

    // both sound and visual
    private void GetHitEffect()
    {
        StartCoroutine(CoroGetHitChangeColor());
        SFXManager.Instance.PlayPlayerGetHitAudio();
    }

    private IEnumerator CoroGetHitChangeColor()
    {
        material.color = getHitColor;
        yield return new WaitForSeconds(0.4f);
        material.color = normalColor;
    }
    public int Defense()
    {
        return defense;
    }

    public void Dead()
    {
        if (!isDead)
        {
            Destroy(gunHoldRig);
            currentHp = 0;
            isDead = true;
            animator.SetBool(deadString, isDead);
            animator.SetBool(fireMainGunString, false);
            characterController.enabled = false;
            Debug.Log(GameManager.Instance.uiManager is null);
            GameManager.Instance.uiManager.ShowDefeatedUI();
            // UIManager.Instance.ShowDefeatedUI();
        }
        
    }

    // 玩家中两个毒时，伤害取最高值
    public void PlayerInPoison(int poisonLevel)
    {
        if (!inPoison)
        {
            inPoison = true;
            this.poisonLevel = poisonLevel;
            playerInPoisonCoro = StartCoroutine(CoroInPoison());
        }else if (poisonLevel > this.poisonLevel)
        {
            this.poisonLevel = poisonLevel;
        }
    }

    public void PlayerOutOfPoison(int poisonLevel)
    {
        if (inPoison && this.poisonLevel == poisonLevel)
        {
            inPoison = false;
            this.poisonLevel = -1;
            if (playerInPoisonCoro is not null)
            {
                StopCoroutine(playerInPoisonCoro);
                playerInPoisonCoro = null;
            }
           
        }
    }
    private IEnumerator CoroInPoison()
    {
        while (true)
        {
            GetHit(poisonLevel);
            yield return new WaitForSeconds(1f);
        }
    }
}
}