using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroismDiscordBot.Service.Transformer
{
    public interface ITransformer<TIn, TOut>
    {
        TOut Transform(TIn data);
    }
}
