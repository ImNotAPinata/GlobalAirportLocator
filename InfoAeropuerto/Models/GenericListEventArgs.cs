using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoAeropuerto.Models
{
    public class GenericListEventArgs<T> : EventArgs
    {
        public List<T> Results { get; private set; }

        public GenericListEventArgs(List<T> results)
        {
            this.Results = results;
        }
    }
}
