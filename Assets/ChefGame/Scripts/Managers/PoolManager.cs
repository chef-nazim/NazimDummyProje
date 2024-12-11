using System;
using gs.chef.game.Scripts.Item;
using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.Pools;
using gs.chef.game.models;
using gs.chef.vcontainer.core.managers;
using UnityEngine;
using VContainer;

namespace gs.chef.game.Managers
{
    public class PoolManager : BaseSubscribable
    {
        [Inject] private readonly Func<Transform, BaseItemModel, CellItem> _cellItemSpawner;
        [Inject] private readonly Func<Transform, BaseItemModel, SlotItem> _slotItemSpawner;
        [Inject] private readonly Func<Transform, BaseItemModel, GoalItem> _goalItemSpawner;
        [Inject] private readonly Func<Transform, BaseItemModel, GridItem> _gridItemSpawner;

        
        [Inject] private readonly Containers _containers;

        private BaseItemPooling _baseItemPooling;

        public BaseItemPooling BaseItemPooling
        {
            get
            {
                if (_baseItemPooling == null)
                    _baseItemPooling = new BaseItemPooling(_cellItemSpawner, 1,
                        _containers.CellItemPoolContainer.transform,
                        true);
                return _baseItemPooling;
            }
        }

        private BaseItemPooling _slotItemPooling;

        public BaseItemPooling SlotItemPooling
        {
            get
            {
                if (_slotItemPooling == null)
                    _slotItemPooling = new BaseItemPooling(_slotItemSpawner, 1,
                        _containers.SlotItemPoolContainer.transform,
                        true);
                return _slotItemPooling;
            }
        }

        private BaseItemPooling _goalItemPooling;

        public BaseItemPooling GoalItemPooling
        {
            get
            {
                if (_goalItemPooling == null)
                    _goalItemPooling = new BaseItemPooling(_goalItemSpawner, 1,
                        _containers.GoalItemPoolContainer.transform,
                        true);
                return _goalItemPooling;
            }
        }

        private BaseItemPooling _gridItemPooling;

        public BaseItemPooling GridItemPooling
        {
            get
            {
                if (_gridItemPooling == null)
                    _gridItemPooling = new BaseItemPooling(_gridItemSpawner, 1,
                        _containers.GridItemPoolContainer.transform, true);
                return _gridItemPooling;
            }
        }
        
       
        
        public void DisposeAllPool()
        {
            if (_baseItemPooling != null)
            {
                _baseItemPooling.HideAllObjects();
                _baseItemPooling.Dispose();
                _baseItemPooling = null;
            }

            if (_slotItemPooling != null)
            {
                _slotItemPooling.HideAllObjects();
                _slotItemPooling.Dispose();
                _slotItemPooling = null;
            }

            if (_goalItemPooling != null)
            {
                _goalItemPooling.HideAllObjects();
                _goalItemPooling.Dispose();
                _goalItemPooling = null;
            }

            if (_gridItemPooling != null)
            {
                _gridItemPooling.HideAllObjects();
                _gridItemPooling.Dispose();
                _gridItemPooling = null;
            }

            
            
        }
    }
}