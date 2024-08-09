using UnityEngine;

public class AttackableObject : MonoBehaviour 
{
    public CustomMonoBehavior customMonoBehavior;

    void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    private void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.Mouse0)) Attack();
        if (Input.GetKey(KeyCode.Alpha1)) CastSpellShort();
        if (Input.GetKey(KeyCode.Alpha2)) CastSpellMiddle();
        if (Input.GetKey(KeyCode.Alpha3)) CastSpellLong();
    }

    public bool canAttack = true;
    public void Attack()
    {
        if (canAttack)
        {
            if (customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Attack)) {canAttack = false;}
        }
    }

    public void StopAttack()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Attack);
        canAttack = true;
    }

    public bool canCastSpellShort = true;
    public void CastSpellShort()
    {
        if (canCastSpellShort)
        {
            if (customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.CastSpellShort)) canCastSpellShort = false;
        }
    }

    public void StopCastSpellShort()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.CastSpellShort);
        canCastSpellShort = true;
    }

    public bool canCastSpellMiddle = true;
    public void CastSpellMiddle()
    {
        if (canCastSpellMiddle)
        {
            if (customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.CastSpellMiddle)) canCastSpellMiddle = false;
        }
    }

    public void StopCastSpellMiddle()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.CastSpellMiddle);
        canCastSpellMiddle = true;
    }

    public bool canCastSpellLong = true;
    public void CastSpellLong()
    {
        if (canCastSpellLong)
        {
            if (customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.CastSpellLong)) canCastSpellLong = false;
        }
    }

    public void StopCastSpellLong()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.CastSpellLong);
        canCastSpellLong = true;
    }
}