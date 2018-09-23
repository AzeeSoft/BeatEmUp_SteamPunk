using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public float speed = 5;
    public float rotationSpeed = 3;
    public float dodgeSpeed = 10;
    public float dodgeDuration = 3;

    private CharacterModel _playerModel;
    private CharacterCombatController _characterCombatController;
    private CharacterInputController _characterInputController;
    private Animator _animator;

    private bool isDodging = false;
    private float dodgeStart = 0;
    private Vector3 dodgingDir;

    public bool IsDodging
    {
        get { return isDodging; }
    }

    void InitIfNeeded()
    {
        if (!_characterInputController)
        {
            _characterInputController = GetComponentInChildren<CharacterInputController>();
        }

        if (!_characterCombatController)
        {
            _characterCombatController = GetComponent<CharacterCombatController>();
        }
    }

    void Awake()
    {
        _playerModel = GetComponent<CharacterModel>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        InitIfNeeded();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDodging)
        {
            Dodge();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        if (!_characterInputController)
        {
            return;
        }

        Vector3 cameraForward = ArenaManager.Instance.arenaCamera.transform.forward;
        Vector3 cameraRight = ArenaManager.Instance.arenaCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        CharacterInputController.CharacterInput characterInput = _characterInputController.characterInput;

        float xDelta = characterInput.horizontal * speed;
        float zDelta = characterInput.vertical * speed;

        Vector3 movementDelta = (cameraRight * xDelta) + (cameraForward * zDelta);

        if (_characterCombatController.isBlocking || _characterCombatController.isChargingSpirit ||
            _characterCombatController.isSpecialAttackActive)
        {
            movementDelta = Vector3.zero;
        }

        Vector3 finalPos = transform.position + movementDelta;

        Vector3 newPos = Vector3.Lerp(transform.position, finalPos, Time.fixedDeltaTime);
        Quaternion newRotation = transform.rotation;

        if (!isDodging && characterInput.facingDir.magnitude > 0)
        {
            newRotation = Quaternion.LookRotation(characterInput.facingDir, transform.up);
        }

        Vector3 moveDir = transform.forward;
        if (movementDelta.magnitude > 0f)
        {
            moveDir = finalPos - transform.position;
            moveDir.y = transform.position.y;

            _animator.SetBool("isWalking", true);

            if (Vector3.Angle(transform.forward, movementDelta) > 90f)
            {
                _animator.SetBool("isWalkingBack", true);
            }
            else
            {
                _animator.SetBool("isWalkingBack", false);
            }
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }

        newRotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
        newRotation.x = 0;
        newRotation.z = 0;

        transform.position = newPos;
        transform.rotation = newRotation;

        if (characterInput.dodge && !_characterCombatController.isChargingSpirit &&
            !_characterCombatController.isSpecialAttackActive)
        {
            isDodging = true;
            dodgingDir = moveDir.normalized;
            dodgingDir.y = 0;
            dodgeStart = Time.time;

            _animator.SetTrigger("Roll");
        }
    }

    void Dodge()
    {
        Vector3 movementDelta = dodgingDir * dodgeSpeed;
        Vector3 finalPos = transform.position + movementDelta;
        Vector3 newPos = Vector3.Lerp(transform.position, finalPos, Time.fixedDeltaTime);

        Quaternion finalRotation = Quaternion.LookRotation(dodgingDir, transform.up);
//        Quaternion newRotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.fixedDeltaTime);
        Quaternion newRotation = finalRotation;

        transform.position = newPos;
        transform.rotation = newRotation;

        if (Time.time - dodgeStart > dodgeDuration)
        {
            isDodging = false;
        }
    }
}