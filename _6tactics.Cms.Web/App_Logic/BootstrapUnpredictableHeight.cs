using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _6tactics.Cms.Web.App_Logic
{
    public class BootstrapUnpredictableHeight
    {
        private readonly List<string> _bootstrapClassesCache;
        private string _displayTypeCache = null;

        private List<double> ElementsWidthPercentage => new List<double>
        {
            0,
            8.33333333,
            16.66666667,
            25,
            33.33333333,
            41.66666667,
            50,
            58.33333333,
            66.66666667,
            75,
            83.33333333,
            91.66666667,
            100,
        };

        public BootstrapUnpredictableHeight()
        {
            _bootstrapClassesCache = new List<string>();
        }

        private int ConvertClassNameToInt(string bootstrapClass)
        {
            return Convert.ToInt32(Regex.Match(bootstrapClass, @"\d+").Value);
        }

        public void ClearCache()
        {
            _bootstrapClassesCache.Clear();
        }

        public void ClearCacheWithoutLast()
        {
            var firsItemFromCache = _bootstrapClassesCache.Last();
            _bootstrapClassesCache.Clear();
            _bootstrapClassesCache.Add(firsItemFromCache);
        }

        private void StoreNewDisplayTypeAndClearCache(string displayType)
        {
            if (_displayTypeCache == null || !_displayTypeCache.Equals(displayType))
            {
                _displayTypeCache = displayType;
                ClearCache();
            }
        }

        public bool IsElementInNewRow
        {
            get
            {
                double elementsWidthPercentage = 0;

                foreach (var item in _bootstrapClassesCache)
                {
                    elementsWidthPercentage += ElementsWidthPercentage[ConvertClassNameToInt(item)];

                    if (Math.Floor(elementsWidthPercentage) > 100)
                        return true;
                }

                return false;
            }
        }

        public static string ClearBothClassHandler(BootstrapUnpredictableHeight bootstrapUnpredictableHeight, string displayType, string bootstrapClass)
        {
            string clearBoth = "";

            bootstrapUnpredictableHeight.StoreNewDisplayTypeAndClearCache(displayType);
            bootstrapUnpredictableHeight._bootstrapClassesCache.Add(bootstrapClass);

            if (!bootstrapUnpredictableHeight.IsElementInNewRow) return clearBoth;

            clearBoth = "clear-both";
            bootstrapUnpredictableHeight.ClearCacheWithoutLast();

            return clearBoth;
        }

    }

}

