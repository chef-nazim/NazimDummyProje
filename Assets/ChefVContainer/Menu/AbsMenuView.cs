using UnityEngine;

namespace gs.chef.vcontainer.menu
{
    public class AbsMenuView : MonoBehaviour, IMenu
    {
        [SerializeField] private bool _destroyWhenClosed;

        public bool ActiveSelf => gameObject.activeInHierarchy;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public bool DestroyWhenClosed => _destroyWhenClosed;

        public void InitializeView()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
        }

        public void UpdateView()
        {
            InitializeView();
        }

        public void Close()
        {
            SetActive(false);
        }

        public void Open()
        {
            SetActive(true);
        }

        public void Dispose()
        {
            SetActive(false);
        }

        public void Dispose(bool disposing)
        {
            Dispose();
            if (disposing && _destroyWhenClosed)
                Destroy(gameObject);
            
        }
    }
}