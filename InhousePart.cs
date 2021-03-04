using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Manager
{
    class InhousePart : Part
    {
        public int MachineID { get; set; }

        // Constructor
        public InhousePart(int partID, string name, decimal price, int inStock, int min, int max, int machineID)
        {
            PartID = partID;
            Name = name;
            Price = price;
            InStock = inStock;
            Max = max;
            Min = min;
            MachineID = machineID;
        }
    }
}
