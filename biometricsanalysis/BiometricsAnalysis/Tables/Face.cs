using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiometricsAnalysis.Tables
{
    public class Face
    {
        private int _faceId;
        private int _entryId;
        private float _percentage;
        private bool _isSample;
        private string _comment;
        private System.Windows.Media.Imaging.BitmapImage _img;

        public Face(int faceId, int entryId, float percentage, bool isSample, string comment, System.Windows.Media.Imaging.BitmapImage img)
        {
            this.faceId = faceId;
            this.entryId = entryId;
            this.percentage = percentage;
            this.isSample = isSample;
            this.comment = comment;
            this.img = img;
        }

        public int faceId { get { return _faceId; } set { _faceId = value; } }
        public int entryId { get { return _entryId; } set { _entryId = value; } }
        public float percentage { get { return _percentage; } set { _percentage = value; } }
        public bool isSample { get { return _isSample; } set { _isSample = value; } }
        public string comment { get { return _comment; } set { _comment = value; } }
        public System.Windows.Media.Imaging.BitmapImage img { get { return _img; } set { _img = value; } }

    }
}
