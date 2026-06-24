using UnityEngine;

public class WeaponScript : MonoBehaviour
{
	[SerializeField] float damage = 20.0f;
	[SerializeField] bool hit = false;

	private void OnCollisionEnter(Collision collision)
	{
		if (hit) return;
		
		hit = true;

		Debug.Log(gameObject.name + "Ç…" + damage + "É_ÉÅÅ[ÉW");

		collision.gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
		Rigidbody rbody =GetComponent<Rigidbody>();

		if(rbody != null )
		{
			rbody.linearVelocity = Vector3.zero;
			rbody.isKinematic = true;
		}
	}
}
