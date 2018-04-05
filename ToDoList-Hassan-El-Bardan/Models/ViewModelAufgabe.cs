using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList_Hassan_El_Bardan.Models
{
    public class ViewModelAufgabe
    {
        public List<Kategorie> _lKategorie { get; set; }
        public Kategorie _kategorie { get; set; }
        public main _main { get; set; }
    }
}