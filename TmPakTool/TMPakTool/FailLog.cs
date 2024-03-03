using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace paktool
{
    public partial class FailLog : Form
    {
        private class FailEntry
        {
            private string _text;
            private Exception _exception;

            public FailEntry(string text, Exception exception)
            {
                _text = text;
                _exception = exception;
            }

            public string Text
            {
                get { return _text; }
            }

            public Exception Exception
            {
                get { return _exception; }
            }

            public string ExceptionString
            {
                get { return string.Format("{0}\r\n\r\n{1}", _exception.Message, _exception.StackTrace); }
            }
        }
        private BindingList<FailEntry> _entries = new BindingList<FailEntry>();
      
        public FailLog()
        {
            InitializeComponent();

            _lstFails.DataSource = _entries;
            _lstFails.DisplayMember = "Text";
            _txtStacktrace.DataBindings.Add("Text", _entries, "ExceptionString");
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public void Add(string text, Exception exception)
        {
            _entries.Add(new FailEntry(text, exception));
        }

        public void Display()
        {
            if (_entries.Count == 0)
                return;

            ShowDialog();
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
