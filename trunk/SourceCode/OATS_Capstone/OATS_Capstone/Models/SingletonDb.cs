using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class SingletonDb
    {
        private static OATSDBEntities _oatsEntities= null;
        public static OATSDBEntities Instance()
        {
            if (_oatsEntities == null)
            {
                _oatsEntities = new OATSDBEntities();
            }
            return _oatsEntities;
        }
    }
}