using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace DashBoard.Presentation
{
    public class Disposition : INotifyPropertyChanged, INotifyPropertyChanging 
    {
        #region Variable Declaration

        private string _DispositionName;
        private int _DispositionCount;

        public static int TotalCount;
              
        #endregion

        public Disposition(string name, int count)
        {
            _DispositionName = name;
            _DispositionCount = count;
        }

        

        #region Properties

        public string DispositionName
        {
            get
            {
                return _DispositionName;
            }
            set
            {
                _DispositionName = value;
                OnPropertyChanged("DispositionName");
                OnPropertyChanging();
            }
        }


        public int DispositionCount
        {
            get
            {
                return _DispositionCount;
            }
            set
            {
                _DispositionCount = value;
                OnPropertyChanged("DispositionCount");
                OnPropertyChanging();
            }
        }


        #endregion
        
        public static Dictionary<string, int> ManageDictonary(string name, Dictionary<string, int> DispositionList)
        {
            if(!DispositionList.ContainsKey(name))
            {
                DispositionList.Add(name,1);
            }
            else
            {
                DispositionList[name] += 1;
            }

            return DispositionList;
        }

        public static ObservableCollection<Disposition> ManageCollection(string name, ObservableCollection<Disposition> obj)
        {
            bool find = false;
            TotalCount = 0;

            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].DispositionName == name )
                {
                    obj[i].DispositionCount += 1;
                    find = true;
                }
            }
            if (!find)
            {
                if(name!="")
                    obj.Add(new Disposition(name,1));
            }

            for (int j = 0; j < obj.Count; j++)
            {
                TotalCount = obj[j].DispositionCount + TotalCount;
                
            }
            
            return obj;
        }

        public int FncSetValue()
        {
            return TotalCount;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string PopName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PopName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanging()
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        #endregion

        

        

        
    }
}
