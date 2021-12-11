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
        [SerializeField]
        private float areaDamageRange = 0f;
        [SerializeField]
        private float areaDamageMultiplier = 1f;

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
                transform.LookAt(GetAimLocation(target.transform));
			}
        }

        void Update()
        {
			if (isHoming && target != null && !target.IsDead())
			{
				transform.LookAt(GetAimLocation(target.transform));
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

        private Vector3 GetAimLocation(Transform targetTransform)
        {
            var collider = targetTransform.GetComponent<CapsuleCollider>();
            if (collider == null) return targetTransform.position;

            return targetTransform.position + Vector3.up * collider.height / 2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == attacker.gameObject) return;
            var health = other.GetComponent<Health>();
            if (health == null) return;
            if (health.IsDead()) return;

            ShowImpactEffect(other.transform);

            AudioSource.PlayClipAtPoint(impactSound, transform.position);
            Camera.main.GetComponent<CameraShake>().TriggerShake(feedbackShakePower);

            health.DealDamage(damage, attacker);

            if(!Mathf.Approximately(areaDamageRange, 0))
			{
                AreaDamage(other);
            }
            
            Destroy(gameObject);
        }

        private void ShowImpactEffect(Transform targetTransform) 
        { 
            if (impactEffect == null) return;
            //var location = collider.transform.position + Vector3.up* collider.height / 2f;
            Instantiate(impactEffect, GetAimLocation(targetTransform), transform.rotation);
        }

        private void AreaDamage(Collider other)
		{
            var hits = Physics.OverlapSphere(transform.position, areaDamageRange);
            foreach(var h in hits)
			{
                if (h == other) continue;
                if (attacker != null && h.tag == attacker.tag) continue;
                var health = h.GetComponent<Health>();
                if (health == null) continue;

                health.DealDamage((int)(damage * areaDamageMultiplier), attacker);
            }
		}
    }
}
