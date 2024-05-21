using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBooster : AbstractBooster
{
    protected override void TriggerBoost(AbstractCharacter character)
    {
        base.TriggerBoost(character);

        // +50 gold if u can kill someone
    }
}
