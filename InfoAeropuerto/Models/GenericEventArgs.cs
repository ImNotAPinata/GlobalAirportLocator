using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoAeropuerto.Models
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Results { get; private set; }

        public GenericEventArgs(T results)
        {
            this.Results = results;
        }
    }
}
