using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Wiki_Game
{
    public class Threader
    {
        public List<Task> ThreadTasks = new();
    }
}