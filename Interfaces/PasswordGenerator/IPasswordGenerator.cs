using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.PasswordGenerator;

public interface IPasswordGenerator
{
    public string GeneratePassword(PasswordGeneratorArgs args);
}
