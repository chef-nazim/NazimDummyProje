using System.Linq;
using NCG.template.Controllers;
using NCG.template.enums;
using NCG.template.GameFolders.Scripts.Level;
using NCG.template.Managers;
using NCG.template.models;
using NCG.template.Objects;
using NCG.template.Scripts.Item;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;

namespace NCG.template.Scripts.State
{
    public class LevelCreatState : ProcessState
    {
        private LevelModelController _levelModelController => LevelModelController._instance;
        private PoolManager _poolManager => PoolManager.Instance;
        private Containers _containers => Containers.instance;
        
        LevelModel _levelModel;
        WrapperLevelData levelData;
        GameHelper _gameHelper;
        


        private float _gridItemWidth;
        private float _gridItemHeight;
       
        public async override void Handle(LevelProcess product)
        {
            _levelModel = _levelModelController.LevelModel;
            levelData = _levelModel._levelData;
            _gameHelper = _containers.GameHelper;
            
            _gridItemWidth = _gameHelper.GridItemWidth;
            _gridItemHeight = _gameHelper.GridItemHeight;
            
            LevelCreat();
            WaitForComplete();
            
        }
           void LevelCreat()
        {
            //_levelModel.GameModel.MoveCount = levelData.MoveCount;
           // _levelModel.SetCameraSettings();

            for (int i = 0; i < levelData.RowCount; i++)
            {
                for (int j = 0; j < levelData.ColumnCount; j++)
                {
                    var gridData = levelData.GridDataList[(i * levelData.ColumnCount) + j];

                    
                    
                    if (gridData.CellType == (int)StockModelType.Empty)
                    {
                        continue;
                    }

                    GridItemModel gridItemModel = new GridItemModel();
                    GridItem gridItem = _poolManager.GridItemPooling.GetItem(gridItemModel);
                    gridItemModel.SetParent(_containers.SelectGrid.transform);
                    gridItemModel.SetPosition(new Vector3(j * _gridItemWidth, 0 /*i * .8f*/, i * _gridItemHeight));
                    gridItemModel.GridAddress = new Vector2Int(i, j);
                    if (gridData.CellType == (int)StockModelType.CellItemModel)
                    {
                        for (int k = 0; k < gridData.CellDatas.Count; k++)
                        {
                            for (int l = 0; l < gridData.CellDatas[k].Count; l++)
                            {
                                CellItemModel cellItemModel = new CellItemModel();
                                cellItemModel.ColorID = gridData.CellDatas[k].ColorID;
                                _poolManager.CellItemPooling.GetItem(cellItemModel);
                                gridItemModel.CreatTakeModel(cellItemModel);
                            }
                        }
                    }
                    else if (gridData.CellType == (int)StockModelType.Box)
                    {
                        for (int k = 0; k < gridData.CellDatas[0].Count; k++)
                        {
                            BoxItemModel boxItemModel = new BoxItemModel();
                            _poolManager.BoxItemPooling.GetItem(boxItemModel);
                            gridItemModel.CreatTakeModel(boxItemModel);
                        }
                    }
                    else if (gridData.CellType == (int)StockModelType.Dispenser)
                    {
                        DispenserItemModel dispenserItemModel = new DispenserItemModel(_poolManager.CellItemPooling, gridItemModel, gridData.CellDatas);
                        _poolManager.DispenserItemPooling.GetItem(dispenserItemModel);
                        gridItemModel.CreatTakeModel(dispenserItemModel);
                    }
                    else if (gridData.CellType == (int)StockModelType.Plate)
                    {
                        gridItemModel.CreatPlate(gridData.CellDatas[1].Count);
                        var list = gridData.CellDatas.ToList();
                        list.RemoveAt(1);
                        for (int l = 0; l < list.Count; l++)
                        {
                            CellItemModel cellItemModel = new CellItemModel();
                            cellItemModel.ColorID = list[l].ColorID;
                            _poolManager.CellItemPooling.GetItem(cellItemModel);
                            gridItemModel.CreatTakeModel(cellItemModel);
                        }
                    }
                    else if (gridData.CellType == (int)StockModelType.MailBox)
                    {
                        MailBoxItemModel mailBoxItemModel = new MailBoxItemModel();
                        _poolManager.MailBoxItemPooling.GetItem(mailBoxItemModel);
                        gridItemModel.CreatTakeModel(mailBoxItemModel);
                    }
                    else if (gridData.CellType == (int)StockModelType.Egg)
                    {
                        EggItemModel eggItemModel = new EggItemModel();
                        _poolManager.EggItemPooling.GetItem(eggItemModel);
                        gridItemModel.CreatTakeModel(eggItemModel);
                    }
                    
                    
                    _levelModel.GridItemModels.Add(gridItemModel);
                }
            }
            foreach (var v in levelData.GoalDataList)
            {
                GoalItemModel goalItemModel = new GoalItemModel();
                GoalItem goalItem = _poolManager.GoalItemPooling.GetItem(goalItemModel);
                goalItemModel.SetParent(_levelModel.GoalParent);
                goalItemModel.SetMaterial(v.ColorID);
                goalItemModel.SetGoalCount(v.Count);
                _levelModel.GoalItemModels.Add(goalItemModel);
            }
            foreach (var v in _levelModel.GridItemModels)
            {
                float leftDown = 0;
                float rightDown = 0;
                float leftUp = 0;
                float rightUp = 0;

                GridItemModel up = _levelModel.GridItemModels.Find(x =>
                    x.GridAddress.y == v.GridAddress.y && x.GridAddress.x == v.GridAddress.x + 1);
                GridItemModel down = _levelModel.GridItemModels.Find(x =>
                    x.GridAddress.y == v.GridAddress.y && x.GridAddress.x == v.GridAddress.x - 1);
                GridItemModel left = _levelModel.GridItemModels.Find(x =>
                    x.GridAddress.y == v.GridAddress.y - 1 && x.GridAddress.x == v.GridAddress.x);
                GridItemModel right = _levelModel.GridItemModels.Find(x =>
                    x.GridAddress.y == v.GridAddress.y + 1 && x.GridAddress.x == v.GridAddress.x);
                if (left == null &&
                    down == null )
                {
                    leftDown = 0;
                }
                else
                {
                      leftDown = 100;    
                }

                if (right == null &&
                    down == null )
                {
                    rightDown = 0;
                }
                else
                {
                    rightDown = 100;    
                }
              
                
                if (left == null &&
                    up == null )
                {
                    leftUp = 0;
                }
                else
                {
                    leftUp =100;    
                }
                
                if (right== null &&
                    up== null )
                {
                    rightUp = 0;
                }
                else
                {
                    rightUp = 100;    
                }
                
                v.SetBlendShapeValue(leftDown,rightDown,leftUp,rightUp);
            }
        }
    }
}