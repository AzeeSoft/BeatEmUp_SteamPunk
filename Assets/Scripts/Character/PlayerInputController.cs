using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerInputController : CharacterInputController
{
    public enum ControllerType
    {
        Joystick,
        KeyboardAndMouse
    }

    public ControllerType controllerType = ControllerType.Joystick;

    public override void UpdateCharacterInput()
    {
        UpdateMoveInput();
        UpdateFacingDirInput();
        UpdateBlockInput();
        UpdateAttackInput();
    }

    void UpdateMoveInput()
    {
        characterInput.horizontal = Input.GetAxis("Horizontal");
        characterInput.vertical = Input.GetAxis("Vertical");
        characterInput.dodge = Input.GetButtonDown("Dodge");

        Vector2 tester = new Vector2(characterInput.horizontal, characterInput.vertical);
        if (tester.magnitude > 1)
        {
            tester.Normalize();
            characterInput.horizontal = tester.x;
            characterInput.vertical = tester.y;
        }
    }

    void UpdateFacingDirInput()
    {
        ArenaManager arenaManager = ArenaManager.Instance;

        Vector3 cameraForward = arenaManager.arenaCamera.transform.forward;
        Vector3 cameraRight = arenaManager.arenaCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        if (controllerType == ControllerType.Joystick)
        {
            float horRot = Input.GetAxis("Rotation Horizontal");
            float verRot = Input.GetAxis("Rotation Vertical");

            characterInput.facingDir =
                (cameraRight * horRot) +
                (cameraForward * verRot);
            characterInput.facingDir.y = 0;
        }
        else if (controllerType == ControllerType.KeyboardAndMouse)
        {
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Ray ray = arenaManager.arenaCamera.GetComponent<Camera>().ScreenPointToRay(pos);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, float.MaxValue,
                LayerMask.NameToLayer("MousePositionDetector")))
            {
                Vector3 targetPos = raycastHit.point;
                targetPos.y = GetComponentInParent<CharacterMovementController>().transform.position.y;

                characterInput.facingDir = targetPos - transform.position;
            }
        }
    }

    void UpdateBlockInput()
    {
        characterInput.block = Input.GetButton("Block");
    }

    void UpdateAttackInput()
    {
        characterInput.lightAttack = Input.GetButton("Fire1");
        characterInput.specialAttackCharge = Input.GetButton("Fire2");
        characterInput.specialAttack = Input.GetButtonUp("Fire2");
    }
}