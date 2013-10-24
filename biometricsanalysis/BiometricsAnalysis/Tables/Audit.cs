using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiometricsAnalysis.Tables
{
    public class Audit
    {
        int _id_log;
        DateTime _date_audit;
        string _name_audit;
        string _info_audit;

        public Audit(int id_log, string action_name, string action_info)
        {
            this.id_log = id_log;
            this.date_audit = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss.fff"));
            this.name_audit = action_name;
            this.info_audit = action_info;
        }

        public int id_log { get { return _id_log; } set { _id_log = value; } }
        public DateTime date_audit { get { return _date_audit; } set { _date_audit = value; } }
        public string name_audit { get { return _name_audit; } set { _name_audit = value; } }
        public string info_audit { get { return _info_audit; } set { _info_audit = value; } }
    }
}
