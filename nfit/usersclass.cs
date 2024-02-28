using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfit
{
    public partial class users
    {
        public override string ToString()
        {
            //return base.ToString();
            return login + " - " + ФИО;
        }
    }

    public partial class pacienty
    {
        public override string ToString()
        {
            //return base.ToString();
            return fio;
        }
    }
    public partial class vrachi
    {
        public override string ToString()
        {
            //return base.ToString();
            return fio1;
        }
    }

    public partial class osmotry
    {
        public override string ToString()
        {
            //return base.ToString();
            return "Номер осмотра - " + Id_osmotra + ", Дата осмотра - " + data_osmotra;
        }
    }

    public partial class predpisanya
    {
        public override string ToString()
        {
            //return base.ToString();
            return "Номер предписания - " + Id_predpisanya + ", номер осмотра - " + id_osmotra;
        }
    }

    //public class OsmotryDisplayItem
    //{
    //    internal object osmotry;

    //    public string FullInfo { get; set; } // ФИО пациента - дата осмотра

    //    public OsmotryDisplayItem(pacienty pacient, DateTime data_osmotra)
    //    {
    //        FullInfo = pacient.fio + " - " + data_osmotra.ToString("dd.MM.yyyy"); // Форматирование даты по нужному шаблону
    //    }
    //}
}
