using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restorator
{
    class Restoran
    {
        public string Filename;
        public byte[] imagedata;
        public string Name;
        public string Type;
        public string WorkTime;
        public string Sity;
        public string Street;
        public string Price;
        public string Disk;
        public string Cord;
        public Restoran(string Filename, byte[] imagedata,string Name, string Type,string Worktime, string Sity, string Street,string Price,string Disk,string Cord)
        {
            this.Filename = Filename;
            this.imagedata = imagedata;
            this.Name = Name;
            this.Type = Type;
            this.WorkTime = Worktime;
            this.Sity = Sity;
            this.Street = Street;
            this.Price = Price;
            this.Disk = Disk;
            this.Cord = Cord;
        }
    }

}
