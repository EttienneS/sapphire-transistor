using Assets.Helpers;
using UnityEngine;

namespace Assets.Cards
{
    public enum CardColor
    {
        Red, Yellow, Blue, Green
    }

    public static class CardColorExtensions
    {
        public static Color GetActualColor(this CardColor cardColor)
        {
            return cardColor switch
            {
                CardColor.Blue => ColorExtensions.GetColorFromHex("337DC1"),
                CardColor.Red => ColorExtensions.GetColorFromHex("DF0101"),
                CardColor.Yellow => ColorExtensions.GetColorFromHex("FFC20A"),
                CardColor.Green => ColorExtensions.GetColorFromHex("5AA300"),
                _ => Color.magenta,
            };
        }
    }
}