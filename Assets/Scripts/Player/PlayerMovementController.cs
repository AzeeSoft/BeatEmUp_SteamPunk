using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 5;

    private PlayerModel _playerModel;
    private PlayerInputController _playerInputController;

    void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _playerInputController = GetComponent<PlayerInputController>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    Vector3 cameraForward = _playerModel.arenaCamera.transform.forward;
	    Vector3 cameraRight = _playerModel.arenaCamera.transform.right;

	    cameraForward.y = 0;
	    cameraRight.y = 0;

	    PlayerInputController.PlayerInput playerInput = _playerInputController.playerInput;

	    float xDelta = playerInput.horizontal * speed;
	    float zDelta = playerInput.vertical * speed;

	    Vector3 movementDelta = (cameraRight * xDelta) + (cameraForward * zDelta);
	    Vector3 finalPos = transform.position + movementDelta;
        
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.fixedDeltaTime);
	}
}
