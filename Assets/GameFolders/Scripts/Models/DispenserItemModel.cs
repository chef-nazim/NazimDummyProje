using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NCG.template._NCG.Core.Model;
using NCG.template.GameFolders.Scripts.Level;
using NCG.template.Scripts.Interfaces;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.models
{
    [Serializable]
    public class DispenserItemModel : BaseItemModel, IStockModel
   
    {
        public DispenserItem Item;
        public CellItemPooling CellItemPooling;
        [SerializeField] GridItemModel _gridItemModel;
        public List<IStockModel> _stockModels = new List<IStockModel>();
        private List<CellData> CellDatas;

        public DispenserItemModel(CellItemPooling cellItemPooling, GridItemModel gridItemModel,
            List<CellData> cellDatas)
        {
            CellDatas = cellDatas;
            CellItemPooling = cellItemPooling;
            _gridItemModel = gridItemModel;
        }

        public int ColorID
        {
            get
            {
                if (_stockModels.Count > 0)
                {
                    return (_stockModels[_stockModels.Count - 1] as CellItemModel).ColorID;
                }
                else
                {
                    return -1;
                }
            }
            set { }
        }


        public void CreateSettings()
        {
            for (int i = 0; i < CellDatas.Count; i++)
            {
                CellItemModel cellItemModel = new CellItemModel();
                cellItemModel.ColorID = CellDatas[i].ColorID;
                CellItemPooling.GetItem(cellItemModel);
                _stockModels.Add(cellItemModel);
                cellItemModel.SetParent(_gridItemModel.Item.transform);
                cellItemModel.SetLocalPosition(
                    new Vector3(0, _stockModels.Count * ItemHelper.DispenserItemPositionOffset, 0));
            }

            Item.Damage((float)_stockModels.Count / (float)CellDatas.Count,(float)_stockModels.Count / (float)CellDatas.Count);
        }

        public void SetLocalPosition(int index)
        {
            Item.SetLocalPosition(new Vector3(0, index * ItemHelper.CellItemPositionOffset, 0));
        }

        public void SetParent(Transform itemTransform)
        {
            Item.transform.parent = itemTransform;
        }

        public async UniTask FallMovePosition(Vector3 localPos)
        {
            /*DOTween.Kill(Item.transform);

            await Item.transform.DOLocalMove(localPos, ItemHelper.FallMovePositionDuration).SetEase(Ease.OutExpo);*/
        }

        public void Dispose()
        {
            Item.Clear();
            Item.SetActive(false);
        }

        public IStockModel GiveModel()
        {
            if (_stockModels.Count == 0)
            {
                return null;
            }

            
            var model = _stockModels[_stockModels.Count - 1];
            _stockModels.Remove(model);

            Item.Damage((float)(_stockModels.Count+1) / (float)CellDatas.Count,(float)_stockModels.Count / (float)CellDatas.Count);
            Item.CreateEffect();
            return model;
        }
    }
}