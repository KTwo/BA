using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiometricsAnalysis.Tables
{
    public class Log
    {
        private int _id_log;
        private int _id_user;
        private DateTime _login_date;
        private DateTime _logout_date;
        private string _ip;


        public Log(int id_user)
        {
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);

            this.id_user = id_user;
            this.login_date = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss.fff"));
            this.ip = ip.AddressList[1].ToString();
        }

        public int id_log { get { return _id_log; } set { _id_log = value; } }
        public int id_user { get { return _id_user; } set { _id_user = value; } }
        public DateTime login_date { get { return _login_date; } set { _login_date = value; } }
        public DateTime logout_date { get { return _logout_date; } set { _logout_date = value; } }
        public string ip { get { return _ip; } set { _ip = value; } }
    }
}
