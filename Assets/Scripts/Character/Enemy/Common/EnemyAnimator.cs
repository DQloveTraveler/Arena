using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator
{
    #region field
    private readonly EnemyMover enemyMover = null;
    private readonly EnemyRotator enemyRotator = null;
    private readonly Animator animator = null;
    private readonly EffectGenerator[] effectGenerators;
    private readonly PhysicsAttackable[] attackables;
    #endregion

    #region constructor
    public EnemyAnimator(Animator animator, EnemyMover enemyMover, EnemyRotator enemyRotator, EffectGenerator[] effectGenerators, PhysicsAttackable[] attackables)
    {
        this.animator = animator;
        this.enemyMover = enemyMover;
        this.enemyRotator = enemyRotator;
        this.effectGenerators = effectGenerators;
        this.attackables = attackables;
    }
    #endregion

    #region public function
    public void Update()
    {
        float runFloat;
        if (enemyMover.IsBackking)
        {
            runFloat = -1;
        }
        else
        {
            runFloat = Mathf.Clamp(enemyMover.NavMeshVelocity, 0, 1);
            if (enemyRotator.IsRotating)
            {
                runFloat = 1;
            }
        }
        animator.SetFloat("Run", runFloat);
    }

    public void Attack1()
    {
        animator.SetBool("Attack1", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack1"));
    }
    public void Attack2()
    {
        animator.SetBool("Attack2", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack2"));
    }
    public void Attack3()
    {
        animator.SetBool("Attack3", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack3"));
    }
    public void Attack4()
    {
        animator.SetBool("Attack4", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack4"));
    }
    public void Attack5()
    {
        animator.SetBool("Attack5", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack5"));
    }
    public void Attack6()
    {
        animator.SetBool("Attack6", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack6"));
    }
    public void Attack7()
    {
        animator.SetBool("Attack7", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Attack7"));
    }
    public void TakeOff()
    {
        animator.SetBool("TakeOff", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("TakeOff"));

    }
    public void Land()
    {
        animator.SetBool("Land", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Land"));
    }
    public void GlideEnd()
    {
        animator.SetTrigger("GlideEnd");
    }
    public void Break()
    {
        animator.SetBool("Break", true);
        CoroutineHandler.StartStaticCoroutine(_AnimBoolFalse("Break"));
    }
    public void Die()
    {
        animator.SetTrigger("Die");
    }
    #endregion

    #region private enumerator
    private IEnumerator _AnimBoolFalse(string variableName)
    {
        for(int i = 0; i < 10; i++)
        {
            yield return null;
        }
        animator.SetBool(variableName, false);
    }
    #endregion


    //以下、アニメーションイベント用
    #region public animation event function
    public void SlideForward(float power)
    {
        enemyMover.AddForceForward(power);
    }
    public void Decelerate()
    {
        CoroutineHandler.StartStaticCoroutine(enemyMover._Decelerate(0.9f, 30));
    }
    public void GenerateEffect(int num)
    {
        effectGenerators[num].Generate();
    }
    public void ResetAttackedList(int num)
    {
        attackables[num].ResetAttackedList();
    }
    public void AttackableColliderOFF(int num)
    {
        attackables[num].ColliderOFF();
    }
    #endregion

}
