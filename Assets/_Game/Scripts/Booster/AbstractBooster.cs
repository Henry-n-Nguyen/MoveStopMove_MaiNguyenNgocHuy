using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBooster : MonoBehaviour
{
    const string TRIGGER_SPAWN = "spawn";

    [Header("Set-up")]
    [SerializeField] private Animator anim;

    private void OnEnable()
    {
        anim.ResetTrigger(TRIGGER_SPAWN);
        anim.SetTrigger(TRIGGER_SPAWN);

        StartCoroutine(SelfDetroy(15f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER))
        {
            AbstractCharacter character = other.gameObject.GetComponent<AbstractCharacter>();
            TriggerBoost(character);
            Despawn();
        }
    }

    protected virtual void TriggerBoost(AbstractCharacter character)
    {

    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator SelfDetroy(float time)
    {
        yield return new WaitForSeconds(time);
        Despawn();
    }
}
