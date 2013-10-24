using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiometricsAnalysis.Signature
{
    public class puncte
    {

        public int x = 0;
        public int y = 0;


    }
    class SignatureModule
    {
        string[] lines1, lines2, x1, x2, y1, y2;

       public puncte[] points1;
       public puncte[] points2;
       public float rasp=0, rasp2=0, rasp3=0;
        public string file1_text = "";
        public string file2_text = "";
        public string output = "";
        public string output1 = "", output2 = "", output3 = "";
        public void getFile1(string filePath)
        {   
                 
            lines1 = System.IO.File.ReadAllLines(filePath);


            x1 = new string[lines1.Length];
            y1 = new string[lines1.Length];
            points1 = new puncte[lines1.Length];
            for (int i = 0; i < lines1.Length; i++)
            {
                x1[i] = ""; y1[i] = "";
                points1[i] = new puncte();
            }

            for (int i = 0; i < lines1.Length; i++)
            {
                string[] temp = lines1[i].Split(' ');
                x1[i] = temp[0];
                y1[i] = temp[1];
                file1_text += x1[i] + " " + y1[i] + "\r\n";
                points1[i].x = Convert.ToInt32(x1[i])-8000;
                points1[i].y = Convert.ToInt32(y1[i])-17000;
            }
        }

        public void getFile2(string filePath)
        {
            


            lines2 = System.IO.File.ReadAllLines(filePath);


            x2 = new string[lines2.Length];
            y2 = new string[lines2.Length];
            points2 = new puncte[lines2.Length];
            for (int i = 0; i < lines2.Length; i++)
            {
                x2[i] = ""; y2[i] = "";
                points2[i] = new puncte();
            }

            for (int i = 0; i < lines2.Length; i++)
            {
                string[] temp = lines2[i].Split(' ');
                x2[i] = temp[0];
                y2[i] = temp[1];
                file2_text += x2[i] + " " + y2[i] + "\r\n";
                points2[i].x = Convert.ToInt32(x2[i])-7000;
                points2[i].y = Convert.ToInt32(y2[i])-18000;
            }
        }

        public void compute()
        {
             rasp = compare_raport(points1, points2);
             rasp2 = compare3(points1, points2);
             rasp3 = centru_intern(points1, points2);
            if (rasp == 0) output += "Nu corespund";
            else
            {

                output = "LR:" + rasp + "% DS:" + rasp2 + "% CI:" + rasp3 + "% TOTAL: " +
                    ((rasp + rasp3)/2 + rasp2) / 2 + "%";
                output1 =""+ rasp;
                output2 =""+ rasp2;
                output3 =""+ rasp3;

            }

        }

        public float compare_raport(puncte[] points, puncte[] _points)
        {


            int err = 50;
            List<int[]> raporturi = new List<int[]>();
            List<int[]> _raporturi = new List<int[]>();

            int size = 0;
            if (points.Length < _points.Length)
                size = points.Length;
            else size = _points.Length;

            if (Math.Abs(points.Length - _points.Length) > points.Length / 2) { return 0; }

            int k = 0;
            for (int i = 0; i < points.Length; i++)
            {
                int temp = 1;
                int[] rap = new int[1000];
                for (int j = 0; j < points.Length; j++)
                {
                    temp = (int)Math.Sqrt(Math.Pow((points[i].x - points[j].x), 2)
                           + Math.Pow((points[i].y - points[j].y), 2));

                    rap[j] = temp;
                }
                raporturi.Add(rap);

            }

            for (int i = 0; i < _points.Length; i++)
            {
                int temp = 1;
                int[] _rap = new int[1000];

                for (int j = 0; j < _points.Length; j++)
                {
                    temp = (int)Math.Sqrt(Math.Pow((_points[i].x - _points[j].x), 2)
                           + Math.Pow((_points[i].y - _points[j].y), 2));

                    _rap[j] = temp;
                }

                _raporturi.Add(_rap);

            }

            int nr = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (Math.Abs(raporturi.ElementAt(i)[j] - _raporturi.ElementAt(i)[j]) <= err)
                        nr++;

            return nr * 100 / (points.Length * points.Length);

        }
        float compare3(puncte[] points, puncte[] _points)
        {

            int[] unghi = new int[points.Length];
            int[] _unghi = new int[_points.Length];
            int[] lung = new int[points.Length];
            int[] _lung = new int[_points.Length];

            int size = 0;
            if (points.Length < _points.Length)
                size = points.Length;
            else size = _points.Length;

            if (Math.Abs(points.Length - _points.Length) > points.Length / 2) { return 0; }



            for (int i = 1; i < points.Length - 1; i++)
            {
                unghi[i] = (int)unghi_(points.ElementAt(i - 1), points.ElementAt(i), points.ElementAt(i + 1));
                lung[i] = (int)Math.Sqrt(Math.Pow((points.ElementAt(i).x - points.ElementAt(i + 1).x), 2)
                       + Math.Pow((points.ElementAt(i).y - points.ElementAt(i + 1).y), 2));
            }
            for (int i = 1; i < _points.Length - 1; i++)
            {
                _unghi[i] = (int)unghi_(_points.ElementAt(i - 1), _points.ElementAt(i), _points.ElementAt(i + 1));
                _lung[i] = (int)Math.Sqrt(Math.Pow((_points.ElementAt(i).x - _points.ElementAt(i + 1).x), 2)
                       + Math.Pow((_points.ElementAt(i).y - _points.ElementAt(i + 1).y), 2));
            }


            int nr = 0;
            for (int i = 1; i < size - 2; i++)

                if (Math.Abs(unghi[i] - _unghi[i]) <= 20) //&& Math.Abs(lung[i] - _lung[i]) < 50)
                    nr++;

            return nr * 100 / (points.Length - 2);
        }
        double unghi_(puncte A1, puncte A2, puncte A3)
        {
            double unghi;

            double cos_unghi;
            cos_unghi = (double)(((A1.x - A2.x) * (A3.x - A2.x) + (A1.y - A2.y) * (A3.y - A2.y)) /
                    (Math.Sqrt(Math.Pow(A1.x - A2.x, 2) + Math.Pow(A1.y - A2.y, 2))
                            * Math.Sqrt((Math.Pow(A3.x - A2.x, 2) + Math.Pow(A3.y - A2.y, 2)))));
            //cos_unghi=Math.toRadians(cos_unghi);



            unghi = Math.Acos(cos_unghi);

            //unghi=Math.acos(cos_unghi);  
            // Log.v("ceva:", "ceva:" + unghi);

            return (unghi * (180.0 / Math.PI));


        }

        float centru_intern(puncte[] points, puncte[] _points)
        {
            // luam punctele de centru din ambele
            puncte centru = new puncte();
            puncte _centru = new puncte();

            int minX = 99999, maxX = 0, minY = 99999, maxY = 0;
            int _minX = 99999, _maxX = 0, _minY = 99999, _maxY = 0;


            for (int i = 0; i < points1.Length; i++)
            {
                if (points1.ElementAt(i).x < minX)
                    minX = points1.ElementAt(i).x;
                if (points1.ElementAt(i).x > maxX)
                    maxX = points1.ElementAt(i).x;
                if (points1.ElementAt(i).y < minY)
                    minY = points1.ElementAt(i).y;
                if (points1.ElementAt(i).y > maxY)
                    maxY = points1.ElementAt(i).y;


            }
            for (int i = 0; i < points2.Length; i++)
            {
                if (points2.ElementAt(i).x < _minX)
                    _minX = points2.ElementAt(i).x;
                if (points2.ElementAt(i).x > _maxX)
                    _maxX = points2.ElementAt(i).x;
                if (points2.ElementAt(i).y < _minY)
                    _minY = points1.ElementAt(i).y;
                if (points2.ElementAt(i).y > _maxY)
                    _maxY = points1.ElementAt(i).y;

            }


            _centru.x = (_minX + _maxX) / 2;
            _centru.y = (_minY + _maxY) / 2;
            centru.x = (minX + maxY) / 2;
            centru.y = (minY + maxY) / 2;

            // terminat de luat punctele





            int[] rap = new int[1000];
            int[] _rap = new int[1000];

            int size = 0;
            if (points.Length < _points.Length)
                size = points.Length;
            else size = _points.Length;

            if (Math.Abs(points.Length - _points.Length) > points.Length / 2) { return 0; }

            int k = 0;
            for (int i = 0; i < points.Length; i++)
            {
                int temp = 1;


                temp = (int)Math.Sqrt(Math.Pow((points.ElementAt(i).x - centru.x), 2)
                       + Math.Pow((points.ElementAt(i).y - centru.y), 2));

                rap[i] = temp;



                //	temp = (int)((int) Math.sqrt(Math.pow((points.get(0).x-points.get(points.size()-1).x),2)
                //	   +Math.pow((points.get(0).y-points.get(points.size()-1).y),2)))/temp;

            }


            for (int i = 0; i < _points.Length; i++)
            {
                int temp = 1;



                temp = (int)Math.Sqrt(Math.Pow((_points.ElementAt(i).x - _centru.x), 2)
                       + Math.Pow((_points.ElementAt(i).y - _centru.y), 2));

                _rap[i] = temp;



                //	temp = (int)((int) Math.sqrt(Math.pow((_points.get(0).x-_points.get(_points.size()-1).x),2)
                //	   +Math.pow((_points.get(0).y-_points.get(_points.size()-1).y),2)))/temp;

            }



            int nr = 0;
            for (int i = 0; i < size; i++)
                if (Math.Abs(rap[i] - _rap[i]) <= 5000)
                    nr++;

            return nr * 100 / (points.Length);

        }


    }
}
