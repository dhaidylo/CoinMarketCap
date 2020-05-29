using System;
using System.Windows.Forms;

namespace CoinMarketCap
{
    public partial class FindByIDForm : Form
    {
        public FindByIDForm()
        {
            InitializeComponent();
        }

        private void btOKOnClick(object sender, EventArgs e)
        {
            try
            {
                var owner = Owner as MainForm;
                var info = owner.CoinMarketCap.GetCurrencyInfoById(numID.Value.ToString());
                var infoForm = new CurrencyInfoForm(info);
                infoForm.Show(Owner);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btCancelOnClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
