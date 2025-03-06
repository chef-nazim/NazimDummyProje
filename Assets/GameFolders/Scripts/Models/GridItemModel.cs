using System;
using System.Collections.Generic;
using System.Linq;
using NCG.template._NCG.Core.Model;
using NCG.template.enums;
using NCG.template.Scripts.Interfaces;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.models
{
    [Serializable]
    public class GridItemModel : BaseItemModel
    {
        

        public Vector2 GridAddress; // x = row, y = column
        public GridItem Item;


        public bool IsEmpty => _isEmpty;
        [SerializeField] bool _isEmpty = true;


        public bool IsRunning
        {
            get => _isRunning;
        }

        [SerializeField] bool _isRunning = false;
        [SerializeField] bool _isPlate = false;
        public bool IsPlate => _isPlate;


        public StockModelType StockModelType
        {
            get
            {
                if (_stockModels.Count <= 0)
                {
                    return StockModelType.None;
                }

                switch (_stockModels.First())
                {
                    case CellItemModel:
                        return StockModelType.CellItemModel;
                    case BoxItemModel:
                        return StockModelType.Box;
                    case DispenserItemModel:
                        return StockModelType.Dispenser;
                    case MailBoxItemModel:
                        return StockModelType.MailBox;
                    case EggItemModel:
                        return StockModelType.Egg;
                    default:
                        return StockModelType.None;
                }
            }
            set { _stockModelType = value; }
        }

        private StockModelType _stockModelType = StockModelType.None;


        [SerializeField] private List<IStockModel> _stockModels = new List<IStockModel>();
        [HideInInspector]public List<SkinnedMeshRenderer> _plateSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();

        public int ColorID
        {
            get
            {
                if (StockModelType == StockModelType.CellItemModel)
                {
                    return (_stockModels.Last() as CellItemModel).ColorID;
                }
                else if (StockModelType == StockModelType.Dispenser)
                {
                    return (_stockModels[0] as DispenserItemModel).ColorID;
                }
                else
                {
                    return -1;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            Item.SetLocalPosition(position);
        }

        public void SetParent(Transform itemTransform)
        {
            Item.SetParent(itemTransform);
        }


        public CellItemModel GiveCellItemModel()
        {
            if (!(StockModelType == StockModelType.CellItemModel || StockModelType == StockModelType.Dispenser))
            {
                return null;
            }

            if (_stockModels.Count == 0)
            {
                _isEmpty = true;
                StockModelType = StockModelType.None;
                return null;
            }
            else
            {
                if (StockModelType == StockModelType.Dispenser)
                {
                    var dispenser = _stockModels[0];

                    var model = (dispenser as DispenserItemModel).GiveModel();
                    if ((dispenser as DispenserItemModel)._stockModels.Count == 0)
                    {
                        (dispenser as DispenserItemModel).Dispose();
                        _stockModels.Clear();
                        _isEmpty = true;
                        StockModelType = StockModelType.None;
                    }

                    return model as CellItemModel;
                }
                else
                {
                    var model = _stockModels[_stockModels.Count - 1];

                    _stockModels.Remove(model);
                    if (_stockModels.Count == 0)
                    {
                        _isEmpty = true;
                        StockModelType = StockModelType.None;
                    }

                    Item.UpdateText(_stockModels.Count);
                    return model as CellItemModel;
                }
            }
        }

        public IStockModel GetStockModel()
        {
            if (_stockModels.Count <= 0)
            {
                return null;
            }

            var model = _stockModels[_stockModels.Count - 1];
            _stockModels.Remove(model);
            if (_stockModels.Count == 0)
            {
                _isEmpty = true;
                StockModelType = StockModelType.None;
            }

            return model;
        }

        public void CountTextOnOff(bool b)
        {
            Item.CountTextOnOff(b, _stockModels.Count);
        }

        public int GetStackCount()
        {
            return _stockModels.Count;
        }

        public void ClearStack()
        {
            _stockModels.Clear();
            Item.UpdateText(_stockModels.Count);
            _isEmpty = true;
            StockModelType = StockModelType.None;
        }

        public Vector3 GetNextStackPosition()
        {
            Vector3 itemPos = Item.transform.position;
            Vector3 position =
                itemPos + new Vector3(0, _stockModels.Count * (float)ItemHelper.CellItemPositionOffset, 0);
            return position;
        }

        public Vector3 GetLastStackPosition()
        {
            Vector3 itemPos = Item.transform.position;

            Vector3 position = new Vector3(itemPos.x,
                itemPos.y, itemPos.z);
            if (StockModelType == StockModelType.Dispenser)
            {
                position = new Vector3(itemPos.x,
                    itemPos.y + (_stockModels.Count) * (float)ItemHelper.DispenserItemPositionOffset, itemPos.z);
            }
            else if (StockModelType == StockModelType.Box)
            {
                position = new Vector3(itemPos.x,
                    itemPos.y + (_stockModels.Count) * (float)ItemHelper.BoxItemPositionOffset, itemPos.z);
            }
            else
            {
                position = new Vector3(itemPos.x,
                    itemPos.y + (_stockModels.Count) * (float)ItemHelper.CellItemPositionOffset, itemPos.z);
            }

            return position;
        }

        public List<IStockModel> GetStackModels()
        {
            return _stockModels.ToList();
        }

        public void ChangeRun(bool b)
        {
            _isRunning = b;
        }

        public void SetAllItemPosition()
        {
            var models = GetStackModels();
            for (int i = 0; i < models.Count; i++)
            {
                models[i].SetLocalPosition(i);
            }
        }

        public void CreatTakeModel(IStockModel model)
        {
            if (IsEmpty)
            {
                _isEmpty = false;
            }

            model.SetParent(Item.transform);
            _stockModels.Add(model);
            Item.UpdateText(_stockModels.Count);
            model.SetLocalPosition(_stockModels.Count);
            model.CreateSettings();
            Item.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            /*switch (model)
            {
                case CellItemModel cellItemModel:
                    _stockModelType = StockModelType.CellItemModel;
                    cellItemModel.SetParent(Item.transform);
                    _stockModels.Add(model);
                    Item.UpdateText(_stockModels.Count);
                    model.SetLocalPosition(new Vector3(0, _stockModels.Count * ItemHelper.CellItemPositionOffset, 0));
                    Item.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case BoxItemModel boxItemModel:
                    _stockModelType = StockModelType.Box;
                    boxItemModel.SetParent(Item.transform);
                    _stockModels.Add(model);
                    Item.UpdateText(_stockModels.Count);
                    model.SetLocalPosition(new Vector3(0, -0.8f + _stockModels.Count * ItemHelper.BoxItemPositionOffset, 0));
                    Item.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case DispenserItemModel dispenserItemModel:
                    _stockModelType = StockModelType.Dispenser;
                    dispenserItemModel.SetParent(Item.transform);
                    _stockModels.Add(model);
                    Item.UpdateText(_stockModels.Count);
                    model.SetLocalPosition(new Vector3(0, _stockModels.Count * ItemHelper.CellItemPositionOffset, 0));
                    Item.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    dispenserItemModel.CreateSettings();
                    break;
            }*/
        }

        public void FeedTakeCell(IStockModel model, List<float> columnDelay)
        {
            if (IsEmpty)
            {
                _isEmpty = false;
            }

            switch (model)
            {
                case CellItemModel cellItemModel:
                    StockModelType = StockModelType.CellItemModel;
                    cellItemModel.SetParent(Item.transform);
                    _stockModels.Add(model);
                    Item.UpdateText(_stockModels.Count);
                    cellItemModel.FeedAnimation(
                        new Vector3(0, _stockModels.Count * ItemHelper.CellItemPositionOffset, 0),
                        columnDelay[(int)GridAddress.y]);
                    break;
            }
        }

        public async Task TakeAndFallMove(List<IStockModel> models)
        {
            if (IsEmpty)
            {
                _isEmpty = false;
                if (models.Any(s => s is CellItemModel) == true)
                {
                    StockModelType = StockModelType.CellItemModel;
                }
            }

            foreach (var v in models)
            {
                v.SetParent(Item.transform);
                _stockModels.Add(v);
                v.FallMovePosition(new Vector3(0, _stockModels.Count * ItemHelper.CellItemPositionOffset, 0));
                Item.UpdateText(_stockModels.Count);
            }
        }

        public CollectItemMovePositionJob GetCollectItemMovePositionJob(IStockModel model, float delay)
        {
            if (IsEmpty)
            {
                _isEmpty = false;
            }


            model.SetParent(Item.transform);
            _stockModels.Add(model);
            Item.UpdateText(_stockModels.Count);
            var pos = new Vector3(0, _stockModels.Count * ItemHelper.CellItemPositionOffset, 0);

            switch (model)
            {
                case CellItemModel cellItemModel:
                    return new CollectItemMovePositionJob(model as CellItemModel, pos, delay);
                    break;
            }

            return null;
        }

        public void BoxDamage()
        {
            if (StockModelType != StockModelType.Box)
            {
                return;
            }

            BoxItemModel boxItemModel = _stockModels.Last() as BoxItemModel;
            _stockModels.Remove(boxItemModel);
            boxItemModel.Damage();
            if (_stockModels.Count == 0)
            {
                _isEmpty = true;
                StockModelType = StockModelType.None;
            }
        }

        public void SetBlendShapeValue(float leftDown, float rightDown, float leftUp, float rightUp)
        {
            Item.SetBlendShapeValue(leftDown, rightDown, leftUp, rightUp);
        }

        public void CreatPlate(int count)
        {
            _isPlate = true;
            _plateSkinnedMeshRenderers = Item.CreatPlate(count);
        }

        public void TakePlateDamage()
        {
            if (_isPlate)
            {
                if (_plateSkinnedMeshRenderers.Count > 0)
                {
                    var plate = _plateSkinnedMeshRenderers.Last();
                    _plateSkinnedMeshRenderers.Remove(plate);
                    plate.gameObject.SetActive(false);
                    Item.PlatePartcle(_plateSkinnedMeshRenderers.Count + 1);
                    if (_plateSkinnedMeshRenderers.Count<=0)
                    {
                        _isPlate = false;   
                    }
                    
                }
                else
                {
                    _isPlate = false;
                }
            }
        }
    }

    
}