using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WeightWizard.Model;
using WeightWizard.Model.Drawables;
using WeightWizard.View;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using WeightWizard.View.WeightWizard;

namespace WeightWizard.ViewModel
{
    public partial class TrendPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<weightModel> data;


        public ObservableCollection<weightModel> webdata;



        public enum ShowStates
        {
            All,
            ThreeMonths,
            Month
        }

        public ShowStates state;


        public TrendPageViewModel()
        {
            Data = new ObservableCollection<weightModel>();

            webdata = new ObservableCollection<weightModel>();

            state = ShowStates.All;

            getData();

            ShowData();
        }

        public void getData()
        {
            var today = DateTime.Now;
            double weight = 70;
            int steps = 10000;
            int calories = 2500;
            Random ran = new Random();

            for (DateTime date = today; date <= today.AddDays(365); date = date.AddDays(1))
            {

                webdata.Add(new weightModel(date.Date, weight, steps, calories));
                if (ran.NextDouble() < 0.5)
                {
                    weight += ran.NextDouble();
                    steps -= 600;
                    calories += 10;
                }
                else
                {
                    weight -= ran.NextDouble();
                    steps += 600;
                    calories -= 10;
                }
            }
        }


        [RelayCommand]

        public void SeeThreeMonths()
        {
            state = ShowStates.ThreeMonths;
            ShowData();
        }

        [RelayCommand]

        public void SeeAll()
        {
            state = ShowStates.All;
            ShowData();
        }


        [RelayCommand]
        public void SeeMonth()
        {
            state = ShowStates.Month;
            ShowData();
        }


        [RelayCommand]
        public void ShowData()
        {


            switch (state)
            {
                case ShowStates.All:
                    Data.Clear();
                    Data = new ObservableCollection<weightModel>(webdata);
                    break;
                case ShowStates.ThreeMonths:
                    Data.Clear();
                    foreach (var item in webdata)
                        if (item.Date <= DateTime.Now.AddDays(90))
                        {
                            Data.Add(item);
                        }
                    break;
                case ShowStates.Month:
                    Data.Clear();
                    foreach (var item in webdata)
                        if (item.Date <= DateTime.Now.AddDays(7))
                        {
                            Data.Add(item);
                        }
                    break;
                default:
                    break;
            }


        }

        [RelayCommand]
        public void AddWeights360()
        {
            Data.Clear();
            var today = DateTime.Now;
            double weight = 70;
            int steps = 10000;
            int calories = 2500;
            Random ran = new Random();
            for (DateTime date = today; date <= today.AddDays(360); date = date.AddDays(1))
            {

                Data.Add(new weightModel(date.Date, weight, steps, calories));
                if (ran.NextDouble() < 0.5)
                {
                    weight += ran.NextDouble();
                    calories += 50;
                    steps -= 600;
                }
                else
                {
                    weight -= ran.NextDouble();
                    calories -= 50;
                    steps += 600;
                }

            }
        }




    }
}
