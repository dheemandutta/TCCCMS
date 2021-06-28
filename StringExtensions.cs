﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;


namespace TCCCMS
{
    public static class StringExtensions
    {
        public static string Titleize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text).ToSentenceCase();
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        public static string CheckStringLenghtAndGetFirstFewCharecters(this string str, int len)
        {
            string text = str;

            if (text.Length > len+4)
            {
                text = !String.IsNullOrWhiteSpace(text) && text.Length >= len
                                ? text.Substring(0, len)
                                : text;
                text = text + " ...";
            }
            else
                text = text.Substring(0);

            return text;
        }
    }
}