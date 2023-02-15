using FirstGame.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FirstGame.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}