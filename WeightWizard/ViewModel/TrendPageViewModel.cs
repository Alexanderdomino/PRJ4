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

namespace WeightWizard.ViewModel
{
    public partial class TrendPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<weightModel> data;


        public ObservableCollection<weightModel> webdata;



        public enum ShowStates
        {
            Steps,
            Calories,
            Wonly
        }

        public ShowStates state;


        public TrendPageViewModel()
        {
            Data = new ObservableCollection<weightModel>();

            webdata = new ObservableCollection<weightModel>();

            state = ShowStates.Wonly;

            getData();

            ShowData90();
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
                    calories -= 100;
                }
                else
                {
                    weight -= ran.NextDouble();
                    steps += 600;
                    calories += 100;
                }
            }
        }


        [RelayCommand]

        public void SeeSteps()
        {
            state = ShowStates.Steps;
            ShowData90();
        }

        [RelayCommand]

        public void SeeCalories()
        {
            state = ShowStates.Calories;
            ShowData90();
        }


        [RelayCommand]
        public void SeeWonly()
        {
            state = ShowStates.Wonly;
            ShowData90();
        }


        [RelayCommand]
        public void ShowData90()
        {


            switch (state)
            {
                case ShowStates.Steps:
                    Data.Clear();
                    Data = new ObservableCollection<weightModel>(webdata);
                    break;
                case ShowStates.Calories:
                    Data.Clear();
                    Data = new ObservableCollection<weightModel>(webdata);
                    break;
                case ShowStates.Wonly:
                    Data.Clear();
                    Data = new ObservableCollection<weightModel>(webdata);
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
