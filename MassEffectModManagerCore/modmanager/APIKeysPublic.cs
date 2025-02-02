﻿using System.ComponentModel;

namespace ME3TweaksModManager.modmanager
{
    [Localizable(false)]
    public static partial class APIKeys
    {
        public static bool HasAppCenterKey => typeof(APIKeys).GetProperty("Private_AppCenter") != null;
        public static string AppCenterKey => (string)typeof(APIKeys).GetProperty("Private_AppCenter").GetValue(typeof(APIKeys));
    }
}
