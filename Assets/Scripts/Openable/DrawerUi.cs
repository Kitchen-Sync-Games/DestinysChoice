using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Destiny.Openable
{
    /// <summary>
    /// The UI that opens when a drawer is open.
    /// </summary>
	public class DrawerUi : MonoBehaviour
    {
        private Action onDrawerClose;
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private RectTransform itemsParent;
        [SerializeField]
        private GameObject itemPrefab;
        private List<DrawerItem> items;
        private List<DrawerItem> activeItems;
        private int nextAvailableId;


		private void Awake()
		{
            closeButton.onClick.AddListener(close);
            items = new List<DrawerItem>();
            activeItems = new List<DrawerItem>();
            nextAvailableId = 0;
		}

        public void Open(Action onDrawerClose, ref List<DrawerItemData> itemsData)
        {
            this.onDrawerClose = onDrawerClose;
            gameObject.SetActive(true);
            loadItems(ref itemsData);
        }

        private void close()
        {
            onDrawerClose?.Invoke();
            gameObject.SetActive(false);
            deactivateItems();
        }

        private void loadItems(ref List<DrawerItemData> itemsData)
        {
            for (int i = 0; i < itemsData.Count; i++)
            {
                DrawerItem existingItem = null;

                if (itemsData[i].Id != -1)
                {
					int searchId = itemsData[i].Id;
					existingItem = items.Where(x => x.IdMatches(searchId)).FirstOrDefault();
				}

				if (existingItem == null)
                {
                    itemsData[i].Id = nextAvailableId++;
					existingItem = Instantiate(itemPrefab, itemsParent).GetComponent<DrawerItem>();
                    existingItem.LoadDataIntoItem(itemsData[i]);
                    items.Add(existingItem);
				}

                existingItem.gameObject.SetActive(true);
				activeItems.Add(existingItem);
			}
		}

        private void deactivateItems()
        {
            foreach (DrawerItem item in activeItems)
            {
                item.gameObject.SetActive(false);
            }
            activeItems.Clear();
        }
    }
}