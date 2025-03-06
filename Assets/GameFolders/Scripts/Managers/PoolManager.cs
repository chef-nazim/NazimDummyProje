using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template._NCG.Pool;
using NCG.template.EventBus;
using NCG.template.models;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.Managers
{
    public class PoolManager : BaseManager
    {
        public static PoolManager Instance;
        public BasePool<Transform, CellItemModel, CellItem> CellItemPooling;
        public BasePool<Transform, GridItemModel, GridItem> GridItemPooling;
        public BasePool<Transform, BoxItemModel, BoxItem> BoxItemPooling;
        public BasePool<Transform, DispenserItemModel, DispenserItem> DispenserItemPooling;
        public BasePool<Transform, MailBoxItemModel, MailBoxItem> MailBoxItemPooling;
        public BasePool<Transform, EggItemModel, EggItem> EggItemPooling;
        public BasePool<Transform, GoalItemModel, GoalItem> GoalItemPooling;


        public override void Initialize()
        {
            Instance = this;
            EventBus<CreatePoolsEvent>.Subscribe(CreatePools);
            EventBus<LevelModelCreatedEvent>.Subscribe(LevelModelCreated);
        }

        private void LevelModelCreated(LevelModelCreatedEvent obj)
        {
        }

        public void CreatePools(CreatePoolsEvent obj)
        {
        }

        public override void Dispose()
        {
            //CellItemPooling.Dispose();
        }
    }
}