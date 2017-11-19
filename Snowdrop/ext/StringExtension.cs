// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using System;

namespace Snowdrop.ext
{
    public static class StringExtension
    {
        public static string Truncate(this string value, int maxChars, String ellipsis)
        {
            // If the length of the value is greater or equal to the maxChars the String
            // will be truncated and the value of ellipsis will be added to the end.
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + ellipsis;
        }
    }
}
