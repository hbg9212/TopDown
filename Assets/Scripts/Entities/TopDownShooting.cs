using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    private ProjectileManager _projectileManager;
    private TopDownCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
    }

    void Start()
    {
        _projectileManager = ProjectileManager.instance;
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        float projectilesAngleSpace = rangedAttackData.multiplePorjectilesAngel;
        int numberOfProjectilesPreShot = rangedAttackData.numberOfProjectilesPreShot;
        float minAngel = -(numberOfProjectilesPreShot / 2f) * projectilesAngleSpace + 0.5f * projectilesAngleSpace;

        for (int i = 0; i < numberOfProjectilesPreShot; i++)
        {
            float angel = minAngel + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-rangedAttackData.spread, projectilesAngleSpace);
            angel += randomSpread;
            CreateProjectile(rangedAttackData, angel);
        }

    }

    private void CreateProjectile(RangedAttackData rangedAttackData, float angel)
    {
        _projectileManager.ShootBulltet(
            projectileSpawnPosition.position
            , RotateVector2(_aimDirection,angel)
            , rangedAttackData
            );
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}

