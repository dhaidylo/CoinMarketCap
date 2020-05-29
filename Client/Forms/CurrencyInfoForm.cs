using System.Windows.Forms;

namespace CoinMarketCap
{
    public partial class CurrencyInfoForm : Form
    { 
        public CurrencyInfoForm(CurrencyInfo info)
        {
            InitializeComponent();
            pictureBox1.Image = info.Logo;
            lbName.Text = info.Name;
            linkWebsite.Text = info.Website;
            rtbDescription.Text = info.Description;
            lbPrice.Text = $"${info.Price}";
            lb24h.Text = $"{info.PriceChange24h}%";
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            pictureBox1.Image.Dispose();
        }

        private void linkWebsiteOnClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkWebsite.Text);
        }
    }
}
