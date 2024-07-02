using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class EnormousBooster : AbstractBooster
{
    [SerializeField] private BoosterConfigSO config;

    protected override void TriggerBoost(AbstractCharacter character)
    {
        base.TriggerBoost(character);

        EnormousEnhance(character);
    }

    private void EnormousEnhance(AbstractCharacter character)
    {
        character.isBoosted = true;
        character.boostedType = BoostType.EnormousBoost;

        character.CheckBoost();

        character.scaleRatio = config.Stat;
        character.OnScaleRatioChanges();
    }
}
