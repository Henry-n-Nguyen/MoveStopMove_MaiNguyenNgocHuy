using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeBulletBooster : AbstractBooster
{
    protected override void TriggerBoost(AbstractCharacter character)
    {
        base.TriggerBoost(character);

        character.HugeBulletEnhance(2.5f);
    }
}
