using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CoinMarketCap
{
    public partial class HelpForm : Form
    {
        private const string FILE_NAME = "help.json";
        private Dictionary<string, string> _help;

        public HelpForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            var json = File.ReadAllText($"{projectDirectory}\\{FILE_NAME}");
            _help = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            lbHeaders.Items.AddRange(_help.Keys.ToArray());
        }

        private void lbHeadersOnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = lbHeaders.SelectedItem;
            if (selected != null)
                rtbDescription.Text = _help[selected.ToString()];
        }
    }
}
