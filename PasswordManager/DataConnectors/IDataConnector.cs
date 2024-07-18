using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataConnectors
{
    internal interface IDataConnector
    {
        public List<Resource> Load();
        public void Save(Resource resourse);
        public void Update(Resource resourse);
        public void Delete(int id);
    }
}
