﻿using System;
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

namespace RolloBot.View
{
    /// <summary>
    /// Interaktionslogik für CapturePreviewView.xaml
    /// </summary>
    public partial class CapturePreviewView : UserControl
    {
        private readonly ViewModel.MainViewModel viewModel;

        public CapturePreviewView(ViewModel.MainViewModel vm)
        {
            InitializeComponent();

            this.viewModel = vm;
            this.DataContext = viewModel;
        }
    }
}
