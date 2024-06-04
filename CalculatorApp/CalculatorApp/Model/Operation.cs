using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorApp.Model
{
    public class Operation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string OperationType { get; set; }
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public double Result { get; set; }
    }
}
