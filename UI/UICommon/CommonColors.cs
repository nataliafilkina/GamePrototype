using UnityEngine;

namespace UICommon.Colors
{
    public static class CommonColors
    {
        public static Color32 NotAvailable { get; private set; } = new(90, 90, 90, 225);
        public static Color32 Available { get; private set; } = new(255, 255, 255, 255);
    }
}
