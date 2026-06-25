using System;
using System.Runtime.CompilerServices;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.InputSystem;



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

	[SerializeField] Transform target;
	[SerializeField] GameObject weaponPrefab;
	[SerializeField] Transform throwPoint;

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

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (isDead) return;

		if (target != null)
		{
			UnitControllerScript targetUnit = target.GetComponent<UnitControllerScript>();
			if (targetUnit != null && targetUnit.IsDead)
			{
				target = null;
			}
		}

		if (target == null || attacking)
			return;

		float distance = Vector3.Distance(transform.position, target.position);

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
		float spinUp = 0f;
		float spinRight = 0f;

		switch (weaponType)
		{
			case WeaponType.Spear:
				upward = 0.1f;
				spinUp = 0f;
				spinRight = 0f;
				break;

			case WeaponType.Axe:
				upward = 0.8f;
				spinUp = 100f;
				spinRight = 0.1f;
				break;

			case WeaponType.Shuriken:
				upward = 0.1f;
				spinUp = 30f;
				spinRight = 0f;
				break;

			case WeaponType.Ju:
				upward = 0.3f;
				spinUp = 0f;
				spinRight = 15f;
				break;

			case WeaponType.Naginata:
				upward = 0.1f;
				spinUp = 0f;
				spinRight = 0f;
				break;

			case WeaponType.Knife:
				upward = 0.1f;
				spinUp = 10f;
				spinRight = 0f;
				break;
		}

		if (weaponType == WeaponType.Spear || weaponType == WeaponType.Ju || weaponType == WeaponType.Naginata)
		{
			weapon.transform.Rotate(0f, 180f, 0f);
		}

		weaponRbody.linearVelocity = dir * throwPower + Vector3.up * upward * throwPower;
		weaponRbody.angularVelocity = transform.right * spinUp;
		weaponRbody.angularVelocity = transform.up * spinRight;
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
}