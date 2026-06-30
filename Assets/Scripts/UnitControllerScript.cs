using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;



public class UnitControllerScript : MonoBehaviour
{
	enum WeaponType
	{
		Spear,
		Naginata,
		Sword,
		Axe,
		Shuriken,
		Ju,
		Knife
	}

	[SerializeField] GameObject weaponPrefab;
	[SerializeField] Transform throwPoint;
	[SerializeField] Transform target;

	[SerializeField] float attackInterval = 2f;
	[SerializeField] float throwPower = 10f;
	[SerializeField] float moveSpeed = 5.0f;
	[SerializeField] float attackRange = 5.0f;
	[SerializeField] float hp = 100.0f;
	[SerializeField] float spawnOffset = 1f;

	[SerializeField] WeaponType weaponType;

	Animator animator;
	Rigidbody rbody;
	bool attacking;
	bool isDead = false;
	[SerializeField] bool isPlayer;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (isDead) return;

		if (target == null)
			target = FindNearestEnemy();

		if (target != null)
		{
			UnitControllerScript targetUnit = target.GetComponent<UnitControllerScript>();
			if (targetUnit != null && targetUnit.IsDead)
			{
				target = FindNearestEnemy();
			}
		}

		if (target == null || attacking)
			return;

		float distance = Vector3.Distance(transform.position, target.position);

		rbody.AddForce(Physics.gravity * 10f, ForceMode.Acceleration);

		if (distance > attackRange)
		{
			MoveToTarget();
		}
		else
		{
			Attack();
		}
	}

	void MoveToTarget()
	{
		Vector3 direction = target.position - transform.position;

		direction.y = 0f;

		direction.Normalize();

		transform.forward = direction;

		rbody.linearVelocity = direction * moveSpeed;

		animator.SetBool("Run", true);
	}

	void Attack()
	{
		Vector3 direcrtion = target.position - transform.position;
		direcrtion.y = 0f;

		if (direcrtion != Vector3.zero)
		{
			transform.forward = direcrtion.normalized;
		}

		attacking = true;

		rbody.linearVelocity = Vector3.zero;

		animator.SetBool("Run", false);

		switch (weaponType)
		{
			case WeaponType.Spear:
			case WeaponType.Naginata:
				animator.SetTrigger("SpearThrow");
				break;
			case WeaponType.Sword:
			case WeaponType.Axe:
			case WeaponType.Ju:
				animator.SetTrigger("WeaponThrow");
				break;
			case WeaponType.Shuriken:
			case WeaponType.Knife:
				animator.SetTrigger("BoomerangThrow");
				break;
		}

		Invoke(nameof(EndAttack), attackInterval);
	}

	void ThrowWeapon()
	{
		GameObject weapon = Instantiate(weaponPrefab, throwPoint.position + transform.forward * spawnOffset, throwPoint.rotation);

		Rigidbody weaponRbody = weapon.GetComponent<Rigidbody>();

		Vector3 dir = transform.forward;
		float upward = 0.3f;
		float spin = 0f;

		switch (weaponType)
		{
			case WeaponType.Spear:
				upward = 0.1f;
				spin = 0f;
				break;

			case WeaponType.Axe:
				upward = 0.8f;
				spin = 30f;
				weaponRbody.angularVelocity = transform.right * spin;
				break;

			case WeaponType.Shuriken:
				upward = 0.1f;
				spin = 30f;
				weaponRbody.angularVelocity = transform.right * spin;
				break;

			case WeaponType.Ju:
				upward = 1f;
				spin = 15f;
				weaponRbody.angularVelocity = transform.right * spin;
				break;

			case WeaponType.Sword:
				upward = 0.5f;
				spin = 45f;
				weaponRbody.angularVelocity = transform.right * spin;
				break;

			case WeaponType.Naginata:
				upward = 0.1f;
				spin = 0f;
				break;

			case WeaponType.Knife:
				upward = 0.1f;
				spin = 10f;
				weaponRbody.angularVelocity = transform.right * spin;
				break;
		}

		if (weaponType == WeaponType.Spear || weaponType == WeaponType.Ju || weaponType == WeaponType.Naginata || weaponType == WeaponType.Sword)
		{
			weapon.transform.Rotate(0f, 180f, 0f);
		}

		weaponRbody.linearVelocity = dir * throwPower + Vector3.up * upward * throwPower;
	}


	void EndAttack()
	{
		attacking = false;
	}

	void Damage(float damage)
	{
		hp -= damage;
		animator.SetTrigger("Damage");
		if (hp <= 0 && !isDead)
		{
			isDead = true;
			animator.SetTrigger("Death");
			rbody.linearVelocity = Vector3.zero;
		}
	}

	public bool IsDead
	{
		get { return isDead; }
	}

	Transform FindNearestEnemy()
	{
		string targetTag = isPlayer ? "Enemy" : "Player";

		GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

		Transform nearest = null;
		float nearestDistance = Mathf.Infinity;

		foreach (GameObject obj in targets)
		{
			UnitControllerScript unit = obj.GetComponent<UnitControllerScript>();

			if (unit != null && unit.isDead)
			{
				continue;
			}

			float distance = Vector3.Distance(transform.position, obj.transform.position);

			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearest = obj.transform;
			}
		}
		return nearest;
	}
}