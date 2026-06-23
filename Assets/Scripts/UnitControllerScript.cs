using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;



public class UnitControllerScript : MonoBehaviour
{
	enum WeaponType
	{
		SpearBishamon,
		SpearCross,
		Naginata,
		SwordLong,
		SwordWooden,
		SwordCeremon,
		Axe,
		Shuriken1,
		Shuriken2,
		Shuriken3,
		Ju,
		Fan,
		Kunai,
		Knife
	}

	[SerializeField] Transform target;
	[SerializeField] GameObject weaponPrefab;
	[SerializeField] Transform throwPoint;
	[SerializeField] float throwPower = 15f;

	[SerializeField] float moveSpeed = 5.0f;
	[SerializeField] float attackRange = 5.0f;

	[SerializeField] WeaponType weaponType;

	Animator animator;
	Rigidbody rbody;
	bool attacking;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
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
		Vector3 direction = (target.position - transform.position).normalized;

		transform.forward = direction;

		rbody.linearVelocity = direction * moveSpeed;

		animator.SetBool("Run", true);
	}

	void Attack()
	{
		attacking = true;

		rbody.linearVelocity = Vector3.zero;

		animator.SetBool("Run", false);

		switch (weaponType)
		{
			case WeaponType.SpearBishamon:
			case WeaponType.SpearCross:
			case WeaponType.Naginata:
				animator.SetTrigger("SpearThrow");
				break;
			case WeaponType.SwordLong:
			case WeaponType.SwordWooden:
			case WeaponType.SwordCeremon:
			case WeaponType.Axe:
				animator.SetTrigger("WeaponThrow");
				break;
			case WeaponType.Shuriken1:
			case WeaponType.Shuriken2:
			case WeaponType.Shuriken3:
			case WeaponType.Ju:
			case WeaponType.Fan:
			case WeaponType.Kunai:
			case WeaponType.Knife:
				animator.SetTrigger("BoomerangThrow");
				break;
		}
	}

	void ThrowWeapon()
	{
		GameObject weapon = Instantiate(weaponPrefab, throwPoint.position, throwPoint.rotation);

		Rigidbody weaponRbody = weapon.GetComponent<Rigidbody>();

		weapon.GetComponent<Rigidbody>();

		weaponRbody.linearVelocity = transform.forward * throwPower;
	}

	void EndAttack()
	{
		attacking = false;
	}
}