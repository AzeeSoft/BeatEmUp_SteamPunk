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

    private string suffix = "";

    void PrepareSuffix()
    {
        switch (controllerType)
        {
            case ControllerType.KeyboardAndMouse:
                suffix = "";
                break;
            case ControllerType.Joystick:
                suffix = "_J0";
                break;
        }
    }

    public override void UpdateCharacterInput()
    {
        PrepareSuffix();

        UpdateMoveInput();
        UpdateFacingDirInput();
        UpdateBlockInput();
        UpdateAttackInput();
    }

    void UpdateMoveInput()
    {
        characterInput.horizontal = Input.GetAxis("Horizontal" + suffix);
        characterInput.vertical = Input.GetAxis("Vertical" + suffix);
        characterInput.dodge = Input.GetButtonDown("Dodge" + suffix);

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
            float horRot = Input.GetAxis("Rotation Horizontal" + suffix);
            float verRot = Input.GetAxis("Rotation Vertical" + suffix);

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
        characterInput.block = Input.GetButton("Block" + suffix);
    }

    void UpdateAttackInput()
    {
        switch (controllerType)
        {
            case ControllerType.Joystick:
                float fire1_2 = Input.GetAxis("Fire1_2_J0");
                bool oldSpecialAttackCharge = characterInput.specialAttackCharge;

                characterInput.specialAttackCharge = fire1_2 < 0;
                characterInput.lightAttack = fire1_2 > 0;

                if (!characterInput.specialAttackCharge && oldSpecialAttackCharge)
                {
                    characterInput.specialAttack = true;
                }
                
                break;
            case ControllerType.KeyboardAndMouse:
                characterInput.lightAttack = Input.GetButton("Fire1" + suffix);
                characterInput.specialAttackCharge = Input.GetButton("Fire2" + suffix);
                characterInput.specialAttack = Input.GetButtonUp("Fire2" + suffix);
                break;
        }
    }
}