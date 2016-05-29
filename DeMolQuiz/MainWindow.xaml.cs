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
using System.IO;

namespace DeMolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int teller = 0;
        int herstart = 0;
        int tellerFeedback = 0;
        int timerafteller = 50;
        int tellerJuist = 0;
        string naam="";
        System.Timers.Timer aTimer;
        List<Part> vragenLijst = new List<Part>();

        public MainWindow()
        {
            InitializeComponent();

            //Application.Current.MainWindow.WindowState = WindowState.Maximized;

            LayoutRoot.Background = new SolidColorBrush(Colors.LightGray);
            this.Foreground = new SolidColorBrush(Colors.Black);

            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;

            timerText.Text = timerafteller + "s";

            AntwoordA.Visibility = Visibility.Hidden;
            AntwoordB.Visibility = Visibility.Hidden;
            Vraag.Visibility = Visibility.Hidden;
            timerText.Visibility = Visibility.Hidden;
            
            naamList.Items.Add("-");
            naamList.Items.Add("An A");
            naamList.Items.Add("Bart B");
            naamList.Items.Add("Chris C");
            naamList.Items.Add("Devid D");
            naamList.Items.Add("Evy E");
            naamList.SelectedIndex = 0;

            vragenLijst.Add(new Part() { PartName = "Is het een man?", PartId = "Neen" });
            vragenLijst.Add(new Part() { PartName = "Is de kleur van de broek die hij/zij draagt blauw?", PartId = "Neen" });
            vragenLijst.Add(new Part() { PartName = "Draagt hij/zij een bril?", PartId = "Neen" });
            vragenLijst.Add(new Part() { PartName = "Heeft hij/zij zwart haar? ", PartId = "Neen" });
            vragenLijst.Add(new Part() { PartName = "Draagt hij/zij laarzen? ", PartId = "Neen" });
            vragenLijst.Add(new Part() { PartName = "Heeft hij/zij kinderen? ", PartId = "Ja" });
            vragenLijst.Add(new Part() { PartName = "Is hij/zij getrouwd? ", PartId = "Ja" });
            vragenLijst.Add(new Part() { PartName = "Is hij/zij ouder dan 35 jaar?", PartId = "Neen" });
            vragenLijst.Add(new Part() { PartName = "Werkt hij/zij langer dan 9 jaar?", PartId = "Ja" });
            vragenLijst.Add(new Part() { PartName = "Woont hij/zij in Brussel?", PartId = "Ja" });
        }

        private void EventClickVolgende(object sender, RoutedEventArgs e)
        {
            if (herstart != 0)
            {
                Volgende.Content = "Start!";
                teller = 0;
                tellerFeedback = 0;
                timerafteller = 40;
                timerText.Text = timerafteller + "s";
                timerText.Visibility = Visibility.Hidden;
                tellerJuist = 0;
                naam = "";
                naamList.SelectedIndex = 0;
                naamList.IsReadOnly = false;
                naamList.IsEnabled = true;
                Feedback.Text = "Selecteer LINKS je naam en druk Start!";

                herstart = 0;
            }
            if (teller == vragenLijst.Count)
            {

                ///// END OF THE GAME FOR THE USER (NO QUESTIONS ANYMORE)
                ///// Eerst, laatst antwoord checken
                if (AntwoordA.IsChecked == true && (string)AntwoordA.Content == (string)vragenLijst[teller - 1].PartId)
                {
                    tellerJuist++;
                }
                else if (AntwoordB.IsChecked == true && (string)AntwoordB.Content == (string)vragenLijst[teller - 1].PartId)
                {
                    tellerJuist++;
                }
                ///// Dan, behandelen met het juiste aantal antwoorden
                aTimer.Stop();
                AntwoordA.Visibility = Visibility.Hidden;
                AntwoordB.Visibility = Visibility.Hidden;
                Vraag.Visibility = Visibility.Hidden;
                timerText.Visibility = Visibility.Hidden;
                Feedback.Text = "GEDAAN! (score: " + tellerJuist + "/10 ;)";
                Volgende.Content = "Volgende gebruiker!";
                
                ///// Wegschrijven in CSV: Naam, Uitslag, Tijd overgebleven seconden (timerafteller)
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1},{2}{3}", (string)naamList.SelectedItem, tellerJuist, timerafteller, Environment.NewLine);
                csv.Append(newLine);
                File.AppendAllText("Resultaat.csv", csv.ToString());

                herstart = 1;
            }
            else
            {
                if (naamList.SelectedIndex == 0)
                {
                    MessageBox.Show("Gelieve je naam te selecteren!!!");
                }
                else
                {
                    naam = (string)naamList.SelectedItem;

                    Vraag.Text = vragenLijst[teller].PartName;
                    tellerFeedback = teller + 1;
                    Feedback.Text = "Vraag " + tellerFeedback + "/" + vragenLijst.Count;// + "(" + vragenLijst[teller].PartId + ")";
                    if ((string)Volgende.Content == "Start!")
                    {
                        Volgende.Content = "Volgende vraag!";
                        aTimer.Enabled = true;
                        AntwoordA.Visibility = Visibility.Visible;
                        AntwoordB.Visibility = Visibility.Visible;
                        Vraag.Visibility = Visibility.Visible;
                        timerText.Visibility = Visibility.Visible;
                        naamList.IsReadOnly = true;
                        naamList.IsEnabled = false;
                    }
                    else
                    {
                        if (AntwoordA.IsChecked == true || AntwoordB.IsChecked == true)
                        {
                            if (AntwoordA.IsChecked == true && (string)AntwoordA.Content == (string)vragenLijst[teller-1].PartId)
                            {
                                LayoutRoot.Background = new SolidColorBrush(Colors.Green);
                                tellerJuist++;
                            }
                            else if (AntwoordB.IsChecked == true && (string)AntwoordB.Content == (string)vragenLijst[teller-1].PartId)
                            {
                                LayoutRoot.Background = new SolidColorBrush(Colors.Green);
                                tellerJuist++;
                            }
                            else
                            {
                                LayoutRoot.Background = new SolidColorBrush(Colors.Red);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Gelieve JA of NEEN te kiezen!");
                            teller--;
                        }
                    }
                    teller++;
                    AntwoordA.IsChecked = false;
                    AntwoordB.IsChecked = false;
                }
            }
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            timerafteller--;
            Application.Current.Dispatcher.Invoke(() => this.timerText.Text = timerafteller + "s");
            Application.Current.Dispatcher.Invoke(() => this.LayoutRoot.Background = new SolidColorBrush(Colors.LightGray));
            if (timerafteller<=0)
            {
                ///// END OF THE GAME FOR THE USER (TIMER IS OVER)
                aTimer.Stop();
                Application.Current.Dispatcher.Invoke(() => AntwoordA.Visibility = Visibility.Hidden);
                Application.Current.Dispatcher.Invoke(() => AntwoordB.Visibility = Visibility.Hidden);
                Application.Current.Dispatcher.Invoke(() => Vraag.Visibility = Visibility.Hidden);
                Application.Current.Dispatcher.Invoke(() => timerText.Text = "0s");
                Application.Current.Dispatcher.Invoke(() => Feedback.Text = "GEDAAN! (score: " + tellerJuist + "/10 ;)");
                Application.Current.Dispatcher.Invoke(() => Volgende.Content = "Volgende gebruiker!");

                ///// Wegschrijven in CSV: Naam, Uitslag, Tijd overgebleven seconden (timerafteller)
                var csv2 = new StringBuilder();
                var newLine2 = string.Format("{0},{1},{2}{3}", naam, tellerJuist, "0", Environment.NewLine);
                csv2.Append(newLine2);
                File.AppendAllText("Resultaat.csv", csv2.ToString());

                herstart = 1;
            }
        }
    }

    internal class Part
    {
        public Part()
        {
        }

        public string PartId { get; set; }
        public string PartName { get; set; }
    }
}