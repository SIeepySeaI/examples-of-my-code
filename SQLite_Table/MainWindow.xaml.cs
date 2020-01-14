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
using System.Windows.Shapes;
using System.Data.Entity;

namespace SQLite_Table
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AplicationContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new AplicationContext();
            db.GraficCards.Load();
            //загрузка данных в локальный кеш контекста данных, а затем список загруженых объектов устанавливается как контекст данных
            this.DataContext = db.GraficCards.Local.ToBindingList();
        }
        // добавление данных в БД
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CardWindow cardWindow = new CardWindow(new GraficCard());
            if (cardWindow.ShowDialog() == true)
            {
                GraficCard graficCard = cardWindow.GraficCard;
                db.GraficCards.Add(graficCard);
                db.SaveChanges();
            }
        }
        //редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            //если не выделено ни одного объекта, выходим из объекта
            if (graficCardList.SelectedItem == null) return;
            //получаем выделенный объект
            GraficCard graficCard = graficCardList.SelectedItem as GraficCard;

            CardWindow cardWindow = new CardWindow(new GraficCard
            {
                Id = graficCard.Id,
                Company = graficCard.Company,
                Price = graficCard.Price,
                Title = graficCard.Title
            });

            if (cardWindow.ShowDialog() == true)
            {
                // получаем измененный объект
                graficCard = db.GraficCards.Find(cardWindow.GraficCard.Id);
                if (graficCard != null)
                {
                    graficCard.Company = cardWindow.GraficCard.Company;
                    graficCard.Title = cardWindow.GraficCard.Title;
                    graficCard.Price = cardWindow.GraficCard.Price;
                    db.Entry(graficCard).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (graficCardList.SelectedItem == null) return;
            GraficCard graficCard = graficCardList.SelectedItem as GraficCard;
            db.GraficCards.Remove(graficCard);
            db.SaveChanges();
        }
    }
}
