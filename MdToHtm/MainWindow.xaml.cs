using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using System.Windows.Input;

namespace MdToHtm
{
    public partial class MainWindow : Window
    {
        private ConverterMD Convertisseur = null;
        private string FileToConvert = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            Convertisseur = new ConverterMD();
            Convertisseur.FileConverted += (sndr, ev) => MessageBox.Show($"Converted file:\n{ev.FileConverted}");

            btnChoisir.Click += (s, e) =>
            {
                OpenMdFile();
            };

            this.Closing += (s, e) =>
            {
              if(MessageBoxResult.No == MessageBox.Show("Do you want to close the window ?", 
                  "Closing...", 
                  MessageBoxButton.YesNo))
                {
                    e.Cancel = true;
                }
            };
        }

        private void OpenMdFile()
        {
            txtUriChoix.Text = "";

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Markdown files (*.md)|*.md"
            };

            var uriFile = (ofd.ShowDialog() == true) ? ofd.FileName : "";
            txtUriChoix.Text = uriFile;

            try
            {
                TextBlockFileMd.Text = File.Exists(uriFile) ? File.ReadAllText(uriFile) : "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Oups ! \n{ex.Message}");
            }

            Convertisseur.Fichier = uriFile;
            btnConvertir.Focus();
        }

        /// <summary>
        /// Convert .md to .html
        /// </summary>
        private void BtnConvertir_Click(object sender, RoutedEventArgs e)
        {
            FileToConvert = txtUriChoix.Text;
            if (File.Exists(FileToConvert))
            {
                if (!string.IsNullOrWhiteSpace(txtUriChoix.Text))
                {
                    Convertisseur.Convert();
                }
                else
                {
                    MessageBox.Show("Vous devez choisir un fichier markdown !");
                }
            }
            else
            {
                MessageBox.Show("Vôtre fichier n'existe pas ou a été supprime entretemps...\nAucune action n'est efectuée.");
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenMdFile();
        }

    }
}