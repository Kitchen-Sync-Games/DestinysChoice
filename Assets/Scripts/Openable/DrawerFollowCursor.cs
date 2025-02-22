using UnityEngine;

namespace Destiny.Openable
{
    public class DrawerFollowCursor : DrawerBase
    {
		private bool opening;

		private float maxX = 1.093f;
		private float minX = 0.87f;

		protected override void checkInteract()
		{
			if (!opening)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
					RaycastHit hit = new RaycastHit();
					int layer = 64;
					if (Physics.Raycast(ray, out hit, 2.0f, layer))
					{
						if (hit.transform.gameObject.Equals(gameObject))
							opening = true;
					}
				}
			}
			else
			{
				if (Input.GetMouseButtonUp(0))
				{
					opening = false;
				}
				else
				{
					updateDrawerPos();
				}
			}
		}

		private void updateDrawerPos()
		{
			float mouseYDelta = Input.mousePositionDelta.y * 0.001f;
			float newX = 0;
			if (transform.localPosition.x + mouseYDelta > maxX)
			{
				newX = maxX;
			}
			else if (transform.localPosition.x + mouseYDelta < minX)
			{
				newX = minX;
			}
			else
			{
				newX = transform.localPosition.x + mouseYDelta;
			}
			transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
		}
    }
}