using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField] [Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRendere;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealthSystme;
    private TopDownMovement _collidingMovement;

    protected override void Start()
    {
        base.Start();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage();
    }

    private Action OnDamage()
    {
        followRange = 100f;
        return null;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if(DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRendere.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        if(!receiver.CompareTag(targetTag))
        {
            return;
        }

        _collidingTargetHealthSystme = receiver.GetComponent<HealthSystem>();
        if(_collidingTargetHealthSystme != null)
        {
            _isCollidingWithTarget = true;
        }

        _collidingMovement = receiver.GetComponent<TopDownMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag))
        {
            return;
        }

        _isCollidingWithTarget = false;
    }

    private void ApplyHealthChange()
    {
        AttackSO attackSO = Stats.CurrentStats.attackSO;
        bool hasBeenChanged = _collidingTargetHealthSystme.ChangeHealth(-attackSO.power);
        if(attackSO.isOnKnockBack && _collidingMovement != null)
        {
            _collidingMovement.ApplyKnockBack(transform, attackSO.knockBackPower, attackSO.knockBackTime);
        }
    }
}