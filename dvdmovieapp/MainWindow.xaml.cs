using dvdmovieapp.DAL;
using dvdmovieapp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dvdmovieapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // select * from film
            // få tillbaka lista med alla filmer i min databas
            // repository pattern = en slags yta där jag kan skapa kopplingar till olika saker


            DbRepository db = new();
            //var film = await db.GetFilm();


            var category = new Category();
            {
                Name = "spökhistorier";
            };
            await db.AddCategory(category);
        }
    }
}
