using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PannelloCharger
{
    public class usParameters
    {
        private string _mainName = "";
        private int _mainCount = 0;
        private int _mainTarget = 0;
        private int _mainPerc = 0;

        private string _secName = "";
        private int _secCount = 0;
        private int _secTarget = 0;
        private int _secPerc = 0;

        public string MainName
        {
            set
            {
                _mainName = value;
            }
            get
            {
                return _mainName;
            }
        }
        public int MainCount
        {
            set
            {
                _mainCount = value;
            }
            get
            {
                return _mainCount;
            }
        }
        public int MainTarget
        {
            set
            {
                _mainTarget = value;
            }
            get
            {
                return _mainTarget;
            }
        }
        public int MainPerc
        {
            set
            {
                _mainPerc = value;
            }
            get
            {
                return _mainPerc;
            }
        }

        public string SecName
        {
            set
            {
                _secName = value;
            }
            get
            {
                return _secName;
            }
        }
        public int SecCount
        {
            set
            {
                _secCount = value;
            }
            get
            {
                return _secCount;
            }
        }
        public int SecTarget
        {
            set
            {
                _secTarget = value;
            }
            get
            {
                return _secTarget;
            }
        }
        public int SecPerc
        {
            set
            {
                _secPerc = value;
            }
            get
            {
                return _secPerc;
            }
        }

    }
}
