using UnityEngine;

namespace Project.Scripts.Ships
{
    public static class Company
    {
        public enum CompanyName
        {
            Blue,
            Red,
            Green,
            Yellow,
            Purple,
            Orange
        }

        public static Color GetColor(CompanyName name)
        {
            return name switch
            {
                CompanyName.Blue => Color.blue,
                CompanyName.Red => Color.red,
                CompanyName.Green => Color.green,
                CompanyName.Yellow => Color.yellow,
                CompanyName.Purple => new Color(0.5f, 0f, 0.5f),
                CompanyName.Orange => new Color(1f, 0.5f, 0f),
                _ => Color.white
            };
        }
    }
}