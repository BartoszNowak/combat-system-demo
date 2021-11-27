using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float projectileSpeed = 10f;
        [SerializeField]
        private bool isHoming = false;
        [SerializeField]
        private GameObject impactEffect = null;
        [SerializeField]
        private float maxLifetime = 10f;
        [SerializeField]
        private AudioClip impactSound = null;
        [SerializeField]
        private float feedbackShakePower = 0f;

        private Vector3 direction;
        private Health target = null;
        private int damage = 0;
        private GameObject attacker = null;

        private void Start()
        {
            if(target == null)
			{
                transform.forward = direction;
            }
            else
			{
                transform.LookAt(GetAimLocation());
			}
        }

        void Update()
        {
			if (isHoming && target != null && !target.IsDead())
			{
				transform.LookAt(GetAimLocation());
			}
			transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        public void SetTarget(Health target, int damage, GameObject attacker)
        {
            this.target = target;
            this.damage = damage;
            this.attacker = attacker;

            Destroy(gameObject, maxLifetime);
        }

        public void SetDirection(Vector3 direction, int damage, GameObject attacker)
        {
            this.direction = direction;
            this.damage = damage;
            this.attacker = attacker;

            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimLocation()
        {
            var collider = target.GetComponent<CapsuleCollider>();
            if (collider == null) return target.transform.position;

            return target.transform.position + Vector3.up * collider.height / 2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == attacker.gameObject) return;
            var health = other.GetComponent<Health>();
            if (health == null) return;
            if (health.IsDead()) return;

            ShowImpactEffect();

            AudioSource.PlayClipAtPoint(impactSound, transform.position);
            Camera.main.GetComponent<CameraShake>().TriggerShake(feedbackShakePower);

            health.DealDamage(damage, attacker);
            Destroy(gameObject);
        }

        private void ShowImpactEffect()
        {
            if (impactEffect == null) return;
            Instantiate(impactEffect, GetAimLocation(), transform.rotation);
        }
    }
}
