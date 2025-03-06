using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NCG.template.Scripts.Interfaces
{
    public interface IStockModel
    {
        public void SetLocalPosition(int index);
        public void SetParent(Transform itemTransform);
        public UniTask FallMovePosition(Vector3 localPos);
        public void CreateSettings();
    }
}