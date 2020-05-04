﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetMeter.Helper
{
    public static class Numeric
    {
        public static string SizeFormat(this double bytes, string suffix = null)
        {
            string[] norm = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int count = norm.Length - 1;
            var size = bytes;
            int x = 0;

            while (size >= 1000 && x < count)
            {
                size /= 1024;
                x++;
            }

            return string.Format($"{Math.Round(size, 2)} {norm[x]}{suffix}", MidpointRounding.AwayFromZero);
        }
    }
}
