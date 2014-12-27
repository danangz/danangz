using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// Get setting from AppSettings node
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string key)
        {
            return GetSetting(key, default(T));
        }

        /// <summary>
        /// Get setting from AppSettings node with default value
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">Setting key</param>
        /// <param name="defaultValue">Default setting value</param>
        /// <returns></returns>
        public static T GetSetting<T>(string key, T defaultValue)
        {
            try
            {
                string appSetting = ConfigurationManager.AppSettings[key];

                if (string.IsNullOrEmpty(appSetting)) return defaultValue;

                return (T)Convert.ChangeType(appSetting, typeof(T), CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static Bitmap CreateBitmapFromBytes(byte[] byteArray)
        {
            Bitmap bitmap = null;
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                bitmap = (Bitmap)tc.ConvertFrom(byteArray);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return bitmap;
        }

        public static byte[] CreateBytesFromBitmap(Bitmap thumbBitmap)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(thumbBitmap, typeof(byte[]));
        }
    }
}
