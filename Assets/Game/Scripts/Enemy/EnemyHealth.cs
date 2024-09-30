﻿using System;
using UnityEngine;

namespace Game.Scripts.Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamagable
    {
        public static event Action OnEnemyDeadEvent;

        [Header("Config")] [SerializeField] private float health;

        public float CurrentHealth { get; private set; }

        private Animator animator;
        private EnemyBrain enemyBrain;
        private EnemyLoot enemyLoot;
        private EnemySelector enemySelector;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyLoot = GetComponent<EnemyLoot>();
            enemyBrain = GetComponent<EnemyBrain>();
            enemySelector = GetComponent<EnemySelector>();
        }

        private void Start()
        {
            CurrentHealth = health;
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth <= 0f)
            {
                DisableEnemy();
            }
            else
            {
                DmgManager.Instance.ShowDamageText(amount, transform);
            }
        }

        private void DisableEnemy()
        {
            animator.SetTrigger("Dead");
            enemyBrain.enabled = false;
            enemySelector.NoSelectionCallback();
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            OnEnemyDeadEvent?.Invoke();
            GameManager.Instance.AddPlayerExp(enemyLoot.ExpDrop);
        }
    }
}