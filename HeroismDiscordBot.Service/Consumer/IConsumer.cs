namespace HeroismDiscordBot.Service.Consumer
{
    public interface IConsumer<TIn>
    {
        void Consume(TIn data);
    }

    public interface IConsumer<TIn, TOut>
    {
        TOut Consume(TIn data);
    }
}