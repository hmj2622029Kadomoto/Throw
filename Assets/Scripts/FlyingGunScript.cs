using UnityEngine;

public class FlyingGun : MonoBehaviour
{
	[SerializeField] GameObject bulletPrefab;
	[SerializeField] Transform muzzle;

	[SerializeField] float fireInterval = 0f;
	[SerializeField] int ammo = 30;

	float timer;

	private void Update()
	{
		timer += Time.deltaTime;

		if(timer >= fireInterval && ammo > 0)
		{
			Fire();
			ammo--;
			timer = 0f;
		}
	}

	void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, muzzle.position + muzzle.forward * 0.5f, muzzle.rotation);

		Rigidbody rbody = bullet.GetComponent<Rigidbody>();

		rbody.linearVelocity = -muzzle.right * 50f;
	}
}