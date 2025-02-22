using UnityEngine;
using UnityEngine.UI;

namespace Destiny.Openable
{
	/// <summary>
	/// The information for a DrawerItem.
	/// </summary>
	public class DrawerItemData : MonoBehaviour
	{
		public int Id { get; set; } = -1;
		public Image image;
	}
}