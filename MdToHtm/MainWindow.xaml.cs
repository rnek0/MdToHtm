﻿using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;

namespace MdToHtm
{
    public partial class MainWindow : Window
    {
        private ConverterMD Convertisseur = null;

        public MainWindow()
        {
            InitializeComponent();

            Convertisseur = new ConverterMD();
            Convertisseur.FileConverted += (sndr,ev) => MessageBox.Show($"Converted file:\n{ev.FileConverted}");

            btnChoisir.Click += (s,e) => 
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
            };
        }

        /// <summary>
        /// Convert .md to .html
        /// </summary>
        private void BtnConvertir_Click(object sender, RoutedEventArgs e)
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
    }
}