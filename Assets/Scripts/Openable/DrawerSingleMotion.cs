using UnityEngine;

namespace Destiny.Openable
{
    public class DrawerSingleMotion : DrawerBase
    {
		[SerializeField]
		private float speedThreshold;
		[SerializeField]
		private Animator animator;
		private bool mouseHeld;
		private bool open;
		private Vector3 prevMousePos;

		private void Awake()
		{
			prevMousePos = Input.mousePosition;
			open = false;
		}

		protected override void checkInteract()
		{
			if (!mouseHeld)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
					RaycastHit hit = new RaycastHit();
					int layer = 64;
					if (Physics.Raycast(ray, out hit, 2.0f, layer))
					{
						if (hit.transform.gameObject.Equals(gameObject))
							mouseHeld = true;
					}
				}
			}
			else
			{
				if (Input.GetMouseButtonUp(0))
				{
					mouseHeld = false;
				}
				else
				{
					Vector3 mouseDelta = Input.mousePosition - prevMousePos;
					float mouseYDelta = mouseDelta.y;
					float vel = mouseYDelta / Time.deltaTime;
					if ((vel > speedThreshold && open) || (vel < -speedThreshold && !open))
					{
						open = !open;
						animator.SetBool("IsOpen", open);
					}
				}
			}

			prevMousePos = Input.mousePosition;
		}
    }
}