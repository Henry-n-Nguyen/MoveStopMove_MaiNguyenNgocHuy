using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class HugeBulletBooster : AbstractBooster
{
    protected override void TriggerBoost(AbstractCharacter character)
    {
        base.TriggerBoost(character);

        HugeBulletEnhance(character);
    }

    public void HugeBulletEnhance(AbstractCharacter character)
    {
        character.isBoosted = true;
        character.isHugeBulletBoosted = true;

        character.boostedType = BoostType.HugeBulletBoost;

        character.CheckBoost();
    }
}
