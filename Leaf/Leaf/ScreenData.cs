using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaf
{
    class ScreenData
    {
        #region Singleton
        private static readonly ScreenData INSTANCE = new ScreenData();
        public static ScreenData Get()
        {
            return INSTANCE;
        }
        #endregion

        int fullScreenWidth;
        int fullScreenHeight;

        public ScreenData()
        {
            this.fullScreenWidth = 1920;
            this.fullScreenHeight = 1080;
        }

        public int GetFullScreenWidth()
        {
            return this.fullScreenWidth;
        }

        public int GetFullScreenHeight()
        {
            return this.fullScreenHeight;
        }
    }
}
