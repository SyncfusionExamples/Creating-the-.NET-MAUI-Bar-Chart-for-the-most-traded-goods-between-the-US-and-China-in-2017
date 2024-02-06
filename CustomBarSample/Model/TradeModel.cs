using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarSample
{
    public class TradeModel
    {
        public string? GoodsName { get; set; }    
        public double ImportValue { get; set; }
        public double ExportValue {  get; set; }
        public string Category { get; set; }   
        
        public double Value { get; set; }

        public string Description { get; set; }

        public Microsoft.Maui.Graphics.IImage? Image { get; set; }

        public TradeModel(string name, double value, string category, string description)
        {
            GoodsName = name;
            Category = category;

            
            if (Category == "import")
            {
                //ImportValue = -value;
                Value = - value;
            }
            else
            {
                //ExportValue = value;
                Value = value;
            }

            Description = description;
        }
    }
}
