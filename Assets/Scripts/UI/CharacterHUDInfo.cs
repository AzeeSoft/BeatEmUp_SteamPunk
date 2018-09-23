using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHUDInfo : MonoBehaviour
{
    public Image image;
    public Slider healthSlider;
    public Slider spiritSlider;

    [SerializeField]
    private CharacterModel characterModel;
    private CharacterCombatController characterCombatController;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (characterCombatController)
        {
            spiritSlider.value = HelperUtilities.Remap(characterCombatController.spirit, 0,
                characterCombatController.maxSpirit, spiritSlider.minValue, spiritSlider.maxValue);

            if (characterCombatController.spirit > characterCombatController.minSpiritConsumption)
            {
                spiritSlider.interactable = true;
            }
            else
            {
                spiritSlider.interactable = false;
            }
        }
    }

    public void AttachCharacter(CharacterModel incomingCharacterModel)
    {
        characterModel = incomingCharacterModel;
        image.sprite = characterModel.CharacterInfo.image;

        CharacterHealthController characterHealthController = characterModel.GetComponent<CharacterHealthController>();
        characterHealthController.onHealthChanged += (oldHealth, health) =>
        {
            healthSlider.value = HelperUtilities.Remap(health, 0, characterHealthController.maxHealth,
                healthSlider.minValue, healthSlider.maxValue);
        };

        characterCombatController = characterModel.GetComponent<CharacterCombatController>();
    }
}