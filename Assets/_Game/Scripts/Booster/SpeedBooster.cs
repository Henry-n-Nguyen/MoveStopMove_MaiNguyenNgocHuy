using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : AbstractBooster
{
    protected override void TriggerBoost(AbstractCharacter character)
    {
        base.TriggerBoost(character);

        character.SpeedUp(2.5f);
    }
}
