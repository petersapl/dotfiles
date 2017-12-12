using System.Collections.Generic;

namespace DotfilesWrapper
{
    interface ICommandable<T>
    {
        List<T> Commands { get; set; }
    }
}
