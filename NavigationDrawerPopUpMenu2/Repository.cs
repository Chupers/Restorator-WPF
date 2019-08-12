using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restorator
{
    interface Repository
    {
        Stack<Restoran> SearchRest(string Restoran);
        Stack<Restoran> LoadRest();
        void PushRest(Restoran restoran);
         void DeleteRest(string Name);
        void DeleteComment(string Comment,string Rest);

    }
}
