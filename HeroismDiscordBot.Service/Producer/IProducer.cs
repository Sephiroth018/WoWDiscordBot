namespace HeroismDiscordBot.Service.Producer
{
    public interface IProducer<TIn, TOut>
    {
        TOut GetData(TIn config);
    }
}