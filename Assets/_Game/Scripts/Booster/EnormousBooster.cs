using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnormousBooster : AbstractBooster
{
    protected override void TriggerBoost(AbstractCharacter character)
    {
        base.TriggerBoost(character);

        character.EnormousEnhance();
    }
}
