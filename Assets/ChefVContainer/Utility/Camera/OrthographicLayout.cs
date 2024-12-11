using UnityEngine;

namespace gs.chef.vcontainer.utility.camera
{
    public class OrthographicLayout
    {
        public const float DEFAULT_ORTHOGRAPHIC_SIZE = 5f;
        
        public static float Aspect { get; private set; }
        public static float OrthographicSize { get; private set; } = DEFAULT_ORTHOGRAPHIC_SIZE;
        public static int Row { get; private set; }
        public static int Column { get; private set; }

        public static Padding Padding { get; private set; }
        public static Vector2 Origin { get; private set; }
        
        public static Vector2 UnitSize { get; private set; }
        
        public static void Initialize(int row, float aspect, Padding padding, Vector2 unitSize)
        {
            Aspect = aspect;
            Padding = padding;
            Row = row;
            UnitSize = unitSize;

            Column = Mathf.FloorToInt(Aspect * ((Row * UnitSize.y) + Padding.Top + Padding.Bottom));

            OrthographicSize = ((Column * UnitSize.x) + (Padding.Left + Padding.Right)) / (Aspect * 2f);

            

            float factor = (OrthographicSize / DEFAULT_ORTHOGRAPHIC_SIZE);

            if (OrthographicSize * 2f - ((Padding.Top + Padding.Bottom) * factor) < (Row * UnitSize.y))
                OrthographicSize = ((DEFAULT_ORTHOGRAPHIC_SIZE * ((Row*UnitSize.y) / 2f)) / ((DEFAULT_ORTHOGRAPHIC_SIZE * 2f - (Padding.Top + Padding.Bottom)) / 2f));

            float additionalColumn = OrthographicSize * Aspect * 2f;
            additionalColumn -= ((Column*UnitSize.x) + (Padding.Left + Padding.Right));
            additionalColumn = Mathf.FloorToInt(additionalColumn);
            Column += (int)additionalColumn;

            Origin = GetOrigin();

        }

        public static void Initialize(int row, int column, float aspect, Padding padding, Vector2 unitSize)
        {
            Aspect = aspect;
            Padding = padding;
            Row = row;
            Column = column;
            UnitSize = unitSize;

            OrthographicSize = ((Column * UnitSize.x) + (Padding.Left + Padding.Right)) / (Aspect * 2f);
            float factor = (OrthographicSize / DEFAULT_ORTHOGRAPHIC_SIZE);

            if (OrthographicSize * 2f - ((Padding.Top + Padding.Bottom) * factor) < (Row* UnitSize.y))
                OrthographicSize = ((DEFAULT_ORTHOGRAPHIC_SIZE * ((Row * UnitSize.y) / 2f)) / ((DEFAULT_ORTHOGRAPHIC_SIZE * 2f - (Padding.Top + Padding.Bottom)) / 2f));

            Origin = GetOrigin();

        }

        private static Vector3 GetOrigin()
        {

            float factor = (OrthographicSize / DEFAULT_ORTHOGRAPHIC_SIZE);
            float left = Padding.Left;
            float top = Padding.Top * factor;
            float bottom = Padding.Bottom * factor;
            float right = Padding.Right;

            return new Vector3((left - right) / 2f, (bottom - top) / 2);
        }
    }
}