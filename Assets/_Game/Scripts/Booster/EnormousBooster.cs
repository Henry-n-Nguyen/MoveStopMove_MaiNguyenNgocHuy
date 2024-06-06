using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class EnormousBooster : AbstractBooster
{
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

        character.scaleRatio = 1.64f;
        character.OnScaleRatioChanges();
    }
}
