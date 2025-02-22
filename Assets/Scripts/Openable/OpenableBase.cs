using UnityEngine;

namespace Destiny.Openable
{
    /// <summary>
    /// Base class for anything that can be opened.
    /// </summary>
    public abstract class OpenableBase : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            checkInteract();
        }

        protected abstract void checkInteract();
    }
}