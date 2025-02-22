using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Destiny.Openable
{
	/// <summary>
	/// Drawer that opens a UI screen showing the contents
	/// of the drawer.
	/// </summary>
	public class DrawerWithUi : DrawerBase
	{
		private bool isOpen;
		[SerializeField]
		private DrawerUi DrawerUi;
		[SerializeField]
		private Transform itemsParent;
		private List<DrawerItemData> items = new List<DrawerItemData>();

		private void Start()
		{
			items = itemsParent.GetComponentsInChildren<DrawerItemData>().ToList();
			isOpen = false;
		}

		protected override void checkInteract()
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
				RaycastHit hit = new RaycastHit();
				int layer = 64;
				if (Physics.Raycast(ray, out hit, 2.0f, layer)
					)
				{
					if (hit.transform.gameObject.Equals(gameObject)
					&& !isOpen)
						open();
				}
			}
		}

		private void open()
		{
			isOpen = true;
			Cursor.lockState = CursorLockMode.None;
			DrawerUi.Open(close, ref items);
		}

		private void close()
		{
			Cursor.lockState = CursorLockMode.Locked;
			isOpen = false;
		}
	}
}