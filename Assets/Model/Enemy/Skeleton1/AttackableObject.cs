using UnityEngine;

public class AttackableObject : MonoBehaviour 
{
    public MyGameObject1 myGameObject;

    void Awake()
    {
        myGameObject = GetComponent<MyGameObject1>();
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
            if (myGameObject.humanLikeAnimatorBrain.ChangeState(State.Attack)) {canAttack = false;}
        }
    }

    public void StopAttack()
    {
        myGameObject.humanLikeAnimatorBrain.StopState(State.Attack);
        canAttack = true;
    }

    public bool canCastSpellShort = true;
    public void CastSpellShort()
    {
        if (canCastSpellShort)
        {
            if (myGameObject.humanLikeAnimatorBrain.ChangeState(State.CastSpellShort)) canCastSpellShort = false;
        }
    }

    public void StopCastSpellShort()
    {
        myGameObject.humanLikeAnimatorBrain.StopState(State.CastSpellShort);
        canCastSpellShort = true;
    }

    public bool canCastSpellMiddle = true;
    public void CastSpellMiddle()
    {
        if (canCastSpellMiddle)
        {
            if (myGameObject.humanLikeAnimatorBrain.ChangeState(State.CastSpellMiddle)) canCastSpellMiddle = false;
        }
    }

    public void StopCastSpellMiddle()
    {
        myGameObject.humanLikeAnimatorBrain.StopState(State.CastSpellMiddle);
        canCastSpellMiddle = true;
    }

    public bool canCastSpellLong = true;
    public void CastSpellLong()
    {
        if (canCastSpellLong)
        {
            if (myGameObject.humanLikeAnimatorBrain.ChangeState(State.CastSpellLong)) canCastSpellLong = false;
        }
    }

    public void StopCastSpellLong()
    {
        myGameObject.humanLikeAnimatorBrain.StopState(State.CastSpellLong);
        canCastSpellLong = true;
    }
}