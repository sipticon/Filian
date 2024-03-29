﻿using System.IO;

namespace Filian.MVVM.Model
{
    public struct OneFromFourTestInfo
    {
        public string WordName;
        public string WordTranslation;
        public string PicturePath1;
        public string PicturePath2;
        public string PicturePath3;
        public string PicturePath4;

        public OneFromFourTestInfo(string wordName, string wordTranslation, string picturePath1, string picturePath2, string picturePath3, string picturePath4)
        {
            this.WordName = wordName;
            this.WordTranslation = wordTranslation;
            this.PicturePath1 = Path.GetFullPath(picturePath1);
            this.PicturePath2 = Path.GetFullPath(picturePath2);
            this.PicturePath3 = Path.GetFullPath(picturePath3);
            this.PicturePath4 = Path.GetFullPath(picturePath4);
        }
    }
}