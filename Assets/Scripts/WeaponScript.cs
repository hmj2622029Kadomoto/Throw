using UnityEngine;

public class WeaponScript : MonoBehaviour
{
	[SerializeField] float damage = 20.0f;
	bool hit = false;

	private void OnCollisionEnter(Collision collision)
	{
		if (hit) return;
		
		hit = true;

		collision.gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
		Rigidbody rbody =GetComponent<Rigidbody>();

		Collider col = GetComponent<Collider>();

		if(col != null)
		{
			col.enabled = false;
		}

		if(rbody != null )
		{
			rbody.linearVelocity = Vector3.zero;
			rbody.angularVelocity = Vector3.zero;
			rbody.isKinematic = true;
		}
	}
}
