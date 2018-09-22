using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerInputController : MonoBehaviour
{
    public struct PlayerInput
    {
        public float horizontal;
        public float vertical;
    }

    public PlayerInput playerInput = new PlayerInput();

    private PlayerModel _playerModel;

    void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
    }

    void Update()
    {
        playerInput.horizontal = Input.GetAxis("Horizontal");
        playerInput.vertical = Input.GetAxis("Vertical");

        Vector2 tester = new Vector2(playerInput.horizontal, playerInput.vertical);
        if (tester.magnitude > 1)
        {
            tester.Normalize();
            playerInput.horizontal = tester.x;
            playerInput.vertical = tester.y;
        }
    }
}