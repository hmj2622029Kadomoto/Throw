using UnityEngine;

public class WeaponScript : MonoBehaviour
{
	[SerializeField] float damage = 20.0f;

	private void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
