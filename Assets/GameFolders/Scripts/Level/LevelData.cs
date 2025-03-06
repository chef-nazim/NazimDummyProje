using System;
using System.Collections.Generic;

namespace NCG.template.GameFolders.Scripts.Level
{
    [Serializable]
    public class WrapperLevelData
    {
        public int HardLevel;
        public int PathPossibility;
        public float EmptyPossibility;
        public int RowCount;
        public int ColumnCount;
        public int MoveCount;
        public List<GridDatas> GridDataList;
        public List<GoalData> GoalDataList;
        public List<int> UsableColorIDList;
    }



    [Serializable]
    public class GridDatas
    {
        public int CellType;
        public List<CellData> CellDatas;
    }

    [Serializable]
    public class CellData
    {
        public int ColorID;
        public int Count;
    }

    [Serializable]
    public class GoalData
    {
        public int ColorID;
        public int Count;
    }
}