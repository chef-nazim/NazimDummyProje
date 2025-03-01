using System;
using System.Collections.Generic;
using System.Linq;
using NCG.template.models;
using UnityEngine;
using Object = System.Object;

namespace NCG.template._NCG.Pool
{
    public class BasePool<TTransform, TItemData, TItem> : IDisposable
        where TTransform : Transform
        where TItemData : class, ISpawnItemModel
        where TItem : MonoBehaviour, ISpawnItem<TItemData>
    {
        private List<TItem> _pooledItems;

        private TTransform _poolParent;

        private TItem _prefab;

        //maximum number of objects to have in the list.
        private int _poolSize;

        private bool _willGrow;

        private bool _alreadyDisposed;

        public BasePool(TItem prefab, int poolSize, TTransform poolParent,
            bool willGrow = false)
        {
            _alreadyDisposed = false;
            _poolParent = poolParent;

            _poolSize = poolSize;
            _willGrow = willGrow;
            _prefab = prefab;

            _pooledItems = new List<TItem>();

            for (int i = 0; i < _poolSize; i++)
            {
                var o = UnityEngine.Object.Instantiate(prefab, _poolParent);
                o.SetActive(false);
                _pooledItems.Add(o);
            }
        } 

        public TItem GetItem(TItemData data)
        {
            var item = _pooledItems.FirstOrDefault(s => s.gameObject.activeSelf == false);

            if (item != null)
            {
                item.SetParent(_poolParent);
                item.ReInitialize(data);
                item.SetActive(true);
                return item;
            }

            if (_willGrow || _poolSize > _pooledItems.Count)
            {
                var o = UnityEngine.Object.Instantiate(_prefab, _poolParent);
                o.ReInitialize(data);
                o.SetActive(true);
                _pooledItems.Add(o);
                return o;
            }

            return null;
        }

        public void HideAllObjects()
        {
            foreach (var o in _pooledItems)
            {
                o.gameObject.SetActive(false);
                o.SetParent(_poolParent);
            }
        }

        public void RemoveAllObjects()
        {
            var toRemove = _pooledItems?.Where(s => s != null && s.gameObject != null).ToList();

            toRemove?.ForEach(s =>
            {
                _pooledItems?.Remove(s);
                s?.DisposeItem();
            });
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool explicitCall)
        {
            if (!_alreadyDisposed)
            {
                if (explicitCall)
                {
                    Debug.Log("Not in the destructor");
                    RemoveAllObjects();
                    _alreadyDisposed = true;
                }

                _alreadyDisposed = true;
            }
        }
    }

    public class BasePool<TTransform, TItem> : IDisposable
        where TTransform : Transform
        where TItem : MonoBehaviour, ISpawnItem
    {
        private List<TItem> _pooledItems;

        private TTransform _poolParent;

        private TItem _prefab;

        //maximum number of objects to have in the list.
        private int _poolSize;

        private bool _willGrow;

        private bool _alreadyDisposed;

        public BasePool(TItem prefab, int poolSize, TTransform poolParent,
            bool willGrow = false)
        {
            _alreadyDisposed = false;
            _poolParent = poolParent;

            _poolSize = poolSize;
            _willGrow = willGrow;
            _prefab = prefab;

            _pooledItems = new List<TItem>();

            for (int i = 0; i < _poolSize; i++)
            {
                var o = UnityEngine.Object.Instantiate(prefab, _poolParent);
                o.SetActive(false);
                _pooledItems.Add(o);
            }
        }

        public TItem GetItem()
        {
            var item = _pooledItems.FirstOrDefault(s => s.gameObject.activeSelf == false);

            if (item != null)
            {
                item.SetParent(_poolParent);
                item.ReInitialize();
                item.SetActive(true);
                return item;
            }

            if (_willGrow || _poolSize > _pooledItems.Count)
            {
                var o = UnityEngine.Object.Instantiate(_prefab, _poolParent);
                o.ReInitialize();
                o.SetActive(true);
                _pooledItems.Add(o);
                return o;
            }

            return null;
        }

        public void HideAllObjects()
        {
            foreach (var o in _pooledItems)
            {
                o.gameObject.SetActive(false);
                o.SetParent(_poolParent);
            }
        }

        public void RemoveAllObjects()
        {
            var toRemove = _pooledItems?.Where(s => s != null && s.gameObject != null).ToList();

            toRemove?.ForEach(s =>
            {
                _pooledItems?.Remove(s);
                s?.DisposeItem();
            });
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool explicitCall)
        {
            if (!_alreadyDisposed)
            {
                if (explicitCall)
                {
                    Debug.Log("Not in the destructor");
                    RemoveAllObjects();
                    _alreadyDisposed = true;
                }

                _alreadyDisposed = true;
            }
        }
    }

    public interface ISpawnItem<TModel> : ISpawnItemModel
    {
        TModel ItemModel { get; set; }

        void SetActive(bool isActive);

        void ReInitialize(TModel itemModel);
        void SetParent(Transform parent);
        void DisposeItem();
    }

    public interface ISpawnItem
    {
        void SetActive(bool isActive);
        void ReInitialize();
        void SetParent(Transform parent);
        void DisposeItem();
    }

    public interface ISpawnItemModel
    {
    }

    public interface IBasePool
    {
    }
}