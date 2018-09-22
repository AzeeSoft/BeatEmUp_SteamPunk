using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerInputController : CharacterInputController
{
    public override void UpdateCharacterInput()
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
}