using System;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CoinMarketCap
{
    public partial class MainForm : Form
    {
        private int _currentPage = 1;

        public readonly CoinMarketCap CoinMarketCap;

        public MainForm()
        {
            InitializeComponent();
            InitDataGridView();
            CoinMarketCap = new CoinMarketCap();
            LoadPage(_currentPage);
        }

        private void InitDataGridView()
        {
            dgv.ContextMenuStrip = dataGridViewContext;
            dgv.ColumnCount = 6;
            dgv.Columns[0].Name = "ID";
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Market Cap";
            dgv.Columns[3].Name = "Symbol";
            dgv.Columns[4].Name = "Price";
            dgv.Columns[5].Name = "% 24h";
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            CoinMarketCap.Dispose();
        }

        private void DataGridViewOnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgv.HitTest(e.X, e.Y);
                dgv.ClearSelection();
                dgv.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void FillDataGridView(dynamic data)
        {
            dgv.Rows.Clear();
            foreach (var item in data)
            {
                dgv.Rows.Add(
                    item.id,
                    item.name,
                    $"${item.quote.USD.market_cap}",
                    item.symbol,
                    $"${item.quote.USD.price}",
                    $"{item.quote.USD.percent_change_24h}%"
                    );
            }
        }

        private void LoadPage(int page)
        {
            try
            {
                dynamic obj = JsonConvert.DeserializeObject(CoinMarketCap.GetPage(page));
                var data = obj.data;
                if (!data.HasValues)
                    return;

                FillDataGridView(data);
                _currentPage = page;
            }
            catch(WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ContextInfoOnClick(object sender, EventArgs e)
        {
            var index = dgv.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            var id = dgv.Rows[index].Cells[0].Value.ToString();
            var form = new CurrencyInfoForm(CoinMarketCap.GetCurrencyInfoById(id));
            form.Show(this);
        }

        private void menuFindByIdOnClick(object sender, EventArgs e)
        {
            var form = new FindByIDForm();
            form.Show(this);
        }

        private void menuExitOnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void menuRefreshOnClick(object sender, EventArgs e)
        {
            LoadPage(_currentPage);
        }

        private void menuViewHelpOnClick(object sender, EventArgs e)
        {
            var form = new HelpForm();
            form.Show(this);
        }

        private void btToBeginingOnClick(object sender, EventArgs e)
        {
            if (_currentPage <= 1)
                return;

            LoadPage(1);
        }

        private void btPreviousOnClick(object sender, EventArgs e)
        {
            if (_currentPage <= 1)
                return;

            LoadPage(_currentPage - 1);
        }

        private void btNextOnClick(object sender, EventArgs e)
        {
            LoadPage(_currentPage + 1);
        }
    }
}
