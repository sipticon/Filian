using System;
using System.Windows.Controls;
using Filian.Core;

namespace Filian.MVVM.ViewModel
{
    public class ViewModel : ObservableObject
    {
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public readonly string sqlConnectionString = @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

        public readonly Random _random = new Random();

        protected static string _answer = "";

        protected static int _countOfTests;

        public int CountOfTests
        {
            get => _countOfTests;
            set => _countOfTests = value;
        }

        public static Grid Grid
        {
            get;
            set;
        }

        public static int Column { get; set; }
        public static int Row { get; set; }
    }
}