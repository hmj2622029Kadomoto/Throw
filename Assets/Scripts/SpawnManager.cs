using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] Camera MainCamera;
	[SerializeField] int cost;

	GameObject selectedPrefab;

	public void SelectUnit(GameObject prefab)
	{
		selectedPrefab = prefab;
	}

	private void Update()
	{
		if (selectedPrefab == null)
			return;
		if(Mouse.current.leftButton.wasPressedThisFrame)
		{
			Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

			if(Physics.Raycast(ray,out RaycastHit hit))
			{
				Instantiate(selectedPrefab,hit.point,Quaternion.identity);
			}
		}
		if(Mouse.current.rightButton.wasPressedThisFrame)
		{
			SceneManager.LoadScene("GameScene");
		}
	}
}
