using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Destiny.Openable
{
    /// <summary>
    /// Items that appear in the UI of a DrawerWithUi object.
    /// </summary>
	public class DrawerItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private DrawerItemData data;
        [SerializeField]
        private Image image;
        private RectTransform rt => transform as RectTransform;

        private bool dragging = false;
        private Vector3 prevMousePos;

		private void Start()
		{
            prevMousePos = Input.mousePosition;
		}

		private void Update()
		{
			if (dragging)
            {
                rt.position += Input.mousePosition - prevMousePos;
            }
            prevMousePos = Input.mousePosition;
		}

		public bool IdMatches(int id)
        {
            return data.Id.Equals(id);
        }

        public void LoadDataIntoItem(DrawerItemData itemData)
        {
            data = itemData;
            image.sprite = itemData.image.sprite;
            image.color = itemData.image.color;
            image.material = itemData.image.material;
            rt.sizeDelta = new Vector2 (image.sprite.rect.width, image.sprite.rect.height);
        }

		public void OnPointerDown(PointerEventData eventData)
		{
            dragging = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
            dragging = false;
		}
	}
}