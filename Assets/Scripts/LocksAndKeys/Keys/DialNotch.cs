using UnityEngine;
using TMPro;

namespace Destiny.LocksAndKeys
{
    /// <summary>
    /// A notch on a DialKey
    /// </summary>
    public class DialNotch : MonoBehaviour
    {
        private const int MAX_FACTOR = 5;

        public float ZRot => transform.rotation.z;
        public string Val => label.text;

        [SerializeField]
        private GameObject notchImage;
        private RectTransform notchImageRT => notchImage.transform as RectTransform;
        [SerializeField]
        private TextMeshProUGUI label;
        public Transform NotchSatellite;

        private float radius;
        private float circumference => radius * 2 * Mathf.PI;

        public void Setup(float radius, float notchWidth, string label)
        {
            this.label.text = label;
            this.radius = radius;
            var rt = notchImageRT;
            rt.sizeDelta = new Vector2(notchWidth, rt.sizeDelta.y);
            (NotchSatellite as RectTransform).sizeDelta = new Vector2(notchWidth, (transform as RectTransform).sizeDelta.y);
            NotchSatellite.position = new Vector2 (transform.position.x, transform.position.y + radius);
        }

        public void SetPositionAndRotation(int notchIndex, int totalNotches)
        {
            float arcLength = circumference / totalNotches;
            float rad = arcLength / radius;
            float deg = Mathf.Rad2Deg * rad;
            float notchDeg = deg * notchIndex;
            transform.Rotate(0, 0, -notchDeg);

            if (notchIndex == 0)
            {
                setLabelActive(true);
            }
            else
            {
                int mod = 2;

                for (int i = mod + 1; i <= MAX_FACTOR; i++)
                {
                    if (totalNotches % i == 0 || totalNotches % i > totalNotches % mod)
                        mod = i;
                }

                if (notchIndex % mod == 0)
                {
                    setLabelActive(true);
				}
                else
                {
                    setLabelActive(false);
                }
			}
        }

        private void setLabelActive(bool active)
        {
            label.gameObject.SetActive(active);
			notchImageRT.offsetMin = new Vector2(notchImageRT.offsetMin.x, active ? 0 : 50);
        }
    }
}