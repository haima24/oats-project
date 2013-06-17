using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class CommonService
    {
        bool _success = false;
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }

        string _message = Constants.DefaultProblemMessage;
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        string _generatedHtml = string.Empty;
        public string generatedHtml
        {
            get { return _generatedHtml; }
            set { _generatedHtml = value; }
        }

        List<Object> _resultlist = new List<object>();
        public List<Object> resultlist
        {
            get { return _resultlist; }
            set { _resultlist = value; }
        }

        ArrayList _arraylist = new ArrayList();
        public ArrayList arraylist
        {
            get { return _arraylist; }
            set { _arraylist = value; }
        }

    }
}