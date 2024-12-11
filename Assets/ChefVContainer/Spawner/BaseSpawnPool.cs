using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace gs.chef.vcontainer.spawner
{
    public class BaseSpawnPool<TItemModel, TItem> : IDisposable where TItemModel : class, ISpawnItemModel
        where TItem : MonoBehaviour,ISpawnItem<TItemModel>
    {
        private readonly Func<TItemModel, TItem> _spawnFunc;

        private List<TItem> _pooledItems;


        //maximum number of objects to have in the list.
        private int _poolSize;

        private bool _willGrow;

        private bool _alreadyDisposed;

        public BaseSpawnPool(Func<TItemModel, TItem> spawnFunc, int poolSize, bool willGrow = false)
        {
            _alreadyDisposed = false;
            _spawnFunc = spawnFunc;

            _poolSize = poolSize;
            _willGrow = willGrow;

            _pooledItems = new List<TItem>();

            for (int i = 0; i < _poolSize; i++)
            {
                var o = _spawnFunc.Invoke(null);
                o.SetActive(false);
                _pooledItems.Add(o);
            }
        }

        public TItem GetItem(TItemModel data)
        {
            var item = _pooledItems.FirstOrDefault(s => s.gameObject.activeSelf == false);

            if (item != null)
            {
                item.transform.SetParent(null);
                item.ReInitialize(data);
                item.SetActive(true);
                return item;
            }

            if (_willGrow || _poolSize > _pooledItems.Count)
            {
                var o = _spawnFunc.Invoke(null);
                o.transform.SetParent(null);
                //o.ReInitialize(data);
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
                o.transform.SetParent(null);
            }
        }

        public void RemoveAllObjects()
        {
            var toRemove = _pooledItems?.Where(s => s != null && s.gameObject != null).ToList();

            toRemove?.ForEach(s =>
            {
                _pooledItems?.Remove(s);
                Object.Destroy(s.gameObject);
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

        ~BaseSpawnPool()
        {
            Debug.Log("in the destructor");
            Dispose(false);
        }
    }

    public class BaseSpawnPool<TTransform, TItemData, TItem> : IDisposable
        where TTransform : Transform
        where TItemData : class, ISpawnItemModel
        where TItem : MonoBehaviour, ISpawnItem<TItemData>
    {
        private readonly Func<TTransform, TItemData, TItem> _spawnFunc;

        private List<TItem> _pooledItems;

        private TTransform _poolTarget;

        //maximum number of objects to have in the list.
        private int _poolSize;

        private bool _willGrow;

        private bool _alreadyDisposed;

        public BaseSpawnPool(Func<Transform, TItemData, TItem> spawnFunc, int poolSize, TTransform poolTarget,
            bool willGrow = false)
        {
            _alreadyDisposed = false;
            _spawnFunc = spawnFunc;
            _poolTarget = poolTarget;

            _poolSize = poolSize;
            _willGrow = willGrow;

            _pooledItems = new List<TItem>();

            for (int i = 0; i < _poolSize; i++)
            {
                var o = _spawnFunc.Invoke(poolTarget, null);
                o.SetActive(false);
                _pooledItems.Add(o);
            }
        }

        public TItem GetItem(TItemData data)
        {
            var item = _pooledItems.FirstOrDefault(s => s.gameObject.activeSelf == false);

            if (item != null)
            {
                item.transform.SetParent(_poolTarget);
                item.ReInitialize(data);
                item.SetActive(true);
                return item;
            }

            if (_willGrow || _poolSize > _pooledItems.Count)
            {
                var o = _spawnFunc.Invoke(_poolTarget, data);
                o.transform.SetParent(_poolTarget);
                //o.ReInitialize(data);
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
                o.transform.SetParent(_poolTarget);
            }
        }

        public void RemoveAllObjects()
        {
            var toRemove = _pooledItems?.Where(s => s != null && s.gameObject != null).ToList();

            toRemove?.ForEach(s =>
            {
                _pooledItems?.Remove(s);
                Object.Destroy(s.gameObject);
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

        ~BaseSpawnPool()
        {
            Debug.Log("in the destructor");
            Dispose(false);
        }
    }
}